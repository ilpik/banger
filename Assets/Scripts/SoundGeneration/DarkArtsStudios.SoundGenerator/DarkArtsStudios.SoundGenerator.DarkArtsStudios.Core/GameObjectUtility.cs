using UnityEngine;

namespace DarkArtsStudios.SoundGenerator.DarkArtsStudios.Core
{
	internal static class GameObjectUtility
	{
		public static GameObject FindRecursivelyWithinChildren(GameObject gameobject, string name)
		{
			foreach (Transform item in gameobject.transform)
			{
				if (item.gameObject.name == name)
				{
					return item.gameObject;
				}
			}
			GameObject gameObject = null;
			foreach (Transform item2 in gameobject.transform)
			{
				gameObject = FindRecursivelyWithinChildren(item2.gameObject, name);
				if ((bool)gameObject)
				{
					return gameObject;
				}
			}
			return null;
		}
	}
}
