<?php

include "PasswordHasher.php";

class Database
{
    /** @var mysqli */
    private $mysqli;
    /** @var string */
    private $server;
    /** @var string */
    private $database;
    /** @var string */
    private $user;
    /** @var string */
    private $password;
    /** @var PasswordHasher */
    private $hasher;

    function __construct()
    {
        $this->server = "db"; // docker container name change to 127.0.0.1 when database is the same host
        $this->database = "alexvanderlinden";
        $this->user = "alex";
        $this->password = "bidsprinkhaan420";
        $this->hasher = new PasswordHasher();
        $this->mysqli = new mysqli($this->server, $this->user, $this->password, $this->database);

        if ( ! $this->mysqli) {
            die('Could not connect: ' . $this->mysqli->connect_error);
        }

        //Set default database
        $this->mysqli->select_db($this->database);
        //Set encoding
        $this->mysqli->set_charset('utf8');
    }

    public function authorizeUser($username, $password)
    {
        $query = "	SELECT 
                      id,
                      username,
                      password
                    FROM 
                      users 
                    WHERE 
                      username= %s
                ";

        $query = $this->escape($query, [$username, $this->hasher->hash($password)]);
        $result = $this->mysqli->query($query, MYSQLI_ASSOC)
            ->fetch_array(MYSQLI_ASSOC) or die($this->mysqli->error);

        //If password correct
        if ($this->hasher->isMatch($password, $result['password'])) {
            return ['id' => $result['id'], 'authorized' => true];
        }

        return ['authorized' => false];
    }

    public function updateUser($userId, $password, $email)
    {
        $query = " 	UPDATE 
 	                  users
                    SET 
                      password = %s, 
                      email = %s
                    WHERE 
                      id = %s
        ";
        $query = $this->escape($query, [$this->hasher->hash($password), $email, $userId]);

        $this->mysqli->query($query) or die($this->mysqli->error);
    }

    public function createUser($username, $password, $email)
    {
        $query = " 	INSERT INTO 
 	                  users (`username`, `password`, `email`)
                    VALUES 
                      (%s, %s, %s)
        ";
        $query = $this->escape($query, [$username, $this->hasher->hash($password), $email]);

        $this->mysqli->query($query) or die($this->mysqli->error);
    }


    /**
     * @param \DateTime|null $since default is since beginning of time
     * @param int $limit
     *
     * @return array
     */
    public function getScoresWithUserSince($since = null, $limit = 100)
    {
        if ( ! $since) {
            $since = new DateTime('1970-01-01');
        }

        $query = "	  SELECT
                        users.username AS username,
                        scores.score AS score,
                        scores.published AS published,
                        games.name AS game
                      FROM
                        scores
                      LEFT JOIN
                        users
                       ON
                        users.id = scores.user_id
                      LEFT JOIN
                        games
                       ON
                        scores.game_id = games.id
                      WHERE 
                        scores.published > %s
                      ORDER BY 
                        scores.score DESC
                      LIMIT
                        %s 
                    ";
        $query = $this->escape($query, [$since->format("Y-m-d H:i:s"), $limit]);

        $result = $this->mysqli->query($query) or die($this->mysqli->error);
        return $this->fetchMultipleRows($result);
    }

    public function getTotalPlayedGames()
    {
        $query = "	  SELECT
	                    games.name AS game,
                        count(*) AS total
                      FROM
                        scores
                      LEFT JOIN
                        games
                       ON
                        scores.game_id = games.id
                      GROUP by 
                        game
                      ORDER BY 
                        total DESC
                    ";

        $result = $this->mysqli->query($query) or die($this->mysqli->error);
        return $this->fetchMultipleRows($result);
    }

    /**
     * @return array
     */
    public function getAllGames()
    {
        $query = "	  SELECT
                        id,
                        `name`
                      FROM
                        games
                    ";

        return $this->fetchMultipleRows($this->mysqli->query($query));
    }

    public function insertScore($userId, $gameId, $score)
    {
        $query = " 	INSERT INTO
                        scores(user_id, game_id, score)
                    VALUES
                        (%s, %s, %s)	
                ";
        $query = $this->escape($query, [$userId, $gameId, $score]);

        $this->mysqli->query($query) or die($this->mysqli->error);
    }

    /**
     * @param mysqli_result $mysqli
     *
     * @return array|null
     */
    private function fetchMultipleRows($mysqli)
    {
        if ( ! $mysqli) {
            return [];
        }

        $result = [];

        while($row = $mysqli->fetch_array(MYSQLI_ASSOC)) {
            array_push($result, $row);
        }

        return $result;
    }

    /**
     * @param string $query
     * @param array $values
     *
     * @return string
     */
    private function escape($query, $values)
    {
        //gets the arguments
        $escapedValues = [];
        foreach ($values as $key => $val) {
            //note the single quotes when the value is not null
            if (is_numeric($val)) {
                $escapedValues[$key] = sprintf("%s", $this->mysqli->escape_string($val));
            } else if ( ! empty($val)) {
                $escapedValues[$key] = sprintf("'%s'", $this->mysqli->escape_string($val));
            } else {
                $escapedValues[$key] = 'NULL';
            }
        }

        return vsprintf($query, $escapedValues);
    }
}

?>
