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

        QuestRewardType type = (QuestRewardType)typeProp.enumValueIndex;

        if (type == QuestRewardType.Coins)
        {
            var finishMultiplierProp = property.FindPropertyRelative("CoinsCount");
            Rect finishRect = new(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
            EditorGUI.PropertyField(finishRect, finishMultiplierProp, new GUIContent("Coins"));
        }
        else if (type == QuestRewardType.UnlockUpgrade)
        {
            var upgradeTypeProp = property.FindPropertyRelative("PinType");
            Rect upgradeRect = new(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
            EditorGUI.PropertyField(upgradeRect, upgradeTypeProp, new GUIContent("Pin Type"));
        }
        else if (type == QuestRewardType.IncreaseBallSize)
        {
            var increaseValueProp = property.FindPropertyRelative("IncreaseValue");
            Rect increaseRect = new(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
            EditorGUI.PropertyField(increaseRect, increaseValueProp, new GUIContent("Increase Value"));
        }

        EditorGUI.EndProperty();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        var typeProp = property.FindPropertyRelative("Type");
        QuestRewardType type = (QuestRewardType)typeProp.enumValueIndex;
        int lines = 1;
        lines++;

        if (type == QuestRewardType.Coins)
        {
            lines++;
        }
        else if (type == QuestRewardType.UnlockUpgrade)
        {
            lines++;
        }
        else if (type == QuestRewardType.IncreaseBallSize)
        {
            lines++;
        }

        return lines * EditorGUIUtility.singleLineHeight + (lines - 1) * EditorGUIUtility.standardVerticalSpacing;
    }
}
