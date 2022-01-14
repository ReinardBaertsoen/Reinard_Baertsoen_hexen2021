using DAE.Commons;
using System;

namespace DAE.BoardSystem
{
    public class PlacedEventArgs<THex, TPiece> : EventArgs
    {
        public THex ToPosition { get; }

        public TPiece Piece { get; }

        public PlacedEventArgs(THex toPosition, TPiece piece)
        {
            ToPosition = toPosition;
            Piece = piece;
        }
    }

    public class MovedEventArgs<THex, TPiece> : EventArgs
    {
        public THex ToPosition { get; }
        public THex FromPosition { get; }
        public TPiece Piece { get; }

        public MovedEventArgs(THex toPosition, THex fromPosition, TPiece piece)
        {
            ToPosition = toPosition;
            FromPosition = fromPosition;
            Piece = piece;
        }
    }

    public class TakenEventArgs<THex, TPiece> : EventArgs
    {
        public THex FromPosition { get; }
        public TPiece Piece { get; }

        public TakenEventArgs(THex fromPosition, TPiece piece)
        {
            FromPosition = fromPosition;
            Piece = piece;
        }
    }


    public class Board<THex, TPiece> 
    {

        public event EventHandler<PlacedEventArgs<THex, TPiece>> Placed;
        public event EventHandler<MovedEventArgs<THex, TPiece>> Moved;
        public event EventHandler<TakenEventArgs<THex, TPiece>> Taken;

        private BidirectionalDictionary<THex, TPiece> _positionPiece = new BidirectionalDictionary<THex, TPiece>();


        public bool Place(TPiece piece, THex toPosition)
        {
            if (TryGetPieceAt(toPosition, out _))
                return false;

            if (TryGetPositionOf(piece, out _))
                return false;

            _positionPiece.Add(toPosition, piece);
            OnPlaced(new PlacedEventArgs<THex, TPiece>(toPosition, piece));

            return true;
        }

        public bool Move(TPiece piece, THex toPosition)
        {
            if (TryGetPieceAt(toPosition, out _))
                return false;

            if (!TryGetPositionOf(piece, out var fromPosition) || !_positionPiece.Remove(piece))
                return false;
            
            _positionPiece.Add(toPosition, piece);
            OnMoved(new MovedEventArgs<THex, TPiece>(toPosition, fromPosition, piece));

            return true;
            
        }

        public bool Take(TPiece piece)
        {
            if (!TryGetPositionOf(piece, out var fromPosition))
                return false;

            if (!_positionPiece.Remove(piece))
                return false;
            
            OnTaken(new TakenEventArgs<THex, TPiece>(fromPosition, piece));
            return true;
            
        }


        public bool TryGetPieceAt(THex position, out TPiece piece)
            =>  _positionPiece.TryGetValue(position, out piece);

        public bool TryGetPositionOf(TPiece piece, out THex position)
            => _positionPiece.TryGetKey(piece, out position);


        #region EventTriggers
        protected virtual void OnPlaced(PlacedEventArgs<THex, TPiece> eventArgs)
        {
            var handler = Placed;
            handler?.Invoke(this, eventArgs);
        }

        protected virtual void OnMoved(MovedEventArgs<THex, TPiece> eventArgs)
        {
            var handler = Moved;
            handler?.Invoke(this, eventArgs);
        }

        protected virtual void OnTaken(TakenEventArgs<THex, TPiece> eventArgs)
        {
            var handler = Taken;
            handler?.Invoke(this, eventArgs);
        } 
        #endregion

    }
}