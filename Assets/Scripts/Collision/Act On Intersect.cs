using UnityEngine;
using UnityEngine.Events;

namespace Colliding
{
    /// <summary>
    /// Area that triggers events based on who interacts with collider.
    /// 
    /// Author: William Min
    /// Date: 11/14/25
    /// </summary>
    public class ActOnIntersect : MonoBehaviour
    {
        #region Serialized Fields


        [Header("Act On Intersect Properties")]

        [Header("Act On Intersect Toggles")]
        [SerializeField] private bool _willRespondToEnter = true;   // True if the interactable will act on entering the area
        [SerializeField] private bool _willRespondToStay = true;    // True if the interactable will act on staying in the area
        [SerializeField] private bool _willRespondToExit = true;    // True if the interactable will act on exiting the area
        [Space]
        [SerializeField] private bool _willInteractWithOwner;   // True if the interactable can detect and interact with owner

        [Space]
        [Header("Act On Intersect References")]
        [SerializeField] private GameObject _owner; // GameObject reference to the owner

        [Header("Act On Intersect Events")]
        [Space]

        /// <summary>
        /// Events for when something enters the area that gives the owner.
        /// </summary>
        public UnityEvent<GameObject> EnterEventPassOwner;

        /// <summary>
        /// Events for when something enters the area that gives the collider's gameObject.
        /// </summary>
        public UnityEvent<GameObject> EnterEventPassCollided;

        /// <summary>
        /// Events for when something stays in the area that gives the owner.
        /// </summary>
        public UnityEvent<GameObject> StayEventPassOwner;

        /// <summary>
        /// Events for when something stays in the area that gives the collider's gameObject.
        /// </summary>
        public UnityEvent<GameObject> StayEventPassCollided;

        /// <summary>
        /// Events for when something exits the area that gives the owner.
        /// </summary>
        public UnityEvent<GameObject> ExitEventPassOwner;

        /// <summary>
        /// Events for when something exits the area that gives the collider's gameObject.
        /// </summary>
        public UnityEvent<GameObject> ExitEventPassCollided;


        #endregion

        #region MonoBehavior Callbacks


        protected virtual void Awake()
        {
            if (_owner == null)
            {
                RemoveOwner();
            }
        }


        #endregion

        #region Public Methods


        /// <summary>
        /// Changes the owner of the Intersectable.
        /// </summary>
        /// <param name="newOwner">GameObject reference of the new owner</param>
        public void ChangeOwner(GameObject newOwner)
        {
            _owner = newOwner;
        }

        /// <summary>
        /// Resets the owner of the Intersectable to itself.
        /// </summary>
        public void RemoveOwner()
        {
            _owner = gameObject;
        }

        /// <summary>
        /// Toggles whether the enter events will occur or not.
        /// </summary>
        /// <param name="willActivate"></param>
        public void ToggleEnterEvents(bool willActivate)
        {
            _willRespondToEnter = willActivate;
        }

        /// <summary>
        /// Toggles whether the stay events will occur or not.
        /// </summary>
        /// <param name="willActivate"></param>
        public void ToggleStayEvents(bool willActivate)
        {
            _willRespondToStay = willActivate;
        }

        /// <summary>
        /// Toggles whether the exit events will occur or not.
        /// </summary>
        /// <param name="willActivate"></param>
        public void ToggleExitEvents(bool willActivate)
        {
            _willRespondToExit = willActivate;
        }


        #endregion

        #region Private Methods


        protected void _activateEnterEvents(GameObject collidedObject)
        {
            _activateEvents(collidedObject, _willRespondToEnter, EnterEventPassOwner, EnterEventPassCollided);
        }

        protected void _activateStayEvents(GameObject collidedObject)
        {
            _activateEvents(collidedObject, _willRespondToStay, StayEventPassOwner, StayEventPassCollided);
        }

        protected void _activateExitEvents(GameObject collidedObject)
        {
            _activateEvents(collidedObject, _willRespondToExit, ExitEventPassOwner, ExitEventPassCollided);
        }

        protected void _activateEvents(GameObject collidedObject, bool eventToggle, UnityEvent<GameObject> ownerEvent, UnityEvent<GameObject> colliderEvent)
        {
            if (isActiveAndEnabled && eventToggle && 
                (_willInteractWithOwner || 
                collidedObject != _owner && (!collidedObject.TryGetComponent(out ActOnIntersect intersect) || intersect._owner != _owner)))
            {
                ownerEvent?.Invoke(_owner);
                colliderEvent?.Invoke(collidedObject);
            }
        }


        #endregion
    }
}
