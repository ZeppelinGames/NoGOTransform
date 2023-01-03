using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Transform))]
[CanEditMultipleObjects]
public class NoGOTransformSceneGUI : Editor
{
    private static NoGOTransform selected = null;

    List<NoGOTransform> RecurseFieldSearch<T>(T c, Type searchType, Type searchFor, List<NoGOTransform> found)
    {
        FieldInfo[] fields = searchType.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
        if (fields.Length == 0)
        {
            return found;
        }

        foreach (FieldInfo t in fields)
        {
            if (t.FieldType.Equals(searchFor))
            {
                NoGOTransform newT = (NoGOTransform)t.GetValue(c);
                found.Add(newT);
            }
            else
            {
                //Debug.Log(t.FieldType.Name);
                if (t.FieldType.IsArray /*&& t.FieldType.GetElementType().Equals(searchFor)*/)
                {
                    Array arr = (Array)t.GetValue(c);
                    // Debug.Log("ARRAY " + arr.Length);
                    if (arr != null)
                    {
                        for (int i = 0; i < arr.Length; i++)
                        {
                            NoGOTransform nogo = (NoGOTransform)arr.GetValue(i);
                            found.Add(nogo);
                        }
                    }
                    else
                    {
                        //Debug.Log("Null array");
                    }
                    return found;
                }


                if (t.FieldType.IsClass)
                {
                    //Debug.Log("IS CLASS");
                    object val = t.GetValue(c);
                    if (val != null)
                    {
                        return RecurseFieldSearch(val, t.FieldType, searchFor, found);
                    }
                }
            }
        }
        return found;
    }

    private void OnDisable()
    {
        selected = null;
        NoGOTransformPopup.ClosePopup();
    }

    void OnSceneGUI()
    {
        //Handles.BeginGUI();

        GameObject go = ((Transform)target).gameObject;

        MonoBehaviour[] comps = go.GetComponents<MonoBehaviour>();

        for (int i = 0; i < comps.Length; i++)
        {
            Type compType = comps[i].GetType();
            List<NoGOTransform> transforms = RecurseFieldSearch(comps[i], compType, typeof(NoGOTransform), new List<NoGOTransform>());

            Handles.color = Color.yellow;
            foreach (NoGOTransform t in transforms)
            {
                bool clicked = Handles.Button(t.position, Quaternion.identity, 0.1f, 0.1f, Handles.SphereHandleCap);
                if (clicked)
                {
                    // selected node
                    selected = t;
                    NoGOTransformPopup.Init(t);
                }
            }
        }

        if (selected != null)
        {
            Handles.TransformHandle(ref selected.position, ref selected.rotation, ref selected.scale);
            NoGOTransformPopup.Update();
        }
    }

    public override void OnInspectorGUI()
    {
        //base.OnInspectorGUI(); Unity doin waacky shit. Dont ask

        Transform t = (Transform)target;
        t.localPosition = EditorGUILayout.Vector3Field("Position", t.localPosition);
        t.eulerAngles = EditorGUILayout.Vector3Field("Rotation", t.eulerAngles);
        t.localScale = EditorGUILayout.Vector3Field("Scale", t.localScale);
    }
}