
namespace Crest
{
    using System;
    using System.Collections.Generic;
    using UnityEditor;

    public abstract class ValidatedInspectorEditor : Editor
    {
        readonly List<ValidatedMessage> _validationMessages = new List<ValidatedMessage>();
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


            // // NOTE: Function Proposal. Doesn't really work. Detailed in PR
            // target.OnInspectorValidation(ValidatedHelper.HelpBox);

            // ADVANCED PROPOSAL
            // The following is the more advanced proposal. It would allow us to reuse validation code.

            var messageTypes = Enum.GetValues(typeof(MessageType));

            _validationMessages.Clear();
            target.OnInspectorValidation(_validationMessages);

            // We only want space before and after the list of help boxes. We don't want space between.
            var needsSpaceAbove = true;
            var needsSpaceBelow = false;

            // We loop through in reverse order so Error appears at the top
            for (var messageTypeIndex = messageTypes.Length - 1; messageTypeIndex >= 0; messageTypeIndex--)
            {
                // We would want make sure we don't produce garbage here
                var filtered = _validationMessages.FindAll(x => (int) x.type == messageTypeIndex);
                if (filtered.Count > 0)
                {
                    if (needsSpaceAbove)
                    {
                        // Double space looks correct at top.
                        EditorGUILayout.Space();
                        EditorGUILayout.Space();
                        needsSpaceAbove = false;
                    }

                    needsSpaceBelow = true;

                    // We join the messages together to reduce vertical space since HelpBox has padding and borders etc.
                    var joinedMessages = "";
                    for (var ii = 0; ii < filtered.Count; ii++)
                    {
                        // We would want to reduce garbage here using string builder.
                        joinedMessages += filtered[ii].message;
                        if (ii < filtered.Count - 1) joinedMessages += "\n";
                    }

                    EditorGUILayout.HelpBox(joinedMessages, (MessageType)messageTypeIndex);

                    // We could conditionally break here to hide less important messages like MessageType.Info
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
    // We can use inheritence here so that will save a fair bit of definitions.
    [CustomEditor(typeof(RegisterLodDataInputBase), true), CanEditMultipleObjects]
    class RegisterLodDataInputBaseValidatedInspectorEditor : ValidatedInspectorEditor {}
}
