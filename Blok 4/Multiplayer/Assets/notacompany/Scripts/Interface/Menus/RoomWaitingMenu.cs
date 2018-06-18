using System.Collections.Generic;
using ExitGames.Client.Photon;
using UnityEngine;
using UnityEngine.UI;

namespace notacompany
{
    public class RoomWaitingMenu : BaseMenu
    {
        [SerializeField] private Transform _menuRoomBrowser;

        [SerializeField] private Button _buttonStart;
        [SerializeField] private Button _buttonLeave;
        [SerializeField] private Text _labelOwnerName;
        [SerializeField] private Text _labelRoomName;

        [SerializeField] private GameObject _roomUserEntry;
        [SerializeField] private GameObject _roomUserListContent;


        private List<GameObject> _roomUserList;


        /**
         * Description: 
         */
        private void Awake()
        {
            _roomUserList = new List<GameObject>();

            _buttonStart.onClick.AddListener(OnButtonStartClicked);
            _buttonLeave.onClick.AddListener(OnButtonLeaveClicked);
        }


        /**
         * Description:
         *              s
         */
        private void OnEnable()
        {
            _roomUserList.Clear();

            foreach (Transform transform in _roomUserListContent.transform)
                Destroy(transform.gameObject);

            _labelRoomName.text = PhotonNetwork.room.Name;


            foreach (PhotonPlayer photonPlayer in PhotonNetwork.playerList)
            {
                GameObject go = Instantiate(_roomUserEntry);

                go.transform.SetParent(_roomUserListContent.transform);
                go.GetComponent<RoomUserListEntry>().Initialize(photonPlayer);

                photonPlayer.SetCustomProperties(new Hashtable() { { "ready", false } });

                _roomUserList.Add(go);

                if (photonPlayer.IsMasterClient)
                {
                    _labelOwnerName.text = photonPlayer.NickName + "'s room";
                }
            }

            _buttonStart.GetComponentInChildren<Text>().text = PhotonNetwork.isMasterClient ? "Start" : "Ready";
        }


        /**
         * Description: 
         */
        public override void OnPhotonPlayerConnected(PhotonPlayer newPlayer)
        {
            base.OnPhotonPlayerConnected(newPlayer);

            GameObject go = Instantiate(_roomUserEntry);

            go.transform.SetParent(_roomUserListContent.transform);
            go.GetComponent<RoomUserListEntry>().Initialize(newPlayer);

            _roomUserList.Add(go);
        }


        /**
         * Description: 
         */
        public override void OnPhotonPlayerDisconnected(PhotonPlayer otherPlayer)
        {
            base.OnPhotonPlayerDisconnected(otherPlayer);

            for (int i = 0; i < _roomUserList.Count; i++)
            {
                GameObject go = _roomUserList[i];

                if (go.GetComponent<RoomUserListEntry>().Name() == otherPlayer.NickName)
                {
                    _roomUserList.RemoveAt(i);
                    Destroy(go);
                }
            }
        }


        /**
         * Description: 
         */
        public override void OnMasterClientSwitched(PhotonPlayer newMasterClient)
        {
            base.OnMasterClientSwitched(newMasterClient);

            _buttonStart.GetComponentInChildren<Text>().text = PhotonNetwork.isMasterClient ? "Start" : "Ready";
            _labelOwnerName.text = newMasterClient.NickName + "'s room";

            newMasterClient.SetCustomProperties(new Hashtable() { { "ready", false } });
        }


        /**
         * Description: 
         */
        private void OnButtonStartClicked()
        {
            if (PhotonNetwork.isMasterClient)
            {
                foreach (PhotonPlayer photonPlayer in PhotonNetwork.playerList)
                {
                    if (!photonPlayer.IsMasterClient)
                    {
                        if (!(bool) photonPlayer.CustomProperties["ready"])
                        {
                            _errorPopup.Initialize("Make sure everyone is ready!");
                            return;
                        }
                    }
                }


                /**
                 * Setup the room and start the game
                 */
                Hashtable properties = new Hashtable();

                properties["order"] = PhotonNetwork.playerList; // set turn order (todo: randomize?)
                properties["index"] = 0;

                PhotonNetwork.room.SetCustomProperties(properties);
                PhotonNetwork.room.IsOpen = false;
                PhotonNetwork.room.IsVisible = false;

                PhotonNetwork.LoadLevel("SceneLevelDemo"); // maybe async to have loading screens?
            }
            else
            {
                /**
                 * Set the Photon Player to ready
                 */
                Hashtable properties = PhotonNetwork.player.CustomProperties;

                properties["ready"] = !(bool) properties["ready"];

                PhotonNetwork.player.SetCustomProperties(properties);
            }
        }


        /**
         * Description: 
         */
        private void OnButtonLeaveClicked()
        {
            PhotonNetwork.LeaveRoom();

            Open(_menuRoomBrowser);
        }
    }
}