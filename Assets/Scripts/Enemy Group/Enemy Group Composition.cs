using UnityEngine;

using Spawners;

namespace ChickenSnakes.Enemy
{
    /// <summary>
    /// 
    /// 
    /// Author: William Min
    /// Date: 12/16/25
    /// </summary>
    [CreateAssetMenu(fileName = "New Enemy Group Composition", menuName = "Chicken VS Snakes Scriptables/Enemy Group Composition")]
    public class EnemyGroupComposition : ScriptableObject
    {
        #region Serialized Fields


        [Header("Group Position Properties")]
        [SerializeField] private Vector2Int _dimensions;
        [SerializeField] private Vector2 _displacement;

        [Header("Group Spawn Compositions")]
        [SerializeField] private EnemySpawnRoll[] _spawnRolls;


        #endregion

        #region Public Methods


        public void FillGroupWithSpawns(EnemyGroupData groupData)
        {
            bool[][] _occupiedSpot = new bool[_dimensions.x][];

            // Grid Setup

            for (int x = 0; x < _dimensions.x; x++)
            {
                if (_occupiedSpot[x] == null)
                {
                    _occupiedSpot[x] = new bool[_dimensions.y];
                }

                /*
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
                */
            }

            foreach (EnemySpawnRoll roll in _spawnRolls)
            {
                for (int x = 0; x < _dimensions.x; x++)
                {
                    for (int y = 0; y < _dimensions.y; y++)
                    {
                        if (roll.CanFitInSpace(_occupiedSpot, x, y) && roll.SuccessfulChance())
                        {
                            Vector2Int lowerBound = new Vector2Int(x, y);
                            Vector2Int upperBound = roll.GetUpperEnemyBound(x, y);
                            EnemyPort spawnedObject = ObjectPoolManager.SpawnObject(roll.GetSpawnPrefabRoll(), Vector3.zero, Quaternion.identity);

                            groupData.AddEnemy(spawnedObject, lowerBound, upperBound);

                            roll.OccupySpace(_occupiedSpot, x, y);
                        }
                    }
                }
            }

            float horizontalDisplacement = -_dimensions.x / 2 + (_dimensions.x % 2 == 0 ? 0.5f : 0) + _displacement.x;
            float verticalDisplacement = _displacement.y;

            groupData.UpdatePositions(horizontalDisplacement, verticalDisplacement);
        }


        #endregion
    }

    [System.Serializable]
    public class EnemySpawnRoll
    {
        [SerializeField] private float _spawnChance = 1f;
        [SerializeField] private Vector2Int _sizeDimensions;
        [SerializeField] private WeightProcesser<EnemyPort, WeightEntry<EnemyPort>> _rolls;

        public bool SuccessfulChance()
        {
            return Random.Range(0f, 1f) <= _spawnChance;
        }

        public bool CanFitInSpace(bool[][] occupiedSpaces, int startX, int startY)
        {
            if (startX < 0 || startY < 0)
            {
                Debug.LogWarning($"Cannot check occupied space in ({startX}, {startY}).");
                return false;
            }

            int maxXBound = _sizeDimensions.x + startX;
            int xGridLength = occupiedSpaces.Length;

            if (maxXBound > xGridLength)
            {
                Debug.LogWarning($"{maxXBound} > {xGridLength}. Horizontal dimension of enemy exceeds position grid by {maxXBound - xGridLength}.");
                return false;
            }

            int maxYBound = _sizeDimensions.y + startY;
            int yGridLength = occupiedSpaces[startX].Length;

            if (maxYBound > yGridLength)
            {
                Debug.LogWarning($"{maxXBound} > {yGridLength}. Vertical dimension of enemy exceeds position grid by {maxXBound - yGridLength}.");
                return false;
            }

            if (occupiedSpaces[startX][startY])
            {
                return false;
            }

            for (int x = startX; x < maxXBound; x++)
            {
                for (int y = startY; y < maxYBound; y++)
                {
                    if (occupiedSpaces[x][y])
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        public bool OccupySpace(bool[][] occupiedSpaces, int startX, int startY)
        {
            if (startX < 0 || startY < 0)
            {
                Debug.LogWarning($"Cannot check occupied space in ({startX}, {startY}).");
                return false;
            }

            int maxXBound = _sizeDimensions.x + startX;
            int xGridLength = occupiedSpaces.Length;

            if (maxXBound > xGridLength)
            {
                Debug.LogWarning($"{maxXBound} > {xGridLength}. Horizontal dimension of enemy exceeds position grid by {maxXBound - xGridLength + 1}.");
                return false;
            }

            int maxYBound = _sizeDimensions.y + startY;
            int yGridLength = occupiedSpaces[startX].Length;

            if (maxYBound > yGridLength)
            {
                Debug.LogWarning($"{maxXBound} > {yGridLength}. Vertical dimension of enemy exceeds position grid by {maxXBound - yGridLength}.");
                return false;
            }

            for (int x = startX; x < maxXBound; x++)
            {
                for (int y = startY; y < maxYBound; y++)
                {
                    occupiedSpaces[x][y] = true;
                }
            }

            return true;
        }

        public Vector2Int GetUpperEnemyBound(int startX, int startY)
        {
            return new Vector2Int(startX + _sizeDimensions.x - 1, startY + _sizeDimensions.y - 1);
        }

        public EnemyPort GetSpawnPrefabRoll()
        {
            return _rolls.GetNextEntry();
        }
    }
}
