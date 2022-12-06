using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UI {
    public class EnemyCupboard : MonoBehaviour {
        // Start is called before the first frame update
        [SerializeField] private GameObject enemyPrefab;
        private List<GameObject> enemies = new();
        void Start()
        {
            for (int i = 0; i < 2; ++i) {
                AddEnemy();
            }
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        private void AddEnemy() {
            var obj = Instantiate(enemyPrefab, transform);
            obj.AddComponent<BoxCollider2D>();
            enemies.Add(obj);
            AlignEnemies();
        }

        private void AlignEnemies() {
            var totalWidth = enemies.Select(e => e.GetComponent<BoxCollider2D>().size.x * e.transform.localScale.x).Sum();
            var currentX = -totalWidth / 2;
            foreach (var enemy in enemies) {
                enemy.transform.position = new Vector3(currentX, 0, 0);
                currentX += enemy.GetComponent<BoxCollider2D>().size.x * enemy.transform.localScale.x;
            }
        }
    }
}
