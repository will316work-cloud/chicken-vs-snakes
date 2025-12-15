using UnityEngine;

namespace Colliding
{
    /// <summary>
    /// Collision collider that triggers events based on who interacts with collider.
    /// 
    /// Author: William Min
    /// Date: 11/14/25
    /// </summary>
    [RequireComponent(typeof(Collider))]
    public class ActOnCollision : ActOnIntersect
    {
        #region MonoBehavior Callbacks


        protected override void Awake()
        {
            base.Awake();

            EnterEventPassCollided.AddListener((gameObject) => Debug.Log($"{name} Collision Entered with {gameObject.name}"));
            StayEventPassCollided.AddListener((gameObject) => Debug.Log($"{name} Collision Stayed with {gameObject.name}"));
            ExitEventPassCollided.AddListener((gameObject) => Debug.Log($"{name} Collision Exited with {gameObject.name}"));
        }

        private void OnCollisionEnter(Collision collision)
        {
            _activateEnterEvents(collision.gameObject);
        }

        private void OnCollisionStay(Collision collision)
        {
            _activateStayEvents(collision.gameObject);
        }

        private void OnCollisionExit(Collision collision)
        {
            _activateExitEvents(collision.gameObject);
        }


        #endregion
    }
}
