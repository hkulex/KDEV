using Photon;
using UnityEngine;

namespace notacompany
{
    public class BaseMenu : PunBehaviour
    {
        [SerializeField] protected ErrorPopup _errorPopup;

        protected void Open(Transform menu)
        {
            transform.gameObject.SetActive(false);
            menu.gameObject.SetActive(true);
        }
    }
}