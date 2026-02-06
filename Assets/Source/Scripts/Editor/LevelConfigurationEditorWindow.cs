using UnityEditor;
using UnityEngine;

public class LevelConfigurationEditorWindow : EditorWindow
{
    LevelConfiguration _configuration;
    SerializedObject _serializedConfiguration;
    SerializedProperty _widthProperty;
    SerializedProperty _heightProperty;
    SerializedProperty _gridProperty;
    GameBlockType _selectedBlockType = GameBlockType.Fire;
    const float CellSize = 48f;

    [MenuItem("Window/Level Configuration Editor")]
    public static void Open()
    {
        GetWindow<LevelConfigurationEditorWindow>("Level Map Editor");
    }

    void OnEnable()
    {
        RefreshSerialized();
    }

    void OnSelectionChange()
    {
        if (Selection.activeObject is LevelConfiguration selected)
        {
            _configuration = selected;
            RefreshSerialized();
            Repaint();
        }
    }

    void RefreshSerialized()
    {
        if (_configuration == null)
        {
            _serializedConfiguration = null;
            _widthProperty = null;
            _heightProperty = null;
            _gridProperty = null;
            return;
        }
        _serializedConfiguration = new SerializedObject(_configuration);
        _widthProperty = _serializedConfiguration.FindProperty("_width");
        _heightProperty = _serializedConfiguration.FindProperty("_height");
        _gridProperty = _serializedConfiguration.FindProperty("_grid");
    }

    void OnGUI()
    {
        EditorGUILayout.LabelField("Level Configuration", EditorStyles.boldLabel);
        EditorGUI.BeginChangeCheck();
        _configuration = (LevelConfiguration)EditorGUILayout.ObjectField("Configuration", _configuration, typeof(LevelConfiguration), false);
        if (EditorGUI.EndChangeCheck())
            RefreshSerialized();

        if (_configuration == null)
        {
            EditorGUILayout.HelpBox("Назначь Level Configuration или создай через Assets > Create > Game > Level Configuration.", MessageType.Info);
            return;
        }

        _serializedConfiguration.Update();

        EditorGUILayout.Space(8f);
        _selectedBlockType = (GameBlockType)EditorGUILayout.EnumPopup("Тип блока", _selectedBlockType);

        int width = _widthProperty.intValue;
        int height = _heightProperty.intValue;
        if (width < 1) width = 1;
        if (height < 1) height = 1;
        if (_gridProperty.arraySize != width * height)
        {
            _gridProperty.arraySize = width * height;
            _widthProperty.intValue = width;
            _heightProperty.intValue = height;
        }

        EditorGUILayout.Space(8f);
        EditorGUILayout.LabelField("Сетка (клик по ячейке — поставить выбранный тип)");
        DrawGrid(width, height);

        EditorGUILayout.Space(12f);
        if (GUILayout.Button("Сохранить", GUILayout.Height(28f)))
            Save();

        _serializedConfiguration.ApplyModifiedPropertiesWithoutUndo();
    }

    void DrawGrid(int width, int height)
    {
        Rect area = GUILayoutUtility.GetRect(width * CellSize, height * CellSize);
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                int index = y * width + x;
                Rect cell = new Rect(area.x + x * CellSize, area.y + y * CellSize, CellSize - 2f, CellSize - 2f);
                GameBlockType current = (GameBlockType)_gridProperty.GetArrayElementAtIndex(index).enumValueIndex;
                GUI.Box(cell, "");
                if (current != GameBlockType.None)
                    GUI.Label(cell, current.ToString(), new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter });
                if (GUI.Button(cell, "", GUIStyle.none))
                {
                    _gridProperty.GetArrayElementAtIndex(index).enumValueIndex = (int)_selectedBlockType;
                }
            }
        }
    }

    void Save()
    {
        if (_configuration == null)
            return;
        _serializedConfiguration.ApplyModifiedPropertiesWithoutUndo();
        EditorUtility.SetDirty(_configuration);
        AssetDatabase.SaveAssets();
    }
}
