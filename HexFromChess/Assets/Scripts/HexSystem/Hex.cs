using DAE.HexSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace DAE.HexSystem
{
    public class PositionEventArgs : EventArgs
    {
        public Hex Position { get; }

        public PositionEventArgs(Hex position)
        {
            Position = position;
        }
    }

    public class Hex : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
    {
        private PositionModel _model;

        public PositionModel Model
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



        public event EventHandler<PositionEventArgs> Dropped;
        public event EventHandler<PositionEventArgs> Entered;
        public event EventHandler<PositionEventArgs> Exitted;

        [SerializeField]
        private UnityEvent OnActivate;

        [SerializeField]
        private UnityEvent OnDeactivate;

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (eventData.pointerDrag == null)
                return;

            OnEntered(new PositionEventArgs(this));
        }
        public void OnPointerExit(PointerEventData eventData)
        {
            if (eventData.pointerDrag == null)
                return;

            OnExitted(new PositionEventArgs(this));
        }
        public void OnDrop(PointerEventData eventData)
        {
            if (eventData.pointerDrag == null)
                return;

            OnDropped(new PositionEventArgs(this));
        }

        protected virtual void OnDropped(PositionEventArgs eventArgs)
        {
            var handler = Dropped;
            handler?.Invoke(this, eventArgs);
        }
        protected virtual void OnEntered(PositionEventArgs eventArgs)
        {
            var handler = Entered;
            handler?.Invoke(this, eventArgs);
        }
        protected virtual void OnExitted(PositionEventArgs eventArgs)
        {
            var handler = Exitted;
            handler?.Invoke(this, eventArgs);
        }

        public void PositionDeactivated(object sender, EventArgs e)
            => OnDeactivate.Invoke();

        public void PositionActivated(object sender, EventArgs e)
            => OnActivate.Invoke();
    }
}
