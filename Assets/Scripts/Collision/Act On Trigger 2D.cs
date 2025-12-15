using UnityEngine;

namespace Colliding
{
    /// <summary>
    /// Trigger 2D collider that triggers events based on who interacts with collider.
    /// 
    /// Author: William Min
    /// Date: 11/14/25
    /// </summary>
    [RequireComponent(typeof(Collider2D))]
    public class ActOnTrigger2D : ActOnIntersect
    {
        #region MonoBehavior Callbacks


        protected override void Awake()
        {
            base.Awake();

            EnterEventPassCollided.AddListener((gameObject) => Debug.Log($"{name} Trigger 2D Entered with {gameObject.name}"));
            StayEventPassCollided.AddListener((gameObject) => Debug.Log($"{name} Trigger 2D Stayed with {gameObject.name}"));
            ExitEventPassCollided.AddListener((gameObject) => Debug.Log($"{name} Trigger 2D Exited with {gameObject.name}"));
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            _activateEnterEvents(other.gameObject);
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            _activateStayEvents(other.gameObject);
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            _activateExitEvents(other.gameObject);
        }


        #endregion
    }
}
