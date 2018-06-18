<?php
	session_start();
	//Check whether the session variable USER_ID is present or not
	if( ! isset($_SESSION['USER_ID']) || (trim($_SESSION['USER_ID']) == '')) {
		header("location: logout.php");
		exit();
	}
?>


