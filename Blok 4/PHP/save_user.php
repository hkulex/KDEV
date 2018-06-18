<?php
require_once('auth.php');
require_once('database.php');

$db = new Database();

$db->updateUser($_SESSION['USER_ID'], $_POST['password'], $_POST['email']);

header("location: scores.php");
exit();
?>