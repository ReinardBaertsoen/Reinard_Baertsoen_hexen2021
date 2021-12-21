using DAE.HexSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;


[Serializable]
public class HighlightEvent : UnityEvent<bool> {}

class PieceEventArgs : EventArgs
{
    public Piece Piece { get; }

    public PieceEventArgs(Piece piece)
        => Piece = piece;
}

class Piece : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    private HighlightEvent OnHighlight;

    public bool Highlight
    {
        set
        {
            OnHighlight.Invoke(value);
        }
    }

    public event EventHandler<PieceEventArgs> Clicked;

    public void OnPointerClick(PointerEventData eventData)
        => OnClicked(this, new PieceEventArgs(this));
    

    protected virtual void  OnClicked(object source, PieceEventArgs e)
    {
        var handle = Clicked;
        Debug.Log("clicked");
        handle?.Invoke(this, e);
    }

    public override string ToString()
    {
        return gameObject.name;
    }
}

