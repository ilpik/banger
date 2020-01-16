using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.SoundGeneration
{
    [CustomEditor(typeof(SoundGeneratorControls))]
    public class SoundGeneratorControlsEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            SoundGeneratorControls controls = (SoundGeneratorControls)target;

            if (GUILayout.Button("Reset composition"))
            {
                controls.UpdateComposition();
            }

            if (GUILayout.Button("Clear composition"))
            {
                controls.ClearComposition();
            }

            //serializedObject.Update();
            ////EditorGUIUtility.LookLikeInspector();
            //SerializedProperty tps = serializedObject.FindProperty("waveForm");
            //EditorGUI.BeginChangeCheck();
            //EditorGUILayout.PropertyField(tps, true);
            //if (EditorGUI.EndChangeCheck())
            //    serializedObject.ApplyModifiedProperties();
            ////EditorGUIUtility.LookLikeControls();

        }
    }
}