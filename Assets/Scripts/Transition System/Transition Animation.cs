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


    /// <summary>
    /// 
    /// 
    /// Author: William Min
    /// Date: 
    /// </summary>
    [Serializable]
    public abstract class SetTimeTransition : TransitionAnimation
    {
        [SerializeField] protected float _duration = 1;

        protected float _currentTime;

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
                    _hasEnded = true;
                }
            }
        }
    }


    /// <summary>
    /// 
    /// 
    /// Author: William Min
    /// Date: 
    /// </summary>
    [Serializable]
    public class TranslateTransition : SetTimeTransition
    {
        [SerializeReference, SubclassSelector] private TransitionEndPoint _startPosition;
        [SerializeReference, SubclassSelector] private TransitionEndPoint _endPosition;

        public override void UpdateTransition(Transform subject, float t)
        {
            _startPosition.UpdatePoint(subject);
            _endPosition.UpdatePoint(subject);

            subject.position = Vector3.Lerp(_startPosition.GetPoint(), _endPosition.GetPoint(), t);
        }

        public override void Reset()
        {
            base.Reset();

            _startPosition.ResetPoint();
            _endPosition.ResetPoint();
        }
    }


    /// <summary>
    /// 
    /// 
    /// Author: William Min
    /// Date: 
    /// </summary>
    [Serializable]
    public class ScaleTransition : SetTimeTransition
    {
        [SerializeReference, SubclassSelector] private TransitionEndPoint _startScale;
        [SerializeReference, SubclassSelector] private TransitionEndPoint _endScale;
        [SerializeField] private bool _preserveChildrenScales;

        public override void UpdateTransition(Transform subject, float t)
        {
            _startScale.UpdatePoint(subject);
            _endScale.UpdatePoint(subject);

            Vector3 scale = Vector3.Lerp(_startScale.GetPoint(), _endScale.GetPoint(), t);

            subject.localScale = scale;

            for (int i = 0; i < subject.childCount; i++)
            {
                Transform child = subject.GetChild(i);

                child.SetParent(null);
                child.localScale = Vector3.one;
                child.SetParent(subject);
                child.SetSiblingIndex(i);
            }
        }
        public override void Reset()
        {
            base.Reset();

            _startScale.ResetPoint();
            _endScale.ResetPoint();
        }
    }


    /// <summary>
    /// 
    /// 
    /// Author: William Min
    /// Date: 
    /// </summary>
    [Serializable]
    public class IdleTransition : SetTimeTransition
    {
        public override void UpdateTransition(Transform subject, float t)
        {
            
        }
    }


    /// <summary>
    /// 
    /// 
    /// Author: William Min
    /// Date: 
    /// </summary>
    [Serializable]
    public class RandomTransition : TransitionAnimation
    {
        [SerializeField] private bool _resetEntryWeightsOnStart;
        [SerializeField] private WeightProcesser<TransitionAnimation> _transitions;

        private TransitionAnimation _currentTransition;
        private IEnumerator _currentEnumerator;

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
    }


    /// <summary>
    /// 
    /// 
    /// Author: William Min
    /// Date: 
    /// </summary>
    [Serializable]
    public class SequenceTransition : TransitionAnimation
    {
        [SerializeReference, SubclassSelector] private TransitionAnimation[] _transitions;

        private IEnumerator _currentEnumerator;
        private int _currentTransitionIndex;

        public override void UpdateTransition(Transform subject, float t)
        {
            if (_currentTransitionIndex >= _transitions.Length)
            {
                _hasEnded = true;
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

                if (!_currentEnumerator.MoveNext() || currentTransition.HasEnded)
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
    }
}
