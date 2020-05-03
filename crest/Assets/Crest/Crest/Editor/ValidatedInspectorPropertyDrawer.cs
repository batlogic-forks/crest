
namespace Crest
{
    using UnityEditor;
    using UnityEngine;

    [CustomPropertyDrawer(typeof(ValidatedInspector))]
    public class ValidatedInspectorPropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var target = property.serializedObject.targetObject as IValidatedInspector;

            target.OnInspectorValidation(out bool showMessage, out string message, out MessageType messageType);

            if (showMessage)
            {
                EditorGUILayout.Space();
                EditorGUILayout.HelpBox(message, messageType);
                EditorGUILayout.Space();
            }
        }
    }
}
