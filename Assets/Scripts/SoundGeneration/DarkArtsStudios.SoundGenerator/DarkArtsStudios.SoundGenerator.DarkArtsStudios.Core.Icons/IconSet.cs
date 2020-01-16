using System;
using System.Collections.Generic;
using UnityEngine;

namespace DarkArtsStudios.SoundGenerator.DarkArtsStudios.Core.Icons
{
	internal class IconSet
	{
		private Dictionary<string, string> _icons = new Dictionary<string, string>();

		private Dictionary<string, Texture2D> _icon_textures = new Dictionary<string, Texture2D>();

		protected internal void Add(string key, string value)
		{
			_icons.Add(key, value);
		}

		internal Texture2D icon(string iconName)
		{
			if (!_icons.ContainsKey(iconName))
			{
				return null;
			}
			if (!_icon_textures.ContainsKey(iconName) || _icon_textures[iconName] == null)
			{
				Texture2D texture2D = new Texture2D(24, 24);
				texture2D.LoadImage(Convert.FromBase64String(_icons[iconName]));
				if (_icon_textures.ContainsKey(iconName))
				{
					_icon_textures[iconName] = texture2D;
				}
				else
				{
					_icon_textures.Add(iconName, texture2D);
				}
			}
			return _icon_textures[iconName];
		}
	}
}
