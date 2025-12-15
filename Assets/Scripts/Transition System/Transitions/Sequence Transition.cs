using System;
using System.Collections;

using UnityEngine;

namespace ChickenSnakes.Transitions
{
    /// <summary>
    /// 
    /// 
    /// Author: William Min
    /// Date: 
    /// </summary>
    [Serializable]
    public class SequenceTransition : TransitionAnimation
    {
        #region Serialized Fields


        [SerializeReference, SubclassSelector] private TransitionAnimation[] _transitions;


        #endregion

        #region Private Fields


        private IEnumerator _currentEnumerator;
        private int _currentTransitionIndex;


        #endregion

        #region Transition Animation Callbacks


        public override void UpdateTransition(Transform subject, float t)
        {
            if (_currentTransitionIndex >= _transitions.Length)
            {
                _hasFinished = true;
                _currentTransitionIndex = 0;
                _currentEnumerator = null;
            }
            else
            {
                TransitionAnimation currentTransition = _transitions[_currentTransitionIndex];

                if (_currentEnumerator == null)
                {
                    _currentEnumerator = currentTransition.Start(subject);
                }

                if (!_currentEnumerator.MoveNext())
                {
                    _currentTransitionIndex++;
                    _currentEnumerator = null;
                }
            }
        }

        public override void Reset()
        {
            base.Reset();

            _currentTransitionIndex = 0;
            _currentEnumerator = null;
        }


        #endregion
    }
}
