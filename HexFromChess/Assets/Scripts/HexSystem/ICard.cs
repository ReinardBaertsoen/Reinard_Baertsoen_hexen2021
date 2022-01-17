using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DAE.HexSystem
{
    public interface ICard
    {
        CardType StoredCardType { get; }
    }

    public enum CardType
    {
        Teleport,
        Slash,
        Beam,
        Push,
        Bomb
    }
}