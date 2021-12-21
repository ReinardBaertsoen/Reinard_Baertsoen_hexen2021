using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace DAE.BoardSystem
{
    public class Grid<TPosition>
    {

        public int Rows { get; }

        public int Columns { get; }

        public Grid(int rows, int columns)
        {
            Rows = rows;
            Columns = columns;
        }

        private Dictionary<(int x, int y), TPosition> _positions = new Dictionary<(int, int), TPosition>();
        public bool TryGetPositionAt(int x, int y, out TPosition position)
            => _positions.TryGetValue((x, y), out position);            
        

        public void Register(int x, int y, TPosition position)
        {
            _positions.Add((x,y), position);
        }
    }
}
