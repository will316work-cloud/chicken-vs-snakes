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
    public class MultipleTransition : TransitionAnimation
    {
        #region Serialized Fields


        [SerializeReference, SubclassSelector] private TransitionAnimation[] _transitions;


        #endregion

        #region Private Fields


        private IEnumerator[] _currentEnumerators;
        private bool _startedAll;


        #endregion

        #region Transition Animation Callbacks


        public override void UpdateTransition(Transform subject, float t)
        {
            if (!_startedAll)
            {
                for (int i = 0; i < _transitions.Length; i++)
                {
                    _currentEnumerators[i] = _transitions[i].Start(subject);
                }

                _startedAll = true;
            }
            else
            {
                _hasFinished = true;

                foreach (IEnumerator enumerator in _currentEnumerators)
                {
                    if (enumerator.MoveNext())
                    {
                        _hasFinished = false;
                    }
                }
            }
        }

        public override void Reset()
        {
            base.Reset();

            _startedAll = false;

            if (_currentEnumerators == null)
            {
                _currentEnumerators = new IEnumerator[_transitions.Length];
            }
            else
            {
                for (int i = 0; i < _transitions.Length; i++)
                {
                    _currentEnumerators[i] = null;
                }
            }
        }


        #endregion
    }
}
