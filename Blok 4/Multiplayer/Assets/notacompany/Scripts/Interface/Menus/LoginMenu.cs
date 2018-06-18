using ExitGames.Client.Photon;
using UnityEngine;
using UnityEngine.UI;

namespace notacompany
{
    public class LoginMenu : BaseMenu
    {
        private const int MINIMUM_USERNAME_LENGTH = 4;
        private const int MAXIMUM_USERNAME_LENGTH = 12;

        [SerializeField] private Transform _menuToOpen;
        [SerializeField] private InputField _inputUsername;
        [SerializeField] private Button _buttonConnect;

        private string _username;
        private bool _isValidUsername;


        private void Awake()
        {
            Application.runInBackground = true;
            Application.targetFrameRate = 60;

            Screen.SetResolution(800, 600, false);

            PhotonNetwork.autoJoinLobby = true;
            PhotonNetwork.automaticallySyncScene = true;

            _username = "";
            _isValidUsername = false;
        }


        private void Start()
        {
            _inputUsername.contentType = InputField.ContentType.Alphanumeric;
            _inputUsername.onValueChanged.AddListener(OnInputUsernameValueChanged);

            _buttonConnect.onClick.AddListener(OnConnectButtonClicked);
            _buttonConnect.interactable = false;
        }


        public override void OnJoinedLobby()
        {
            base.OnJoinedLobby();
            // change screen to whatever
            PhotonNetwork.playerName = _username;
            PhotonNetwork.player.SetCustomProperties(new Hashtable() { { "ping", PhotonNetwork.GetPing() } });

            Open(_menuToOpen);
        }

        public override void OnConnectedToMaster()
        {
            base.OnConnectedToMaster();

            PhotonNetwork.automaticallySyncScene = true;
            PhotonNetwork.playerName = _username;
            PhotonNetwork.player.SetCustomProperties(new Hashtable() { { "ping", PhotonNetwork.GetPing() } } );

            Open(_menuToOpen);
        }




        private void OnInputUsernameValueChanged(string input)
        {
            _isValidUsername = input.Length >= MINIMUM_USERNAME_LENGTH && input.Length <= MAXIMUM_USERNAME_LENGTH;
            _buttonConnect.interactable = _isValidUsername;
            _username = input;
        }

        private void OnConnectButtonClicked()
        {
            if (!PhotonNetwork.connected)
            {
                PhotonNetwork.ConnectUsingSettings(Settings.CLIENT_VERSION);

                _buttonConnect.interactable = false;
            }

            //use this to change regions (optional)
            // PhotonNetwork.OverrideBestCloudServer(CloudRegionCode.eu);
        }
    }
}