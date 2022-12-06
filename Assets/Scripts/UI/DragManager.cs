using UnityEngine;

namespace UI {
    public class DragManager {
        public enum DragStage {
            Begin, Drag, Done
        }
        public delegate void DragPerformed(GameObject draggedObject, Vector2 position, DragStage stage);

        public event DragPerformed onDragPerformed;
        public GameObject draggedObject { get; private set; }
        private GameObject pendingDragged;
        public bool isDragging { get; private set; }
        private Vector2 initialClickPosition;
        private const float deadZone = 0.09f;

        public void BeginDrag(GameObject ob, Vector2 position) {
            pendingDragged = ob;
            initialClickPosition = position;
        }

        public void MoveMouse(Vector2 position) {
            if (pendingDragged == null) return;
            if (isDragging) {
                onDragPerformed?.Invoke(draggedObject, position, DragStage.Drag);
            } else if (Vector2.Distance(initialClickPosition, position) >= deadZone) {
                isDragging = true;
                draggedObject = pendingDragged;
                onDragPerformed?.Invoke(draggedObject, position, DragStage.Begin);
            }

        }

        public void EndDrag(Vector2 position) {
            var draggie = draggedObject;
            draggedObject = null;
            pendingDragged = null;
            if (isDragging && draggie != null) {
                isDragging = false;
                onDragPerformed?.Invoke(draggie, position, DragStage.Done);
            }
        }
    }
}
