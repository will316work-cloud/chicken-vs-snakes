using UnityEngine;

namespace Colliding
{
    /// <summary>
    /// Collision 2D collider that triggers events based on who interacts with collider.
    /// 
    /// Author: William Min
    /// Date: 11/14/25
    /// </summary>
    [RequireComponent(typeof(Collider2D))]
    public class ActOnCollision2D : ActOnIntersect
    {
        #region MonoBehavior Callbacks


        protected override void Awake()
        {
            base.Awake();

            EnterEventPassCollided.AddListener((gameObject) => Debug.Log($"{name} Collision 2D Entered with {gameObject.name}"));
            StayEventPassCollided.AddListener((gameObject) => Debug.Log($"{name} Collision 2D Stayed with {gameObject.name}"));
            ExitEventPassCollided.AddListener((gameObject) => Debug.Log($"{name} Collision 2D Exited with {gameObject.name}"));
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            _activateEnterEvents(collision.gameObject);
        }

        private void OnCollisionStay2D(Collision2D collision)
        {
            _activateStayEvents(collision.gameObject);
        }

        private void OnCollisionExit2D(Collision2D collision)
        {
            _activateExitEvents(collision.gameObject);
        }


        #endregion
    }
}
