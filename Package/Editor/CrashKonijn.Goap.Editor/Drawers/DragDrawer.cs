using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace CrashKonijn.Goap.Editor
{
    public class DragDrawer : PointerManipulator
    {
        private readonly Action<Vector2> callback;

        private bool enabled;
        private Vector2 offset = Vector2.zero;
        private Vector2 delta = Vector2.zero;

        public DragDrawer(VisualElement target, Action<Vector2> callback)
        {
            this.callback = callback;
            this.target = target;
        }

        protected override void RegisterCallbacksOnTarget()
        {
            this.target.RegisterCallback<PointerDownEvent>(this.PointerDownHandler);
            this.target.RegisterCallback<PointerMoveEvent>(this.PointerMoveHandler);
            this.target.RegisterCallback<PointerUpEvent>(this.PointerUpHandler);
            this.target.RegisterCallback<PointerCaptureOutEvent>(this.PointerCaptureOutHandler);
            this.target.RegisterCallback<PointerLeaveEvent>(this.PointerLeveHandler);
        }

        protected override void UnregisterCallbacksFromTarget()
        {
            this.target.UnregisterCallback<PointerDownEvent>(this.PointerDownHandler);
            this.target.UnregisterCallback<PointerMoveEvent>(this.PointerMoveHandler);
            this.target.UnregisterCallback<PointerUpEvent>(this.PointerUpHandler);
            this.target.UnregisterCallback<PointerCaptureOutEvent>(this.PointerCaptureOutHandler);
        }

        private void PointerDownHandler(PointerDownEvent evt)
        {
            this.enabled = true;
        }

        private void PointerMoveHandler(PointerMoveEvent evt)
        {
            if (!this.enabled)
                return;

            this.delta += (Vector2) evt.deltaPosition;
            this.callback(this.offset + this.delta);
        }

        private void PointerUpHandler(PointerUpEvent evt)
        {
            if (!this.enabled)
                return;

            this.enabled = false;
            this.offset += this.delta;
            this.delta = Vector2.zero;
        }

        private void PointerLeveHandler(PointerLeaveEvent evt)
        {
            if (!this.enabled)
                return;

            this.enabled = false;
            this.offset += this.delta;
            this.delta = Vector2.zero;
        }

        private void PointerCaptureOutHandler(PointerCaptureOutEvent evt)
        {
            if (!this.enabled)
                return;

            this.enabled = false;
            this.offset += this.delta;
            this.delta = Vector2.zero;
        }

        public void Reset()
        {
            this.enabled = false;
            this.offset = Vector2.zero;
            this.delta = Vector2.zero;

            this.callback(Vector2.zero);
        }
    }
}
