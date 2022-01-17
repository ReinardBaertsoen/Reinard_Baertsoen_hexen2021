using DAE.BoardSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DAE.HexSystem
{
    class BombAction<TCard, TPiece> : ActionBase<TCard, TPiece>
            where TCard : ICard
            where TPiece : IPiece
    {
        public BombAction(Positiongetter positiongetter) : base(positiongetter) { }

        public override void ExecuteAction(Board<Hex, TPiece> board, Grid<Hex> grid, TPiece piece, Hex position, CardType card)
        {
            foreach (var hex in ValidPositions(board, grid, piece, position, card))
            {
                hex.gameObject.SetActive(false);
                if (board.TryGetPieceAt(hex, out var anyPiece))
                {
                    board.Take(anyPiece);
                }
            }
        }

        public override List<Hex> ValidPositions(Board<Hex, TPiece> board, Grid<Hex> grid, TPiece piece, Hex position, CardType card)
        {
            List<Hex> isolatedPositions = new List<Hex>();

            board.TryGetPositionOf(piece, out var piecePosition);
            grid.TryGetCoordinateOf(piecePosition, out var pieceCoordinate);

            grid.TryGetCoordinateOf(position, out var coordinate);

            isolatedPositions.Add(position);

            if (grid.TryGetPositionAt(coordinate.q + 1, coordinate.r - 1, out var position1))
                isolatedPositions.Add(position1);

            if (grid.TryGetPositionAt(coordinate.q - 1, coordinate.r, out var position2))
                isolatedPositions.Add(position2);

            if (grid.TryGetPositionAt(coordinate.q + 1, coordinate.r, out var position3))
                isolatedPositions.Add(position3);

            if (grid.TryGetPositionAt(coordinate.q - 1, coordinate.r + 1, out var position4))
                isolatedPositions.Add(position4);

            if (grid.TryGetPositionAt(coordinate.q, coordinate.r + 1, out var position5))
                isolatedPositions.Add(position5);

            if (grid.TryGetPositionAt(coordinate.q, coordinate.r - 1, out var position6))
                isolatedPositions.Add(position6);

            return isolatedPositions;
        }
    }
}
