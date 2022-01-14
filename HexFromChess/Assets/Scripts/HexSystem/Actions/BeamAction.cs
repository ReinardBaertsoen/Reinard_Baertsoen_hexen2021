using DAE.BoardSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DAE.HexSystem
{
    class BeamAction<TCard, TPiece> : ActionBase<TCard, TPiece>
        where TCard : ICard
        where TPiece : IPiece
    {
        public BeamAction(Positiongetter positiongetter) : base(positiongetter) { }
        public override void ExecuteAction(Board<Hex, TPiece> board, Grid<Hex> grid, TPiece piece, Hex position, CardType card)
        {
            foreach (var hex in ValidPositions(board, grid, piece, position, card))
            {
                if (board.TryGetPieceAt(hex, out var enemy))
                {
                    board.Take(enemy);
                }
            }
        }
        public override List<Hex> ValidPositions(Board<Hex, TPiece> board, Grid<Hex> grid, TPiece piece, Hex position, CardType card)
        {
            return base.ValidPositions(board, grid, piece, position, card);
        }
    }
}