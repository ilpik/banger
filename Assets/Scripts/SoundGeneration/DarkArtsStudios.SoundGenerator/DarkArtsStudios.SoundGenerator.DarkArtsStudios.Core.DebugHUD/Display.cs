using System.Collections.Generic;
using UnityEngine;

namespace DarkArtsStudios.SoundGenerator.DarkArtsStudios.Core.DebugHUD
{
	internal class Display : MonoBehaviour
	{
		public static Display Instance = null;

		private static object _lock = new object();

		private List<string> displayText;

		private float deltaTime;

		private bool _visible;

		private bool _ready;

		public bool ready => _ready;

		public int AllocateTextItem()
		{
			lock (_lock)
			{
				displayText.Add("");
				return displayText.Count - 1;
			}
		}

		public void SetItemText(int itemEnum, string text)
		{
			displayText[itemEnum] = text;
		}

		private void Start()
		{
			lock (_lock)
			{
				if (Instance != null)
				{
					Debug.LogWarning("[DarkArtsStudios.Core.DebugHUD] Duplicate Display detected, destroying dupe!");
					Object.Destroy(base.gameObject);
				}
				else
				{
					Instance = this;
					displayText = new List<string>();
					Object.DontDestroyOnLoad(this);
					_ready = true;
				}
			}
		}

		private void Update()
		{
			deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
			if (Input.GetKeyDown(KeyCode.F10))
			{
				_visible = !_visible;
			}
		}

		private void OnGUI()
		{
			if (_visible)
			{
				int num = Screen.height * 2 / 100;
				GUIStyle gUIStyle = new GUIStyle();
				gUIStyle.alignment = TextAnchor.UpperLeft;
				gUIStyle.fontSize = num;
				gUIStyle.normal.textColor = new Color(0f, 0f, 0.5f, 1f);
				int num2 = 0;
				foreach (string item in displayText)
				{
					GUI.Label(new Rect(0f, num2 * num, Screen.width, num), item, gUIStyle);
					num2++;
				}
			}
		}
	}
}
