using UnityEngine;

namespace DarkArtsStudios.SoundGenerator.DarkArtsStudios.Core
{
	internal static class MaterialUtility
	{
		private static Material defaultMaterial;

		public static Material DefaultMaterial()
		{
			if ((bool)defaultMaterial)
			{
				return defaultMaterial;
			}
			GameObject gameObject = GameObject.CreatePrimitive(PrimitiveType.Plane);
			gameObject.SetActive(value: false);
			defaultMaterial = gameObject.GetComponent<MeshRenderer>().sharedMaterial;
			Object.DestroyImmediate(gameObject);
			return defaultMaterial;
		}
	}
}
