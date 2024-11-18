using System;
using UnityEngine.UIElements;

namespace CrashKonijn.Goap.Editor
{
    public class Card : VisualElement
    {
        public Card(Action<Card> callback)
        {
            this.name = "card";
            callback(this);
        }
    }
}