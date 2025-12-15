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
    public class ScaleTransition : SetTimeTransition
    {
        #region Serialized Fields


        [SerializeReference, SubclassSelector] private TransitionEndPoint _startScale;
        [SerializeReference, SubclassSelector] private TransitionEndPoint _endScale;
        [SerializeField] private bool _preserveChildrenScales;


        #endregion

        #region Transition Animation Callbacks


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


        #endregion
    }
}
