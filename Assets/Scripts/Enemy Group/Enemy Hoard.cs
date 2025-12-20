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
        [SerializeField] private Vector2Int _deployDimensions;
        [SerializeField] private bool _willDeploy;


        #endregion

        private void Start()
        {
            ToggleDeployCycle(_willDeploy);
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

        public void DeployOnCornerPositions(int groupIndex, Vector2Int bottomLeftCorner, Vector2Int topRightCorner)
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
                        unitData.GetEnemyUnit().Deploy();
                        //Debug.Log("Deployed");
                    }
                }
            }
        }

        public void DeployOnRectangle(int groupIndex, Vector2Int bottomLeftCorner, Vector2Int dimensions)
        {
            DeployOnCornerPositions(groupIndex, bottomLeftCorner, new Vector2Int(bottomLeftCorner.x + dimensions.x - 1, bottomLeftCorner.y + dimensions.y - 1));
        }

        public void ToggleDeployCycle(bool willDeploy)
        {
            _willDeploy = willDeploy;

            if (_willDeploy)
            {
                StartCoroutine(_deployCycles());
            }
        }

        [ContextMenu("Toggle Cycle On")]
        public void ToggleDeployCycleOn()
        {
            ToggleDeployCycle(true);
        }

        [ContextMenu("Toggle Cycle Off")]
        public void ToggleDeployCycleOff()
        {
            ToggleDeployCycle(false);
        }


        private IEnumerator _deployCycles()
        {
            while (_willDeploy)
            {
                int groupIndex = Random.Range(0, 4);
                EnemyGroupData enemyGroup = _enemyGroups[groupIndex];
                (Vector2Int, Vector2Int) bounds = enemyGroup.GetBounds();

                int randomX = Random.Range(bounds.Item1.x, bounds.Item2.x - _deployDimensions.x + 2);
                int randomY = Random.Range(bounds.Item1.y, bounds.Item2.y - _deployDimensions.y + 2);
                Vector2Int bottomLeftCorner = new Vector2Int(randomX, randomY);

                //Debug.Log($"{bottomLeftCorner} | {_deployDimensions}");

                DeployOnRectangle(groupIndex, bottomLeftCorner, _deployDimensions);

                yield return new WaitForSeconds(_timeBetweenCycles);
            }
        }


        #endregion
    }
}
