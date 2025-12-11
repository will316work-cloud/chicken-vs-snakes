using UnityEngine;

using ChickenSnakes.Transitions;

namespace ChickenSnakes.EnemyGroup
{
    public class EnemyGroup : MonoBehaviour
    {
        [SerializeField] private bool _loopTransitions;
        [SerializeField] private SequenceTransition _transitions;

        private Coroutine _currentTransition;

        private void Update()
        {
            if (_currentTransition == null)
            {
                _currentTransition = StartCoroutine(_transitions.Start(transform));
            }
            else if (_transitions.HasEnded && _loopTransitions)
            {
                _currentTransition = null;
            }
        }
    }
}
