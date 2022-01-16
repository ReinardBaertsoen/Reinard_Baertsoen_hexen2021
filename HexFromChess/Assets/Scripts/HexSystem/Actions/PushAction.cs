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
            List<TPiece> enemies = new List<TPiece>();

            var validPositions = ValidPositions(board, grid, piece, position, card);
            validPositions.Reverse();

            foreach (var hex in validPositions)
            {
                if (board.TryGetPieceAt(hex, out var enemy))
                {
                    board.TryGetPositionOf(enemy, out var enemyPosition);
                    grid.TryGetCoordinateOf(enemyPosition, out var enemyCoordinate);

                    board.TryGetPositionOf(piece, out var playerPosition);
                    grid.TryGetCoordinateOf(playerPosition, out var playerCoordinate);

                    var q = enemyCoordinate.q - playerCoordinate.q;
                    var r = enemyCoordinate.r - playerCoordinate.r;
                    var s = -q - r;

                    q += q + playerCoordinate.q;
                    r += r + playerCoordinate.r;

                    grid.TryGetPositionAt(q, r, out var nextEnemyPosition);

                    if (!grid.TryGetPositionAt(q, r, out _))
                    {
                        board.Take(enemy);
                    }
                    else
                    {
                        board.Move(enemy, nextEnemyPosition);
                        enemies.Add(enemy);
                    }
                }
            }
        }
        public override List<Hex> ValidPositions(Board<Hex, TPiece> board, Grid<Hex> grid, TPiece piece, Hex position, CardType card)
        {
            return base.ValidPositions(board, grid, piece, position, card);
        }
    }
}