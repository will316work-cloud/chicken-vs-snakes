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
    public abstract class SetTimeTransition : TransitionAnimation
    {
        #region Serialized Fields


        [SerializeField] protected float _duration = 1;


        #endregion

        #region Private Fields


        protected float _currentTime;


        #endregion

        #region Transition Animation Callbacks


        public override void Reset()
        {
            base.Reset();

            _currentTime = 0;
        }

        protected override void _doOnLoop(Transform subject)
        {
            _currentTime += Time.deltaTime;

            float t = _currentTime / _duration;
            UpdateTransition(subject, t);

            if (_currentTime >= _duration)
            {
                if (_instance < _instances - 1)
                {
                    _instance++;
                    _currentTime = 0;
                }
                else
                {
                    _hasFinished = true;
                }
            }
        }


        #endregion
    }
}
