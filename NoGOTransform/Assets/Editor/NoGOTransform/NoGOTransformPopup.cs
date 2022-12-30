using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEditor.Events;
using Events = UnityEditor.Events;

public class NoGOTransformPopup : EditorWindow
{
    static NoGOTransformPopup window;
    static EditorWindow sceneView;

    static Rect sceneViewRect;
    const float padding = 10f;

    static NoGOTransform popupNode;

    private const int windowWidth = 250;
    private const int windowHeight = 125;

    public static void Init(NoGOTransform node)
    {
        popupNode = node;

        if (window == null)
        {
            window = CreateInstance<NoGOTransformPopup>();
        }
        if (sceneView == null)
        {
            sceneView = GetWindow<SceneView>();
            sceneViewRect = sceneView.position;
        }

        Reposition();
        window.ShowPopup();
    }

    void OnGUI()
    {
        if (popupNode == null)
        {
            ClosePopup();
            return;
        }

        popupNode.position = EditorGUILayout.Vector3Field("Position", popupNode.position);
        popupNode.eulerAngles = EditorGUILayout.Vector3Field("Rotation", popupNode.eulerAngles);
        popupNode.scale = EditorGUILayout.Vector3Field("Scale", popupNode.scale);
    }

    public static void ClosePopup()
    {
        if (window != null)
        {
            popupNode = null;
            window.Close();
        }

        if (HasOpenInstances<NoGOTransformPopup>())
        {
            EditorWindow window = GetWindow(typeof(NoGOTransformPopup));
            window.Close();
        }
    }

    public static void Update()
    {
        // the != 1 is a weird thing that it defaults to when you scale the scene view too fast?
        // plz dont move your scene view 1 pixel in :) (someone if you know a fix / better way to do this plz fix)
        if(sceneView == null || window == null)
        {
            return;
        }
        if (sceneView.position != sceneViewRect && sceneView.position.x != 1)
        {
            sceneViewRect = sceneView.position;
            Debug.Log(sceneView.position);
            Reposition();
        }
        window.Repaint();
    }

    private static void Reposition()
    {
        window.position = new Rect(sceneView.position.x + padding, (sceneView.position.y + sceneView.position.height) - windowHeight + padding, windowWidth, windowHeight);
    }
}