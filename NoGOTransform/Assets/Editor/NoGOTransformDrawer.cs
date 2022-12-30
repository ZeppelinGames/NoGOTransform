using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(NoGOTransform))]
public class NoGOTransformDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        float hWidth = position.width / 2;
        Rect positionLabelRect = new Rect(position.x, position.y, hWidth, 30);
        Rect rotationLabelRect = new Rect(position.x, position.y + 30, hWidth, 30);

        Rect positionRect = new Rect(position.x + hWidth, position.y, hWidth, 30);
        Rect rotationRect = new Rect(position.x + hWidth, position.y + 30, hWidth, 30);

        EditorGUI.LabelField(positionLabelRect, "Position");
        EditorGUI.LabelField(rotationLabelRect, "Rotation");

        EditorGUI.PropertyField(positionRect, property.FindPropertyRelative("position"), GUIContent.none);
        EditorGUI.PropertyField(rotationRect, property.FindPropertyRelative("rotation"), GUIContent.none);

        EditorGUI.EndProperty();
    }
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {

        float totalHeight = EditorGUI.GetPropertyHeight(property, label, true) + EditorGUIUtility.standardVerticalSpacing;
        totalHeight += EditorGUI.GetPropertyHeight(property.FindPropertyRelative("position"));
        totalHeight += EditorGUI.GetPropertyHeight(property.FindPropertyRelative("rotation"));

        return totalHeight;
    }
}