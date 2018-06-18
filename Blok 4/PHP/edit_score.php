<?php

require_once('auth.php');
require_once('database.php');
$db = new Database();

?>

<html>
<head>
    <title>Fanatasy4 insert your scores</title>
</head>
<body>
<?php require_once('nav.php'); ?>
<h4>Insert your score for a game</h4>
<form action="save_score.php" method="get">
    <table>
        <tr>
            <td><label>Game:</label></td>
            <td><select name="game">
                <?php
                    foreach ($db->getAllGames() as $game) {
                        echo "<option value=" . $game['id'] . ">" . $game['name'] . "</option>";
                    }
                ?>
            </select>
        </td>
        </tr>
        <tr>
            <td><label>Score:</label></td>
            <td><input type="text" name="score" value=""></td>
        </tr>
        <td><input type="submit" value="Save"></td>
        </tr>
    </table>
</form>

</body>
</html>

