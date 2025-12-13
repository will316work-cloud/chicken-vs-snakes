using UnityEngine;
using UnityEngine.Events;

namespace ChickenSnakes.EnemyGroup
{
    /// <summary>
    /// 
    /// 
    /// Author: William Min
    /// Date: 12/13/25
    /// </summary>
    public class EnemyPort : MonoBehaviour
    {
        #region Serialized Fields


        [SerializeField] private GameObject _enemyOnPort;
        [Space] public UnityEvent _onDeploy;
        [Space] public UnityEvent _onReturn;


        #endregion


        #region Public Methods


        public void ChangeEnemyOnPort(GameObject newEnemy)
        {
            _enemyOnPort.transform.SetParent(null);
            _enemyOnPort = newEnemy;
            _enemyOnPort.transform.SetParent(transform);
        }

        [ContextMenu("Deploy")]
        public void Deploy()
        {
            _enemyOnPort.transform.SetParent(null);

            _onDeploy?.Invoke();
        }

        [ContextMenu("Return")]
        public void Return()
        {
            _enemyOnPort.transform.SetParent(transform);

            _enemyOnPort.transform.localPosition = Vector3.zero;
            _enemyOnPort.transform.localRotation = Quaternion.identity;

            _onReturn?.Invoke();
        }


        #endregion
    }
}
