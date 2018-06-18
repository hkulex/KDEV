<?php

require_once('auth.php');
require_once('database.php');
$db = new Database();

?>

<html>
<head>
    <title>Edit profile</title>
</head>
<body>
    <?php require_once('nav.php'); ?>
    <h4>Edit your profile</h4>
    <form action="save_user.php" method="post" enctype="multipart/form-data">
        <table>
            <tr class='password'>
                <td><label>&nbsp;Password:&nbsp;</label></td>
                <td><input name="password" type="password"/></td>
            </tr>
            <tr class='email'>
                <td><label>&nbsp;Email:&nbsp;</label></td>
                <td><input id="email" type="text" name="email" value=""></td>
            </tr>
            </tr>
                <td><input type="submit" value="Save"></td>
            </tr>
        </table>
    </form>

</body>
</html>

