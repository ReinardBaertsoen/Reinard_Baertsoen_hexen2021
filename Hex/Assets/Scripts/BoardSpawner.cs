using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardSpawner : MonoBehaviour
{
    [SerializeField] private GameObject Hex;
    private float _hexScale = 0.9f;

    private int _gridRadius = 5;

    void Awake()
    {
        //Instantiate(Hex);
        //for (int j = 0; j < 2; j++)
        //{
        //    for (int i = 0; i < 6; i++)
        //    {
        //        Vector3 position = new Vector3(0, 0, 1.8f * (j + 1));
        //        Vector3 rotatedPosition = Quaternion.Euler(0, i * 60, 0) * position;
        //        Instantiate(Hex, rotatedPosition, Quaternion.identity);
        //    }
        //
        //    if (j > 0) //fillerAmount is only needed if there is place for hexes between the axis'
        //    {
        //        int fillerAmount = 6 * j;
        //        for (int i = 0; i < fillerAmount; i++)
        //        {
        //            Vector3 position = new Vector3(0, 0, 1.8f * (j + 1) - 0.5f);
        //
        //            float offsetAngle = (360 / fillerAmount);
        //
        //            Vector3 rotatedPosition = Quaternion.Euler(0, 30 + (offsetAngle * i), 0) * position;
        //
        //            GameObject hex = Instantiate(Hex, rotatedPosition, Quaternion.identity);
        //            hex.gameObject.name = "Filler" + i;
        //        }
        //    }
        //}
        
        for (int q = -_gridRadius; q <= _gridRadius; q++)
        {
            int rNegative = Mathf.Max(-_gridRadius, -q - _gridRadius);
            int rPositive = Mathf.Min(_gridRadius, -q + _gridRadius);
            for (int r = rNegative; r <= rPositive; r++)
            {
                var s = -q - r;

                var x = (float)(Mathf.Sqrt(3f) * q + Mathf.Sqrt(3f) / 2f * r) * 1f;
                var z = (3f / 2f * r) * 1f;

                var hexShape = Instantiate(Hex, new Vector3(x, 0, z), Quaternion.identity);
                hexShape.transform.localScale *= _hexScale;
                hexShape.name = $"Hex {q},{s},{r}";
            }
        }
    }
}
