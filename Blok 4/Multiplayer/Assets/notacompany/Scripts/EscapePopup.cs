using Photon;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace notacompany
{
    public class EscapePopup : PunBehaviour
    {
        [SerializeField] private Button _buttonResume;
        [SerializeField] private Button _buttonLeave;


        private void Awake()
        {
			_buttonResume.onClick.AddListener(OnButtonResumeClicked);
            _buttonLeave.onClick.AddListener(OnButtonLeaveClicked);
        }

        

        private void OnButtonResumeClicked()
        {
            Dispose();
        }


        private void OnButtonLeaveClicked()
        {
            Remove();
        }


        public void Set()
        {
            gameObject.SetActive(!gameObject.activeInHierarchy);
        }


        private void Dispose()
        {
            Set();
        }


        private void Remove()
        {
            Dispose();

            _buttonResume.onClick.RemoveAllListeners();
            _buttonLeave.onClick.RemoveAllListeners();

            PhotonNetwork.automaticallySyncScene = false;
            PhotonNetwork.LeaveRoom();

            SceneManager.LoadScene("SceneMenu");
        }
    }
}