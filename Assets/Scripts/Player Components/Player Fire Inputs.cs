using UnityEngine;
using UnityEngine.InputSystem;

using Spawners;

namespace ChickenSnakes.Inputs
{
    /// <summary>
    /// Player controls for firing projectiles.
    /// 
    /// Author: William Min
    /// Date: 12/13/25
    /// </summary>
    public class PlayerFireInputs : MonoBehaviour
    {
        #region Serialized Fields


        [Header("Player Control References")]
        [SerializeField] private InputActionReference _fireInput;   // Reference to input for moving
        [SerializeField] private FireProjectile _projectile;        // Projectile reference
        [SerializeField] private GameObject _owner;                 // Object that will be the fired projectiles' owner
        [SerializeField] private Transform _originOfProjectile;     // Transform reference that contains spawn position and rotation of projectile


        #endregion

        #region Private Fields


        private InputAction _fireAction;    // Input Action reference for firing projectiles


        #endregion

        #region MonoBehavior Callbacks


        private void Awake()
        {
            _fireAction = _fireInput.action;
        }

        private void OnEnable()
        {
            _fireAction.Enable();

            _fireAction.performed += _onFirePerformed;
            //_fireAction.canceled += _onFireCanceled;
        }

        private void OnDisable()
        {
            _fireAction.performed -= _onFirePerformed;
            //_fireAction.canceled -= _onFireCanceled;

            _fireAction.Disable();
        }


        #endregion

        #region Input Binds


        private void _onFirePerformed(InputAction.CallbackContext context)
        {
            _projectile.SummonProjectile(_originOfProjectile, _owner);
        }


        #endregion
    }
}
