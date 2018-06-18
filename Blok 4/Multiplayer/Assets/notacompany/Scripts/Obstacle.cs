using Photon;
using UnityEngine;

namespace notacompany
{
    public class Obstacle : PunBehaviour
    {
        private void Update()
        {
            if (PhotonNetwork.isMasterClient)
            {
                if (!GameManagement.instance.IsRunning)
                    return;

                transform.Translate(new Vector3(-1 * Time.deltaTime, 0f, 0f));

                if (transform.position.x <= -5)
                    PhotonNetwork.Destroy(gameObject);
            }
        }
    }
}