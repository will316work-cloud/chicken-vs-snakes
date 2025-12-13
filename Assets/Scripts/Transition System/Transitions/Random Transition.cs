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
    public class RandomTransition : TransitionAnimation
    {
        #region Serialized Fields


        [SerializeField] private bool _resetEntryWeightsOnStart;
        [SerializeField] private WeightProcesser<TransitionAnimation> _transitions;


        #endregion

        #region Private Fields


        private TransitionAnimation _currentTransition;
        private IEnumerator _currentEnumerator;


        #endregion

        #region Transition Animation Callbacks


        public override void UpdateTransition(Transform subject, float t)
        {
            if (_currentTransition == null)
            {
                _currentTransition = _transitions.GetNextEntry();
            }

            if (_currentEnumerator == null)
            {
                _currentEnumerator = _currentTransition.Start(subject);
            }

            if (!_currentEnumerator.MoveNext() || _currentTransition.HasEnded)
            {
                _currentTransition = null;
                _currentEnumerator = null;
                _hasEnded = true;
            }
        }

        public override void Reset()
        {
            base.Reset();

            _currentTransition = null;
            _currentEnumerator = null;

            if (_resetEntryWeightsOnStart)
            {
                _transitions.Reset();
            }
        }


        #endregion
    }
}
