using System.Collections;
using UnityEngine;

namespace alternatereality
{
    public class GameManagement : MonoBehaviour
    {
        public static GameManagement Instance;

        private const float ENEMY_SIZE = 1f;
        private const int ENEMY_ROWS = 5;
        private const int ENEMY_COLUMNS = 5;
        private const float SPACING = 1f;

        [SerializeField] private GameObject _camera;

        private BaseEnemy[,] _enemies;
        private PlayerController[] _players;
        
        private float _delay;
        private int _index;

        private bool _active;

        private void Awake()
        {
            Instance = this;

            _enemies = new BaseEnemy[ENEMY_COLUMNS, ENEMY_ROWS];
            _players = new PlayerController[4];

            _delay = 1f;
            _index = 0;

            _active = true;


            for (int column = 0; column < ENEMY_COLUMNS; column++)
            {
                for (int row = 0; row < ENEMY_ROWS; row++)
                {
                    string path = "ResTankEnemy"; // different enemy for rows/columns would be possible
                    float x = ENEMY_SIZE * column + SPACING * column;
                    float z = ENEMY_SIZE * row + SPACING * row;

                    Vector3 position = new Vector3(x - ENEMY_COLUMNS + 1, 0f, z - ENEMY_ROWS + 1);
                    GameObject enemy = Instantiate(Resources.Load<GameObject>(path), position, Quaternion.identity);

                    _enemies[column, row] = enemy.GetComponent<BaseEnemy>();

                    enemy.SetActive(true);
                }
            }


            for (int index = 0; index < _players.Length; index++)
            {
                GameObject player = Instantiate(Resources.Load<GameObject>("ResPlayerController"));

                _players[index] = player.GetComponent<PlayerController>();
                _players[index].Initialize(index);
            }

            _players[_index].SetActive(true);

            StartCoroutine(Movement());
        }


        private void Update()
        {
            if (!_active)
                return;

            if (Input.GetKeyDown(KeyCode.UpArrow))
                Rotate(-1);
            else if (Input.GetKeyDown(KeyCode.DownArrow))
                Rotate(1);



            _camera.transform.rotation = Quaternion.RotateTowards(_camera.transform.rotation, Quaternion.Euler(0, _index * 90, 0), 750f * Time.deltaTime);
        }


        private void Rotate(int direction)
        {
            _index += direction;

            if (_index < 0)
                _index = 3;
            if (_index > 3)
                _index = 0;

            foreach (PlayerController controller in _players)
                controller.SetActive(false);

            _players[_index].SetActive(true);
        }


        private IEnumerator Movement()
        {
            yield return new WaitForSeconds(_delay);

            foreach (BaseEnemy enemy in _enemies)
            {
                //enemy.Move(0, 1f);
            }

            StartCoroutine(Movement());
        }
    }
}