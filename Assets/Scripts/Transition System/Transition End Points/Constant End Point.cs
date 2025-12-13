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
    public class ConstantEndPoint : TransitionEndPoint
    {
        #region Serialized Fields


        [SerializeField] private Vector3 _endPoint;


        #endregion

        #region Transition End Point Callbacks


        public override Vector3 GetPoint()
        {
            return _endPoint;
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
