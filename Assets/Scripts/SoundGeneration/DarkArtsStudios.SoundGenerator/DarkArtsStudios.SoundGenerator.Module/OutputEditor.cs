using DarkArtsStudios.SoundGenerator.DarkArtsStudios.Audio;
using DarkArtsStudios.SoundGenerator.DarkArtsStudios.Core;
using DarkArtsStudios.SoundGenerator.DarkArtsStudios.Core.Icons;
using UnityEditor;
using UnityEngine;

namespace DarkArtsStudios.SoundGenerator.Module
{
	[CustomEditor(typeof(Output), true)]
	internal class OutputEditor : BaseModuleEditor
	{
		private AudioSource audioSource;

		private string AudioSourceGameObjectName = "DarkArtsStudios SoundGenerator AudioSource";

		private bool loopSound;

		public override Rect OnModuleGUI(Rect innerRect)
		{
			innerRect.width = 200f;
			Rect rect = base.OnModuleGUI(innerRect);
			Output output = base.target as Output;
			Color contentColor = GUI.contentColor;
			GUI.contentColor = GUI.skin.button.normal.textColor;
			Rect position = new Rect(innerRect.x, rect.y + rect.height, innerRect.width, BaseModuleEditor.AttributeHeight);
			output.gameObject.name = EditorGUI.TextField(position, global::DarkArtsStudios.SoundGenerator.DarkArtsStudios.Core.GUIUtility.TempContent("Name", Fugue.icon("speaker")), output.gameObject.name);
			Rect position2 = new Rect(innerRect.x, position.y + position.height, innerRect.width, BaseModuleEditor.AttributeHeight);
			output.sampleRate = EditorGUI.IntPopup(position2, global::DarkArtsStudios.SoundGenerator.DarkArtsStudios.Core.GUIUtility.TempContent("Quality", Fugue.icon("sort-quantity")), output.sampleRate, new GUIContent[8]
			{
				new GUIContent("8kHz (Telephone)"),
				new GUIContent("11kHz"),
				new GUIContent("16kHz(VOIP)"),
				new GUIContent("22kHz"),
				new GUIContent("32kHz (CamCorder)"),
				new GUIContent("44kHz (CD)"),
				new GUIContent("48kHz (DVD)"),
				new GUIContent("96kHz (Blue-Ray)")
			}, new int[8]
			{
				8000,
				11025,
				16000,
				22050,
				32000,
				44100,
				48000,
				96000
			});
			Rect position3 = new Rect(innerRect.x, position2.y + position2.height, innerRect.width - BaseModuleEditor.AttributeHeight, BaseModuleEditor.AttributeHeight);
			Rect position4 = new Rect(innerRect.x + innerRect.width - BaseModuleEditor.AttributeHeight, position2.y + position2.height, BaseModuleEditor.AttributeHeight, BaseModuleEditor.AttributeHeight);
			EditorGUI.LabelField(position3, "Smooth Loop");
			output.smoothLoop = EditorGUI.Toggle(position4, output.smoothLoop);
			Rect position5 = new Rect(innerRect.x, position3.y + position3.height, innerRect.width / 3f, BaseModuleEditor.AttributeHeight);
			Rect position6 = new Rect(innerRect.x + innerRect.width / 3f, position3.y + position3.height, innerRect.width / 3f, BaseModuleEditor.AttributeHeight);
			EditorGUI.BeginChangeCheck();
			bool num = GUI.Button(position5, global::DarkArtsStudios.SoundGenerator.DarkArtsStudios.Core.GUIUtility.TempContent("Play", Fugue.icon("control"), "Generate and play this output"));
			loopSound = GUI.Toggle(position6, loopSound, global::DarkArtsStudios.SoundGenerator.DarkArtsStudios.Core.GUIUtility.TempContent("Loop", Fugue.icon("arrow-circle"), "Generate and loop this output"), "Button");
			if (EditorGUI.EndChangeCheck())
			{
				if (audioSource == null)
				{
					GameObject gameObject = GameObject.Find(AudioSourceGameObjectName);
					if (!gameObject)
					{
						gameObject = new GameObject(AudioSourceGameObjectName);
						gameObject.transform.position = Vector3.zero;
						AudioListener audioListener = (AudioListener)Object.FindObjectOfType(typeof(AudioListener));
						if ((bool)audioListener)
						{
							gameObject.transform.position = audioListener.gameObject.transform.position;
						}
						gameObject.AddComponent(typeof(AudioSource));
						gameObject.hideFlags = HideFlags.HideAndDontSave;
					}
					audioSource = gameObject.GetComponent<AudioSource>();
					audioSource.rolloffMode = AudioRolloffMode.Linear;
				}
				audioSource.Stop();
			}
			if (num || loopSound)
			{
				output.Generate();
				audioSource.clip = output.audioClip;
				audioSource.loop = loopSound;
				audioSource.Play();
			}
			if (GUI.Button(new Rect(innerRect.x + 2f * innerRect.width / 3f, position5.y, innerRect.width / 3f, BaseModuleEditor.AttributeHeight), global::DarkArtsStudios.SoundGenerator.DarkArtsStudios.Core.GUIUtility.TempContent("Export", Fugue.icon("disk-black"), "Export WAV Audio File")))
			{
				output.Generate();
				string text = UnityEditor.EditorUtility.SaveFilePanel("Export WAV Audio File", "Assets/", output.name, "wav");
				if (text != "")
				{
					global::DarkArtsStudios.SoundGenerator.DarkArtsStudios.Audio.AudioClipUtility.ExportWAV(output.audioClip, text);
					AssetDatabase.Refresh();
				}
			}
			GUI.contentColor = contentColor;
			innerRect.height = rect.height + position.height + position2.height + position3.height + position5.height;
			return innerRect;
		}
	}
}
