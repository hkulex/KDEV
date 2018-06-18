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

//If there are input validations, redirect back to the login form
if(count($_SESSION['ERROR']) > 0) {
    session_write_close();
    header("location: index.php");
    exit();
}

$user = $db->authorizeUser($_POST['username'], $_POST['password']);
//Authorize user
if($user['authorized']) {
    session_regenerate_id();
    //Store the user id for future reference
    $_SESSION['USER_ID'] = $user['id'];
    session_write_close();

    header("location: edit_user.php");
}else {
    $_SESSION['ERROR'][] = "user name and/or password not found";
    session_write_close();
    header("location: index.php");
    exit();
}
?>