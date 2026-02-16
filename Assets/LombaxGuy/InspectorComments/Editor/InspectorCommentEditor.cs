using System;
using UnityEngine;
using UnityEditor;

namespace LombaxGuy.InspectorComment
{
    [CustomEditor(typeof(InspectorComment))]
    public class InspectorCommentEditor : Editor
    {
        SerializedProperty _text;
        SerializedProperty _isLocked;
        SerializedProperty _messageType;

        private void OnEnable()
        {
            _text = serializedObject.FindProperty("_text");
            _isLocked = serializedObject.FindProperty("_isLocked");
            _messageType = serializedObject.FindProperty("_messageType");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            // if the comment is locked
            if (_isLocked.boolValue)
            {
                EditorGUILayout.HelpBox(_text.stringValue, (MessageType)_messageType.enumValueIndex, true);
            }
            // if the comment is unlocked
            else
            {
                // enum dropdown
                GUIContent typeContent = new GUIContent()
                {
                    text = "Comment type",
                    tooltip = "Determins what icon is displayed next to the comment."
                };

                MessageType selected = (MessageType)_messageType.enumValueIndex;
                Enum newSelected = EditorGUILayout.EnumPopup(typeContent, selected);
                _messageType.enumValueIndex = (int)(MessageType)newSelected;

                // text area
                _text.stringValue = EditorGUILayout.TextArea(_text.stringValue);

                serializedObject.ApplyModifiedProperties();
            }
        }
    }
}
