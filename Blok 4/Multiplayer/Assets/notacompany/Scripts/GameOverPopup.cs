using Photon;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace notacompany
{
    public class GameOverPopup : PunBehaviour
    {
        [SerializeField] private Button _buttonConfirm;


        public void Initialize(int score)
        {
            gameObject.SetActive(true);
            
            _buttonConfirm.onClick.AddListener(OnButtonConfirmClicked);
        }


        private void OnButtonConfirmClicked()
        {
            gameObject.SetActive(false);

            _buttonConfirm.onClick.RemoveListener(OnButtonConfirmClicked);

            PhotonNetwork.automaticallySyncScene = false;
            PhotonNetwork.LeaveRoom();

            SceneManager.LoadScene("SceneMenu");
        }
    }
}