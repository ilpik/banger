using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace DarkArtsStudios.SoundGenerator.DarkArtsStudios.Core
{
	internal static class ResourceUtility
	{
		private static Dictionary<string, AudioClip> _audioClipFromResource;

		private static Dictionary<string, Texture2D> _textureFromResource;

		public static string ImagePath(string projectName, string path, string name)
		{
			return $"Assets/{path}/{name}.png";
		}

		public static string ProjectImagePath(string projectName, string path, string name)
		{
			return ImagePath(projectName, $"DarkArts Studios/{projectName}/{path}", name);
		}

		private static string ResourceName(string name, string type)
		{
			return string.Format("{0}_{1}", name.Replace(" ", "_").Replace("-", "_"), type.ToLower());
		}

		//internal static AudioClip AudioClipFromResource(string name)
		//{
		//	if (_audioClipFromResource == null)
		//	{
		//		_audioClipFromResource = new Dictionary<string, AudioClip>();
		//	}
		//	string text = ResourceName(name, "wav");
		//	if (_audioClipFromResource.ContainsKey(text) && _audioClipFromResource[text] != null)
		//	{
		//		return _audioClipFromResource[text];
		//	}

  //          object @object = Resources.Load(text);//global::DarkArtsStudios.SoundGenerator.Properties.Resources.ResourceManager.GetObject(text, global::DarkArtsStudios.SoundGenerator.Properties.Resources.Culture));
		//	if (@object == null)
		//	{
		//		throw new Exception($"Unable to locate built-in {ProductInformation.primaryProduct.name} AudioClip Resource: {name}");
		//	}
		//	string text2 = $"{System.IO.Path.GetTempPath()}{Guid.NewGuid().ToString()}-{text}.wav";
		//	File.WriteAllBytes(text2, (byte[])@object);
		//	WWW audioClipLoader = new WWW($"file://{text2}");
		//	ContinuationManager.Add(() => audioClipLoader.isDone, delegate
		//	{
		//		if (!string.IsNullOrEmpty(audioClipLoader.error))
		//		{
		//			Debug.Log("WWW failed: " + audioClipLoader.error);
		//		}
		//	});
		//	AudioClip audioClip = audioClipLoader.GetAudioClip(threeD: false, stream: false, AudioType.WAV);
		//	if (audioClip == null)
		//	{
		//		throw new Exception($"Failed to load built-in {ProductInformation.primaryProduct.name} AudioClip Resource: {name} tried {text2}");
		//	}
		//	_audioClipFromResource[text] = audioClip;
		//	return audioClip;
		//}

		internal static Texture2D TextureFromResource(string name)
		{
			if (_textureFromResource == null)
			{
				_textureFromResource = new Dictionary<string, Texture2D>();
			}
			string text = ResourceName(name, "png");
			if (_textureFromResource.ContainsKey(text) && _textureFromResource[text] != null)
			{
				return _textureFromResource[text];
			}

            object @object = Resources.Load(name); //global::DarkArtsStudios.SoundGenerator.Properties.Resources.ResourceManager.GetObject(text, global::DarkArtsStudios.SoundGenerator.Properties.Resources.Culture);
			Texture2D texture2D = new Texture2D(0, 0);
			texture2D.LoadImage((byte[])@object);
			_textureFromResource[text] = texture2D;
			return texture2D;
		}

		public static void EnsureImage(string projectName, string path, string name)
		{
			string text = ImagePath(projectName, path, name);
			if (!File.Exists(text) || new FileInfo(text).Length == 0L)
			{
				Path.CreateFolder(path);
				FileStream fileStream = File.Create(string.Format(text));
				BinaryWriter binaryWriter = new BinaryWriter(fileStream);
				string text2 = string.Format("{0}_png", name.Replace(" ", "_").Replace("-", "_"));
                object @object = Resources.Load(text2);//global::DarkArtsStudios.SoundGenerator.Properties.Resources.ResourceManager.GetObject(text2, global::DarkArtsStudios.SoundGenerator.Properties.Resources.Culture));
				if (@object == null)
				{
					throw new Exception($"Unable to locate Resource: {text2}");
				}
				binaryWriter.Write((byte[])@object);
				binaryWriter.Close();
				fileStream.Close();
				AssetDatabase.ImportAsset(text, ImportAssetOptions.ForceUpdate);
			}
			TextureImporter textureImporter = AssetImporter.GetAtPath(text) as TextureImporter;
			if (textureImporter != null && (textureImporter.textureType != TextureImporterType.GUI || textureImporter.mipmapEnabled || textureImporter.filterMode != 0))
			{
				textureImporter.textureType = TextureImporterType.GUI;
				textureImporter.mipmapEnabled = false;
				textureImporter.filterMode = FilterMode.Point;
				AssetDatabase.ImportAsset(text, ImportAssetOptions.ForceUpdate);
			}
			AssetDatabase.LoadAssetAtPath(text, typeof(Texture2D));
		}

		public static void EnsureProjectImage(string projectName, string path, string name)
		{
			EnsureImage(projectName, $"DarkArts Studios/{projectName}/{path}", name);
		}

		public static void EnsureEditorDefaultResourceIcon(string projectName, string windowName)
		{
			EnsureImage(projectName, "Editor Default Resources/Icons", windowName);
		}

		public static void EnsureEditorDefaultResourceIcon(string projectName)
		{
			EnsureEditorDefaultResourceIcon(projectName, projectName);
		}
	}
}
