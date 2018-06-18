using ExitGames.Client.Photon;
using Photon;
using UnityEngine;
using UnityEngine.UI;

namespace notacompany
{
    public class RoomUserListEntry : PunBehaviour
    {
        [SerializeField] private Text _labelTitle;
        [SerializeField] private Text _labelUsername;
        [SerializeField] private Text _labelPing;
        [SerializeField] private Image _iconAvatar;
        [SerializeField] private Image _iconHost;
        [SerializeField] private Image _iconReady;

        private PhotonPlayer _photonPlayer;


        /**
         * Description: Initialize the class.
         */
        public void Initialize(PhotonPlayer photonPlayer)
        {
            _photonPlayer = photonPlayer;

            _labelTitle.text = ""; //= get from playerproperties
            _labelTitle.color = new Color(150, 0, 190, 255); //= get from playerproperties
            _labelUsername.text = photonPlayer.NickName;
            _labelPing.text = "-"; // todo
            // _iconAvatar = get from playerproperties
            _iconHost.gameObject.SetActive(photonPlayer.IsMasterClient);
        }


        /**
         * Description: Callback when the properties of a Photon Player are changed.
         *              Enables or disables the ready icon for the Photon Player.
         */
        public override void OnPhotonPlayerPropertiesChanged(object[] playerAndUpdatedProps)
        {
            base.OnPhotonPlayerPropertiesChanged(playerAndUpdatedProps);

            PhotonPlayer photonPlayer = playerAndUpdatedProps[0] as PhotonPlayer;

            if (photonPlayer.ID == _photonPlayer.ID)
            {
                Hashtable properties = playerAndUpdatedProps[1] as Hashtable;

                _iconReady.gameObject.SetActive((bool)properties["ready"]);
            }
        }


        /**
         * Description: Callback when the host is given to another player.
         *              Sets the host icon to the new host entry.
         */
        public override void OnMasterClientSwitched(PhotonPlayer newMasterClient)
        {
            base.OnMasterClientSwitched(newMasterClient);

            _iconHost.gameObject.SetActive(newMasterClient.ID == _photonPlayer.ID);
        }


        /**
         * Description: Return the nickname of the Photon Player
         */
        public string Name()
        {
            return _photonPlayer.NickName;
        }
    }
}