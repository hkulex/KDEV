using ExitGames.Client.Photon;
using UnityEngine;
using UnityEngine.UI;

namespace notacompany
{
    public class RoomCreationMenu : BaseMenu
    {
        [SerializeField] private Transform _waitingMenu;
        [SerializeField] private Transform _serverBrowserMenu;
        [SerializeField] private InputField _inputName;
        [SerializeField] private InputField _inputPassword;
        [SerializeField] private Button _buttonCreate;
        [SerializeField] private Button _buttonCancel;

        private string _roomName;


        /**
         * Description: 
         */
        private void Awake()
        {
            _inputName.onValueChanged.AddListener(OnInputNameValueChanged);
            _inputName.contentType = InputField.ContentType.Alphanumeric;

            _inputPassword.onValueChanged.AddListener(OnInputPasswordValueChanged);

            _buttonCreate.onClick.AddListener(OnButtonCreateClick);
            _buttonCancel.onClick.AddListener(OnButtonCancelClick);

            _roomName = "";
        }


        /**
         * Description: 
         */
        private void OnEnable()
        {
            _inputName.text = "";
            _inputPassword.text = "";

            _buttonCreate.interactable = false;
        }


        /**
         * Description: 
         */
        private void OnInputNameValueChanged(string value)
        {
            _roomName = value;
            _buttonCreate.interactable = value.Length >= Settings.MINUMUM_INPUT_LENGTH && value.Length <= Settings.MAXIMUM_INPUT_LENGTH;
        }


        /**
         * Description: 
         */
        private void OnInputPasswordValueChanged(string value)
        {
            
        }


        /**
         * Description:
         *              Create the room.
         */
        private void OnButtonCreateClick()
        {
            _buttonCreate.interactable = false;

            Hashtable properties = new Hashtable();
            RoomOptions roomOptions = new RoomOptions();

            roomOptions.PublishUserId = false;
            roomOptions.MaxPlayers = Settings.MAXIMUM_ROOM_SIZE;
            roomOptions.CustomRoomProperties = properties;

            PhotonNetwork.CreateRoom(_roomName, roomOptions, TypedLobby.Default);
        }


        /**
         * Description: 
         */
        private void OnButtonCancelClick()
        {
            Open(_serverBrowserMenu);
        }


        /**
         * Description: 
         */
        public override void OnPhotonCreateRoomFailed(object[] codeAndMsg)
        {
            base.OnPhotonCreateRoomFailed(codeAndMsg);

            _errorPopup.Initialize("Failed to create the server. Please try again.");

            _buttonCreate.interactable = true;
        }


        /**
         * Description: 
         */
        public override void OnJoinedRoom()
        {
            base.OnJoinedRoom();

            Open(_waitingMenu);
        }
    }
}