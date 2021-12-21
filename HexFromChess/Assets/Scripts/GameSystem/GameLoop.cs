using DAE.BoardSystem;
using DAE.HexSystem;
using DAE.SelectionSystem;
using UnityEngine;

namespace DAE.GameSystem
{
    class GameLoop : MonoBehaviour
    {
        [SerializeField]
        private PositionHelper _positionHelper;

        internal void DebugPosition(TileView tile)
        {
            var (x,y) = _positionHelper.ToGridPostion(_grid, _boardParent, tile.transform.position);
            Debug.Log($"Value of Tile {tile.name} is X: {x} and Y: {y}");
        }

        [SerializeField]
        private Transform _boardParent;

        private SelectionManager<Piece> _selectionManager;
        private Grid<TileView> _grid;
        private Board<TileView, Piece> _board;

        public void Start()
        {
            _grid = new Grid<TileView>(7, 7);
            ConnectGrid(_grid);

            _board = new Board<TileView, Piece>();

            _selectionManager = new SelectionManager<Piece>();

            ConnectPiece(_selectionManager);

            _selectionManager.Selected += (s, e) =>
            {
                e.SelectableItem.Highlight = true;
            };

            _selectionManager.Deselected += (s, e) =>
            {
                e.SelectableItem.Highlight = false;
            };
        }



        public void DeselectAll()
        {
            _selectionManager.DeselectAll();
        }

        private void ConnectGrid(Grid<TileView> grid)
        {
            var tiles = FindObjectsOfType<TileView>();
            foreach (var tile in tiles)
            {
                var (q, r) = _positionHelper.ToGridPostion(grid, _boardParent, tile.transform.position);
                
                grid.Register(q, r, tile);
            }
            
        }

        private void ConnectPiece(SelectionManager<Piece> selectionManager)
        {
            var pieces = FindObjectsOfType<Piece>();
            foreach (var piece in pieces)
                piece.Clicked += (s, e) => selectionManager.Toggle(s as Piece);
        }
    }
}
