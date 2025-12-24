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
    public class ScaleTransition : SetTimeTransition
    {
        #region Serialized Fields


        [Space]
        [SerializeReference, SubclassSelector] private TransitionEndPoint _startScale;
        [Space]
        [SerializeReference, SubclassSelector] private TransitionEndPoint _endScale;
        [Space]
        [SerializeField] private bool _preserveChildrenScales;
        [Space]
        [SerializeField] private TimeModifier _modifier;


        #endregion

        #region Transition Animation Callbacks


        public override void UpdateTransition(Transform subject, float t)
        {
            _startScale.UpdatePoint(subject);
            _endScale.UpdatePoint(subject);

            Vector3 startSize = _startScale.GetPoint();
            Vector3 endSize = _endScale.GetPoint();

            subject.localScale = _modifier.GetWorldInbetweenVector(subject, startSize, endSize, t);

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


        #endregion
    }
}
