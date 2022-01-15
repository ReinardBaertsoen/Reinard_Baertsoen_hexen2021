using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DAE.SpawningSystem
{
    public class BoardSpawner : MonoBehaviour
    {
        [SerializeField] private GameObject Hex;
        private float _hexScale = 0.95f;
        [SerializeField] public int _radiusWithoutCenter = 3;

        void Awake()
        {
            AdjustCameraDistance();

            var boardParent = new GameObject("BoardParent");
            for (int q = -_radiusWithoutCenter; q <= _radiusWithoutCenter; q++)
            {
                int rNegative = Mathf.Max(-_radiusWithoutCenter, -q - _radiusWithoutCenter);
                int rPositive = Mathf.Min(_radiusWithoutCenter, -q + _radiusWithoutCenter);
                for (int r = rNegative; r <= rPositive; r++)
                {
                    var s = -q - r;

                    var spawnX = (float)(Mathf.Sqrt(3f) * q + Mathf.Sqrt(3f) / 2f * r) * 0.5f;
                    var spawnZ = (3f / 2f * r) * 0.5f;

                    var spawnPosition = new Vector3(spawnX, 0, spawnZ);

                    var hexShape = Instantiate(Hex, spawnPosition, Quaternion.identity, boardParent.transform);
                    hexShape.transform.localScale *= _hexScale;
                }
            }
        }

        private void AdjustCameraDistance()
        {
            var minCameraHeight = 10;
            var cameraHeight = 2.5f * _radiusWithoutCenter;
            cameraHeight = Mathf.Max(cameraHeight, minCameraHeight);

            FindObjectOfType<Camera>().transform.position = new Vector3(0, cameraHeight, -cameraHeight / (2.25f * 2.5f));
        }
    }
}