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


        #endregion

        #region Transition Animation Callbacks


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


        #endregion
    }
}
