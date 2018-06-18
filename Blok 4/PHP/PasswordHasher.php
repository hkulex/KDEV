<?php

class PasswordHasher
{
    /**
     * @param string $password
     *
     * @return string
     */
    public function hash($password) {
        return password_hash($password, PASSWORD_BCRYPT);
    }

    /**
     * @param string $password
     * @param string $hash
     *
     *  @return bool
     */
    public function isMatch($password, $hash) {
        return password_verify($password, $hash);
    }
}