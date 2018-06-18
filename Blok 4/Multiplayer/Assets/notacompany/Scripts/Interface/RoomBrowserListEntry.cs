using UnityEngine;
using UnityEngine.UI;

namespace notacompany
{
    public class RoomBrowserListEntry : MonoBehaviour
    {
        [SerializeField] private Text _labelRoomName;
        [SerializeField] private Text _labelUserCount;
        [SerializeField] private Image _imagePassword;
        [SerializeField] private Button _buttonJoin;

        private RoomInfo _roomInfo;


        /**
         * Description: 
         */
        private void Awake()
        {
            _buttonJoin.onClick.AddListener(OnButtonJoinClicked);
        }


        /**
         * Description: Initialze the entry.
         */
        public void Initialize(RoomInfo roomInfo, int index)
        {
            _roomInfo = roomInfo;

            _labelRoomName.text = roomInfo.Name;
            _labelUserCount.text = roomInfo.PlayerCount + "/" + roomInfo.MaxPlayers;
            _imagePassword.gameObject.SetActive(false); // Password rooms are not yet implemented

            // Slightly change the color of every other entry
            if (index % 2 == 0)
                GetComponent<Image>().color = new Color(245, 245, 245, 255);
        }


        /**
         * Description: Callback when the button is clicked.
         *              Joins the selected room.
         */
        private void OnButtonJoinClicked()
        {
            PhotonNetwork.JoinRoom(_roomInfo.Name);
        }
    }
}