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

class Piece : MonoBehaviour, IPiece
{
    public void MoveTo(Vector3 toCoordinate)
    {
        gameObject.transform.position = toCoordinate;
    }
    public void Place(Vector3 toCoordinate)
    {
        gameObject.transform.position = toCoordinate;
        gameObject.SetActive(true);
    }
    public void Taken()
    {
        gameObject.SetActive(false);
    }
}

