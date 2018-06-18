using UnityEngine;
using UnityEngine.UI;

namespace notacompany
{
    public class MainMenu : BaseMenu
    {
        [SerializeField] private Transform _menuToOpen;

        [SerializeField] private Button _buttonServerBrowser;
        [SerializeField] private Button _buttonQuit;

        private void Awake()
        {
            _buttonServerBrowser.onClick.AddListener(OnButtonServerBrowserClicked);
            _buttonQuit.onClick.AddListener(OnButtonQuitClicked);
        }

        private void OnButtonServerBrowserClicked()
        {
            Open(_menuToOpen);
        }

        private void OnButtonQuitClicked()
        {
            Application.Quit();
        }
    }
}