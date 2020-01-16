using UnityEditor;
using UnityEngine;

namespace DarkArtsStudios.SoundGenerator.DarkArtsStudios.Core
{
	internal static class GradientUtility
	{
		public static Gradient Field(Gradient gradient)
		{
			GradientSerializationWrapper gradientSerializationWrapper = ScriptableObject.CreateInstance<GradientSerializationWrapper>();
			gradientSerializationWrapper.gradient = gradient;
			SerializedObject serializedObject = new SerializedObject(gradientSerializationWrapper);
			SerializedProperty property = serializedObject.FindProperty("gradient");
			EditorGUI.BeginChangeCheck();
			EditorGUILayout.PropertyField(property);
			if (EditorGUI.EndChangeCheck())
			{
				serializedObject.ApplyModifiedProperties();
				return gradientSerializationWrapper.gradient;
			}
			return gradient;
		}

		public static Gradient Field(Rect position, Gradient gradient)
		{
			GradientSerializationWrapper gradientSerializationWrapper = ScriptableObject.CreateInstance<GradientSerializationWrapper>();
			gradientSerializationWrapper.gradient = gradient;
			SerializedObject serializedObject = new SerializedObject(gradientSerializationWrapper);
			SerializedProperty property = serializedObject.FindProperty("gradient");
			EditorGUI.BeginChangeCheck();
			EditorGUI.PropertyField(position, property);
			if (EditorGUI.EndChangeCheck())
			{
				serializedObject.ApplyModifiedProperties();
				return gradientSerializationWrapper.gradient;
			}
			return gradient;
		}
	}
}
