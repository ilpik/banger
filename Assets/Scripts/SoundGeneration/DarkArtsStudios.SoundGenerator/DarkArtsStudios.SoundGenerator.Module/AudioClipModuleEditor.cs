using UnityEditor;
using UnityEngine;

namespace DarkArtsStudios.SoundGenerator.Module
{
	[CustomEditor(typeof(AudioClip), true)]
	internal class AudioClipModuleEditor : BaseModuleEditor
	{
		public override Rect OnModuleGUI(Rect innerRect)
		{
			Rect rect = base.OnModuleGUI(innerRect);
			AudioClip audioClip = base.target as AudioClip;
			Rect position = new Rect(innerRect.x, rect.y + rect.height, innerRect.width, BaseModuleEditor.AttributeHeight);
			EditorGUI.BeginChangeCheck();
			audioClip.audioClip = (UnityEngine.AudioClip)EditorGUI.ObjectField(position, audioClip.audioClip, typeof(UnityEngine.AudioClip), allowSceneObjects: false);
			if (EditorGUI.EndChangeCheck())
			{
				audioClip.audioData = null;
			}
			innerRect.height = BaseModuleEditor.AttributeHeight;
			return innerRect;
		}
	}
}
