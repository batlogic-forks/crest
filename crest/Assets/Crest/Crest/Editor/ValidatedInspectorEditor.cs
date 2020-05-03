
namespace Crest
{
    using System;
    using System.Collections.Generic;
    using UnityEditor;

    public abstract class ValidatedInspectorEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            IValidatedInspector target = (IValidatedInspector)this.target;

            // // NOTE: This is the basic proposal.
            // target.OnInspectorValidation(out bool showMessage, out string message, out MessageType messageType);
            // if (showMessage)
            // {
            //     EditorGUILayout.Space();
            //     EditorGUILayout.HelpBox(message, messageType);
            //     EditorGUILayout.Space();
            // }


            // Function Proposal. Doesn't really work
            // target.OnInspectorValidation(ValidatedHelper.HelpBox);

            // The following is the more advanced proposal. It would allow use to reuse validation code.

            var messageTypes = Enum.GetValues(typeof(MessageType));

            var messages = new List<ValidatedMessage>();
            target.OnInspectorValidation(messages);

            var needsSpaceAbove = true;
            var needsSpaceBelow = false;

            for (var messageTypeIndex = messageTypes.Length - 1; messageTypeIndex >= 0; messageTypeIndex--)
            {
                // We would want make sure we don't produce garbage here
                var filtered = messages.FindAll(x => (int) x.type == messageTypeIndex);
                if (filtered.Count > 0)
                {
                    if (needsSpaceAbove)
                    {
                        EditorGUILayout.Space();
                        EditorGUILayout.Space();
                        needsSpaceAbove = false;
                    }

                    needsSpaceBelow = true;

                    var joinedMessages = "";
                    for (var ii = 0; ii < filtered.Count; ii++)
                    {
                        // We would want to reduce garbage here with string builder
                        joinedMessages += filtered[ii].message;
                        if (ii < filtered.Count - 1) joinedMessages += "\n";
                    }

                    EditorGUILayout.HelpBox(joinedMessages, (MessageType)messageTypeIndex);

                    // We could conditionally break here to hide less important messages like info
                    // break;

                }
            }

            if (needsSpaceBelow)
            {
                EditorGUILayout.Space();
            }

            base.OnInspectorGUI();
        }
    }

    // This is how we target inspectors. We would have to create one for each one we are targetting
    [CustomEditor(typeof(RegisterLodDataInputBase), true), CanEditMultipleObjects]
    class RegisterLodDataInputBaseValidatedInspectorEditor : ValidatedInspectorEditor {}
}
