using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace LombaxGuy.InspectorComment
{
    public class InspectorComment : MonoBehaviour
    {
#if UNITY_EDITOR 
        [SerializeField]
        private string _text;
        [SerializeField]
        private bool _isLocked = false;
        [SerializeField]
        private MessageType _messageType = MessageType.None;

        /// <summary>
        /// adds a right click menu option to lock and unlock the comment
        /// </summary>
        [ContextMenu("Toggle comment lock")]
        private void ToggleLock()
        {
            _isLocked = !_isLocked;
        }
#endif
    }
}
