using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UI {
    public class EnemyCupboard : MonoBehaviour {
        // Start is called before the first frame update
        [SerializeField] private GameObject enemyPrefab;
        private List<GameObject> enemies = new();

        // Update is called once per frame
        void Update()
        {
        
        }

        public void AddEnemy(EnemyData data) {
            var obj = Instantiate(enemyPrefab, transform);
            obj.GetComponent<EnemyController>().enemyData = data;
            obj.GetComponent<SpriteRenderer>().sprite = data.appearance;
            obj.AddComponent<BoxCollider2D>();
            enemies.Add(obj);
            AlignEnemies();
        }

        public void RemoveEnemy(ulong id) {
            var i = enemies.FindIndex(it => it.GetComponent<EnemyController>().NetworkObjectId == id);
            Destroy(enemies[i]);
            enemies.RemoveAt(i);
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
