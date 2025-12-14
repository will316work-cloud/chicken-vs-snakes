using UnityEngine;
using UnityEngine.InputSystem;

using ChickenSnakes.Movement;

namespace ChickenSnakes.Inputs
{
    /// <summary>
    /// Player controls for moving player character.
    /// 
    /// Author: William Min
    /// Date: 12/13/25
    /// </summary>
    [RequireComponent(typeof(EntityMovement))]
    public class PlayerMovementInputs : MonoBehaviour
    {
        #region Serialized Fields


        [Header("Player Control References")]
        [SerializeField] private InputActionReference _movementInput;   // Reference to Input for moving


        #endregion

        #region Private Fields


        private InputAction _movementAction;    // Input Action reference for movement
        private EntityMovement _controller;     // Reference to entity movement


        #endregion

        #region MonoBehavior Callbacks


        private void Awake()
        {
            _movementAction = _movementInput.action;
            _controller = GetComponent<EntityMovement>();
        }

        private void OnEnable()
        {
            _movementAction.Enable();

            _movementAction.performed += _onMovePerformed;
            _movementAction.canceled += _onMoveCanceled;
        }

        private void OnDisable()
        {
            _movementAction.performed -= _onMovePerformed;
            _movementAction.canceled -= _onMoveCanceled;

            _movementAction.Disable();
        }


        #endregion

        #region Input Binds


        private void _onMovePerformed(InputAction.CallbackContext context)
        {
            Vector2 inputVector = context.ReadValue<Vector2>().normalized;
            _controller.ApplyMoveControlVector(inputVector);
        }

        private void _onMoveCanceled(InputAction.CallbackContext context)
        {
            _controller.ApplyMoveControlVector(Vector2.zero);
        }


        #endregion
    }
}
