using DAE.BoardSystem;
using DAE.HexSystem;
using System;
using UnityEngine;

namespace DAE.GameSystem
{

    [CreateAssetMenu(menuName = "DAE/Position Helper")]
    public class PositionHelper : ScriptableObject
    {

        private void OnValidate()
        {
            if (_tileDimension <= 0)
                _tileDimension = 1;
        }

        [SerializeField]
        private float _tileDimension = 1;

        public (int x, int y) ToGridPostion(Vector3 worldPosition)
        {
            var q = (Mathf.Sqrt(3f) / 3 * worldPosition.x - 1f / 3f * worldPosition.z) / 0.5f;
            var r = (2f / 3f * worldPosition.z) / 0.5f;

            int x = (int)Mathf.Round(q);
            int y = (int)Mathf.Round(r);

            return (x, y);
        }

        public Vector3 ToWorldPosition(int q, int r)
        {
            var worldPosition = Vector3.zero;

            worldPosition.x = (float)(Mathf.Sqrt(3f) * q + Mathf.Sqrt(3f) / 2f * r) * 0.5f;
            worldPosition.z = (3f / 2f * r) * 0.5f;

            return worldPosition;
        }
    }
}
