using System.Collections;
using System.Collections.Generic;
using Photon;
using UnityEngine;
using UnityEngine.UI;
using Hashtable = ExitGames.Client.Photon.Hashtable;

namespace notacompany
{
    public class GameManagement : PunBehaviour
    {
        public static GameManagement instance;

        [SerializeField] private EscapePopup _escapePopup;
        [SerializeField] private GameOverPopup _gameOverPopup;

        [SerializeField] private GameObject _gameController;
        [SerializeField] private GameObject _userListEntry;
        [SerializeField] private GameObject _userListContent;

        private List<GameUserListEntry> _userList;

        private bool _isRunning;

        private void Awake()
        {
            if (!instance)
                instance = this;

            _userList = new List<GameUserListEntry>();

            _isRunning = true;
        }


        private void Start()
        {
            Hashtable properties = PhotonNetwork.room.CustomProperties;
            PhotonPlayer[] photonPlayerList = properties["order"] as PhotonPlayer[];

            if (PhotonNetwork.isMasterClient)
            {
                GameObject go = PhotonNetwork.InstantiateSceneObject("GameController", new Vector3(-3f, 0f, 0f), Quaternion.identity, 0, null);

                go.GetPhotonView().TransferOwnership(photonPlayerList[0]);
            }

            foreach (PhotonPlayer photonPlayer in photonPlayerList)
            {
                GameObject go = Instantiate(_userListEntry);
                GameUserListEntry user = go.GetComponent<GameUserListEntry>();

                photonPlayer.SetCustomProperties(new Hashtable() { { "ready", false } });
                go.transform.SetParent(_userListContent.transform);
                user.Initialize(photonPlayer);

                _userList.Add(user);
            }

            StartCoroutine(Spawn());
        }



        private IEnumerator Spawn()
        {
            yield return new WaitForSeconds(2f);

            if (PhotonNetwork.isMasterClient)
            {
                if (Random.Range(0, 2) == 0)
                    PhotonNetwork.InstantiateSceneObject("ObjectSpawn1", new Vector3(5f, -1f, 0f), Quaternion.identity, 0, null);
                else
                    PhotonNetwork.InstantiateSceneObject("ObjectSpawn2", new Vector3(5f, 2f, 0f), Quaternion.identity, 0, null);
            }

            StartCoroutine(Spawn());
        }



        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                _escapePopup.Set();
            }
        }


        public void OnGameFinished()
        {
            StopAllCoroutines();

            _isRunning = false;
            _gameOverPopup.Initialize(0);
        }



        public override void OnOwnershipTransfered(object[] viewAndPlayers)
        {
            base.OnOwnershipTransfered(viewAndPlayers);

            PhotonPlayer newOwner = viewAndPlayers[1] as PhotonPlayer;

            foreach (GameUserListEntry user in _userList)
            {
                if (user.Id == newOwner.ID)
                    user.BeginTurn();
                else
                    user.EndTurn();
            }
        }


        /**
         * Description: Callback when a player in the game disconnects.
         *              Updates the current order of the game and passes the turn to the next player.
         */
        public override void OnPhotonPlayerDisconnected(PhotonPlayer otherPlayer)
        {
            base.OnPhotonPlayerDisconnected(otherPlayer);

            Hashtable properties = PhotonNetwork.room.CustomProperties;
            PhotonPlayer[] order = properties["order"] as PhotonPlayer[];
            int index = (int)properties["index"];

            List<PhotonPlayer> list = new List<PhotonPlayer>();

            foreach (PhotonPlayer photonPlayer in order)
                if (photonPlayer.ID != otherPlayer.ID)
                    list.Add(photonPlayer);

            if (index >= list.Count)
                index = 0;

            properties["order"] = list.ToArray();
            properties["index"] = index;

            PhotonNetwork.room.SetCustomProperties(properties);

            foreach (GameUserListEntry user in _userList)
            {
                if (user.Id == list[index].ID)
                    user.BeginTurn();
                else
                    user.EndTurn();
            }
        }


        public bool IsRunning
        {
            get { return _isRunning; }
        }
    }
}