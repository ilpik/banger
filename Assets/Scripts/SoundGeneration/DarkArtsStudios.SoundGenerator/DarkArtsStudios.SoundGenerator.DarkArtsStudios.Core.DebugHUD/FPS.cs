using UnityEngine;

namespace DarkArtsStudios.SoundGenerator.DarkArtsStudios.Core.DebugHUD
{
	internal class FPS : DisplayItem
	{
		private float deltaTime;

		private void Update()
		{
			deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
		}

		private void OnGUI()
		{
			if (base.ready)
			{
				float num = deltaTime * 1000f;
				float num2 = 1f / deltaTime;
				string text = $"{num:0.0} ms ({num2:0.} fps)";
				SetText(text);
			}
		}
	}
}
