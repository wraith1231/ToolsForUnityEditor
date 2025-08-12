using UnityEngine;
using UnityEditor;
using System.IO;
using NUnit.Framework;
using System.Collections.Generic;
using UnityEditorInternal;


public class StateMaker
{
    public static string PlayerSuperPath = "Assets/Scripts/Player/States/Super/";
    public static string PlayerSubPath = "Assets/Scripts/Player/States/Sub/";

    #region Player State Template
    public static string PlayerStateTemplate = $@"using UnityEngine;

public class P_State_State : IState
{{
    public P_State_State(PlayerStateMachine stateMachine_Value_) : base(stateMachine)
    {{
    }}

    public override void Enter()
    {{

    }}

    public override void Exit()
    {{

    }}

    public override void Update()
    {{

    }}
}}";

    #endregion

    [MenuItem("Tools/Make Player Super State")]
    static void CustomMenu()
    {
        EditorWindow.GetWindow(typeof(StateMakerWindow), false, "Player Super State Maker");
    }
}

public class StateMakerWindow : EditorWindow
{
    string _stateName = "";
    string _valList = "";
    
    private void OnGUI()
    {
        _stateName = EditorGUILayout.TextField("State Name(Only state name, ex:Normal)", _stateName);
        _valList = EditorGUILayout.TextField("State Value List", _valList);

        if(GUILayout.Button("Make Super"))
        {
            string path = StateMaker.PlayerSuperPath + $"P{_stateName}State.cs";
            MakeState(path);
        }

        if(GUILayout.Button("Make Sub"))
        {
            string path = StateMaker.PlayerSubPath + $"P{_stateName}State.cs";
            MakeState(path);
        }
    }

    private void MakeState(string path)
    {

        if (File.Exists(path))
        {
            EditorUtility.DisplayDialog("Warning", "File Already Exists", "OK");
            return;
        }

        try
        {
            using (StreamWriter writer = new StreamWriter(path))
            {
                Debug.Log("uhh");
                string data = StateMaker.PlayerStateTemplate;
                data = data.Replace("_State_", _stateName);

                if (_valList.Length > 0)
                {
                    data = data.Replace("_Value_", $", {_valList}");
                }
                else
                {
                    data = data.Replace("_Value_", "");
                }

                Debug.Log(data);
                writer.WriteLine(data);
            }

            AssetDatabase.Refresh();

            Debug.Log($"P{_stateName}State.cs Made!");
        }
        catch (System.Exception e)
        {
            Debug.Log(e.Message);
        }
    }
}