<?php
//Start session
session_start();
//Unset the variables stored in session
unset($_SESSION['USER_ID']);
?>

<h4>Login</h4>
<form name="loginform" action="login_submit.php" method="post">
    <table width="309" border="0" align="center" cellpadding="2" cellspacing="5">
        <tr>
            <td colspan="2">
                <!--the code bellow is used to display the message of the input validation-->
                <?php
                if (count($_SESSION['ERROR']) > 0) {
                    echo '<ul class="err">';
                    foreach($_SESSION['ERROR'] as $error) {
                        echo '<li>' . $error . '</li>';
                    }
                    echo '</ul>';
                    unset($_SESSION['ERROR']);
                }
                ?>
            </td>
        </tr>
        <tr>
            <td><div align="right">Username</div></td>
            <td><input name="username" type="text" /></td>
        </tr>
        <tr>
            <td><div align="right">Password</div></td>
            <td><input name="password" type="password" /></td>
        </tr>
        <tr>
            <td><div align="right"></div></td>
            <td><input name="" type="submit" value="login" /></td>
        </tr>
    </table>
</form>
<h4>Or signup</h4>
<form name="singup" action="signup_submit.php" method="post">
    <table width="309" border="0" align="center" cellpadding="2" cellspacing="5">
        <tr>
            <td colspan="2">
                <!--the code bellow is used to display the message of the input validation-->
                <?php
                if (count($_SESSION['ERROR']) > 0) {
                    echo '<ul class="err">';
                    foreach($_SESSION['ERROR'] as $error) {
                        echo '<li>' . $error . '</li>';
                    }
                    echo '</ul>';
                    unset($_SESSION['ERROR']);
                }
                ?>
            </td>
        </tr>
        <tr>
            <td><div align="right">Username</div></td>
            <td><input name="username" type="text" /></td>
        </tr>
        <tr>
            <td><div align="right">Password</div></td>
            <td><input name="password" type="password" /></td>
        </tr>
        <tr>
            <td><div align="right">Email</div></td>
            <td><input name="email" type="text" /></td>
        </tr>
        <tr>
            <td><div align="right"></div></td>
            <td><input name="" type="submit" value="signup" /></td>
        </tr>
    </table>
</form>