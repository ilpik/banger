using DarkArtsStudios.SoundGenerator.DarkArtsStudios.Core;
using System;
using UnityEditor;
using UnityEngine;

namespace DarkArtsStudios.SoundGenerator.Module
{
	[CustomEditor(typeof(Composition), true)]
	internal class CompositionModuleEditor : BaseModuleEditor
	{
		public override Rect OnModuleGUI(Rect innerRect)
		{
			Rect rect = base.OnModuleGUI(innerRect);
			Composition composition = base.target as Composition;
			Rect position = new Rect(innerRect.x, rect.y + rect.height, innerRect.width, BaseModuleEditor.AttributeHeight);
			global::DarkArtsStudios.SoundGenerator.Composition composition2 = null;
			EditorGUI.BeginChangeCheck();
			composition2 = (global::DarkArtsStudios.SoundGenerator.Composition)EditorGUI.ObjectField(position, composition.composition, typeof(global::DarkArtsStudios.SoundGenerator.Composition), allowSceneObjects: true);
			if (EditorGUI.EndChangeCheck())
			{
				bool flag = composition.composition != null;
				if (flag)
				{
					UnityEngine.Object.DestroyImmediate(composition.composition);
				}
				if (composition2 != null && composition2.gameObject != composition.gameObject)
				{
					composition.composition = UnityEngine.Object.Instantiate(composition2);
					if (!SettingsEditor.Debug)
					{
						composition.composition.hideFlags = HideFlags.HideInHierarchy;
					}
					composition.name = composition2.name;
					if (composition.gameObject.activeInHierarchy)
					{
						composition.composition.transform.parent = composition.transform;
					}
					else
					{
						string name = composition.gameObject.name;
						composition.gameObject.name = Guid.NewGuid().ToString();
						GameObject gameObject = PrefabUtility.InstantiatePrefab(PrefabUtility.FindPrefabRoot(composition.gameObject)) as GameObject;
						GameObject gameObject2 = null;
						gameObject2 = ((!(gameObject.name == composition.gameObject.name)) ? global::DarkArtsStudios.SoundGenerator.DarkArtsStudios.Core.GameObjectUtility.FindRecursivelyWithinChildren(gameObject, composition.gameObject.name) : gameObject);
						composition.composition.transform.parent = gameObject2.transform;
						gameObject2.name = name;
						PrefabUtility.ReplacePrefab(gameObject, PrefabUtility.FindPrefabRoot(composition.gameObject), ReplacePrefabOptions.ConnectToPrefab);
						UnityEngine.Object.DestroyImmediate(gameObject);
						AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath(composition), ImportAssetOptions.ForceUpdate | ImportAssetOptions.ForceSynchronousImport);
					}
				}
				else
				{
					composition.composition = null;
					composition.name = "Composition";
					if (flag)
					{
						Repaint();
					}
				}
				composition.InitializeAttributes();
				Repaint();
			}
			innerRect.height = BaseModuleEditor.AttributeHeight;
			return innerRect;
		}
	}
}
