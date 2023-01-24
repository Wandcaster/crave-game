using System;
using UnityEngine;

namespace UI {
    public class Clickable : MonoBehaviour {
        public event Action<GameObject> onClick;

        private void OnMouseUpAsButton() {
            onClick?.Invoke(gameObject);
            Debug.Log("Clicked this thingy");
        }
    }
}
