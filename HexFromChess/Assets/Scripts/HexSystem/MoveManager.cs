using DAE.BoardSystem;
using DAE.ChessSystem.Moves;
using DAE.Commons;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DAE.ChessSystem
{
    public class MoveManager<TPiece>
        where TPiece : IPiece
    {

        private MultiValueDictionary<PieceType, IMove<TPiece>> _moves = new MultiValueDictionary<PieceType, IMove<TPiece>>();

        private readonly Board<Position, TPiece> _board;
        private readonly Grid<Position> _grid;

        public MoveManager(Board<Position, TPiece> board, Grid<Position> grid)
        {
            _board = board;
            _grid = grid;

            InitializeMoves();
        }

        public List<Position> ValidPositionFor(TPiece piece)
        {

            return _moves[piece.PieceType]
               .Where(m => m.CanExecute(_board, _grid, piece))
               .SelectMany(m => m.Positions(_board, _grid, piece))
               .ToList();
        }

        public void Move(TPiece piece, Position position)
        {
            _moves[piece.PieceType]
                .Where(m => m.CanExecute(_board, _grid, piece))
                .First(m => m.Positions(_board, _grid, piece).Contains(position))
                .Execute(_board, _grid, piece, position);
        }

        private void InitializeMoves()
        {
            _moves.Add(PieceType.Pawn, new ConfigurableMove<TPiece>(
                (Board<Position, TPiece> b, Grid<Position> g, TPiece p) => new MovementHelper<TPiece>(b, g, p)
                                .North(1, MovementHelper<TPiece>.IsEmptyTile)
                                .NorthEast(1, MovementHelper<TPiece>.HasEnemyPiece)
                                .NorthWest(1, MovementHelper<TPiece>.HasEnemyPiece)
                                .Collect()));


            _moves.Add(PieceType.Pawn, new PawnDoubleMove<TPiece>());

            _moves.Add(PieceType.King, new ConfigurableMove<TPiece>(
                (b, g, p) => new MovementHelper<TPiece>(b, g, p)
                    .North(1)
                    .NorthEast(1)
                    .East(1)
                    .SouthEast(1)
                    .South(1)
                    .SouthWest(1)
                    .West(1)
                    .NorthWest(1)
                    .Collect()));
        }
    }
}
