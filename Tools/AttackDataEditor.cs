using NUnit.Framework;
using NUnit.Framework.Constraints;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

[System.Serializable]
public class AttackData
{
    //Enum 되있으면 LoadJsonData 함수랑 DrawAttackData 함수 둘 다 수정해줘야함
    public int ID = -1;
    public AttackType AttackType = AttackType.Normal;
    public AttackCollider AttackCollider = AttackCollider.Thrust;
    public float AttackDamage = 1.0f;

    public HitEffect HitEffect = HitEffect.Physical;
    public string HitSound = "";
    public string Comment = "";
}
[System.Serializable]
public class AttackDataContainer
{
    public List<AttackData> AttackData = new List<AttackData>();
}

public static class EnumExtensions
{
    public static bool IsDefined<TEnum>(this TEnum value) where TEnum : Enum
    {
        return Enum.IsDefined(typeof(TEnum), value);
    }
}

public class AttackDataEditor : EditorWindow
{
    public static string DataPath = "Assets/Data/AttackData.json";

    private string _jsonContent;
    private AttackDataContainer _container;

    private ReorderableList _reorderableList;
    private int _selected = -1;
    private Vector2 _scrollPosition;

    [MenuItem("Tools/Attack Data Editor")]
    static void OpenMenu()
    {
        EditorWindow.GetWindow(typeof(AttackDataEditor), false, "Attack Data Editor");
    }

    private void LoadJsonData()
    {
        if (File.Exists(DataPath) == false)
        {
            Debug.Log("Make json!");
            StreamWriter writer = new StreamWriter(DataPath);
            writer.WriteLine("{}");
            AssetDatabase.Refresh();

            writer.Close();
        }

        _jsonContent = File.ReadAllText(DataPath);
        _container = JsonUtility.FromJson<AttackDataContainer>(_jsonContent);

        foreach(var data in _container.AttackData)
        {
            if (data.AttackType.IsDefined() == false)
                data.AttackType = AttackType.Normal;

            if (data.AttackCollider.IsDefined() == false)
                data.AttackCollider = AttackCollider.Thrust;

            if (data.HitEffect.IsDefined() == false)
                data.HitEffect = HitEffect.Physical;
        }
        Debug.Log("Load");
    }

    private void SaveData()
    {
        if (File.Exists(DataPath) == false)
        {
            Debug.Log("Make json!");
            StreamWriter writerr = new StreamWriter(DataPath);
            writerr.WriteLine("{}");
            AssetDatabase.Refresh();

            writerr.Close();
        }

        string json = JsonUtility.ToJson(_container, true);
        File.WriteAllText(DataPath, json);
        AssetDatabase.Refresh();
        Debug.Log("Save");
    }

    public void OnEnable()
    {
        LoadJsonData();
        CreateList();
    }

    private void OnGUI()
    {
        //toolbar
        GUILayout.BeginHorizontal();
        if(GUILayout.Button("Save All", EditorStyles.toolbarButton))
        {
            SaveData();
        }
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();

        //Left List
        GUILayout.BeginVertical(GUILayout.Width(position.width * 0.3f));
        _reorderableList.DoLayoutList();
        GUILayout.EndVertical();

        //Right Data
        GUILayout.BeginVertical();
        _scrollPosition = GUILayout.BeginScrollView(_scrollPosition);
        if(_selected >= 0 && _selected < _container.AttackData.Count)
        {
            AttackData selected = _container.AttackData[_selected];
            DrawAttackData(selected);
        }
        GUILayout.EndScrollView();
        GUILayout.EndVertical();

        GUILayout.EndHorizontal();
    }

    private void CreateList()
    {
        Debug.Log("Create!!");
        _reorderableList = new ReorderableList(_container.AttackData, typeof(AttackData), true, true, true, true);

        _reorderableList.drawHeaderCallback = (Rect rect) =>
        {
            EditorGUI.LabelField(rect, "Attack List");
        };

        _reorderableList.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
        {
            AttackData data = _container.AttackData[index];
            rect.y += 2;
            EditorGUI.LabelField(new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight), data.ID.ToString());
        };

        _reorderableList.onSelectCallback = (ReorderableList list) =>
        {
            _selected = list.index;
        };

        _reorderableList.onAddCallback = (ReorderableList list) =>
        {
            AttackData data = new AttackData();
            data.ID = _container.AttackData.Count;
            _container.AttackData.Add(data);
            list.index = _container.AttackData.Count - 1;
            _selected = list.index;
        };

        _reorderableList.onRemoveCallback = (ReorderableList list) =>
        {
            int index = list.index;
            
            _container.AttackData.RemoveAt(index);

            for (int i = index; i < _container.AttackData.Count; i++)
                _container.AttackData[i].ID -= 1;

        };
    }

    private void DrawAttackData(AttackData data)
    {
        GUILayout.Label($"ID : {data.ID}");
        data.AttackType = (AttackType)EditorGUILayout.EnumPopup("Attack Type", data.AttackType);
        data.AttackCollider = (AttackCollider)EditorGUILayout.EnumPopup("Attack Collider", data.AttackCollider);
        data.AttackDamage = EditorGUILayout.FloatField("Attack Damage", data.AttackDamage);
        data.HitEffect = (HitEffect)EditorGUILayout.EnumPopup("Hit Effect", data.HitEffect);
        data.HitSound = EditorGUILayout.TextField("Hit Sound", data.HitSound);
        data.Comment = EditorGUILayout.TextField("Comment", data.Comment);
    }
}
