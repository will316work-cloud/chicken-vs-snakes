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


        [SerializeField] private TransformComponentType _componentFromTransform;
        [SerializeField] private Transform _endTransform;


        #endregion

        #region Transition End Point Callbacks


        public override Vector3 GetPoint()
        {
            return _getComponentFromTransform(_componentFromTransform, _endTransform);
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
