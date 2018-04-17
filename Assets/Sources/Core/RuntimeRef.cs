using UnityEngine;

namespace IkigaiGames.IBLDemo.Core
{
    public abstract class RuntimeRef<T> : ScriptableObject where T : class
    {
#if UNITY_EDITOR
        [Multiline]
        public string DeveloperDescription = "";
#endif
        // Proper refered object name is not shown in Inspector
        // to overcome this issue we need custom inspector script
        // Clicking on the reference still getting us to the proper object
        public T Value;

        public void Register(T value)
        {
            Value = value;
        }

        public void Unregister(T value)
        {
            if (Value == value)
            {
                Value = null;
            }
        }
    }
}
