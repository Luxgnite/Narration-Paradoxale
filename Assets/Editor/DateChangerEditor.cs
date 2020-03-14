using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(DateChanger))]
public class DateChangerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DateChanger dateChanger = (DateChanger)target;

        dateChanger.Date = EditorGUILayout.TextField("Date (YYYY/MM/DD)", dateChanger.Date);
        dateChanger.Time = EditorGUILayout.TextField("Time (HH:MM:SS)", dateChanger.Time);

        if (GUILayout.Button("Apply Date & Time"))
        {
            dateChanger.ApplyDateTime();
        }
    }
}
