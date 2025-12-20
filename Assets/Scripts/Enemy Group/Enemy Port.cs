using UnityEngine;
using UnityEngine.Events;

using Spawners;

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
        [Space] public UnityEvent OnDeploy;                // 
        [Space] public UnityEvent OnReturn;                // 
        [Space] public UnityEvent OnDestroy;               // 


        #endregion

        #region Private Fields


        private bool _isOnPort;   // 


        #endregion

        #region Public Methods


        /// <summary>
        /// Changes the enemy on the port.
        /// </summary>
        /// <param name="newEnemy">The new enemy for the port</param>
        public void ChangeEnemyOnPort(GameObject newEnemy)
        {
            _enemyOnPort.transform.SetParent(null);
            _enemyOnPort = newEnemy;
            _enemyOnPort.transform.SetParent(transform);
        }

        //[ContextMenu("Deploy")]
        /// <summary>
        /// Deploys the enemy from the port.
        /// </summary>
        public void Deploy()
        {
            if (!_isOnPort)
            {
                return;
            }

            DeployRaw();

            OnDeploy?.Invoke();
        }

        /// <summary>
        /// Deploys the enemy from the port without any event callbacks.
        /// </summary>
        public void DeployRaw()
        {
            if (!_isOnPort)
            {
                return;
            }

            _enemyOnPort.transform.SetParent(null);
            _isOnPort = false;
        }

        //[ContextMenu("Return")]
        /// <summary>
        /// Returns the enemy to the port.
        /// </summary>
        public void Return()
        {
            if (_isOnPort)
            {
                return;
            }

            ReturnRaw();

            OnReturn?.Invoke();
        }

        /// <summary>
        /// Returns the enemy to the port without any event callbacks.
        /// </summary>
        public void ReturnRaw()
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
        /// Destroys the enemy and the port.
        /// </summary>
        public void Destroy()
        {
            Return();
            ObjectPoolManager.ReturnObjectToPool(gameObject);

            OnDestroy?.Invoke();
        }


        #endregion
    }
}
