using UnityEditor;
using UnityEngine;

public class AnimationEditor : EditorWindow
{
    private ModelImporter _importer = null;
    bool _rotate;
    bool _pos;
    bool _loop;
    bool _jitter;
    ModelImporterAnimationType _type;

    [UnityEditor.MenuItem("Tools/Animation Editor")]
    public static void OpenGui()
    {
        EditorWindow.GetWindow(typeof(AnimationEditor), false, "Animation Editor");
    }

    private void OnGUI()
    {
        if(_importer != null)
        {
            //lock rotate, loock pos xz, loop time, defending shaking, animation type
            GUILayout.Label($"Set {_importer.assetPath} options");
            _type = (ModelImporterAnimationType)EditorGUILayout.EnumPopup("Attack Type", _type);
            _jitter = EditorGUILayout.Toggle("Enable Anti-Jitter", _jitter);
            
            _loop = EditorGUILayout.Toggle("Loop Time", _loop);
            _pos = EditorGUILayout.Toggle("Lock Position XZ", _pos);
            _rotate = EditorGUILayout.Toggle("Lock Rotate", _rotate);

            if (GUILayout.Button("Apply Animation Setting"))
            {
                WorkAnimation(_importer, _type, _jitter, _rotate, _pos, _loop);
            }
        }
        if (GUILayout.Button("Open File"))
        {
            string absolutePath = EditorUtility.OpenFilePanel("Select a file", "", "fbx");
            if (string.IsNullOrEmpty(absolutePath) == true)
            {
                Debug.LogError("Can't Open File!");
                return;
            }
            string path = "Assets" + absolutePath.Replace(Application.dataPath, "");
            Debug.Log(path);
            _importer = AssetImporter.GetAtPath(path) as ModelImporter;

            if(_importer == null)
            {
                Debug.LogError("It looks like not fbx!");
                return;
            }

        }
    }

    private void WorkAnimation(ModelImporter importer, ModelImporterAnimationType type, bool jitter, bool rotate, bool pos, bool loop)
    {
        importer.animationType = type;
        //��鸲 ���� ������
        if (jitter == true)
        {
            importer.bakeIK = jitter;
            importer.animationPositionError = 0f;
            importer.animationRotationError = 0f;
            importer.animationScaleError = 0f;
            //off�� �ϸ� ���� ���� �ִ뼭 �ϴ� ��α� �ߴµ� ����
            //importer.animationCompression = ModelImporterAnimationCompression.Off;
        }

        ModelImporterClipAnimation[] clips = importer.clipAnimations;
        Debug.Log(importer.clipAnimations.Length);
        if (clips == null || clips.Length == 0)
        {
            clips = importer.defaultClipAnimations;
        }

        int size = clips.Length;
        for (int i = 0; i < size; i++)
        {
            clips[i].lockRootRotation = rotate;
            clips[i].lockRootPositionXZ = pos;
            clips[i].loopTime = loop;

            clips[i].keepOriginalOrientation = true;
        }

        importer.clipAnimations = clips;
        importer.SaveAndReimport();

        Debug.Log("It's over!");
    }
}
