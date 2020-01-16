using System.IO;
using UnityEditor;
using UnityEngine;

namespace DarkArtsStudios.SoundGenerator.DarkArtsStudios.Core
{
	[InitializeOnLoad]
	internal class Path
	{
		protected const string BASE = "DarkArts Studios";

		static Path()
		{
			CreateFolder("DarkArts Studios");
		}

		public static void CreateFolder(string path)
		{
			if (!Directory.Exists(System.IO.Path.Combine(Application.dataPath, path)))
			{
				string directoryName = System.IO.Path.GetDirectoryName(path);
				if (!Directory.Exists(System.IO.Path.Combine(Application.dataPath, directoryName)))
				{
					CreateFolder(directoryName);
				}
				AssetDatabase.CreateFolder(System.IO.Path.GetDirectoryName("Assets/" + path), System.IO.Path.GetFileName(path));
			}
		}
	}
}
