using System;
using System.Collections.Generic;

using UnityEngine;

using Spawners;

namespace ChickenSnakes.EnemyGroup
{
    public class EnemyHoard : MonoBehaviour
    {
        #region Serialized Fields


        [SerializeField] private Vector2Int _dimensions;
        [SerializeField] private Vector2 _displacement;

        [SerializeField] private WeightProcesser<EnemyPort, WeightEntry<EnemyPort>> _enemies;
        [SerializeField] private EnemyGroupData[] _enemyGroups;


        #endregion

        [ContextMenu("Clear Enemies")]
        public void ResetEnemies()
        {
            foreach (EnemyGroupData data in _enemyGroups)
            {
                data.ClearGroup();
            }
        }

        [ContextMenu("Spawn Group")]
        public void SpawnGroup()
        {
            ResetEnemies();

            for (int i = 0; i < _enemyGroups.Length; i++)
            {
                EnemyGroupData groupData = _enemyGroups[i];

                bool[][] _occupiedSpot = new bool[_dimensions.x][];

                // Grid Setup
                for (int x = 0; x < _dimensions.x; x++)
                {
                    if (_occupiedSpot[x] == null)
                    {
                        _occupiedSpot[x] = new bool[_dimensions.y];
                    }

                    // Create Enemy Units

                    for (int y = 0; y < _dimensions.y; y++)
                    {
                        if (!_occupiedSpot[x][y])
                        {
                            Vector2Int pos = new Vector2Int(x, y);

                            EnemyPort spawnedObject = ObjectPoolManager.SpawnObject(_enemies.GetNextEntry(), Vector3.zero, Quaternion.identity);
                            groupData.AddEnemy(spawnedObject, pos, pos);

                            _occupiedSpot[x][y] = true;
                        }
                    }
                }

                float horizontalDisplacement = -_dimensions.x / 2 + (_dimensions.x % 2 == 0 ? 0.5f : 0) + _displacement.x;
                float verticalDisplacement = _displacement.y;

                groupData.UpdatePositions(horizontalDisplacement, verticalDisplacement);
            }

            /*
            for (int i = 0; i < _enemyGroupParents.Length; i++)
            {
                Transform groupTransform = _enemyGroupParents[i];

                bool[][] _occupiedSpot = new bool[_dimensions.x][];

                // Grid Setup
                for (int x = 0; x < _dimensions.x; x++)
                {
                    if (_occupiedSpot[x] == null)
                    {
                        _occupiedSpot[x] = new bool[_dimensions.y];
                    }

                    /*
                    for (int y = 0; y < _dimensions.y; y++)
                    {
                        _occupiedSpot[x][y]
                    }
                }


                // Create Enemy Units

                for (int x = 0; x < _dimensions.x; x++)
                {
                    for (int y = 0; y < _dimensions.y; y++)
                    {
                        if (!_occupiedSpot[x][y])
                        {
                            Vector2Int pos = new Vector2Int(x, y);

                            EnemyPort spawnedObject = ObjectPoolManager.SpawnObject(_enemies.GetNextEntry(), Vector3.zero, Quaternion.identity);
                            spawnedObject.transform.SetParent(groupTransform);
                            //spawnedObject.gameObject.SetActive(false);
                            _positionsAndEnemies.Add(pos, spawnedObject);
                        }
                    }
                }



                for (int i = 0; i < _dimensions.x; i++)
                {
                    for (int j = 0; j < _dimensions.y; j++)
                    {
                        Vector2Int pos = new Vector2Int(i, j);

                        EnemyPort spawnedObject = ObjectPoolManager.SpawnObject(_enemies.GetNextEntry(), Vector3.zero, Quaternion.identity);
                        spawnedObject.transform.SetParent(transform);
                        //spawnedObject.gameObject.SetActive(false);
                        _positionsAndEnemies.Add(pos, spawnedObject);
                    }
                }

                foreach (EnemyUnitData data in _enemyUnits)
                {
                    float horizontalDisplacement = -_dimensions.x / 2 + (_dimensions.x % 2 == 0 ? 0.5f : 0);
                    float verticalDisplacement = 0.5f;

                    Vector3 realLocalPosition = data.CenterPosition;
                    realLocalPosition.x += horizontalDisplacement;
                    realLocalPosition.y += verticalDisplacement;

                    realLocalPosition.x *= _space.x;
                    realLocalPosition.y *= _space.y;

                    EnemyPort enemyUnit = data.GetEnemyUnit();

                    enemyUnit.transform.localPosition = realLocalPosition;
                    enemyUnit.transform.localRotation = Quaternion.Euler(0, 0, 90);
                }
            }
            */
        }
    }

