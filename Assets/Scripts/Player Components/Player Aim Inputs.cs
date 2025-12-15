using UnityEngine;
using UnityEngine.InputSystem;

namespace ChickenSnakes.Inputs
{
    /// <summary>
    /// Player controls for aiming the character to a target position.
    /// 
    /// Author: William Min
    /// Date: 12/13/25
    /// </summary>
    public class PlayerAimInputs : MonoBehaviour
    {
        #region MonoBehavior Callbacks


        public void Update()
        {
            Vector3 target = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            target.z = 0;

            transform.LookAt(target, Vector3.back);
        }


        #endregion
    }
}
