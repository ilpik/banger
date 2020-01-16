using System;
using System.Reflection;
using UnityEditorInternal;

namespace DarkArtsStudios.SoundGenerator.DarkArtsStudios.Core
{
	internal static class RenderTextureUtility
	{
		private static bool _enabled;

		public static bool Enabled
		{
			get
			{
				return _enabled;
			}
			set
			{
				if (!InternalEditorUtility.HasPro())
				{
					try
					{
						typeof(EditorUtility).InvokeMember("SetTemporarilyAllowIndieRenderTexture", BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.InvokeMethod, null, null, new object[1]
						{
							value
						});
					}
					catch (Exception)
					{
					}
				}
				_enabled = value;
			}
		}
	}
}
