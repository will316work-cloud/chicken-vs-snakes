using System.Collections;

using UnityEngine;

namespace ChickenSnakes.Enemy
{
    /// <summary>
    /// 
    /// 
    /// Author: William Min
    /// Date: 12/15/25
    /// </summary>
    public class EnemyHoard : MonoBehaviour
    {
        #region Serialized Fields


        [SerializeField] private EnemyGroupData[] _enemyGroups;
        [SerializeField] private float _timeBetweenCycles = 1f;
        [SerializeField] private Vector2Int _commandDimensions;
        [SerializeField] private bool _willCommand;


        #endregion

        private void Start()
        {
            ToggleCommandCycle(_willCommand);
        }


        #region Public Methods


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
                _enemyGroups[i].SpawnFromComposition();
            }
        }

        public void CommandOnCornerPositions(int groupIndex, Vector2Int bottomLeftCorner, Vector2Int topRightCorner)
        {
            EnemyGroupData enemyGroup = _enemyGroups[groupIndex];

            for (int x = bottomLeftCorner.x; x <= topRightCorner.x; x++)
            {
                for (int y = bottomLeftCorner.y; y <= topRightCorner.y; y++)
                {
                    Vector2Int position = new Vector2Int(x, y);

                    EnemyUnitData unitData = enemyGroup.GetEnemy(position);

                    if (unitData != null)
                    {
                        if (unitData.GetEnemyUnit().GetCommandsOnPort() == null)
                        {
                            unitData.GetEnemyUnit().Setup();
                        }

                        unitData.GetEnemyUnit().GetCommandsOnPort().DoNextCommand();
                    }
                }
            }
        }

        public void CommandOnRectangle(int groupIndex, Vector2Int bottomLeftCorner, Vector2Int dimensions)
        {
            CommandOnCornerPositions(groupIndex, bottomLeftCorner, new Vector2Int(bottomLeftCorner.x + dimensions.x - 1, bottomLeftCorner.y + dimensions.y - 1));
        }

        public void ToggleCommandCycle(bool willCommand)
        {
            _willCommand = willCommand;

            if (_willCommand)
            {
                StartCoroutine(_commandCycles());
            }
        }

        [ContextMenu("Toggle Cycle On")]
        public void ToggleCycleOn()
        {
            ToggleCommandCycle(true);
        }

        [ContextMenu("Toggle Cycle Off")]
        public void ToggleCycleOff()
        {
            ToggleCommandCycle(false);
        }


        private IEnumerator _commandCycles()
        {
            while (_willCommand)
            {
                int groupIndex = Random.Range(0, 4);
                EnemyGroupData enemyGroup = _enemyGroups[groupIndex];
                (Vector2Int, Vector2Int) bounds = enemyGroup.GetBounds();

                int randomX = Random.Range(bounds.Item1.x, bounds.Item2.x - _commandDimensions.x + 2);
                int randomY = Random.Range(bounds.Item1.y, bounds.Item2.y - _commandDimensions.y + 2);
                Vector2Int bottomLeftCorner = new Vector2Int(randomX, randomY);

                //Debug.Log($"{bottomLeftCorner} | {_deployDimensions}");

                CommandOnRectangle(groupIndex, bottomLeftCorner, _commandDimensions);

                yield return new WaitForSeconds(_timeBetweenCycles);
            }
        }


        #endregion
    }
}
