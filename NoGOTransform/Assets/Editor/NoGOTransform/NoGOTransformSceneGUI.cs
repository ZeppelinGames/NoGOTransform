using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(GameObject))]
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
            //Debug.Log(t.Name + " " + t.FieldType.Name);
            if (t.FieldType.IsPrimitive)
            {
                continue;
            }

            if (t.FieldType.Equals(searchFor))
            {
                NoGOTransform newT = (NoGOTransform)t.GetValue(c);
                found.Add(newT);
                return found;
            }
            else
            {
                if (!t.FieldType.IsPrimitive && t.FieldType.IsClass)
                {
                    object val = t.GetValue(c);
                    Debug.Log(val);
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

        GameObject go = (GameObject)target;

        MonoBehaviour[] comps = go.GetComponents<MonoBehaviour>();

        for (int i = 0; i < comps.Length; i++)
        {
            Type compType = comps[i].GetType();
            List<NoGOTransform> transforms = RecurseFieldSearch(comps[i], compType, typeof(NoGOTransform), new List<NoGOTransform>());

            Handles.color = Color.yellow;
            foreach (NoGOTransform t in transforms)
            {
                bool clicked = Handles.Button(t.position, t.rotation, 0.1f, 0.1f, Handles.SphereHandleCap);
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
}