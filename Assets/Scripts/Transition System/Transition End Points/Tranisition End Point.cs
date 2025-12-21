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


        protected Vector3 _getVectorFromTransform(Transform transform, TransformComponentType componentType, Space relativeTo, Vector3 offset)
        {
            switch (componentType)
            {
                case TransformComponentType.POSITION:
                    
                    return relativeTo == Space.World ? 
                            transform.position + offset : 
                            transform.TransformPoint(offset);

                case TransformComponentType.ROTATION:
                    
                    return transform.rotation.eulerAngles + offset;

                case TransformComponentType.SCALE:
                    
                    return relativeTo == Space.World ?
                            transform.lossyScale + offset :
                            transform.localScale + offset;

                default:
                    
                    return default(Vector3);
            }
        }


        #endregion
    }
}
