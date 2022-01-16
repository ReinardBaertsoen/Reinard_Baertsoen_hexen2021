using DAE.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace DAE.BoardSystem
{
    public class Grid<THex>
    {
        private BidirectionalDictionary<(int q, int r), THex> _positions = new BidirectionalDictionary<(int, int), THex>();
        public bool TryGetPositionAt(int q, int r, out THex position)
            => _positions.TryGetValue((q, r), out position);

        public bool TryGetCoordinateOf(THex position, out (int q, int r) coordinate)
            => _positions.TryGetKey(position, out coordinate);

        public void Register(int q, int r, THex position)
        {
            _positions.Add((q,r), position);
        }
    }
}
