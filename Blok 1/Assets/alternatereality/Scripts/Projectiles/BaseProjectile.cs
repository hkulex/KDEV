using UnityEngine;

namespace alternatereality
{
    public class BaseProjectile : MonoBehaviour
    {
        protected PlayerController owner;
        protected Vector3 axis;
        protected float velocity;
        protected int damage;


        public virtual void Initialize(Vector3 axis, PlayerController owner)
        {
            this.owner = owner;
            this.axis = axis;

            velocity = 10f;
            damage = 1;
        }


        protected virtual void Update()
        {
            transform.Translate(new Vector3(axis.z, 0, axis.x) * -axis.y * Time.deltaTime * velocity);

            if (transform.position.x > 20 || transform.position.x < -20 || transform.position.z > 20 || transform.position.z < -20)
                gameObject.SetActive(false);
        }


        protected virtual void OnTriggerEnter(Collider other)
        {
            PlayerController controller = other.GetComponent<PlayerController>();

            if (controller)
            {
                if (controller == owner)
                    return;

                controller.Hit(damage);
                gameObject.SetActive(false);
            }


            BaseEnemy enemy = other.GetComponent<BaseEnemy>();

            if (enemy)
            {
                enemy.Hit(damage);
                gameObject.SetActive(false);
            }
        }
    }
}