using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DAE.SpawningSystem
{
    public class EnemySpawner : MonoBehaviour
    {
        [SerializeField] private GameObject Enemy;
        private List<Vector3> _usedPositions = new List<Vector3>();

        [SerializeField] private int _amountOfEnemies;

        // Start is called before the first frame update
        void Awake()
        {
            var boardSpawner = FindObjectOfType<BoardSpawner>();
            var radius = boardSpawner._radiusWithoutCenter;

            for (int i = 0; i < _amountOfEnemies; i++)
            {
                int q = Random.Range(-radius, radius);
                int r = Random.Range(-radius, radius);

                while (Mathf.Abs(q + r) > radius)
                {
                    q = Random.Range(-radius, radius);
                    r = Random.Range(-radius, radius);
                }

                var spawnX = (float)(Mathf.Sqrt(3f) * q + Mathf.Sqrt(3f) / 2f * r) * 0.5f;
                var spawnZ = (3f / 2f * r) * 0.5f;

                var spawnPosition = new Vector3(spawnX, 0, spawnZ);

                if (!_usedPositions.Contains(spawnPosition) && spawnPosition != new Vector3(0, 0, 0))
                {
                    Instantiate(Enemy, spawnPosition, Quaternion.identity);
                    _usedPositions.Add(spawnPosition);
                }
                else
                {
                    i -= 1;
                }
            }
        }
    }
}