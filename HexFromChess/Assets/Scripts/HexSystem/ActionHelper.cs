using DAE.BoardSystem;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DAE.HexSystem
{
    class ActionHelper<TCard, TPiece> where TPiece : IPiece where TCard : ICard
    {
        private Board<Hex, TPiece> _board;
        private Grid<Hex> _grid;
        private TPiece _piece;
        private Hex _position;
        private CardType _card;

        private List<Hex> _validPositions = new List<Hex>();

        public ActionHelper(Board<Hex, TPiece> board, Grid<Hex> grid, TPiece piece, Hex position, CardType card)
        {
            _board = board;
            _piece = piece;
            _grid = grid;
            _position = position;
            _card = card;
        }

        public delegate bool Validator(Board<Hex, TPiece> board, Grid<Hex> grid, TPiece piece, Hex position);

        public ActionHelper<TCard, TPiece> Move(int qOffset, int rOffset, int numTiles = int.MaxValue, params Validator[] validators)
        {
            if (!_board.TryGetPositionOf(_piece, out var position))
                return this;

            if (!_grid.TryGetCoordinateOf(position, out var coordinate))
                return this;

            var nextQCoordinate = coordinate.q + qOffset;
            var nextRCoordinate = coordinate.r + rOffset;

            var hasNextPosition = _grid.TryGetPositionAt(nextQCoordinate, nextRCoordinate, out var nextPosition);
            int step = 0;

            while (hasNextPosition && step < numTiles)
            {
                var isOk = validators.All((v) => v(_board, _grid, _piece, nextPosition));
                if (!isOk)
                    return this;
                
                _validPositions.Add(nextPosition);

                nextQCoordinate += qOffset;
                nextRCoordinate += rOffset;

                hasNextPosition = _grid.TryGetPositionAt(nextQCoordinate, nextRCoordinate, out nextPosition);

                step++;
            }
            return this;
        }
        public ActionHelper<TCard, TPiece> TopLeft(int numTiles = int.MaxValue, params Validator[] validators)
            => Move(0, -1, numTiles, validators);

        public ActionHelper<TCard, TPiece> TopRight(int numTiles = int.MaxValue, params Validator[] validators)
            => Move(1, -1, numTiles, validators);

        public ActionHelper<TCard, TPiece> BottomLeft(int numTiles = int.MaxValue, params Validator[] validators)
            => Move(-1, 1, numTiles, validators);

        public ActionHelper<TCard, TPiece> BottomRight(int numTiles = int.MaxValue, params Validator[] validators)
            => Move(0, 1, numTiles, validators);

        public ActionHelper<TCard, TPiece> Left(int numTiles = int.MaxValue, params Validator[] validators)
            => Move(-1, 0, numTiles, validators);

        public ActionHelper<TCard, TPiece> Right(int numTiles = int.MaxValue, params Validator[] validators)
            => Move(1, 0, numTiles, validators);


        public ActionHelper<TCard, TPiece> IsolatedTopLeft(Hex position, int numTiles = int.MaxValue, params Validator[] validators)
            => IsolatedMove(position, 0, -1, numTiles, validators);
        public ActionHelper<TCard, TPiece> IsolatedTopRight(Hex position, int numTiles = int.MaxValue, params Validator[] validators)
            => IsolatedMove(position, 1, -1, numTiles, validators);
        public ActionHelper<TCard, TPiece> IsolatedBottomLeft(Hex position, int numTiles = int.MaxValue, params Validator[] validators)
            => IsolatedMove(position, -1, 1, numTiles, validators);
        public ActionHelper<TCard, TPiece> IsolatedBottomRight(Hex position, int numTiles = int.MaxValue, params Validator[] validators)
            => IsolatedMove(position, 0, 1, numTiles, validators);
        public ActionHelper<TCard, TPiece> IsolatedLeft(Hex position, int numTiles = int.MaxValue, params Validator[] validators)
            => IsolatedMove(position, -1, 0, numTiles, validators);
        public ActionHelper<TCard, TPiece> IsolatedRight(Hex position, int numTiles = int.MaxValue, params Validator[] validators)
            => IsolatedMove(position, 1, 0, numTiles, validators);

        public List<Hex> Collect()
            => _validPositions;

        public ActionHelper<TCard, TPiece> IsolatedMove(Hex hexposition, int qOffset, int rOffset, int numTiles = int.MaxValue, params Validator[] validators)
        {
            if (!_board.TryGetPositionOf(_piece, out var position))
                return this;

            if (!_grid.TryGetCoordinateOf(position, out var coordinate))
                return this;

            var nextQCoordinate = coordinate.q + qOffset;
            var nextRCoordinate = coordinate.r + rOffset;

            var hasNextPosition = _grid.TryGetPositionAt(nextQCoordinate, nextRCoordinate, out var nextPosition);
            int step = 0;

            List<Hex> temporaryList = new List<Hex>();

            while (hasNextPosition && step < numTiles)
            {
                var isOk = validators.All((v) => v(_board, _grid, _piece, nextPosition));
                if (!isOk)
                    return this;

                temporaryList.Add(nextPosition);

                nextQCoordinate += qOffset;
                nextRCoordinate += rOffset;

                hasNextPosition = _grid.TryGetPositionAt(nextQCoordinate, nextRCoordinate, out nextPosition);

                step++;
            }

            if (temporaryList.Contains(hexposition))
            {
                _validPositions = temporaryList;
            }

            return this;
        }

        public ActionHelper<TCard, TPiece> IsolatedAddedPieceOne(Hex hexposition, int numTiles = int.MaxValue, params Validator[] validators)
        {
            _board.TryGetPositionOf(_piece, out var piecePosition);
            _grid.TryGetCoordinateOf(piecePosition, out var pieceCoordinate);

            _grid.TryGetCoordinateOf(hexposition, out var coordinate);

            var tempQ = coordinate.q - pieceCoordinate.q;
            var tempR = coordinate.r - pieceCoordinate.r;
            var tempS = -tempQ - tempR;

            for (int numTile = 1; numTile <= numTiles; numTile++)
            {
                var signedQ = PosNegZero(tempQ);
                var signedR = PosNegZero(tempR);
                var signedS = PosNegZero(tempS);

                var q1 = -signedR * numTile + pieceCoordinate.q;
                var r1 = -signedS * numTile + pieceCoordinate.r;


                if (_grid.TryGetPositionAt(q1, r1, out var position2) && position2 != piecePosition)
                    _validPositions.Add(position2);
                if (!_grid.TryGetPositionAt(q1, r1, out var position1) || position1 == piecePosition)
                    return this;
            }

            return this;
        }
        public ActionHelper<TCard, TPiece> IsolatedAddedPieceTwo(Hex hexposition, int numTiles = int.MaxValue, params Validator[] validators)
        {
            _board.TryGetPositionOf(_piece, out var piecePosition);
            _grid.TryGetCoordinateOf(piecePosition, out var pieceCoordinate);

            _grid.TryGetCoordinateOf(hexposition, out var coordinate);

            var tempQ = coordinate.q - pieceCoordinate.q;
            var tempR = coordinate.r - pieceCoordinate.r;
            var tempS = -tempQ - tempR;

            for (int numTile = 1; numTile <= numTiles; numTile++)
            {
                var signedQ = PosNegZero(tempQ);
                var signedR = PosNegZero(tempR);
                var signedS = PosNegZero(tempS);

                var q2 = -signedS * numTile + pieceCoordinate.q;
                var r2 = -signedQ * numTile + pieceCoordinate.r;


                if (_grid.TryGetPositionAt(q2, r2, out var position2) && position2 != piecePosition)
                    _validPositions.Add(position2);
                if (!_grid.TryGetPositionAt(q2, r2, out var position1) || position1 == piecePosition)
                    return this;
            }

            return this;
        }
        public int PosNegZero(int number)
        {
            if (number > 0)
                return 1;
            else if (number < 0)
                return -1;
            else if (number == 0)
                return 0;
            else
                return 0;
        }
    }
}