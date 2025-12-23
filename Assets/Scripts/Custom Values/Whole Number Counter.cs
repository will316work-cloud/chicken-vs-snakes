using UnityEngine;
using UnityEngine.Events;

namespace ChickenSnakes.Managers
{
    public class WholeNumberCounter : MonoBehaviour
    {
        [SerializeField] private uint _wholeNumber;

        [Space] public UnityEvent<uint> OnWholeNumberChangedBefore;
        [Space] public UnityEvent<uint> OnWholeNumbertChangedAfter;

        public uint WholeNumber
        {
            get => _wholeNumber;
            set
            {
                OnWholeNumberChangedBefore?.Invoke(_wholeNumber);

                _wholeNumber = value;

                OnWholeNumbertChangedAfter?.Invoke(_wholeNumber);
            }
        }

        private void Awake()
        {
            WholeNumber = _wholeNumber;
        }

        public void SetNumber(int newNumber)
        {
            WholeNumber = (uint)newNumber;
        }

        [ContextMenu("Add One")]
        public void AddOne()
        {
            WholeNumber++;
        }
        
        [ContextMenu("Subtract One")]
        public void SubtractOne()
        {
            WholeNumber--;
        }

        public bool IsZero()
        {
            return _wholeNumber <= 0;
        }
    }
}
