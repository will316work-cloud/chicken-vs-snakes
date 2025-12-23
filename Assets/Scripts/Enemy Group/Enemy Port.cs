using UnityEngine;

using ChickenSnakes.Entities;
using ChickenSnakes.Managers;

namespace ChickenSnakes.Enemy
{
    /// <summary>
    /// Location that an enemy will be perched on.
    /// Will launch from and return to during movement attacks.
    /// 
    /// Author: William Min
    /// Date: 12/13/25
    /// </summary>
    [System.Serializable]
    public class EnemyPort : MonoBehaviour
    {
        #region Serialized Fields


        [SerializeField] private GameObject _enemyOnPort;   // 


        #endregion

        #region Private Fields


        private bool _isOnPort = true;              // 
        private Entity _entityReference;            //
        private CommandManager _commandCollection;  // 


        #endregion

        #region MonoBehavior Callbacks


        private void Awake()
        {
            Setup();
        }


        #endregion

        #region Public Methods


        /// <summary>
        /// Sets up entity.
        /// </summary>
        public void Setup()
        {
            ChangeEnemyOnPort(_enemyOnPort);
        }

        /// <summary>
        /// Changes the enemy on the port.
        /// </summary>
        /// <param name="newEnemy">The new enemy for the port</param>
        public void ChangeEnemyOnPort(GameObject newEnemy)
        {
            if (_enemyOnPort != null)
            {
                _enemyOnPort.transform.SetParent(null);
            }

            _entityReference = null;
            _commandCollection = null;

            _enemyOnPort = newEnemy;

            if (_enemyOnPort != null)
            {
                _enemyOnPort.transform.SetParent(transform);
                _entityReference = _enemyOnPort.GetComponentInChildren<Entity>();
                _commandCollection = GetComponentInChildren<CommandManager>();
            }
        }

        [ContextMenu("Deploy")]
        /// <summary>
        /// Deploys the enemy from the port.
        /// </summary>
        public void Deploy()
        {
            if (!_isOnPort)
            {
                return;
            }

            _enemyOnPort.transform.SetParent(null);
            _isOnPort = false;
        }

        [ContextMenu("Return")]
        /// <summary>
        /// Returns the enemy to the port.
        /// </summary>
        public void Return()
        {
            if (_isOnPort)
            {
                return;
            }

            _enemyOnPort.transform.SetParent(transform);

            _enemyOnPort.transform.localPosition = Vector3.zero;
            _enemyOnPort.transform.localRotation = Quaternion.identity;

            _isOnPort = true;
        }

        /// <summary>
        /// Returns the entity component from the enemy in the port.
        /// </summary>
        /// <returns>Entity component from the enemy in the port</returns>
        public Entity GetEntityOnPort()
        {
            return _entityReference;
        }

        /// <summary>
        /// Returns the commmand manager component from the enemy in the port.
        /// </summary>
        /// <returns>Command Manager from the enemy in the port</returns>
        public CommandManager GetCommandsOnPort()
        {
            return _commandCollection;
        }


        #endregion
    }
}
