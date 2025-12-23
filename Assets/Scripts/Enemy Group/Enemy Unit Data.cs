using System;

using UnityEngine;

namespace ChickenSnakes.Enemy
{
    /// <summary>
    /// 
    /// 
    /// Author: William Min
    /// Date: 12/15/25
    /// </summary>
    public class EnemyUnitData : IEquatable<EnemyUnitData>
    {
        #region Serialized Fields


        private EnemyPort _enemyUnit;
        private Vector2Int _lowPositionCallback;
        private Vector2Int _highPositionCallback;


        #endregion

        #region Public Fields


        public Action<EnemyUnitData> OnDestroy;


        #endregion

        #region Properties


        public Vector2Int LowPositionCallback { get => _lowPositionCallback; }
        public Vector2Int HighPositionCallback { get => _highPositionCallback; }
        public Vector2 CenterPosition { get => Vector2.Lerp(_lowPositionCallback, _highPositionCallback, .5f); }


        #endregion

        #region Constructors


        public EnemyUnitData(EnemyPort unit, Vector2Int lowPosition, Vector2Int highPosition)
        {
            _enemyUnit = unit;

            if (unit.GetEntityOnPort() == null)
            {
                unit.Setup();
            }

            unit.GetEntityOnPort().OnDeath.AddListener((gameObject) => Destroy());

            _lowPositionCallback = lowPosition;
            _highPositionCallback = highPosition;
        }


        #endregion

        #region IEquatable Callbacks


        public bool Equals(EnemyUnitData other)
        {
            return _enemyUnit == other._enemyUnit;
        }

        public override int GetHashCode()
        {
            return _enemyUnit.GetHashCode();
        }


        #endregion

        #region Public Methods


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

        public void Destroy()
        {
            OnDestroy?.Invoke(this);
        }


        #endregion
    }
}
