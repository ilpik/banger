using System.Collections.Generic;
using System.IO;

namespace DarkArtsStudios.SoundGenerator.DarkArtsStudios.Core
{
	internal static class TemplateUtility
	{
		public static string String(string template, Dictionary<string, string> values)
		{
			string text = template;
			foreach (string key in values.Keys)
			{
				text = text.Replace($"${key}$", values[key]);
			}
			return text;
		}

		public static string FilePath(string filePath, Dictionary<string, string> values)
		{
			return String(File.ReadAllText(filePath), values);
		}
	}
}
