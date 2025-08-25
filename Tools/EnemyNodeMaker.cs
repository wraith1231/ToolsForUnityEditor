using NUnit.Framework;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

public class EnemyNodeMaker : EditorWindow
{
    public static string NodePath = "Assets/Scripts/Enemy/Behaviors/Logics/Leafs";
    public static string NodeEditorPath = "Assets/Scripts/Enemy/Behaviors/Logics/LeafForEditor/";

    private ReorderableList _reorderableList;
    private int _selected = -1;
    private Vector2 _scrollPosition;

    private List<string> _nodeList;

    [MenuItem("Tools/State Maker/Make Enemy Behavior Tree Node")]
    static void CustomPlayerMenu()
    {
        EditorWindow.GetWindow(typeof(EnemyNodeMaker), false, "Enemy Behavior Node Maker");
    }

    private void LoadNodeList()
    {
        string[] nodes = Directory.GetFiles(NodePath, "*Node.cs");

        _nodeList = new List<string>(nodes);

        int count = _nodeList.Count;
        for(int i = 0; i < count; i++)
        {
            _nodeList[i] = Path.GetFileName(_nodeList[i]);
            Debug.Log(_nodeList[i]);
        }
    }
    private void LoadNode()
    {

    }

    private void SaveNodesAll()
    {

    }

    private void OnEnable()
    {
        LoadNodeList();
    }

    private void OnGUI()
    {
        //toolbar
        GUILayout.BeginHorizontal();

        //Left List
        GUILayout.BeginVertical(GUILayout.Width(position.width * 0.3f));
        //_reorderableList.DoLayoutList();
        GUILayout.EndVertical();

        //Right Data
        GUILayout.BeginVertical();
        //_scrollPosition = GUILayout.BeginScrollView(_scrollPosition);
        //if (_selected >= 0 && _selected < _container.AttackData.Count)
        //{
        //    AttackData selected = _container.AttackData[_selected];
        //    DrawAttackData(selected);
        //}
        GUILayout.EndScrollView();
        GUILayout.EndVertical();

        GUILayout.EndHorizontal();
    }

    private void CreateList()
    {
        /*Debug.Log("Create!!");
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

        };*/
    }

    #region Leaf Template
    public static string LeafNodeTemplate = $@"using UnityEngine;

public class _Name_Node : IBehavior
{{
    _Vars_    

    public _Name_Node(EnemyBehaviorTree tree, IBehavior parent) : base(tree, parent)
    {{
    }}

    public override BehaviorState Execute()
    {{
        _Actions_        

        return BehaviorState.Success;
    }}
}}";
    public static string LeafNodeScriptableTemplate = $@"using UnityEngine;
[CreateAssetMenu(fileName ""NewActionNode"", menuName = ""AI/Action Node"")]
public class ActionNode : ScriptableObject
{{
    public string _nodeName = ""New Action"";
    public string _description = ""description for node's role"";
    
    
}}
";
    /*
     * public class SelectorNode : IBehavior
{
    public SelectorNode(EnemyBehaviorTree tree, IBehavior parent) : base(tree, parent)
    {
    }

    public override BehaviorState Execute()
    {
        return BehaviorState.Success;
    }
}
     */
    #endregion
}
