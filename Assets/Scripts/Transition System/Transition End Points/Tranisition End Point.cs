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
    public abstract class TransitionEndPoint
    {
        #region Enums


        protected enum TransformComponentType
        {
            POSITION,
            ROTATION,
            SCALE
        }


        #endregion

        #region Public Methods


        public abstract Vector3 GetPoint();
        public abstract void ResetPoint();
        public abstract void UpdatePoint(Transform subject);


        #endregion

        #region Private Methods


        protected Vector3 _getComponentFromTransform(TransformComponentType componentType, Transform transform)
        {
            switch (componentType)
            {
                case TransformComponentType.POSITION:
                    return transform.position;

                case TransformComponentType.ROTATION:
                    return transform.rotation.eulerAngles;

                case TransformComponentType.SCALE:
                    return transform.localScale;

                default:
                    return default(Vector3);
            }
        }


        #endregion
    }
}
