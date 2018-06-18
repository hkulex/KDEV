<?php

require_once('database.php');
$db = new Database();

?>

<html>
<head>
    <title>Scores</title>
</head>
<body>
    <?php require_once('nav.php'); ?>

    <h4>Fanatas4 top 5 last week</h4>

    <table cellspacing="15">
        <tr>
            <th align="left">Username</th>
            <th align="left">Score</th>
            <th align="left">Game</th>
            <th align="left">Date</th>
        </tr>
        <?php
        foreach ($db->getScoresWithUserSince((new DateTime())->modify('-7 day'), 5) as $result) {
            echo "<tr>
                      <td>". $result['username'] . "</td>
                      <td>". $result['score'] . "</td>
                      <td>". $result['game'] . "</td>
                      <td>". $result['published'] . "</td>
                      </tr>";
        }
        ?>
    </table>
    <h4>Total plays per game</h4>
    <table cellspacing="15">
        <tr>
            <th align="left">Game</th>
            <th align="left">Total plays</th>
        </tr>
        <?php
            foreach ($db->getTotalPlayedGames() as $result) {
                echo "<tr>
                      <td>". $result['game'] . "</td>
                      <td>". $result['total'] . "</td>
                      </tr>";
            }
        ?>
    </table>

    <h4>All fanatas4 recored scores</h4>

    <table cellspacing="15">
        <tr>
            <th align="left">Username</th>
            <th align="left">Score</th>
            <th align="left">Game</th>
            <th align="left">Date</th>
        </tr>
        <?php
        foreach ($db->getScoresWithUserSince() as $result) {
            echo "<tr>
                      <td>". $result['username'] . "</td>
                      <td>". $result['score'] . "</td>
                      <td>". $result['game'] . "</td>
                      <td>". $result['published'] . "</td>
                      </tr>";
        }
        ?>
    </table>
</body>
</html>

