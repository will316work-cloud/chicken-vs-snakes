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
    public class TransformEndPoint : TransitionEndPoint
    {
        #region Serialized Fields


        [Space]
        [SerializeField] private TransformComponentType _transformPart;
        [SerializeField] private Space _relativeTo;
        [SerializeField] private Transform _endTransform;
        [SerializeField] private Vector3 _offset;


        #endregion

        #region Transition End Point Callbacks


        public override Vector3 GetPoint()
        {
            //return _getComponentFromTransform(_transformPart, _endTransform);
            return _getVectorFromTransform(_endTransform, _transformPart, _relativeTo, _offset);
        }

        public override void ResetPoint()
        {

        }

        public override void UpdatePoint(Transform subject)
        {
            
        }


        #endregion
    }
}
