using DAE.BoardSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAE.HexSystem
{
    public interface ICheckPositions<TCard, TPiece> 
        where TPiece: IPiece 
        where TCard: ICard
    {
        bool CanExecute(Board<Hex, TPiece> board, Grid<Hex> grid, TPiece piece, Hex position, CardType card);
        void ExecuteAction(Board<Hex, TPiece> board, Grid<Hex> grid, TPiece piece, Hex position, CardType card);

        List<Hex> ValidPositions(Board<Hex, TPiece> board, Grid<Hex> grid, TPiece piece, Hex position, CardType card);
    }
}