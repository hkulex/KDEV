using UnityEngine;

namespace notacompany
{
    public class Settings : MonoBehaviour
    {
        public static string CLIENT_VERSION = "0.1";

        // == Room settings ==
        public static byte MINUMUM_ROOM_SIZE = 2;
        public static byte MAXIMUM_ROOM_SIZE = 4;

        public static int MINUMUM_INPUT_LENGTH = 4;
        public static int MAXIMUM_INPUT_LENGTH = 12;
    }
}