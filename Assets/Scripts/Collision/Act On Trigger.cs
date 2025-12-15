using UnityEngine;

namespace Colliding
{
    /// <summary>
    /// Trigger collider that triggers events based on who interacts with collider.
    /// 
    /// Author: William Min
    /// Date: 11/14/25
    /// </summary>
    [RequireComponent(typeof(Collider))]
    public class ActOnTrigger : ActOnIntersect
    {
        #region MonoBehavior Callbacks


        protected override void Awake()
        {
            base.Awake();

            EnterEventPassCollided.AddListener((gameObject) => Debug.Log($"{name} Trigger Entered with {gameObject.name}"));
            StayEventPassCollided.AddListener((gameObject) => Debug.Log($"{name} Trigger Stayed with {gameObject.name}"));
            ExitEventPassCollided.AddListener((gameObject) => Debug.Log($"{name} Trigger Exited with {gameObject.name}"));
        }

        private void OnTriggerEnter(Collider other)
        {
            _activateEnterEvents(other.gameObject);
        }

        private void OnTriggerStay(Collider other)
        {
            _activateStayEvents(other.gameObject);
        }

        private void OnTriggerExit(Collider other)
        {
            _activateExitEvents(other.gameObject);
        }


        #endregion
    }
}
