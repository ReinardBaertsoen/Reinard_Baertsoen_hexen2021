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
        private BidirectionalDictionary<(int x, int y), THex> _positions = new BidirectionalDictionary<(int, int), THex>();
        public bool TryGetPositionAt(int x, int y, out THex position)
            => _positions.TryGetValue((x, y), out position);

        public bool TryGetCoordinateOf(THex position, out (int x, int y) coordinate)
            => _positions.TryGetKey(position, out coordinate);

        public void Register(int x, int y, THex position)
        {
            _positions.Add((x,y), position);
        }
    }
}
