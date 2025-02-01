#if UNITY_EDITOR
using System;
using UnityEditor;
using UnityEngine;
using WaypointSystem;

namespace Editor
{
    [CustomEditor(typeof(Waypoint))]
    public class WaypointEditor : UnityEditor.Editor
    {
        Waypoint Waypoint => target as Waypoint;

        [Obsolete("Obsolete")]
        private void OnSceneGUI()
        {
            Handles.color = Color.cyan;

            for (int i = 0; i < Waypoint.Points.Length; i++)
            {
                EditorGUI.BeginChangeCheck();

                Vector3 currentWaypointPoint = Waypoint.CurrentPosition + Waypoint.Points[i];
                Vector3 newWaypointPoint = Handles.FreeMoveHandle(currentWaypointPoint, Quaternion.identity, 0.7f,
                    new Vector3(0.3f, 0.3f, 0.3f), Handles.SphereHandleCap);

                GUIStyle textStyle = new GUIStyle
                {
                    fontStyle = FontStyle.Bold,
                    fontSize = 16,
                    normal =
                    {
                        textColor = Color.white
                    }
                };
                Vector3 textAlignment = Vector3.down * 0.35f + Vector3.right * 0.35f;
                Handles.Label(Waypoint.CurrentPosition + Waypoint.Points[i] + textAlignment, $"{i + 1}", textStyle);
                EditorGUI.EndChangeCheck();

                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(target, "Free Move Handle");
                    Waypoint.Points[i] = newWaypointPoint - Waypoint.CurrentPosition;
                }
            }
        }
    }
}
#endif