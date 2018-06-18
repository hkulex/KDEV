<?php
//Start session
session_start();

//Include database connection details
require_once('database.php');

$db = new Database();

//Array to store validation errors
$_SESSION['ERROR'] = [];

//Input Validations
if(empty($_POST['username'])) {
    $_SESSION['ERROR'][] = 'Username missing';
}
if(empty($_POST['password'])) {
    $_SESSION['ERROR'][] = 'Password missing';
}
if(empty($_POST['email'])) {
    $_SESSION['ERROR'][] = 'Email missing';
}

//If there are input validations, redirect back to the login form
if(count($_SESSION['ERROR']) > 0) {
    session_write_close();
    header("location: index.php");
    exit();
}

$db->createUser($_POST['username'], $_POST['password'], $_POST['email']);

$_SESSION['ERROR'][] = 'User created, please login';
header("location: index.php");
exit();
?>