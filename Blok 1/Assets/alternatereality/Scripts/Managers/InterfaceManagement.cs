using UnityEngine;
using UnityEngine.UI;

namespace alternatereality
{
    public class InterfaceManagement : MonoBehaviour
    {
        [SerializeField] private Text _txtScore;

        private int _score;

        private void Awake()
        {
            _score = 0;

            EventManagement.OnDeathAction += AddScore;
        }


        private void AddScore(int points)
        {
            _score += points;

            _txtScore.text = _score.ToString();
        }
    }
}