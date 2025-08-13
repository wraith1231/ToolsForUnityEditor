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

    [MenuItem("Tools/State Maker/Make Player State")]
    static void CustomPlayerMenu()
    {
        EditorWindow.GetWindow(typeof(PlayerStateMakerWindow), false, "Player Super State Maker");
    }

    [MenuItem("Tools/State Maker/Make Enemy State")]
    static void CustomEnemyMenu()
    {
        //TODO : Enemy는 Behavior Tree로 만들건데 아직 노드 구성 어떻게 할지 안정함, IState 재활용 안할거라 일단 따로 뺌
    }
}

public class PlayerStateMakerWindow : EditorWindow
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
