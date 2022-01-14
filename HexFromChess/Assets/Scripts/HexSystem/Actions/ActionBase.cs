using DAE.BoardSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DAE.HexSystem
{
    public class ActionBase<TCard, TPiece> : ICheckPositions<TCard, TPiece>
        where TCard : ICard
        where TPiece : IPiece
    {
        public delegate List<Hex> Positiongetter(Board<Hex, TPiece> board, Grid<Hex> grid, TPiece piece, Hex position, CardType card);
        Positiongetter _positiongetter;

        public ActionBase(Positiongetter positiongetter)
        {
            _positiongetter = positiongetter;
        }

        public bool CanExecute(Board<Hex, TPiece> board, Grid<Hex> grid, TPiece piece, Hex position, CardType card)
        {
            return true;
        }

        public virtual void ExecuteAction(Board<Hex, TPiece> board, Grid<Hex> grid, TPiece piece, Hex position, CardType card)
        {

        }

        public virtual List<Hex> ValidPositions(Board<Hex, TPiece> board, Grid<Hex> grid, TPiece piece, Hex position, CardType card)
        {
            return _positiongetter(board, grid, piece, position, card);
        }
    }
}