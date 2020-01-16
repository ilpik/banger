using DarkArtsStudios.SoundGenerator.DarkArtsStudios.Core;
using UnityEditor;

namespace DarkArtsStudios.SoundGenerator
{
	[InitializeOnLoad]
	public static class MainMenuItems
	{
		private const int PRODUCT_ID = 12092;

		private const string PRODUCT_NAME = "Sound Generator";

		private const string ROOT = "Tools/";

		private const string ROOT_PRODUCT_MENU = "Tools/Sound Generator/";

		private const int FUNCTIONALITY_MENU_PRIORITY = -77908000;

		static MainMenuItems()
		{
			ProductInformation.primaryProduct = ProductInformation.ByName("Sound Generator");
		}

		[MenuItem("Tools/Sound Generator/Composition Editor", false, -77908000)]
		public static void OpenCompositionEditorWindow()
		{
			(EditorWindow.GetWindow(typeof(CompositionEditor), utility: false, "Composition") as CompositionEditor).Show();
		}

		[MenuItem("Tools/Sound Generator/About", false, -77907900)]
		private static void OpenAboutWindow()
		{
			ProductInformation.primaryProduct = ProductInformation.ByName("Sound Generator");
			AboutWindow.OpenAboutWindow("Sound Generator");
		}
	}
}
