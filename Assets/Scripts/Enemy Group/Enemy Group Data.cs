using System.Collections.Generic;

using UnityEngine;

namespace ChickenSnakes.Enemy
{
    /// <summary>
    /// 
    /// 
    /// Author: William Min
    /// Date: 12/15/25
    /// </summary>
    [System.Serializable]
    public class EnemyGroupData
    {
        #region Serialized Fields


        [SerializeField] private Transform _groupTransform;
        [SerializeField] private EnemyGroupComposition _composition;
        [SerializeField] private Vector2 _space;


        #endregion

        #region Private Fields


        private HashSet<EnemyUnitData> _enemyUnits;
        private Vector2 _displacement;


        #endregion

        #region Public Methods



        public void Setup()
        {
            if (_enemyUnits == null)
            {
                _enemyUnits = new HashSet<EnemyUnitData>();
            }
        }

        public void UpdatePositions(float horizontalDisplacement, float verticalDisplacement)
        {
            Setup();

            _displacement = new Vector2(horizontalDisplacement, verticalDisplacement);

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
            Setup();

            foreach (EnemyUnitData data in _enemyUnits)
            {
                if (data.WithinCallbackBounds(position))
                {
                    return data;
                }
            }

            return null;
        }

        public void AddEnemy(EnemyPort enemyUnit, Vector2Int lowCallbackBound, Vector2Int highCallbackBound)
        {
            AddEnemyData(new EnemyUnitData(enemyUnit, lowCallbackBound, highCallbackBound));
        }

        public void AddEnemyData(EnemyUnitData newData)
        {
            Setup();

            newData.OnDestroy += (EnemyUnitData data) => RemoveEnemyData(data);
            _enemyUnits.Add(newData);
        }

        public bool RemoveEnemy(Vector2Int position)
        {
            Setup();

            foreach (EnemyUnitData data in _enemyUnits)
            {
                if (data.WithinCallbackBounds(position))
                {
                    _enemyUnits.Remove(data);
                    return true;
                }
            }

            return false;
        }

        public bool RemoveEnemyData(EnemyUnitData data)
        {
            Setup();

            return _enemyUnits.Remove(data);
        }

        public void ClearGroup()
        {
            Setup();

            List<EnemyUnitData> enemyList = new List<EnemyUnitData>(_enemyUnits);

            foreach (EnemyUnitData data in enemyList)
            {
                data.GetEnemyUnit().GetEntityOnPort().DoDeath();
            }

            enemyList.Clear();
        }

        public void SpawnFromComposition(EnemyGroupComposition composition)
        {
            if (_composition != composition)
            {
                _composition = composition;
            }

            _composition.FillGroupWithSpawns(this);
        }

        public void SpawnFromComposition()
        {
            SpawnFromComposition(_composition);
        }

        public (Vector2Int, Vector2Int) GetBounds()
        {
            Setup();

            Vector2Int minBounds = Vector2Int.zero;
            Vector2Int maxBounds = Vector2Int.zero;

            bool gotFirstPositions = false;

            foreach (EnemyUnitData data in _enemyUnits)
            {
                Vector2Int lowBounds = Vector2Int.Min(data.LowPositionCallback, data.HighPositionCallback);
                Vector2Int highBounds = Vector2Int.Max(data.LowPositionCallback, data.HighPositionCallback);

                if (gotFirstPositions)
                {
                    minBounds = lowBounds;
                    maxBounds = highBounds;
                }
                else
                {
                    minBounds = Vector2Int.Min(minBounds, lowBounds);
                    maxBounds = Vector2Int.Max(maxBounds, highBounds);
                }
            }

            return (minBounds, maxBounds);
        }


        #endregion
    }
}
