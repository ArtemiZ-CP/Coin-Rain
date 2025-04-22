using UnityEditor;
using UnityEngine;

public class ReadOnlyAttribute : PropertyAttribute { }

#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(ReadOnlyAttribute))]
public class ReadOnlyDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        GUI.enabled = false;
        EditorGUI.PropertyField(position, property, label, true);
        GUI.enabled = true;
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        if (!property.isArray || property.propertyType == SerializedPropertyType.String || !property.isExpanded)
        {
            return EditorGUI.GetPropertyHeight(property, label, true);
        }

        // Высота заголовка + высота строки с размером + высота всех элементов
        float height = EditorGUIUtility.singleLineHeight * 2; // Заголовок и строка с размером

        for (int i = 0; i < property.arraySize; i++)
        {
            SerializedProperty element = property.GetArrayElementAtIndex(i);
            height += EditorGUI.GetPropertyHeight(element, true);
        }

        return height;
    }
}
#endif