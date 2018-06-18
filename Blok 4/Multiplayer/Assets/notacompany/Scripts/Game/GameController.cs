using ExitGames.Client.Photon;
using Photon;
using UnityEngine;

namespace notacompany
{
    public class GameController : PunBehaviour
    {
        private Rigidbody2D _rigidbody;

        private bool _isActive;


        private void Awake()
        {
            PhotonNetwork.sendRate = 30;
            PhotonNetwork.sendRateOnSerialize = 30;

            _rigidbody = GetComponent<Rigidbody2D>();

            _isActive = true;
        }


        public void Update()
        {
            if (!_isActive)
                return;

            if (photonView.isMine)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    if (transform.position.y >= 2.5)
                        return;

                    Hashtable properties = PhotonNetwork.room.CustomProperties;
                    PhotonPlayer[] order = properties["order"] as PhotonPlayer[];
                    int index = (int)properties["index"];

                    index++;

                    if (index >= PhotonNetwork.playerList.Length)
                        index = 0;

                    properties["index"] = index;

                    PhotonNetwork.room.SetCustomProperties(properties);
                    photonView.TransferOwnership(order[index]);

                    photonView.RPC("RpcOnPhotonPlayerClicked", PhotonTargets.All);
                }

                if (transform.position.y <= -2)
                {
                    photonView.RPC("RpcOnPillarHit", PhotonTargets.All);
                }
            }
        }


        [PunRPC]
        public void RpcOnPhotonPlayerClicked()
        {
            _rigidbody.velocity = new Vector2();
            _rigidbody.AddForce(new Vector2(0, 100f));
        }


        [PunRPC]
        public void RpcOnPillarHit()
        {
            OnPillarHit();
        }


        private void OnPillarHit()
        {
            _isActive = false;
            _rigidbody.velocity = new Vector2();
            _rigidbody.isKinematic = true;

            GameManagement.instance.OnGameFinished();
        }


        private void OnTriggerEnter2D(Collider2D trigger)
        {
            if (PhotonNetwork.isMasterClient)
            {
                photonView.RPC("RpcOnPillarHit", PhotonTargets.All);
            }
        }


        // todo: add get ready screen
        // todo: fix shit bug
        // todo: add score when passing pillar
    }
}