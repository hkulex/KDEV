using UnityEngine;
using UnityEngine.UI;

namespace notacompany
{
    public class RoomBrowserMenu : BaseMenu
    {
        [SerializeField] private Transform _roomCreationMenu;
        [SerializeField] private Transform _mainMenu;

        [SerializeField] private Transform _waitingRoom;

        [SerializeField] private Button _buttonCreateRoom;
        [SerializeField] private Button _buttonRefresh;
        [SerializeField] private Button _buttonBack;

        [SerializeField] private GameObject _roomListEntry;
        [SerializeField] private GameObject _roomListContent;


        /**
         * Description: Callback when the class is first initialized.
         */
        private void Awake()
        {
            _buttonCreateRoom.onClick.AddListener(OnButtonCreateRoomClicked);
            _buttonRefresh.onClick.AddListener(OnButtonRefreshClicked);
            _buttonBack.onClick.AddListener(OnButtonBackClicked);
        }


        /**
         * Description: Callback when the gameObject becomes active.
         */
        private void OnEnable()
        {
            ListAvailableRooms();
        }


        /**
         * Description: Clears and refills the list with available rooms to join.
         */
        private void ListAvailableRooms()
        {
            foreach (Transform t in _roomListContent.transform)
                Destroy(t.gameObject);

            RoomInfo[] rooms = PhotonNetwork.GetRoomList();

            for (int i = 0; i < rooms.Length; i++)
            {
                GameObject go = Instantiate(_roomListEntry);

                go.transform.SetParent(_roomListContent.transform);
                go.GetComponent<RoomBrowserListEntry>().Initialize(rooms[i], i);
            }
        }


        /**
         * Description: Callback when the client successfully joined a room.
         *              Opens the waiting menu.
         */
        public override void OnJoinedRoom()
        {
            base.OnJoinedRoom();

            Open(_waitingRoom);
        }



        /**
         * Description: Callback when the button is clicked.
         *              Opens the create room menu.
         */
        private void OnButtonCreateRoomClicked()
        {
            Open(_roomCreationMenu);
        }


        /**
         * Description: Callback when the button is clicked.
         *              Refreshes the room list.
         */
        private void OnButtonRefreshClicked()
        {
            ListAvailableRooms();
        }


        /**
         * Description: Callback when the button is clicked.
         *              Opens the main menu.
         */
        private void OnButtonBackClicked()
        {
            Open(_mainMenu);
        }
    }
}