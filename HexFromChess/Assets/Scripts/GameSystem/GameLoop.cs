using DAE.BoardSystem;
using DAE.HexSystem;
using UnityEngine;
using System;
using System.Collections.Generic;
using DAE.StateSystem;
using DAE.GameSystem.GameStates;

namespace DAE.GameSystem
{
    class GameLoop : MonoBehaviour
    {
        [SerializeField]
        private PositionHelper _positionHelper;

        private Grid<Hex> _grid;
        private Board<Hex, Piece> _board;

        private ActionManager<Piece, Card> _actionManager;

        public GameObject _playerhand;

        public Card _currentCard;

        [SerializeField] private GameObject _playerObject;
        private Piece _playerPiece;

        [SerializeField] private GameObject _beam;
        [SerializeField] private GameObject _teleport;
        [SerializeField] private GameObject _slash;
        [SerializeField] private GameObject _push;
        [SerializeField] private GameObject _bomb;

        [SerializeField] private int _turns = 0;

        private System.Random _random = new System.Random();

        private List<Card> _usedCards = new List<Card>();

        private StateMachine<GameStateBase> _gameStateMachine;

        [SerializeField] Canvas _startScreenState;
        [SerializeField] Canvas _playingState;
        [SerializeField] Canvas _endScreenState;

        private StartScreenState _startScreen;
        private PlayingState _playingScreen;
        private EndScreenState _endScreen;

        public void Start()
        {
            _grid = new Grid<Hex>();
            ConnectGrid(_grid);

            _board = new Board<Hex, Piece>();

            ConnectPiece();

            _actionManager = new ActionManager<Piece, Card>(_board, _grid);

            _playerPiece = _playerObject.gameObject.GetComponent<Piece>();

            BoardListeners();
            CardListeners();

            var cards = FindObjectsOfType<Card>();
            foreach (var card in cards)
            {
                _turns -= 1;
            }

            _gameStateMachine = new StateMachine<GameStateBase>();


            _startScreen = new StartScreenState(_gameStateMachine, _startScreenState, _playingState, _endScreenState);
            _gameStateMachine.Register(GameState.StartScreenState, _startScreen);

            _playingScreen = new PlayingState(_gameStateMachine, _startScreenState, _playingState, _endScreenState);
            _gameStateMachine.Register(GameState.PlayingState, _playingScreen);

            _endScreen = new EndScreenState(_gameStateMachine, _startScreenState, _playingState, _endScreenState);
            _gameStateMachine.Register(GameState.EndScreenState, _endScreen);

            _startScreen.StartScreenState = _startScreenState;
            _startScreen.PlayingState = _playingState;
            _startScreen.EndScreenState = _endScreenState;

            _playingScreen.StartScreenState = _startScreenState;
            _playingScreen.PlayingState = _playingState;
            _playingScreen.EndScreenState = _endScreenState;

            _endScreen.StartScreenState = _startScreenState;
            _endScreen.PlayingState = _playingState;
            _endScreen.EndScreenState = _endScreenState;

            _gameStateMachine.InitialState = GameState.StartScreenState;
        }


