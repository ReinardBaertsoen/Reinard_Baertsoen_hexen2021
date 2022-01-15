using DAE.BoardSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DAE.HexSystem
{
    public class PushAction<TCard, TPiece> : ActionBase<TCard, TPiece>
        where TCard : ICard
        where TPiece : IPiece
    {
        public PushAction(Positiongetter positiongetter) : base(positiongetter) { }

        public override void ExecuteAction(Board<Hex, TPiece> board, Grid<Hex> grid, TPiece piece, Hex position, CardType card)
        {
            foreach (var hex in ValidPositions(board, grid, piece, position, card))
            {
                if (board.TryGetPieceAt(hex, out var enemy))
                {
                    board.TryGetPositionOf(enemy, out var enemyPosition);
                    grid.TryGetCoordinateOf(enemyPosition, out var enemyCoordinate);

                    board.TryGetPositionOf(piece, out var playerPosition);
                    grid.TryGetCoordinateOf(playerPosition, out var playerCoordinate);

                    var q = enemyCoordinate.x - playerCoordinate.x;
                    var r = enemyCoordinate.y - playerCoordinate.y;

                    q += q + playerCoordinate.x;
                    r += r + playerCoordinate.y;

                    grid.TryGetPositionAt(q, r, out var nextEnemyPosition);

                    if (!grid.TryGetPositionAt(q, r, out var deathPosition))
                    {
                        board.Take(enemy);
                    }
                    else
                        board.Move(enemy, nextEnemyPosition);
                }
            }
        }
        public override List<Hex> ValidPositions(Board<Hex, TPiece> board, Grid<Hex> grid, TPiece piece, Hex position, CardType card)
        {
            return base.ValidPositions(board, grid, piece, position, card);
        }
    }
}