using System;
using UnityEditor;

namespace DarkArtsStudios.SoundGenerator
{
	[InitializeOnLoad]
	public static class SettingsAutoReset
	{
		static SettingsAutoReset()
		{
			SettingsEditor.ResetAllHideFlags();
		}

		private static void hierarchyWindowChanged()
		{
			EditorApplication.hierarchyWindowChanged = (EditorApplication.CallbackFunction)Delegate.Remove(EditorApplication.hierarchyWindowChanged, new EditorApplication.CallbackFunction(hierarchyWindowChanged));
			SettingsEditor.ResetAllHideFlags();
			EditorApplication.hierarchyWindowChanged = (EditorApplication.CallbackFunction)Delegate.Combine(EditorApplication.hierarchyWindowChanged, new EditorApplication.CallbackFunction(hierarchyWindowChanged));
		}
	}
}