    public class EnemyUnitData : IEquatable<EnemyUnitData>
    {
        private EnemyPort _enemyUnit;
        private Vector2Int _lowPositionCallback;
        private Vector2Int _highPositionCallback;

        public Vector2 CenterPosition { get => Vector2.Lerp(_lowPositionCallback, _highPositionCallback, .5f); }

        public EnemyUnitData(EnemyPort unit, Vector2Int lowPosition, Vector2Int highPosition)
        {
            _enemyUnit = unit;
            _lowPositionCallback = lowPosition;
            _highPositionCallback = highPosition;
        }

        public bool WithinCallbackBounds(Vector2Int positionCallback)
        {
            int minX = Mathf.Min(_lowPositionCallback.x, _highPositionCallback.x);
            int maxX = Mathf.Max(_lowPositionCallback.x, _highPositionCallback.x);
            int minY = Mathf.Min(_lowPositionCallback.y, _highPositionCallback.y);
            int maxY = Mathf.Max(_lowPositionCallback.y, _highPositionCallback.y);

            return positionCallback.x >= minX && positionCallback.x <= maxX && positionCallback.y >= minY && positionCallback.y <= maxY;
        }

        public EnemyPort GetEnemyUnit()
        {
            return _enemyUnit;
        }

        public bool Equals(EnemyUnitData other)
        {
            return _enemyUnit == other._enemyUnit;
        }

        public override int GetHashCode()
        {
            return _enemyUnit.GetHashCode();
        }
    }

    [Serializable]
    public class EnemyGroupData
    {
        [SerializeField] private Transform _groupTransform;
        [SerializeField] private Vector2 _space;

        private HashSet<EnemyUnitData> _enemyUnits;
        private Vector2 _displacement;

        public void UpdatePositions(float horizontalDisplacement, float verticalDisplacement)
        {
            _displacement = new Vector2(horizontalDisplacement, verticalDisplacement);

            if (_enemyUnits == null)
            {
                _enemyUnits = new HashSet<EnemyUnitData>();
            }

            foreach (EnemyUnitData data in _enemyUnits)
            {
                Vector3 realLocalPosition = data.CenterPosition;
                realLocalPosition.x += horizontalDisplacement;
                realLocalPosition.y += verticalDisplacement;

                realLocalPosition.x *= _space.x;
                realLocalPosition.y *= _space.y;

                EnemyPort enemyUnit = data.GetEnemyUnit();

                enemyUnit.transform.SetParent(_groupTransform);
                enemyUnit.transform.localPosition = realLocalPosition;
                enemyUnit.transform.localRotation = Quaternion.Euler(0, 0, 90);
            }
        }

        public void UpdatePositions()
        {
            UpdatePositions(_displacement.x, _displacement.y);
        }

        public EnemyUnitData GetEnemy(Vector2Int position)
        {
            if (_enemyUnits == null)
            {
                _enemyUnits = new HashSet<EnemyUnitData>();
            }

            foreach (EnemyUnitData data in _enemyUnits)
            {
                if (data.WithinCallbackBounds(position))
                {
                    return data;
                }
            }

            return default(EnemyUnitData);
        }

        public void AddEnemy(EnemyPort enemyUnit, Vector2Int lowCallbackBound, Vector2Int highCallbackBound)
        {
            if (_enemyUnits == null)
            {
                _enemyUnits = new HashSet<EnemyUnitData>();
            }

            _enemyUnits.Add(new EnemyUnitData(enemyUnit, lowCallbackBound, highCallbackBound));
        }

        public bool RemoveEnemy(Vector2Int position)
        {
            if (_enemyUnits == null)
            {
                _enemyUnits = new HashSet<EnemyUnitData>();
            }

            foreach (EnemyUnitData data in _enemyUnits)
            {
                if (data.WithinCallbackBounds(position))
                {
                    _enemyUnits.Remove(data);
                    return true;
                }
            }

            return true;
        }

        public void ClearGroup()
        {
            if (_enemyUnits == null)
            {
                _enemyUnits = new HashSet<EnemyUnitData>();
            }

            foreach (EnemyUnitData data in _enemyUnits)
            {
                data.GetEnemyUnit().DestroyEnemy();
            }

            _enemyUnits.Clear();
        }
    }
}
