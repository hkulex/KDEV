using UnityEngine;

namespace alternatereality
{
    public abstract class BaseEnemy : MonoBehaviour, IHittable
    {
        protected int health;
        protected bool active;
        protected int points;

        protected virtual void Awake()
        {
            health = -1;
            points = -1;
            active = true;
        }


        public virtual void Move(float x, float z)
        {
            transform.Translate(new Vector3(x, 0, z));
        }


        public void Hit(int damage)
        {
            health -= damage;

            if (health <= 0)
                Death();
        }


        public void Death()
        {
            active = false;
            gameObject.SetActive(false);

            EventManagement.GameObjectIsDead(points);
        }
    }
}