using NUnit.Framework;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

public class EnemyNodeMaker : EditorWindow
{
    public static string NodePath = "Assets/Scripts/Enemy/Behaviors/Logics/Leafs/";

    private string _nodeName = "";

    [MenuItem("Tools/State Maker/Make Enemy Behavior Tree Node")]
    static void CustomPlayerMenu()
    {
        EditorWindow.GetWindow(typeof(EnemyNodeMaker), false, "Enemy Behavior Node Maker");
    }

    private void OnGUI()
    {
        _nodeName = EditorGUILayout.TextField("Node Name(Only Node name, ex:Attack)", _nodeName);

        if(GUILayout.Button("Make Leaf"))
        {
            if (_nodeName.Length == 0)
            {
                Debug.LogError("Must Input Node Name");
                return;
            }

            string path = EnemyNodeMaker.NodePath + $"{_nodeName}Node.cs";
            if (File.Exists(path))
            {
                EditorUtility.DisplayDialog("Warning", "File Already Exists", "OK");
                return;
            }
            try
            {
                using (StreamWriter writer = new StreamWriter(path))
                {
                    string data = EnemyNodeMaker.LeafNodeTemplate;
                    data = data.Replace("_Name_", _nodeName);

                    Debug.Log(data);
                    writer.WriteLine(data);
                }

                AssetDatabase.Refresh();

                Debug.Log($"{_nodeName}Node.cs Made!");
            }
            catch (System.Exception e)
            {
                Debug.Log(e.Message);
            }
        }
        if(GUILayout.Button("Make Condition"))
        {
            if (_nodeName.Length == 0)
            {
                Debug.LogError("Must Input Node Name");
                return;
            }

            string path = EnemyNodeMaker.NodePath + $"{_nodeName}Node.cs";
            if (File.Exists(path))
            {
                EditorUtility.DisplayDialog("Warning", "File Already Exists", "OK");
                return;
            }
            try
            {
                using (StreamWriter writer = new StreamWriter(path))
                {
                    string data = EnemyNodeMaker.ConditionNodeTemplate;
                    data = data.Replace("_Name_", _nodeName);

                    Debug.Log(data);
                    writer.WriteLine(data);
                }

                AssetDatabase.Refresh();

                Debug.Log($"{_nodeName}Node.cs Made!");
            }
            catch (System.Exception e)
            {
                Debug.Log(e.Message);
            }
        }
    }

    #region Leaf Template
    public static string LeafNodeTemplate = $@"using UnityEngine;

public class _Name_Node : IBehavior
{{
    public _Name_Node(EnemyBehaviorTree tree, IBehavior parent) : base(tree, parent)
    {{
    }}

    public override BehaviorState Execute()
    {{


        return BehaviorState.Success;
    }}
}}";
    #endregion

    #region Condition Template
    public static string ConditionNodeTemplate = $@"using UnityEngine;

public class _Name_Node : IBehavior
{{
    public _Name_Node(EnemyBehaviorTree tree, IBehavior parent) : base(tree, parent)
    {{
    }}

    public override BehaviorState Execute()
    {{
        ConditionCheck();

        return BehaviorState.Success;
    }}

    public bool ConditionCheck()
    {{
        return true;
    }}
}}";
    #endregion
}
