using Photon;
using UnityEngine;
using UnityEngine.UI;

namespace notacompany
{
    public class GameUserListEntry : PunBehaviour
    {
        [SerializeField] private Image _avatar;
        [SerializeField] private Image _cross;
        [SerializeField] private Image _outline;
        [SerializeField] private Text _labelTitle;
        [SerializeField] private Text _labelUsername;
        [SerializeField] private Text _labelPing;

        private PhotonPlayer _photonPlayer;

        public void Initialize(PhotonPlayer photonPlayer)
        {
            _photonPlayer = photonPlayer;

            _labelTitle.text = "";
            _labelUsername.text = photonPlayer.NickName;
            _labelPing.text = "?";
        }


        public override void OnPhotonPlayerDisconnected(PhotonPlayer otherPlayer)
        {
            base.OnPhotonPlayerDisconnected(otherPlayer);

            if (otherPlayer.ID == _photonPlayer.ID)
                _cross.gameObject.SetActive(true);
        }


        public void BeginTurn()
        {
            _outline.gameObject.SetActive(true);
        }

        public void EndTurn()
        {
            _outline.gameObject.SetActive(false);
        }


        public int Id
        {
            get { return _photonPlayer.ID; }
        }
    }
}