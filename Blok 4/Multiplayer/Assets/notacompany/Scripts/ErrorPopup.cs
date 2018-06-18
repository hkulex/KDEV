using UnityEngine;
using UnityEngine.UI;

namespace notacompany
{
    public class ErrorPopup : MonoBehaviour
    {
        [SerializeField] private Text _labelInformation;
        [SerializeField] private Button _buttonConfirm;


        public void Initialize(string information)
        {
            gameObject.SetActive(true);

            _labelInformation.text = information;
            _buttonConfirm.onClick.AddListener(OnButtonConfirmClicked);
        }


        private void OnButtonConfirmClicked()
        {
            gameObject.SetActive(false);

            _buttonConfirm.onClick.RemoveListener(OnButtonConfirmClicked);
        }
    }
}