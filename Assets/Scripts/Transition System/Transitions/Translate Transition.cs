using System;

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
    public class TranslateTransition : SetTimeTransition
    {
        #region Serialized Fields


        [Space]
        [SerializeReference, SubclassSelector] private TransitionEndPoint _startPosition;
        [Space]
        [SerializeReference, SubclassSelector] private TransitionEndPoint _endPosition;
        [Space]
        [SerializeField] private TimeModifier _modifier;


        #endregion

        #region Transition Animation Callbacks


        public override void UpdateTransition(Transform subject, float t)
        {
            _startPosition.UpdatePoint(subject);
            _endPosition.UpdatePoint(subject);

            Vector3 startPoint = _startPosition.GetPoint();
            Vector3 endPoint = _endPosition.GetPoint();

            subject.position = _modifier.GetWorldInbetweenVector(subject, startPoint, endPoint, t);
        }

        public override void Reset()
        {
            base.Reset();

            _startPosition.ResetPoint();
            _endPosition.ResetPoint();
        }


        #endregion
    }
}
