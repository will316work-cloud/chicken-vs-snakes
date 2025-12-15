using UnityEngine;

namespace ChickenSnakes.Movement
{
    /// <summary>
    /// Manager of entity movement.
    /// 
    /// Author: William Min
    /// Date: 12/13/25
    /// </summary>
    [RequireComponent(typeof(Rigidbody2D))]
    public class TopDownMovement2D : MonoBehaviour
    {
        #region Serialized Fields


        [SerializeField] private float _defaultMoveSpeed = 10f; // 


        #endregion

        #region Private Fields


        private Vector3 _currentVelocity;   // 
        private Vector3 _currentTorque;     // 
        private Rigidbody2D _rigidbody;     //


        #endregion

        #region MonoBehavior Callbacks


        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
        }

        private void FixedUpdate()
        {
            _rigidbody.linearVelocity = Vector3.zero;

            if (_currentVelocity.sqrMagnitude > 0.001f)
            {
                //transform.Translate(_currentVelocity * Time.deltaTime);


                Vector3 newPosition = (Vector3)(_rigidbody.position) + _currentVelocity * Time.fixedDeltaTime;

                _rigidbody.MovePosition(newPosition);
            }

            if (_currentTorque.sqrMagnitude > 0.001f)
            {
                transform.Rotate(_currentTorque * Time.fixedDeltaTime);
            }
        }

        private void LateUpdate()
        {
            //_rigidbody.linearVelocity = Vector3.zero;
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
