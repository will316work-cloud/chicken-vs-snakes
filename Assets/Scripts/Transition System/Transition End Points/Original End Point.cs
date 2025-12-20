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


        [Space]
        [SerializeField] private TransformComponentType _transformPart;
        [SerializeField] private Space _relativeTo;
        [SerializeField] private Vector3 _offset;
        [Space]


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
                _originalPoint = _getVectorFromTransform(subject, _transformPart, _relativeTo, _offset);//_getComponentFromTransform(_transformPart, subject) + _offset;
                _hasRecordedOriginal = true;
            }
        }


        #endregion
    }
}
