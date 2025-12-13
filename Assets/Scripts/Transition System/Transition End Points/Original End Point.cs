using System;

using UnityEngine;

namespace ChickenSnakes.Transitions
{
    /// <summary>
    /// 
    /// 
    /// Author: William Min
    /// Date: 12/13/25
    /// </summary>
    [Serializable]
    public class OriginalEndPoint : TransitionEndPoint
    {
        #region Serialized Fields


        [SerializeField] private TransformComponentType _componentFromSubject;
        [SerializeField] private Vector3 _offset;


        #endregion

        #region Private Fields


        private bool _hasRecordedOriginal;
        private Vector3 _originalPoint;


        #endregion

        #region Transition End Point Callbacks


        public override Vector3 GetPoint()
        {
            return _originalPoint;
        }

        public override void ResetPoint()
        {
            _originalPoint = Vector3.zero;
            _hasRecordedOriginal = false;
        }

        public override void UpdatePoint(Transform subject)
        {
            if (!_hasRecordedOriginal)
            {
                _originalPoint = _getComponentFromTransform(_componentFromSubject, subject) + _offset;
                _hasRecordedOriginal = true;
            }
        }


        #endregion
    }
}
