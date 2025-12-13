using UnityEngine;
using UnityEngine.Events;

using ChickenSnakes.Transitions;

namespace ChickenSnakes.EnemyGroup
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


        [SerializeField] private Transform _subject;
        [SerializeField] private bool _loopTransitions;
        [SerializeField] private bool _startOnAwake;
        [SerializeField] private SequenceTransition _transitions;
        [Space] public UnityEvent _onStartTransition;
        [Space] public UnityEvent _onEndTransition;
        [Space] public UnityEvent _onStopTransition;


        #endregion

        #region Private Fields


        private Coroutine _currentTransition;


        #endregion

        #region MonoBehavior Callbacks


        private void Awake()
        {
            if (_subject == null)
            {
                _subject = transform;
            }

            if (_startOnAwake)
            {
                StartTransition();
            }
        }

        private void Update()
        {
            if (_transitions.HasEnded)
            {
                _onEndTransition?.Invoke();
                _currentTransition = null;

                if (_loopTransitions)
                {
                    StartTransition();
                }
            }
        }


        #endregion

        #region Public Methods


        [ContextMenu("Start Transition")]
        public void StartTransition()
        {
            if (_currentTransition != null)
            {
                StopCoroutine(_currentTransition);
                _onStopTransition?.Invoke();
            }

            _currentTransition = StartCoroutine(_transitions.Start(_subject));
            _onStartTransition?.Invoke();
        }


        #endregion
    }
}
