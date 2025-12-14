using System.Collections.Generic;

using UnityEngine;

using Spawners;

namespace ChickenSnakes.EnemyGroup
{
    public class SpawnEnemies : MonoBehaviour
    {
        #region Serialized Fields


        [SerializeField] private Vector2Int _dimensions;
        [SerializeField] private Vector2 _space;

        [SerializeField] private WeightProcesser<EnemyPort, WeightEntry<EnemyPort>> _oneUnitEnemies;
        [SerializeField] private WeightProcesser<EnemyPort, WeightEntry<EnemyPort>> _fourUnitEnemies;
        [SerializeField] private WeightProcesser<EnemyPort, WeightEntry<EnemyPort>> _nineUnitEnemies;


        #endregion

        #region Private Fields


        private Dictionary<Vector2Int, EnemyPort> _positionsAndEnemies = new Dictionary<Vector2Int, EnemyPort>();


        #endregion

        #region Public Methods


        [ContextMenu("Clear Enemies")]
        public void ClearEnemies()
        {
            foreach (EnemyPort port in _positionsAndEnemies.Values)
            {
                port.DestroyEnemy();
            }

            _positionsAndEnemies.Clear();
        }

        [ContextMenu("Spawn Group")]
        public void SpawnGroup()
        {
            if (_positionsAndEnemies.Count > 0)
            {
                ClearEnemies();
            }

            float horizontalDisplacement = -_dimensions.x / 2 + (_dimensions.x % 2 == 0 ? 0.5f : 0);
            float verticalDisplacement = 0.5f;

            for (int i = 0; i < _dimensions.x; i++)
            {
                for (int j = 0; j < _dimensions.y; j++)
                {
                    Vector2Int pos = new Vector2Int(i, j);

                    EnemyPort spawnedObject = ObjectPoolManager.SpawnObject(_oneUnitEnemies.GetNextEntry(), Vector3.zero, Quaternion.identity);
                    spawnedObject.transform.SetParent(transform);
                    spawnedObject.gameObject.SetActive(false);
                    _positionsAndEnemies.Add(pos, spawnedObject);

                    Vector3 realLocalPosition = (Vector3Int)pos;
                    realLocalPosition.x += horizontalDisplacement;
                    realLocalPosition.y += verticalDisplacement;

                    realLocalPosition.x *= _space.x;
                    realLocalPosition.y *= _space.y;

                    spawnedObject.transform.localPosition = realLocalPosition;
                    spawnedObject.transform.localRotation = Quaternion.Euler(0, 0, 90);
                }
            }
        }


        #endregion
    }
}
