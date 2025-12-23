using UnityEngine;
using UnityEngine.Events;

namespace ChickenSnakes.Managers
{
    public class CommandManager : MonoBehaviour
    {
        #region Serialized Fields


        [Space][SerializeField] private UnityEvent _approachCommand;
        [Space][SerializeField] private WeightProcesser<UnityEvent, WeightEntry<UnityEvent>> _commands;


        #endregion

        #region Public Methods


        [ContextMenu("Do Approach Command")]
        public void DoApproach()
        {
            _approachCommand?.Invoke();
        }

        public void DoCommand(int index)
        {
            _commands.GetEntryInfo(index).Object?.Invoke();
        }

        [ContextMenu("Do Next Action")]
        public void DoNextCommand()
        {
            _commands.GetNextEntry()?.Invoke();
        }


        #endregion
    }
}
