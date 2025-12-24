using UnityEngine;

namespace ChickenSnakes.Transitions
{
    /// <summary>
    /// 
    /// 
    /// Author: William Min
    /// </summary>
    [System.Serializable]
    public class TimeModifier
    {
        #region Serialized Fields


        [SerializeField] private Space _modifierSpace;
        [SerializeField] private AnimationCurve _modifierX = AnimationCurve.Linear(0, 0, 1, 1);
        [SerializeField] private AnimationCurve _modifierY = AnimationCurve.Linear(0, 0, 1, 1);
        [SerializeField] private AnimationCurve _modifierZ = AnimationCurve.Linear(0, 0, 1, 1);


        #endregion

        #region Public Methods


        public Vector3 GetTimeModifierVector(float t)
        {
            return new Vector3(_modifierX.Evaluate(t), _modifierY.Evaluate(t), _modifierZ.Evaluate(t));
        }

        public Vector3 GetWorldInbetweenVector(Transform subject, Vector3 startVector, Vector3 endVector, float t)
        {
            Vector3 time = GetTimeModifierVector(t);

            Vector3 displacement = endVector - startVector;

            void _modifyDisplacement()
            {
                displacement.x = time.x * displacement.x;
                displacement.y = time.y * displacement.y;
                displacement.z = time.z * displacement.z;
            }

            switch (_modifierSpace)
            {
                case Space.World:

                    _modifyDisplacement();

                    break;

                case Space.Self:

                    if (subject == null)
                    {
                        _modifyDisplacement();
                    }
                    else
                    {
                        displacement = subject.InverseTransformDirection(displacement);
                        _modifyDisplacement();
                        displacement = subject.TransformDirection(displacement);
                    }

                    break;

                default:

                    break;
            }

            return startVector + displacement;
        }


        #endregion
    }
}
