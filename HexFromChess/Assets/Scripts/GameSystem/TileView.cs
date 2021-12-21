using DAE.HexSystem;
using DAE.GameSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class TileEventArgs : EventArgs
{
    public Tile Tile { get; }

    public TileEventArgs(Tile tile)
    {
        Tile = tile;
    }
}

public class TileView : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    private UnityEvent OnActivate;

    [SerializeField]
    private UnityEvent OnDeactivate;

    public event EventHandler<TileEventArgs> Clicked;

    //[SerializeField]
    //private GameLoop _loop;

    private Tile _model;

    public Tile Model
    {
        set
        {
            if (_model != null)
            {
                _model.Activated -= PositionActivated;
                _model.Deactivated -= PositionDeactivated;
            }

            _model = value;

            if (_model != null)
            {
                _model.Activated += PositionActivated;
                _model.Deactivated += PositionDeactivated;
            }

        }
        get
        {
            return _model;
        }
    }

    private void PositionDeactivated(object sender, EventArgs e)
        => OnDeactivate.Invoke();

    private void PositionActivated(object sender, EventArgs e)
        => OnActivate.Invoke();

    public void OnPointerClick(PointerEventData eventData)
        => OnClicked(new TileEventArgs(Model));

    protected virtual void OnClicked(TileEventArgs eventArgs)
    {
        var handler = Clicked;
        handler?.Invoke(this, eventArgs);
    }
}
