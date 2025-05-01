using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(QuestObjectiveData))]
public class QuestObjectiveDataDrawer : PropertyDrawer
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
        EditorGUI.LabelField(labelRect, "Objective", style);
        position.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

        var typeProp = property.FindPropertyRelative("Type");

        Rect typeRect = new(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
        EditorGUI.PropertyField(typeRect, typeProp);

        position.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

        QuestObjectiveType type = (QuestObjectiveType)typeProp.enumValueIndex;

        if (type == QuestObjectiveType.HitFinish)
        {
            var finishMultiplierProp = property.FindPropertyRelative("FinishMultiplierToHit");
            Rect finishRect = new(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
            EditorGUI.PropertyField(finishRect, finishMultiplierProp, new GUIContent("Finish Multiplier To Hit"));
        }
        else if (type == QuestObjectiveType.HitPins)
        {
            var pinsCountProp = property.FindPropertyRelative("PinsCount");
            Rect pinsCountRect = new(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
            EditorGUI.PropertyField(pinsCountRect, pinsCountProp, new GUIContent("Pins Count"));
            position.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            var pinTypeProp = property.FindPropertyRelative("PinType");
            Rect pinTypeRect = new(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
            EditorGUI.PropertyField(pinTypeRect, pinTypeProp, new GUIContent("Pins Type"));
        }
        else if (type == QuestObjectiveType.EarnCoins)
        {
            var coinsCountProp = property.FindPropertyRelative("CoinsCount");
            Rect coinsCountRect = new(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
            EditorGUI.PropertyField(coinsCountRect, coinsCountProp, new GUIContent("Coins Count"));
        }

        EditorGUI.EndProperty();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        var typeProp = property.FindPropertyRelative("Type");
        QuestObjectiveType type = (QuestObjectiveType)typeProp.enumValueIndex;
        int lines = 1;
        lines++;

        if (type == QuestObjectiveType.HitFinish)
        {
            lines++;
        }
        else if (type == QuestObjectiveType.HitPins)
        {
            lines += 2;
        }
        else if (type == QuestObjectiveType.EarnCoins)
        {
            lines++;
        }

        return lines * EditorGUIUtility.singleLineHeight + (lines - 1) * EditorGUIUtility.standardVerticalSpacing;
    }
}
