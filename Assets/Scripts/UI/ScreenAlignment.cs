using System;
using UnityEngine;

namespace UI {
    public class ScreenAlignment : MonoBehaviour {
        // ReSharper disable once UnusedAutoPropertyAccessor.Local
        [field: SerializeField] public VerticalAlignment VerticalAlignment { get; private set; }
        // ReSharper disable once UnusedAutoPropertyAccessor.Local
        [field: SerializeField] public HorizontalAlignment HorizontalAlignment { get; private set; }
        [field: SerializeField] public Vector2 Offset { get; private set; }

        public void Refresh() {
            transform.position = CalculatePosition();
        }

        private void FixedUpdate() {
            Refresh();
        }

        private void Awake() {
            mainCamera = Camera.main;
            Refresh();
        }

        private void OnValidate() {
            mainCamera = Camera.main;
            Refresh();
        }

        private Vector2 CalculatePosition() {
            var camSize = CameraSize;
            var bounds = Bounds;
            var horizontalOffset = HorizontalAlignment switch {
                HorizontalAlignment.Left => -camSize.x + bounds.x,
                HorizontalAlignment.Center => 0.0f,
                HorizontalAlignment.Right => camSize.x - bounds.x,
                _ => throw new ArgumentOutOfRangeException()
            } + Offset.x;
            var verticalOffset = VerticalAlignment switch {
                VerticalAlignment.Bottom => -camSize.y + bounds.y,
                VerticalAlignment.Center => 0.0f,
                VerticalAlignment.Top => camSize.y - bounds.y,
                _ => throw new ArgumentOutOfRangeException()
            } + Offset.y;
            return new Vector2(horizontalOffset, verticalOffset);
        }

        private Vector2? camSize;

        private Vector2 Bounds => GetComponent<Collider2D>().bounds.extents;

        private Camera mainCamera;

        private Vector2 CameraSize {
            get {
                var camHeight = mainCamera.orthographicSize;
                var camWidth = camHeight * mainCamera.aspect;
                return new Vector2(camWidth, camHeight);
            }
        }
    }
}
