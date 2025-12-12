using System;

using UnityEngine;

namespace ChickenSnakes.Transitions
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public abstract class TransitionEndPoint
    {
        protected enum TransformComponentType
        {
            POSITION,
            ROTATION,
            SCALE
        }

        public abstract Vector3 GetPoint();
        public abstract void ResetPoint();
        public abstract void UpdatePoint(Transform subject);

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
    }


    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class ConstantEndPoint : TransitionEndPoint
    {
        [SerializeField] private Vector3 _endPoint;

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
    }


    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class TransformEndPoint : TransitionEndPoint
    {
        [SerializeField] private TransformComponentType _componentFromTransform;
        [SerializeField] private Transform _endTransform;

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
    }


    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class OriginalEndPoint : TransitionEndPoint
    {
        [SerializeField] private TransformComponentType _componentFromSubject;
        [SerializeField] private Vector3 _offset;

        private bool _hasRecordedOriginal;
        private Vector3 _originalPoint;

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
    }
}
