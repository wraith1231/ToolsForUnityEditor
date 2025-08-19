using UnityEngine;
using UnityEditor;
using System.IO;
using NUnit.Framework;
using System.Collections.Generic;
using UnityEditorInternal;
using Unity.VisualScripting;
using System.Security.Cryptography;


public class PlayerStateMaker : EditorWindow
{
    public static string PlayerSuperPath = "Assets/Scripts/Player/States/Super/";
    public static string PlayerSubPath = "Assets/Scripts/Player/States/Sub/";

    private string _stateName = "";
    private string _valList = "";
    private string _superName = "";
    private bool _isSub = false;

    //input list
    private List<bool> _buttons = new List<bool>();

    #region Player State Template
    public static string PlayerStateTemplate = $@"using UnityEngine;

public class P_State_State : IState
{{
    _SuperStateData_

    public P_State_State(PlayerStateMachine stateMachine_SuperState__Value_) : base(stateMachine)
    {{
        _SuperInit_
    }}

    public override void Enter()
    {{
        _InputActionEnter_
    }}

    public override void Exit()
    {{
        _InputActionExit_
    }}

    public override void Update()
    {{

    }}

    _InputActions_
}}";

    #endregion

    [MenuItem("Tools/State Maker/Make Player State")]
    static void CustomPlayerMenu()
    {
        EditorWindow.GetWindow(typeof(PlayerStateMaker), false, "Player State Maker");
    }

    private void OnEnable()
    {
        int size = (int)ButtonInput.Unknown;
        for (int i = 0; i < size; i++)
            _buttons.Add(false);
    }

    private void DrawButtonInput(ButtonInput button)
    {
        _buttons[(int)button] = EditorGUILayout.Toggle($"{button.ToString()} Input", _buttons[(int)button]);
    }

    private void OnGUI()
    {
        _stateName = EditorGUILayout.TextField("State Name(Only state name, ex:Normal)", _stateName);
        _valList = EditorGUILayout.TextField("State Value List", _valList);

        _isSub = EditorGUILayout.Toggle("Sub State", _isSub);

        if(_isSub == true)
        {
            _superName = EditorGUILayout.TextField("Super State Name", _superName);
        }
/*
        if (_buttons.Count == 0)
            ButtonInputListInit();*/
        #region button input list
        int size = (int)ButtonInput.Unknown;
        for (int i = 0; i < size; i++)
            DrawButtonInput((ButtonInput)i);
        #endregion 

        if (_isSub == false)
        {
            if (GUILayout.Button("Make Super"))
            {
                string path = PlayerStateMaker.PlayerSuperPath + $"P{_stateName}State.cs";
                MakeState(path);
            }
        }

        if (_isSub == true)
        {
            if (GUILayout.Button("Make Sub"))
            {
                if (_superName.Length == 0)
                {
                    Debug.LogError("Sub State must need a super state");
                    return;
                }

                string path = PlayerStateMaker.PlayerSubPath + $"P{_stateName}State.cs";
                MakeState(path);
            }
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
                string data = PlayerStateMaker.PlayerStateTemplate;
                data = data.Replace("_State_", _stateName);

                if (_isSub == true)
                {
                    string superString = $"private P{_superName}State _{_superName.FirstCharacterToLower()}State;";
                    //_SuperStateData_
                    //_SuperInit_
                    data = data.Replace("_SuperStateData_", superString);
                    data = data.Replace("_SuperState_", $", P{_superName}State {_superName}");
                    data = data.Replace("_SuperInit_", $"_{_superName.FirstCharacterToLower()}State = {_superName};");
                }
                else
                {
                    data = data.Replace("_SuperStateData_", "");
                    data = data.Replace("_SuperState_", "");
                    data = data.Replace("_SuperInit_", "");
                }

                if (_valList.Length > 0)
                {
                    data = data.Replace("_Value_", $", {_valList}");
                }
                else
                {
                    data = data.Replace("_Value_", "");
                }

                //_InputActions_
                int size = (int)ButtonInput.Unknown;
                string buttonData = "";
                string buttonEnter = "";
                string buttonExit = "";
                for(int i = 0; i < size; i++)
                {
                    if (_buttons[i] == true)
                    {
                        buttonData += $@"private void {((ButtonInput)i).ToString()}Input()
    {{

    }}

";
                        buttonEnter += $@"SingletonManager.Input.AddActions(ButtonInput.{((ButtonInput)i).ToString()}, {((ButtonInput)i).ToString()}Input);
        ";
                        buttonExit += $@"SingletonManager.Input.ClearAction(ButtonInput.{((ButtonInput)i).ToString()});
        ";
                    }
                }
                //SingletonManager.Input.AddActions(ButtonInput.Attack, AttackInput);
                data = data.Replace("_InputActionEnter_", buttonEnter);
                data = data.Replace("_InputActionExit_", buttonExit);
                data = data.Replace("_InputActions_", buttonData);

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
