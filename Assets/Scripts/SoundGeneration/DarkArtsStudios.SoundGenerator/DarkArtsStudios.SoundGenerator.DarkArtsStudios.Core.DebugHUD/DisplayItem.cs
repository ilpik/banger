using System.Collections;
using UnityEngine;

namespace DarkArtsStudios.SoundGenerator.DarkArtsStudios.Core.DebugHUD
{
	internal class DisplayItem : MonoBehaviour
	{
		protected bool _ready;

		protected int allocatedTextEnum = -1;

		public bool ready => _ready;

		protected virtual IEnumerator Start()
		{
			while (Display.Instance == null || !Display.Instance.ready)
			{
				yield return null;
			}
			allocatedTextEnum = Display.Instance.AllocateTextItem();
			_ready = true;
		}

		public void SetText(string text)
		{
			Display.Instance.SetItemText(allocatedTextEnum, text);
		}
	}
}
