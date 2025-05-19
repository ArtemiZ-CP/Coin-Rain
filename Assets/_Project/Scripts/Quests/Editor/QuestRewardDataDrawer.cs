using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(QuestRewardData))]
public class QuestRewardDataDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        Rect labelRect = new(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
        var style = new GUIStyle(GUI.skin.label)
        {
            alignment = TextAnchor.MiddleCenter,
            fontStyle = FontStyle.Bold
        };
        EditorGUI.LabelField(labelRect, "Reward", style);
        position.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

        var typeProp = property.FindPropertyRelative("Type");

        Rect typeRect = new(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
        EditorGUI.PropertyField(typeRect, typeProp);

        position.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

        EditorGUI.EndProperty();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        int lines = 2;
        return lines * EditorGUIUtility.singleLineHeight + (lines - 1) * EditorGUIUtility.standardVerticalSpacing;
    }
}
