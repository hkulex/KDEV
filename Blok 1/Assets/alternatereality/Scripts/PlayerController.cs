using UnityEngine;

namespace alternatereality
{
    public class PlayerController : MonoBehaviour, IHittable
    {
        private int _health;
        private int _index;
        private bool _active;
        private Vector3 _axis;
        private float _velocity;


        public void Initialize(int index)
        {
            _index = index;
            _active = false;
            _velocity = 5f;

            switch (index)
            {
                case 0:
                    _axis = new Vector3(1, -1, 0);
                    transform.position = new Vector3(0, 0, -10);
                    break;

                case 1:
                    _axis = new Vector3(0, -1, 1);
                    transform.position = new Vector3(-10, 0, 0);
                    break;

                case 2:
                    _axis = new Vector3(1, 1, 0);
                    transform.position = new Vector3(0, 0, 10);
                    break;

                case 3:
                    _axis = new Vector3(0, 1, 1);
                    transform.position = new Vector3(10, 0, 0);
                    break;
            }
        }


        private void Update()
        {
            Move();
            Shoot();
        }


        private void Move()
        {
            if (!_active)
                return;

            transform.Translate(new Vector3(_axis.x, 0, _axis.z) * Input.GetAxisRaw("Horizontal") * _velocity * Time.deltaTime);
        }


        private void Shoot()
        {
            if (!Input.GetKeyDown(KeyCode.Space) || !_active)
                return;

            GameObject projectile = PoolManagement.Instance.Get();

            if (projectile)
            {
                projectile.SetActive(true);
                projectile.transform.position = transform.position;
                projectile.GetComponent<BaseProjectile>().Initialize(_axis, this);
            }
        }


        public void SetActive(bool value)
        {
            _active = value;
        }


        public void Hit(int damage)
        {
            _health -= damage;

            if (_health <= 0)
            {

            }
        }


        public void Death()
        {

        }
    }
}