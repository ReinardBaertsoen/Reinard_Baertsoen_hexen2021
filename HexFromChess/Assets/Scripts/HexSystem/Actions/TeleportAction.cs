using DAE.BoardSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DAE.HexSystem
{
    public class TeleportAction<TCard, TPiece> : ActionBase<TCard, TPiece>
        where TCard : ICard
        where TPiece : IPiece
    {
        public TeleportAction(Positiongetter positiongetter): base(positiongetter) { }
        public override void ExecuteAction(Board<Hex, TPiece> board, Grid<Hex> grid, TPiece piece, Hex position, CardType card)
        {
            board.Move(piece, position);
        }
        public override List<Hex> ValidPositions(Board<Hex, TPiece> board, Grid<Hex> grid, TPiece piece, Hex position, CardType card)
        {
            List<Hex> isolatedPositions = new List<Hex>();

            if (!board.TryGetPieceAt(position, out var nextPiece))
                isolatedPositions.Add(position);

            return isolatedPositions;
        }
    }
}

