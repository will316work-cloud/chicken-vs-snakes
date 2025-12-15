using UnityEngine;
using UnityEngine.Events;

using Spawners;

namespace ChickenSnakes.EnemyGroup
{
    /// <summary>
    /// 
    /// 
    /// Author: William Min
    /// Date: 12/13/25
    /// </summary>
    [System.Serializable]
    public class EnemyPort : MonoBehaviour
    {
        #region Serialized Fields


        [SerializeField] private GameObject _enemyOnPort;   // 
        [Space] public UnityEvent _onDeploy;                // 
        [Space] public UnityEvent _onReturn;                // 
        [Space] public UnityEvent _onDestroy;               // 


        #endregion


        #region Public Methods


        /// <summary>
        /// 
        /// </summary>
        /// <param name="newEnemy"></param>
        public void ChangeEnemyOnPort(GameObject newEnemy)
        {
            _enemyOnPort.transform.SetParent(null);
            _enemyOnPort = newEnemy;
            _enemyOnPort.transform.SetParent(transform);
        }

        /// <summary>
        /// 
        /// </summary>
        [ContextMenu("Deploy")]
        public void Deploy()
        {
            _enemyOnPort.transform.SetParent(null);

            _onDeploy?.Invoke();
        }

        /// <summary>
        /// 
        /// </summary>
        [ContextMenu("Return")]
        public void Return()
        {
            _enemyOnPort.transform.SetParent(transform);

            _enemyOnPort.transform.localPosition = Vector3.zero;
            _enemyOnPort.transform.localRotation = Quaternion.identity;

            _onReturn?.Invoke();
        }

        /// <summary>
        /// 
        /// </summary>
        public void DestroyEnemy()
        {
            Return();
            ObjectPoolManager.ReturnObjectToPool(gameObject);

            _onDestroy?.Invoke();
        }


        #endregion
    }
}
