using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardSpawner : MonoBehaviour
{
    [SerializeField] private GameObject Hex;
    private float _hexScale = 0.95f;
    private int _gridRadius = 3;

    void Awake()
    {
        for (int q = -_gridRadius; q <= _gridRadius; q++)
        {
            int rNegative = Mathf.Max(-_gridRadius, -q - _gridRadius);
            int rPositive = Mathf.Min(_gridRadius, -q + _gridRadius);
            for (int r = rNegative; r <= rPositive; r++)
            {
                var s = -q - r;

                var spawnX = (float)(Mathf.Sqrt(3f) * q + Mathf.Sqrt(3f) / 2f * r) * 0.5f;
                var spawnZ = (3f / 2f * r) * 0.5f;

                var spawnPosition = new Vector3(spawnX, 0, spawnZ);

                var hexShape = Instantiate(Hex, spawnPosition, Quaternion.identity);
                hexShape.transform.localScale *= _hexScale;
                hexShape.name = $"Hex {q},{s},{r}";
            }
        }
    }
}
