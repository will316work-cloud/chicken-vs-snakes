using UnityEngine;

namespace ChickenSnakes.Movement
{
    /// <summary>
    /// 
    /// 
    /// Author: William Min
    /// Date: 12/13/25
    /// </summary>
    public class EntityMovement : MonoBehaviour
    {
        #region Serialized Fields


        [SerializeField] private float _defaultMoveSpeed = 10f; // 


        #endregion

        #region Private Fields


        private Vector3 _currentVelocity;   // 
        private Vector3 _currentTorque;     // 


        #endregion

        #region MonoBehavior Callbacks


        private void FixedUpdate()
        {
            if (_currentVelocity.sqrMagnitude > 0.001f)
            {
                transform.position += _currentVelocity * Time.deltaTime;
            }

            if (_currentTorque.sqrMagnitude > 0.001f)
            {
                transform.Rotate(_currentTorque * Time.deltaTime);
            }
        }


        #endregion

        #region Public Methods


        /// <summary>
        /// 
        /// </summary>
        /// <param name="movementVector"></param>
        public void ApplyVelocity(Vector3 movementVector)
        {
            _currentVelocity = movementVector;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="controlVector"></param>
        public void ApplyMoveControlVector(Vector3 controlVector)
        {
            _currentVelocity = controlVector.normalized * _defaultMoveSpeed;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="eulers"></param>
        public void ApplyTorque(Vector3 eulers)
        {
            _currentTorque = eulers;
        }


        #endregion
    }
}
