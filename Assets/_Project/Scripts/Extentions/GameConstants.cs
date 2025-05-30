using UnityEngine;
using UnityEditor;
using System;

[CreateAssetMenu(fileName = nameof(GameConstants), menuName = "Game/Constants")]
public class GameConstants : ScriptableObject
{
    private static GameConstants _instance;
    public static GameConstants Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = Resources.Load<GameConstants>(nameof(GameConstants));
            }
            return _instance;
        }
    }

    [SerializeField] private PinConstants _pinConstants;

    public PinConstants PinConstants => _pinConstants;
}

[Serializable]
public struct PinConstants
{
    public PinsLine Line1Prefab;
    public PinsLine Line2Prefab;
    public PinObject PinPrefab;
    public WinArea WinAreaPrefab;
    public float OffsetBetweenPinsInLine;
    public float MultiplyingBallImpulse;
    
    public readonly float OffsetBetweenLines => OffsetBetweenPinsInLine * Mathf.Sqrt(3) / 2;
}

#if UNITY_EDITOR
public class GameConstantsWindow : EditorWindow
{
    private const int MainHeaderSize = 14;
    private const int SubHeaderSize = 13;

    private Vector2 _scrollPosition;
    private GameConstants _constants;
    private string _searchString = "";

    [MenuItem("Tools/Game/Constants Manager")]
    public static void ShowWindow()
    {
        GetWindow<GameConstantsWindow>("Game Constants");
    }

    private void OnEnable()
    {
        _constants = GameConstants.Instance;
    }

    private void OnGUI()
    {
        if (_constants == null)
        {
            EditorGUILayout.HelpBox("GameConstants asset not found in Resources folder!", MessageType.Error);
            return;
        }

        EditorGUILayout.Space(10);
        DrawSearchBar();
        EditorGUILayout.Space(10);

        _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition);

        var serializedObject = new SerializedObject(_constants);
        serializedObject.Update();

        DrawConstantsSections(serializedObject);

        serializedObject.ApplyModifiedProperties();
        EditorGUILayout.EndScrollView();
    }

    private void DrawSearchBar()
    {
        _searchString = EditorGUILayout.TextField("Search", _searchString);
    }

    private void DrawConstantsSections(SerializedObject serializedObject)
    {
        SerializedProperty baseProperty = serializedObject.GetIterator();

        EditorGUILayout.Space(10);
        DisplayHeader(nameof(GameConstants), true);
        EditorGUILayout.Space(5);

        if (baseProperty.NextVisible(true))
        {
            while (baseProperty.NextVisible(false))
            {
                EditorGUILayout.Space(5);

                SerializedProperty copy = baseProperty.Copy();
                SerializedProperty endProperty = copy.GetEndProperty();

                copy.NextVisible(true);

                if (SerializedProperty.EqualContents(copy, endProperty) == false)
                {
                    DisplayHeader(baseProperty.displayName, false);
                }

                while (SerializedProperty.EqualContents(copy, endProperty) == false)
                {
                    if (string.IsNullOrEmpty(_searchString) ||
                        copy.displayName.ToLower().Contains(_searchString.ToLower()))
                    {
                        EditorGUILayout.PropertyField(copy, true);
                    }

                    if (copy.NextVisible(false) == false)
                    {
                        break;
                    }
                }
            }
        }
    }

    private void DisplayHeader(string text, bool main)
    {
        GUIStyle style = new()
        {
            fontSize = main ? MainHeaderSize : SubHeaderSize,
            fontStyle = FontStyle.Bold,
            normal = { textColor = EditorGUIUtility.isProSkin ? Color.white : Color.black }
        };

        EditorGUILayout.LabelField(text, style);
    }

    private bool IsCurrentContext(SerializedProperty property, SerializedProperty startProperty)
    {
        return property.propertyPath.StartsWith(startProperty.propertyPath) &&
               property.propertyPath != startProperty.propertyPath;
    }
}
#endif