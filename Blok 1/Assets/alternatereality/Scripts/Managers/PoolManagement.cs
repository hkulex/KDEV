using System.Collections.Generic;
using UnityEngine;

namespace alternatereality
{
    public class PoolManagement : MonoBehaviour
    {
        private const int AMOUNT_IN_POOL = 10;

        public static PoolManagement Instance;

        private List<GameObject> _pool;


        private void Awake()
        {
            Instance = this;

            _pool = new List<GameObject>();

            for (int i = 0; i < AMOUNT_IN_POOL; i++)
            {
                GameObject go = Instantiate(Resources.Load<GameObject>("ResBaseProjectile"));
                go.SetActive(false);

                _pool.Add(go);
            }
        }


        public GameObject Get()
        {
            foreach (GameObject go in _pool)
                if (!go.activeInHierarchy)
                    return go;

            return null;
        }
    }
}