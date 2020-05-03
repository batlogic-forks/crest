
namespace Crest
{
    using System.Collections.Generic;
    using UnityEditor;

    public interface IValidatedInspector
    {
        // Basic proposal. This interface could be improved not to use pass throughs
        void OnInspectorValidation(out bool showMessage, out string message, out MessageType messageType);

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
