using DarkArtsStudios.SoundGenerator.Module;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DarkArtsStudios.SoundGenerator
{
	[Serializable]
	[AddComponentMenu("Audio/Sound Generator Composition")]
	public class Composition : MonoBehaviour
	{
		[SerializeField]
		public List<BaseModule> modules = new List<BaseModule>();

		[NonSerialized]
		public Dictionary<string, UnityEngine.AudioClip> audioClips = new Dictionary<string, UnityEngine.AudioClip>();

		private IEnumerator Start()
		{
			int i = 1;
			audioClips.Clear();
			foreach (BaseModule module in modules)
			{
				yield return null;
				if (module && module is Output output)
				{
                    foreach (float item in output.IGenerate())
					{
						_ = item;
						yield return null;
					}
					audioClips[string.Format("{1}", i, output.name)] = output.audioClip;
					yield return null;
					i++;
				}
			}
		}

		public bool hasModule(BaseModule module)
		{
			foreach (BaseModule module2 in modules)
			{
				if ((bool)module && module == module2)
				{
					return true;
				}
			}
			return false;
		}

		public bool hasModuleType(Type moduleType)
		{
			foreach (BaseModule module in modules)
			{
				if ((bool)module && module.GetType() == moduleType)
				{
					return true;
				}
			}
			return false;
		}
	}
}
