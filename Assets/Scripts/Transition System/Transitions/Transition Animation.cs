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
        protected bool _hasEnded;


        #endregion

        #region Properties


        public bool HasEnded { get => _hasEnded; set => _hasEnded = value; }


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

            while (!_hasEnded)
            {
                _doOnLoop(subject);
                yield return null;
            }
        }

        public virtual void Reset()
        {
            _instance = 0;
            _hasEnded = false;
        }

        public abstract void UpdateTransition(Transform subject, float t);


        #endregion

        #region Private Methods


        protected virtual void _doOnLoop(Transform subject)
        {
            UpdateTransition(subject, 0);

            if (_hasEnded && _instance < _instances - 1)
            {
                _instance++;
                _hasEnded = false;
            }
        }


        #endregion
    }
}
