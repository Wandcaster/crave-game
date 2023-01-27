using System;
using UnityEngine;
using UnityEngine.SceneManagement;

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
            SceneManager.sceneLoaded += SetCamera;
            Refresh();
        }
        public void SetCamera(Scene scene, LoadSceneMode loadSceneMode)
        {
            mainCamera = Camera.main;
        }

        private void OnValidate() {
            mainCamera = Camera.main;
            Refresh();
        }

        private Vector2 CalculatePosition() {
            var camSize = CameraSize;
            var scale = transform.localScale;
            var bounds = Bounds;
            var horizontalOffset = HorizontalAlignment switch {
                HorizontalAlignment.Left => -camSize.x + bounds.x /** scale.x*/,
                HorizontalAlignment.Center => 0.0f,
                HorizontalAlignment.Right => camSize.x - bounds.x /** scale.x*/,
                _ => throw new ArgumentOutOfRangeException()
            } + Offset.x;
            var verticalOffset = VerticalAlignment switch {
                VerticalAlignment.Bottom => -camSize.y + bounds.y/* * scale.y*/,
                VerticalAlignment.Center => 0.0f,
                VerticalAlignment.Top => camSize.y - bounds.y /** scale.y*/,
                _ => throw new ArgumentOutOfRangeException()
            } + Offset.y;
            return new Vector2(horizontalOffset, verticalOffset);
        }

        private Vector2? camSize;

        private Vector2 Bounds => GetComponent<Collider2D>().bounds.extents;

        private void OnDrawGizmos() {
            var extends = Bounds;
            
            //Gizmos.DrawSphere();
        }

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
