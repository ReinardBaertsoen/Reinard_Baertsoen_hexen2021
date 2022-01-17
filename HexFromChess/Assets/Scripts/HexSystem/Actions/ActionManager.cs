using DAE.BoardSystem;
using DAE.Commons;
using DAE.HexSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DAE.HexSystem
{
    public class ActionManager<TPiece, TCard>
        where TPiece : IPiece where TCard : ICard
    {

        private MultiValueDictionary<CardType, ICheckPositions<TCard, TPiece>> _actions = new MultiValueDictionary<CardType, ICheckPositions<TCard, TPiece>>();
        private MultiValueDictionary<CardType, ICheckPositions<TCard, TPiece>> _isolatedAcions = new MultiValueDictionary<CardType, ICheckPositions<TCard, TPiece>>();

        private readonly Board<Hex, TPiece> _board;
        private readonly Grid<Hex> _grid;

        public ActionManager(Board<Hex, TPiece> board, Grid<Hex> grid)
        {
            _board = board;
            _grid = grid;

            InitializeMoves();
            IsolatedMoves();
        }

        public List<Hex> ValidPositionFor(TPiece piece, Hex position, CardType cardType)
        {

            return _actions[cardType]
               .Where(m => m.CanExecute(_board, _grid, piece, position, cardType))
               .SelectMany(m => m.ValidPositions(_board, _grid, piece, position, cardType))
               .ToList();
        }

        public List<Hex> IsolatedPositionFor(TPiece piece, Hex position, CardType cardType)
        {

            return _isolatedAcions[cardType]
               .Where(m => m.CanExecute(_board, _grid, piece, position, cardType))
               .SelectMany(m => m.ValidPositions(_board, _grid, piece, position, cardType))
               .ToList();
        }

        public void Action(TPiece piece, Hex position, CardType cardType)
        {
            _isolatedAcions[cardType]
                .Where(m => m.CanExecute(_board, _grid, piece, position, cardType))
                .First(m => m.ValidPositions(_board, _grid, piece, position, cardType).Contains(position))
                .ExecuteAction(_board, _grid, piece, position, cardType);
        }

        private void InitializeMoves()
        {
            _actions.Add(CardType.Beam, new BeamAction<TCard, TPiece>(
                (b, g, pi, po, c) => new ActionHelper<TCard, TPiece>(b, g, pi, po, c)
                .TopLeft()
                .TopRight()
                .BottomLeft()
                .BottomRight()
                .Left()
                .Right()
                .Collect()));
            _actions.Add(CardType.Push, new PushAction<TCard, TPiece>(
                (b, g, pi, po, c) => new ActionHelper<TCard, TPiece>(b, g, pi, po, c)
                .TopLeft(1)
                .TopRight(1)
                .BottomLeft(1)
                .BottomRight(1)
                .Left(1)
                .Right(1)
                .Collect()));
            _actions.Add(CardType.Slash, new SlashAction<TCard, TPiece>(
                (b, g, pi, po, c) => new ActionHelper<TCard, TPiece>(b, g, pi, po, c)
                .TopLeft(1)
                .TopRight(1)
                .BottomLeft(1)
                .BottomRight(1)
                .Left(1)
                .Right(1)
                .Collect()));
            _actions.Add(CardType.Teleport, new TeleportAction<TCard, TPiece>(
                (b, g, pi, po, c) => new ActionHelper<TCard, TPiece>(b, g, pi, po, c)
                .Collect()));
            _actions.Add(CardType.Bomb, new BombAction<TCard, TPiece>(
                (b, g, pi, po, c) => new ActionHelper<TCard, TPiece>(b, g, pi, po, c)
                .Collect()));
        }

        private void IsolatedMoves()
        {
            _isolatedAcions.Add(CardType.Beam, new BeamAction<TCard, TPiece>(
                (b, g, pi, po, c) => new ActionHelper<TCard, TPiece>(b, g, pi, po, c)
                .IsolatedTopLeft(po)
                .IsolatedTopRight(po)
                .IsolatedBottomLeft(po)
                .IsolatedBottomRight(po)
                .IsolatedLeft(po)
                .IsolatedRight(po)
                .Collect()));
            _isolatedAcions.Add(CardType.Push, new PushAction<TCard, TPiece>(
                (b, g, pi, po, c) => new ActionHelper<TCard, TPiece>(b, g, pi, po, c)
                .IsolatedTopLeft(po, 1)
                .IsolatedTopRight(po, 1)
                .IsolatedBottomLeft(po, 1)
                .IsolatedBottomRight(po, 1)
                .IsolatedLeft(po, 1)
                .IsolatedRight(po, 1)
                .IsolatedAddedPieceOne(po, 1)
                .IsolatedAddedPieceTwo(po, 1)
                .Collect()));
            _isolatedAcions.Add(CardType.Slash, new SlashAction<TCard, TPiece>(
                (b, g, pi, po, c) => new ActionHelper<TCard, TPiece>(b, g, pi, po, c)
                .IsolatedTopLeft(po, 1)
                .IsolatedTopRight(po, 1)
                .IsolatedBottomLeft(po, 1)
                .IsolatedBottomRight(po, 1)
                .IsolatedLeft(po, 1)
                .IsolatedRight(po, 1)
                .IsolatedAddedPieceOne(po, 1)
                .IsolatedAddedPieceTwo(po, 1)
                .Collect()));
            _isolatedAcions.Add(CardType.Teleport, new TeleportAction<TCard, TPiece>(
                (b, g, pi, po, c) => new ActionHelper<TCard, TPiece>(b, g, pi, po, c)
                .IsolatedTopLeft(po, 0)
                .IsolatedTopRight(po, 0)
                .IsolatedBottomLeft(po, 0)
                .IsolatedBottomRight(po, 0)
                .IsolatedLeft(po, 0)
                .IsolatedRight(po, 0)
                .Collect()));
            _isolatedAcions.Add(CardType.Bomb, new BombAction<TCard, TPiece>(
                (b, g, pi, po, c) => new ActionHelper<TCard, TPiece>(b, g, pi, po, c)
                .Collect()));
        }
    }
}