        private void ConnectGrid(Grid<Hex> grid)
        {
            var hexes = FindObjectsOfType<Hex>();
            foreach (var hex in hexes)
            {
                hex.Dropped += (s, e) =>
                {
                    var validPositions = _actionManager.ValidPositionFor(_playerPiece, e.Position, _currentCard.StoredCardType);
                    var isolatedPositions = _actionManager.IsolatedPositionFor(_playerPiece, e.Position, _currentCard.StoredCardType);

                    if (isolatedPositions.Contains(e.Position))
                    {
                        _actionManager.Action(_playerPiece, e.Position, _currentCard.StoredCardType);

                        _usedCards.Add(_currentCard);
                        _currentCard.Used();
                        _currentCard = null;
                        DrawCard();
                    }

                    foreach (var position in validPositions)
                    {
                        position.PositionDeactivated(this, e);
                    }

                    foreach (var position in isolatedPositions)
                    {
                        position.PositionDeactivated(this, e);
                    }

                    if (_playerObject.activeSelf == false)
                    {
                        EndGame();
                        foreach (var Card in _usedCards)
                        {
                            Debug.Log($"Used {Card._cardType}");
                        }
                    }
                };

                hex.Entered += (s, e) =>
                {
                    var validPositions = _actionManager.ValidPositionFor(_playerPiece, e.Position, _currentCard.StoredCardType);
                    var isolatedPositions = _actionManager.IsolatedPositionFor(_playerPiece, e.Position, _currentCard.StoredCardType);

                    if (!validPositions.Contains(e.Position))
                    {
                        foreach (var position in validPositions)
                        {
                            position.PositionActivated(this, e);
                        }
                    }

                    if (validPositions.Contains(e.Position))
                    {
                        foreach (var position in isolatedPositions)
                        {
                            position.PositionActivated(this, e);
                        }
                    }
                };

                hex.Exitted += (s, e) =>
                {
                    var validPositions = _actionManager.ValidPositionFor(_playerPiece, e.Position, _currentCard.StoredCardType);
                    var isolatedPositions = _actionManager.IsolatedPositionFor(_playerPiece, e.Position, _currentCard.StoredCardType);

                    foreach (var position in validPositions)
                    {
                        position.PositionDeactivated(this, e);
                    }

                    foreach (var position in isolatedPositions)
                    {
                        position.PositionDeactivated(this, e);
                    }
                };

                var (q, r) = _positionHelper.ToGridPostion(hex.transform.position);

                grid.Register(q, r, hex);

                hex.gameObject.name = $"tile {(int)q}, {(int)r}";
            }

        }

        private void ConnectPiece()
        {
            var pieces = FindObjectsOfType<Piece>();
            foreach (Piece piece in pieces)
            {
                var registerPosition = _positionHelper.ToGridPostion(piece.gameObject.transform.position);
                if (_grid.TryGetPositionAt((int)registerPosition.x, (int)registerPosition.y, out var position))
                {
                    _board.Place(piece, position);
                }
            }
        }

        private void DrawCard()
        {
            if (_turns > 0)
            {
                var cardNumber = _random.Next(0, 5);
                _turns -= 1;
                switch (cardNumber)
                {
                    case 0:
                        var teleport = Instantiate(_teleport, _playerhand.transform);
                        teleport.GetComponent<Card>().BeginDrag += (s, e) =>
                        {
                            _currentCard = e.Card;
                        };
                        return;
                    case 1:
                        var push = Instantiate(_push, _playerhand.transform);
                        push.GetComponent<Card>().BeginDrag += (s, e) =>
                        {
                            _currentCard = e.Card;
                        };
                        return;
                    case 2:
                        var slash = Instantiate(_slash, _playerhand.transform);
                        slash.GetComponent<Card>().BeginDrag += (s, e) =>
                        {
                            _currentCard = e.Card;
                        };
                        return;
                    case 3:
                        var beam = Instantiate(_beam, _playerhand.transform);
                        beam.GetComponent<Card>().BeginDrag += (s, e) =>
                        {
                            _currentCard = e.Card;
                        };
                        return;
                    case 4:
                        var bomb = Instantiate(_bomb, _playerhand.transform);
                        bomb.GetComponent<Card>().BeginDrag += (s, e) =>
                        {
                            _currentCard = e.Card;
                        };
                        return;
                }
            }
        }

        private void BoardListeners()
        {
            _board.Moved += (s, e) =>
            {
                if (_grid.TryGetCoordinateOf(e.ToPosition, out var toCoordinate))
                {
                    var worldPosition = _positionHelper.ToWorldPosition(toCoordinate.q, toCoordinate.r);
                    e.Piece.MoveTo(worldPosition);
                }
            };

            _board.Placed += (s, e) =>
            {
                if (_grid.TryGetCoordinateOf(e.ToPosition, out var toCoordinate))
                {
                    var worldPosition = _positionHelper.ToWorldPosition(toCoordinate.q, toCoordinate.r);
                    e.Piece.Place(worldPosition);
                }
            };

            _board.Taken += (s, e) =>
            {
                e.Piece.Taken();
            };
        }
        private void CardListeners()
        {
            var childCount = _playerhand.transform.childCount;
            for (int i = 0; i < childCount; i++)
            {
                var card = _playerhand.transform.GetChild(i).GetComponent<Card>();
                card.BeginDrag += (s, e) =>
                {
                    _currentCard = e.Card;
                };
            }
        }
        public void StartGame()
        {
            _playingScreen.OnEnter();
        }
        public void EndGame()
        {
            _endScreen.OnEnter();
        }
    }
}
