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
    public abstract class TransitionAnimation
    {
        #region Serialized Fields


        [SerializeField] protected int _instances = 1;


        #endregion

        #region Private Fields


        protected int _instance;
        protected bool _hasFinished;


        #endregion

        #region Public Fields


        public Action OnFinished;


        #endregion

        #region Enums


        protected enum EndValueType
        {
            CONSTANT,
            ORIGINAL,
            TRANSFORM
        }


        #endregion

        #region Public Methods


        public IEnumerator Start(Transform subject)
        {
            Reset();

            while (!_hasFinished)
            {
                _doOnLoop(subject);
                yield return null;
            }

            OnFinished?.Invoke();
        }

        public virtual void Reset()
        {
            _instance = 0;
            _hasFinished = false;
        }

        public abstract void UpdateTransition(Transform subject, float t);


        #endregion

        #region Private Methods


        protected virtual void _doOnLoop(Transform subject)
        {
            UpdateTransition(subject, 0);

            if (_hasFinished && _instance < _instances - 1)
            {
                _instance++;
                _hasFinished = false;
            }
        }


        #endregion
    }
}
