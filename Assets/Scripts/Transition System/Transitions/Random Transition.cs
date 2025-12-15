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


        [SerializeField] private bool _newWeightsOnReset;               //  
        [SerializeField] private WeightProcesser<TransitionAnimation, 
                                                SubclassWeightEntry<TransitionAnimation>> 
                                                _transitionEntries;     // 


        #endregion

        #region Private Fields


        private TransitionAnimation _currentTransition; // 
        private IEnumerator _currentEnumerator;         // 


        #endregion

        #region Transition Animation Callbacks


        public override void UpdateTransition(Transform subject, float t)
        {
            if (_currentTransition == null)
            {
                _currentTransition = _transitionEntries.GetNextEntry();
            }

            if (_currentEnumerator == null)
            {
                _currentEnumerator = _currentTransition.Start(subject);
            }

            if (!_currentEnumerator.MoveNext())
            {
                _currentTransition = null;
                _currentEnumerator = null;
                _hasFinished = true;
            }
        }

        public override void Reset()
        {
            base.Reset();

            _currentTransition = null;
            _currentEnumerator = null;

            if (_newWeightsOnReset)
            {
                _transitionEntries.Reset();
            }
        }


        #endregion
    }
}
