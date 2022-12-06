using System;
using UnityEngine;

namespace UI {
    public class HandCard : MonoBehaviour {
        public delegate void CardMouseEvent(bool isDown);

        public event CardMouseEvent onMouse;
        
        private void OnMouseDown() {
            Debug.Log("mouse down!");
            onMouse?.Invoke(true);
        }

        private void OnMouseUp() {
            Debug.Log("mouse up!");
            onMouse?.Invoke(true);
        }
    }
}
