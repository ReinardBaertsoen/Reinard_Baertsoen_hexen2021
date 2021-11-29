using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    [SerializeField] GameObject Player;

    int _spawnQ = 0;
    int _spawnR = 0;

    // Start is called before the first frame update
    void Start()
    {
        var spawnX = (float)(Mathf.Sqrt(3f) * _spawnQ + Mathf.Sqrt(3f) / 2f * _spawnR) * 1f;
        var spawnZ = (3f / 2f * _spawnR) * 1f;

        Instantiate(Player, new Vector3(spawnX, 0, spawnZ), Quaternion.identity);
    }
}
