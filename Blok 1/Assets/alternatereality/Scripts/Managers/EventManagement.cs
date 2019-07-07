using UnityEngine;

namespace alternatereality
{
    public class EventManagement : MonoBehaviour
    {
        public delegate void ScoreEventHandler(int score);
        public static event ScoreEventHandler OnDeathAction;


        public static void GameObjectIsDead(int score)
        {
            OnDeathAction?.Invoke(score);
        }
    }
}