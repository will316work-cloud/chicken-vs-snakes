using ChickenSnakes.Transitions;
using UnityEngine;
using UnityEngine.Events;

namespace ChickenSnakes.Enemy
{
    /// <summary>
    /// 
    /// 
    /// Author: William Min
    /// Date: 12/13/25
    /// </summary>
    public class TransitionController : MonoBehaviour
    {
        #region Serialized Fields


        [SerializeField] private Transform _subject;                // 
        [SerializeField] private bool _loopTransitions;             // 
        [SerializeField] private bool _startOnActive;               // 
        [SerializeField] private SequenceTransition _transitions;   // 

        [Space] public UnityEvent OnStartTransition;                // 
        [Space] public UnityEvent OnStopTransition;                 // 
        [Space] public UnityEvent OnEndTransition;                  // 


        #endregion

        #region Private Fields


        private Coroutine _currentTransition;   // 


        #endregion

        #region MonoBehavior Callbacks


        private void Awake()
        {
            if (_subject == null)
            {
                _subject = transform;
            }

            _transitions.OnFinished += _doOnFinishedTransition;
        }

        private void OnEnable()
        {
            if (_startOnActive)
            {
                StartTransition();
            }
        }

        private void OnDisable()
        {
            StopTransition();
        }


        #endregion

        #region Public Methods


        /// <summary>
        /// Starts the transition.
        /// </summary>
        [ContextMenu("Start Transition")]
        public void StartTransition()
        {
            _endTransition();

            OnStartTransition?.Invoke();
            _currentTransition = StartCoroutine(_transitions.Start(_subject));
        }

        /// <summary>
        /// Stops the transition.
        /// </summary>
        [ContextMenu("Stop Transition")]
        public void StopTransition()
        {
            _endTransition();
            OnStopTransition?.Invoke();
        }


        #endregion

        #region Private Methods


        // Ends the transition coroutine
        private void _endTransition()
        {
            if (_currentTransition != null)
            {
                StopCoroutine(_currentTransition);
                _currentTransition = null;
            }
        }

        // Does actions after finishing transition
        private void _doOnFinishedTransition()
        {
            _endTransition();
            OnEndTransition?.Invoke();

            if (_loopTransitions)
            {
                StartTransition();
            }
        }


        #endregion
    }
}
