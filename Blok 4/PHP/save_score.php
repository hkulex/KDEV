<?php
require_once('auth.php');
require_once('database.php');

$db = new Database();

$db->insertScore($_SESSION['USER_ID'], $_GET['game'], $_GET['score']);

?>

<html>
<head>
    <title>Scores</title>
</head>
<body>
<?php require_once('nav.php'); ?>

<h4>Saved your score</h4>

</body>
</html>
