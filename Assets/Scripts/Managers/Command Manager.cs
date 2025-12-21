using UnityEngine;
using UnityEngine.Events;

namespace ChickenSnakes.Managers
{
    public class CommandManager : MonoBehaviour
    {
        [SerializeField] private WeightProcesser<UnityEvent, WeightEntry<UnityEvent>> _commands;

        public void DoCommand(int index)
        {
            _commands.GetEntryInfo(index).Object?.Invoke();
        }

        [ContextMenu("Do Next Action")]
        public void DoNextCommand()
        {
            _commands.GetNextEntry()?.Invoke();
        }
    }
}
