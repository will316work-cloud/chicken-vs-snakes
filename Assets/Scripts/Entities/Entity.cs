using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Events;

using Colliding;
using Spawners;

namespace ChickenSnakes.Entities
{
    /// <summary>
    /// Represents object that can be killed.
    /// 
    /// Author: William Min
    /// 
    /// </summary>
    public class Entity : MonoBehaviour
    {
        #region Serialized Fields


        [SerializeField] private GameObject _owner;                         // Reference to owner gameObject
        [SerializeField] private int _teamIndex;                            // Team index of entity
        [SerializeField] private GameObject[] _linkObjects;                 // List of hitboxes the entity is linked to
        [SerializeField] private GameObject _gameObjectToDestroy;           // GameObject to destroy on death

        [Space] public UnityEvent<GameObject> OnSetup;                      // Event for when the entity is set up
        [Space] public UnityEvent<GameObject> OnKilled;                     // Event for when the entity is killed
        [Space] [SerializeField] private float _timeBeforeTrueDeath = 1f;   // Time in seconds for entity to do death after being killed
        [Space] public UnityEvent<GameObject> OnDeath;                      // Event for when the entity completely dies


        #endregion

        #region Private Fields


        private EntityState _entityState;   // Current state of entity
        private Coroutine _dyingCoroutine;  // Coroutine of dying transition
                                            //private Dictionary<GameObject, int> currentlyAttacking; // Catalogue of other entity objects currently interacting with entity


        #endregion

        #region Properties


        /// <summary>
        /// 
        /// </summary>
        public GameObject Owner 
        { 
            get => _owner; 
            set 
            { 
                _owner = value;

                foreach (GameObject obj in _linkObjects)
                {
                    ActOnIntersect[] intersections = obj.GetComponentsInChildren<ActOnIntersect>();

                    foreach (ActOnIntersect intersection in intersections)
                    {
                        intersection.ChangeOwner(_owner);
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public int TeamIndex 
        { 
            get => _teamIndex; 
            set 
            { 
                _teamIndex = value;

                foreach (GameObject obj in _linkObjects)
                {
                    ActOnIntersect[] intersections = obj.GetComponentsInChildren<ActOnIntersect>();

                    foreach (ActOnIntersect intersection in intersections)
                    {
                        intersection.ChangeTeam(_teamIndex);
                    }
                }
            }
        }


        #endregion

        #region Enums


        private enum EntityState
        {
            ALIVE,
            DYING,
            DEAD
        }


        #endregion

        #region MonoBehavior Callbacks


        private void OnEnable()
        {
            SetUp();
        }

        private void LateUpdate()
        {
            if (!HasLivingLinks())
            {
                Kill();
            }
        }


        #endregion

        #region Public Methods


        /// <summary>
        /// Sets up the entity.
        /// </summary>
        public virtual void SetUp()
        {
            //ResetInteractions();

            _entityState = EntityState.ALIVE;

            if (_owner == null)
            {
                _owner = gameObject;
            }

            if (_gameObjectToDestroy == null)
            {
                _gameObjectToDestroy = gameObject;
            }

            if (_dyingCoroutine != null)
            {
                StopCoroutine(_dyingCoroutine);
                _dyingCoroutine = null;
            }

            // Set up all linked objects to entity as owner and team
            foreach (GameObject obj in _linkObjects)
            {
                obj.SetActive(true);

                ActOnIntersect[] intersections = obj.GetComponentsInChildren<ActOnIntersect>();

                foreach (ActOnIntersect intersection in intersections)
                {
                    intersection.ChangeOwner(_owner);
                    intersection.ChangeTeam(_teamIndex);
                }
            }

            OnSetup?.Invoke(gameObject);
        }

        /// <summary>
        /// Checks if the entity has any linked objects active.
        /// </summary>
        /// <returns>True if any linked object is still active</returns>
        public bool HasLivingLinks()
        {
            if (_linkObjects.Length <= 0)
            {
                return true;
            }

            foreach (GameObject obj in _linkObjects)
            {
                if (obj.activeInHierarchy)
                {
                    return true;
                }
            }

            return false;
        }

        [ContextMenu("Kill")]
        public void Kill()
        {
            if (_entityState != EntityState.DYING)
            {
                OnKilled?.Invoke(gameObject);

                _dyingCoroutine = StartCoroutine(_waitForDeath());

                _entityState = EntityState.DYING;
            }
        }

        [ContextMenu("Do Death")]
        public void DoDeath()
        {
            if (_entityState != EntityState.DEAD)
            {
                OnDeath?.Invoke(gameObject);

                ObjectPoolManager.ReturnObjectToPool(_gameObjectToDestroy);

                _entityState = EntityState.DEAD;
            }
        }

        /*
        public void AddInteracted(GameObject newObj)
        {
            if (currentlyAttacking.ContainsKey(newObj))
            {
                currentlyAttacking[newObj] += 1;
            }
            else
            {
                currentlyAttacking[newObj] = 1;
            }
        }

        public bool RemoveInteracted(GameObject obj)
        {
            if (currentlyAttacking.ContainsKey(obj))
            {
                currentlyAttacking[obj] -= 1;

                if (currentlyAttacking[obj] == 0)
                {
                    currentlyAttacking.Remove(obj);
                }

                return true;
            }
            else
            {
                return false;
            }
        }

        public void ResetInteractions()
        {
            currentlyAttacking = new Dictionary<GameObject, int>();
        }

        public bool InteractionContains(GameObject obj)
        {
            return currentlyAttacking.ContainsKey(obj);
        }

        public int InteractionCount(GameObject obj)
        {
            if (currentlyAttacking.ContainsKey(obj))
            {
                return currentlyAttacking[obj];
            }
            else
            {
                return 0;
            }
        }

        public string InteractionsToString()
        {
            string result = "";

            foreach (KeyValuePair<GameObject, int> entry in currentlyAttacking)
            {
                if (result.Length > 0)
                {
                    result += " | ";
                }

                if (entry.Key != null)
                {
                    result += entry.Key.name + " : " + entry.Value;
                }
            }

            return result;
        }
        */


        #endregion

        private IEnumerator _waitForDeath()
        {
            yield return new WaitForSeconds(_timeBeforeTrueDeath);

            DoDeath();
        }
    }
}
