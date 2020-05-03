
namespace Crest
{
    using System.Collections.Generic;
    using UnityEditor;
    using UnityEngine;

    using System;

    public static class ValidatedHelper
    {
        public delegate void ShowMessage(string message, MessageType type);

        public static void DebugLog(string message, MessageType type)
        {
            switch (type)
            {
                case MessageType.Warning: Debug.LogWarning(message); break;
                case MessageType.Error: Debug.LogError(message); break;
                default: Debug.Log(message); break;
            }
        }

        public static void HelpBox(string message, MessageType type)
        {
            EditorGUILayout.HelpBox(message, type);
        }
    }


    public interface IValidatedInspector
    {
        // Basic proposal. This interface could be improved not to use pass throughs
        void OnInspectorValidation(out bool showMessage, out string message, out MessageType messageType);
        void OnInspectorValidation(ValidatedHelper.ShowMessage function);

        // Advanced proposal so we can use same validation code for HelpBox and DebugLog
        void OnInspectorValidation(List<ValidatedMessage> messages);
    }

    // Used for advanced proposal
    public struct ValidatedMessage
    {
        public string message;
        public MessageType type;
    }
}
