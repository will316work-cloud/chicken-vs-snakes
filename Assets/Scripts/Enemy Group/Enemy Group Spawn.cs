using System.Collections.Generic;

using UnityEngine;

namespace ChickenSnakes.EnemyGroup
{
    [CreateAssetMenu(fileName = "Enemy Group Spawn Template", menuName = "Chicken Snakes Scriptables/Enemy Group Spawn")]
    public class EnemyGroupSpawn : ScriptableObject
    {
        [SerializeField] private GameObject _enemyPrefab;

        public Dictionary<GameObject, Vector3Int> GenerateEnemies()
        {
            return null;
        }
    }
}
