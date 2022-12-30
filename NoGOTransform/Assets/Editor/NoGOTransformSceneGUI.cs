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

    List<NoGOTransform> RecurseFieldSearch(FieldInfo fieldType, Type searchFor, List<NoGOTransform> found)
    {
        FieldInfo[] fields = fieldType.FieldType.GetFields();
        if (fields.Length == 0)
        {
            return found;
        }

        foreach (FieldInfo t in fields)
        {
            if (t.FieldType.IsPrimitive)
            {
                continue;
            }

            if (t.FieldType.Equals(searchFor))
            {
                Debug.Log("Found type: " + t.FieldType.Name);
                FieldInfo[] f = t.FieldType.GetFields();

                NoGOTransform newNoGO = new NoGOTransform();
                for (int j = 0; j < f.Length; j++)
                {
                    Debug.Log(j + ": " + f[j]);
                    f[j].GetValue(newNoGO);
                }
                Debug.Log(newNoGO);

                found.Add(newNoGO);
                return found;
            }
            else
            {
                if (!t.FieldType.IsPrimitive)
                {
                    FieldInfo[] fs = t.FieldType.GetFields();
                    foreach (FieldInfo f in fs)
                    {
                        Debug.Log("Recusing on: " + f.FieldType.Name);
                        return RecurseFieldSearch(f, searchFor, found);
                    }
                }
            }
        }
        return found;
    }

    void OnSceneGUI()
    {
        Debug.Log("Drawning");
        Handles.BeginGUI();

        GameObject targetGO = (GameObject)target;
        Component[] scripts = targetGO.GetComponents(typeof(Component));

        for (int i = 0; i < scripts.Length; i++)
        {
            Type myObjectType = scripts[i].GetType();

            //List<FieldInfo> found = RecurseFieldSearch(myObjectType, typeof(NoGOTransform), new List<FieldInfo>());

            FieldInfo[] fields = myObjectType.GetFields();
            for (int j = 0; j < fields.Length; j++)
            {
                Debug.Log(j + ": " + fields[j].FieldType.Name);
                //if (fields[j].FieldType == typeof(FlatMeshGeneratorNode))
               // {
                    Debug.Log("ok " + fields[j].FieldType.Name);
                    List<NoGOTransform> found2 = RecurseFieldSearch(fields[j], typeof(NoGOTransform), new List<NoGOTransform>());

                   /* foreach (FieldInfo f in found2)
                    {
                        NoGOTransform nT = new NoGOTransform();
                        Debug.Log(f.GetValue(nT));                  
                    }*/
              //  }
            }
                /*    foreach (PropertyInfo prop in scripts[i].GetType().GetProperties())
                    {
                        try
                        {
                            Debug.Log("Type:  " + prop.PropertyType + " Value:  " + prop.GetValue(scripts[i], null) + "\n");
                            NoGOTransform node = (NoGOTransform)prop.GetValue(scripts[i], null);
                            Handles.color = Color.yellow;
                            bool clicked = Handles.Button(node.position, node.rotation, 0.1f, 0.1f, Handles.SphereHandleCap);
                            if (clicked)
                            {
                                // selected node
                                selected = node;
                                //NoGOTransformPopup.Init(selectedTransform);
                            }

                        } catch (Exception e)
                        {
                            // Debug.LogError(e);
                        }
                    }*/
            }


            /*SerializedProperty pos = this.serializedObject.FindProperty("position");
            SerializedProperty rot = this.serializedObject.FindProperty("rotation");
            Handles.color = Color.yellow;
            bool clicked = Handles.Button(pos.vector3Value, rot.quaternionValue, 0.1f, 0.1f, Handles.SphereHandleCap);
            if (clicked)
            {
                // selected node
                selected = this.serializedObject;
                //NoGOTransformPopup.Init(selectedTransform);
            }

            if (selected != null && selected.Equals(this.serializedObject))
            {
                SerializedProperty selectedPos = selected.FindProperty("position");
                SerializedProperty selectedRot = selected.FindProperty("rotation");
                SerializedProperty selectedScale = selected.FindProperty("scale");

                Vector3 sPos = selectedPos.vector3Value;
                Quaternion sRot = selectedRot.quaternionValue;
                Vector3 sScale = selectedScale.vector3Value;

                Handles.TransformHandle(ref sPos, ref sRot, ref sScale);
                NoGOTransformPopup.focusedWindow?.Repaint();
            }*/

            Handles.EndGUI();
    }
}