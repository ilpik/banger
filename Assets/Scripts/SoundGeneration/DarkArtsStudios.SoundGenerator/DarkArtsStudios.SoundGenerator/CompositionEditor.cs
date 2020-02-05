using DarkArtsStudios.SoundGenerator.DarkArtsStudios.Core;
using DarkArtsStudios.SoundGenerator.DarkArtsStudios.Core.Icons;
using DarkArtsStudios.SoundGenerator.Module;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace DarkArtsStudios.SoundGenerator
{
	[InitializeOnLoad]
	internal class CompositionEditor : EditorWindow
	{
        private static List<CompositionEditor> instances = new List<CompositionEditor>();

        private static void EditorUpdate()
        {
            foreach (var instance in instances)
            {
                instance.Repaint();
            }
        }

        public CompositionEditor()
        {
            instances.Add(this);
        }
        
        [NonSerialized]
		private static Color drawNodeCurveColour;

		[NonSerialized]
		private Composition composition;

		[NonSerialized]
		private Vector2 lastMouseClick;

		[NonSerialized]
		private BaseModule.Attribute busyAttachingAttribute;

		[NonSerialized]
		private BaseModule busyAttachingModule;

		[NonSerialized]
		private Rect busyAttachingAttributeFrom;

		[NonSerialized]
		private Dictionary<int, Editor> generatorEditors = new Dictionary<int, Editor>();

		[NonSerialized]
		private const float kZoomMin = 0.1f;

		[NonSerialized]
		private const float kZoomMax = 3.5f;

		[NonSerialized]
		private Rect _zoomArea;

		[NonSerialized]
		private float _zoom = 1f;

		[NonSerialized]
		private static readonly float _zoomSpeed;

		[NonSerialized]
		private bool autoTension;

		[NonSerialized]
		private bool autoCentre;

		[NonSerialized]
		private static readonly float RepulsionDistance;

		[NonSerialized]
		private static readonly float RepulsionDistancePlacementBuffer;

		[NonSerialized]
		private static readonly float AttractionDistance;

		[NonSerialized]
		private static readonly float TensionFactor;

		[NonSerialized]
		private bool hasGenerators;

		private float _iconButtonYOffset;

		private float _iconButtonSpacing = 2f;

		private float __iconBarWidth;

		private bool __iconBarWidthInitialized = false;

		private static readonly float _zoomPadding;

		private float _iconBarWidth
		{
			get
			{
				if (!__iconBarWidthInitialized)
				{
					__iconBarWidth = 2 + SettingsEditor.IconSize + 2;
				}
				return __iconBarWidth;
			}
			set
			{
				__iconBarWidth = value;
			}
		}

		[MenuItem("GameObject/Create Other/Sound Generator Composition")]
		private static void CreateComposition()
		{
			GameObject gameObject = new GameObject();
			gameObject.AddComponent<Composition>();
			gameObject.name = "Sound Generator Composition";
			Selection.activeGameObject = gameObject;
		}

		[MenuItem("Assets/Create/Sound Generator Composition")]
		public static void Create()
		{
			string path = string.Format("{0}/{1}.prefab", AssetDatabase.GetAssetPath(Selection.activeObject), "Sound Generator Composition");
			GameObject gameObject = new GameObject();
			gameObject.AddComponent<Composition>();
			gameObject.name = "Sound Generator Composition";
			GameObject activeObject = PrefabUtility.SaveAsPrefabAsset(gameObject, path);
			UnityEngine.Object.DestroyImmediate(gameObject);
			Selection.activeObject = activeObject;
		}

		private Vector2 ConvertScreenCoordsToZoomCoords(Vector2 screenCoords)
		{
			return (screenCoords - new Vector2(_zoomArea.x, _zoomArea.y)) / _zoom;
		}

		private void AddModule(object moduleType)
		{
			GameObject gameObject = null;
			GameObject gameObject2 = null;
			if (composition.gameObject.activeInHierarchy)
			{
				gameObject = composition.gameObject;
			}
			else
			{
				string name = composition.gameObject.name;
				composition.gameObject.name = Guid.NewGuid().ToString();
				gameObject2 = (PrefabUtility.InstantiatePrefab(PrefabUtility.FindPrefabRoot(composition.gameObject)) as GameObject);
				gameObject = ((!(gameObject2.name == composition.gameObject.name)) ? global::DarkArtsStudios.SoundGenerator.DarkArtsStudios.Core.GameObjectUtility.FindRecursivelyWithinChildren(gameObject2, composition.gameObject.name) : gameObject2);
				gameObject.name = name;
			}
			GameObject gameObject3 = new GameObject();
			gameObject3.transform.parent = gameObject.transform;
			if (!SettingsEditor.Debug)
			{
				gameObject3.hideFlags = HideFlags.HideInHierarchy;
			}
			else
			{
				gameObject3.hideFlags = HideFlags.None;
			}
			BaseModule baseModule = (BaseModule)gameObject3.AddComponent((Type)moduleType);
			baseModule.InitializeName();
			baseModule.InitializeAttributes();
			Vector2 vector = ConvertScreenCoordsToZoomCoords(lastMouseClick);
			baseModule.visualPlacementRect.xMin = vector.x;
			baseModule.visualPlacementRect.yMin = vector.y;
			baseModule.visualPlacementRect.xMax = vector.x + 100f;
			baseModule.visualPlacementRect.yMax = vector.y + 100f;
			gameObject.GetComponent<Composition>().modules.Add(baseModule);
			string assetPath = AssetDatabase.GetAssetPath(composition);
			if (assetPath != "")
			{
				PrefabUtility.ReplacePrefab(gameObject2, PrefabUtility.FindPrefabRoot(composition.gameObject), ReplacePrefabOptions.ConnectToPrefab);
				UnityEngine.Object.DestroyImmediate(gameObject2);
				AssetDatabase.ImportAsset(assetPath, ImportAssetOptions.ForceUpdate | ImportAssetOptions.ForceSynchronousImport);
				composition = null;
			}
		}

		private float distanceBetweenRects(Rect a, Rect b)
		{
			float num = (a.xMin >= b.xMax) ? (a.xMin - b.xMax) : ((!(b.xMin >= a.xMax)) ? (-1f) : (b.xMin - a.xMax));
			float num2 = (a.yMin >= b.yMax) ? (a.yMin - b.yMax) : ((!(b.yMin >= a.yMax)) ? (-1f) : (b.yMin - a.yMax));
			if (num2 < 0f && num < 0f)
			{
				return 0f;
			}
			if (num2 <= 0f)
			{
				return num;
			}
			if (num <= 0f)
			{
				return num2;
			}
			return Mathf.Max(num, num2);
		}

		static CompositionEditor()
		{
            EditorApplication.update += EditorUpdate;
            drawNodeCurveColour = new Color(0.5f, 0.75f, 0.5f, 0.75f);
			_zoomSpeed = 1.2f;
			RepulsionDistance = 50f;
			RepulsionDistancePlacementBuffer = 5f;
			AttractionDistance = 100f;
			TensionFactor = 75f;
			_zoomPadding = 30f;
		}

		private void Update()
		{
			if (Application.isPlaying)
			{
				return;
			}
			if (busyAttachingAttribute != null)
			{
				Repaint();
			}
			GameObject activeGameObject = Selection.activeGameObject;
			if ((bool)activeGameObject)
			{
				Composition y = composition;
				composition = activeGameObject.GetComponent<Composition>();
				if (composition != y)
				{
					hasGenerators = false;
				}
			}
			else
			{
				composition = null;
			}
			if (composition == null)
			{
				if (hasGenerators)
				{
					hasGenerators = false;
					Repaint();
				}
			}
			else if (!hasGenerators)
			{
				hasGenerators = true;
				Repaint();
			}
			if (autoCentre)
			{
				Repaint();
			}
			if (autoTension)
			{
				bool flag = false;
				Vector2 zero = Vector2.zero;
				foreach (BaseModule module in composition.modules)
				{
					if ((bool)module)
					{
						zero.x += module.visualPlacementRect.x;
						zero.y += module.visualPlacementRect.y;
					}
				}
				zero /= (float)composition.modules.Count;
				foreach (BaseModule module2 in composition.modules)
				{
					if ((bool)module2)
					{
						Vector2 a = new Vector2(module2.visualPlacementRect.x + module2.visualPlacementRect.width / 2f, module2.visualPlacementRect.y + module2.visualPlacementRect.height / 2f);
						Vector2 zero2 = Vector2.zero;
						foreach (BaseModule module3 in composition.modules)
						{
							if ((bool)module3 && !(module3 == module2))
							{
								float num = 0f;
								float num2 = 0f;
								foreach (BaseModule.Attribute attribute in module3.attributes)
								{
									if (attribute.generator != null)
									{
										num2 += attribute.generator.visualPlacementRect.height + RepulsionDistance + RepulsionDistancePlacementBuffer;
									}
								}
								Vector2 b;
								Vector2 a2;
								if (num2 > 0f)
								{
									num2 -= RepulsionDistance + RepulsionDistancePlacementBuffer;
									float num3 = 0f;
									foreach (BaseModule.Attribute attribute2 in module3.attributes)
									{
										if (attribute2.generator == module2)
										{
											b = new Vector2(module3.visualPlacementRect.x - (AttractionDistance + module3.visualPlacementRect.width / 2f), module3.visualPlacementRect.y + module3.visualPlacementRect.height / 2f - num2 / 2f + num3);
											a2 = a - b;
											zero2 -= a2 / TensionFactor;
											flag = true;
										}
										else if (attribute2.generator != null)
										{
											num3 += attribute2.generator.visualPlacementRect.height + RepulsionDistance + RepulsionDistancePlacementBuffer;
										}
									}
								}
								b = new Vector2(module3.visualPlacementRect.x + module3.visualPlacementRect.width / 2f, module3.visualPlacementRect.y + module3.visualPlacementRect.height / 2f);
								a2 = a - b;
								num = distanceBetweenRects(module2.visualPlacementRect, module3.visualPlacementRect) - RepulsionDistance;
								if (num < 0f)
								{
									Mathf.Abs(num);
									zero2 += a2 / TensionFactor;
									flag = true;
								}
							}
						}
						module2.visualPlacementRect.x += zero2.x;
						module2.visualPlacementRect.y += zero2.y;
					}
				}
				if (flag)
				{
					Vector2 zero3 = Vector2.zero;
					foreach (BaseModule module4 in composition.modules)
					{
						if ((bool)module4)
						{
							zero3.x += module4.visualPlacementRect.x;
							zero3.y += module4.visualPlacementRect.y;
						}
					}
					zero3 /= (float)composition.modules.Count;
					Vector2 vector = zero3 - zero;
					foreach (BaseModule module5 in composition.modules)
					{
						if ((bool)module5)
						{
							module5.visualPlacementRect.x -= vector.x;
							module5.visualPlacementRect.y -= vector.y;
						}
					}
					Repaint();
				}
			}
		}

		private void startIconButtons()
		{
			_iconBarWidth = _iconButtonSpacing + (float)SettingsEditor.IconSize + _iconButtonSpacing;
			_iconButtonYOffset = _iconButtonSpacing;
			GUI.Box(new Rect(0f, 0f, _iconButtonSpacing + (float)SettingsEditor.IconSize + _iconButtonSpacing, Screen.height), "");
		}

		private Rect nextIconRect()
		{
			Rect result = new Rect(_iconButtonSpacing, _iconButtonYOffset, SettingsEditor.IconSize, SettingsEditor.IconSize);
			_iconButtonYOffset += (float)SettingsEditor.IconSize + _iconButtonSpacing;
			return result;
		}

		private bool addIconButton(Texture2D icon, string toolTip)
		{
			GUIContent content = new GUIContent(icon, toolTip);
			return GUI.Button(nextIconRect(), content);
		}

		private bool addIconToggle(bool current, Texture2D icon, string toolTip)
		{
			GUIContent content = new GUIContent(icon, toolTip);
			return GUI.Toggle(nextIconRect(), current, content, "button");
		}

		private void CentreComposition(bool autoZoom)
		{
			Vector2 vector = new Vector2(100000f, 100000f);
			Vector2 vector2 = new Vector2(-100000f, -100000f);
			foreach (BaseModule module in composition.modules)
			{
				if ((bool)module)
				{
					if (module.visualPlacementRect.x < vector.x)
					{
						vector.x = module.visualPlacementRect.x;
					}
					if (module.visualPlacementRect.y < vector.y)
					{
						vector.y = module.visualPlacementRect.y;
					}
					if (module.visualPlacementRect.x + module.visualPlacementRect.width > vector2.x)
					{
						vector2.x = module.visualPlacementRect.x + module.visualPlacementRect.width;
					}
					if (module.visualPlacementRect.y + module.visualPlacementRect.height > vector2.y)
					{
						vector2.y = module.visualPlacementRect.y + module.visualPlacementRect.height;
					}
				}
			}
			float num = vector2.x - vector.x + (float)GUI.skin.window.margin.left + (float)GUI.skin.window.margin.right + (float)GUI.skin.window.padding.left + (float)GUI.skin.window.padding.right;
			float num2 = vector2.y - vector.y + (float)GUI.skin.window.margin.top + (float)GUI.skin.window.margin.bottom + (float)GUI.skin.window.padding.top + (float)GUI.skin.window.padding.bottom;
			Rect rect = new Rect(_zoomPadding + _iconBarWidth, _zoomPadding, (float)Screen.width - (_zoomPadding * 2f + _iconBarWidth), (float)Screen.height - (_zoomPadding * 2f + (float)GUI.skin.window.padding.top + (float)GUI.skin.window.padding.bottom));
			float a = rect.width / num;
			float b = rect.height / num2;
			if (autoZoom)
			{
				_zoom = Mathf.Min(a, b);
			}
			Vector2 b2 = new Vector2((vector2.x + vector.x) / 2f, (vector2.y + vector.y) / 2f);
			Vector2 screenCoords = new Vector2(rect.x + rect.width / 2f, rect.y + rect.height / 2f);
			Vector2 vector3 = ConvertScreenCoordsToZoomCoords(screenCoords) - b2 - new Vector2(GUI.skin.window.margin.left + GUI.skin.window.padding.left, -GUI.skin.window.padding.bottom - GUI.skin.window.margin.bottom + GUI.skin.window.border.bottom);
			foreach (BaseModule module2 in composition.modules)
			{
				if ((bool)module2)
				{
					module2.visualPlacementRect.x += vector3.x;
					module2.visualPlacementRect.y += vector3.y;
				}
			}
		}

		private void DrawIcons()
		{
			startIconButtons();
			Color contentColor = GUI.contentColor;
			if (hasGenerators && (bool)composition)
			{
				GUI.contentColor = new Color(0.75f, 0.75f, 0.5f, 1f);
				if (addIconButton(FontAwesome.icon("fullscreen"), "Fit composition in window"))
				{
					CentreComposition(autoZoom: true);
					Repaint();
				}
				if (addIconButton(FontAwesome.icon("resize-small"), "Reset zoom to 1:1"))
				{
					_zoom = 1f;
					CentreComposition(autoZoom: false);
					Repaint();
				}
				_iconButtonYOffset = (float)Screen.height - (4f * ((float)SettingsEditor.IconSize + _iconButtonSpacing) + (float)GUI.skin.window.padding.top + (float)GUI.skin.window.padding.bottom);
				GUI.contentColor = new Color(0.5f, 1f, 0.5f, 1f);
				autoCentre = addIconToggle(autoCentre, FontAwesome.icon("fullscreen"), "Automatically zoom & centre");
				autoTension = addIconToggle(autoTension, FontAwesome.icon("magnet"), "Automatically re-align composition");
				_iconButtonYOffset += (float)SettingsEditor.IconSize + _iconButtonSpacing;
			}
			else
			{
				_iconButtonYOffset = (float)Screen.height - ((float)SettingsEditor.IconSize + _iconButtonSpacing + (float)GUI.skin.window.padding.top + (float)GUI.skin.window.padding.bottom);
			}
			GUI.contentColor = new Color(0.75f, 0.75f, 0.75f, 1f);
			if (addIconButton(FontAwesome.icon("gear"), "Settings"))
			{
				EditorWindow.GetWindow<SettingsEditor>().titleContent = new GUIContent("Sound Gen", Fugue.icon("gear"));
			}
			GUI.contentColor = contentColor;
		}

		private void DrawWaterMark()
		{
			int fontSize = GUI.skin.label.fontSize;
			Color color = GUI.color;
			GUI.color = new Color(color.r, color.g, color.b, 0.1f);
			GUILayout.BeginArea(new Rect(_iconBarWidth + 8f, 8f, (float)Screen.width - (_iconBarWidth + 16f), Screen.height - 16));
			GUI.skin.label.fontSize = 30;
			GUILayout.Label("DarkArts Studios");
			GUILayout.Label("Sound Generator");
			if (!Application.isPlaying && (bool)composition)
			{
				GUILayout.Space(20f);
				GUI.skin.label.fontSize = 16;
				GUILayout.Label("1. Right-click to add modules");
				GUILayout.Space(12f);
				GUILayout.Label("2. Attach modules by clicking an attribute tab exposed on the left side of a module");
				GUILayout.Space(12f);
				GUILayout.Label("3. Now attach on an attribute tab on the right hand side of the source module.");
				GUILayout.Space(12f);
				GUILayout.Label("4. Make sure you have an \"Output\" module");
			}
			GUILayout.EndArea();
			GUI.skin.label.fontSize = fontSize;
			GUI.color = color;
		}

		private void OnGUI()
		{
			base.titleContent = new GUIContent("Composition", Fugue.icon("speaker"));
			DrawWaterMark();
			if (Application.isPlaying)
			{
				GUILayout.Space(100f);
				EditorGUILayout.HelpBox("You cannot edit Sound Generator Compositions from the Editor while in Play Mode.", MessageType.Error);
				return;
			}
			DrawIcons();
			if (!hasGenerators || !composition)
			{
				GUILayout.BeginHorizontal();
				GUILayout.Space(_iconBarWidth + 20f);
				GUILayout.BeginVertical();
				GUILayout.Space(100f);
				EditorGUILayout.HelpBox("Select a GameObject in the SceneView with a Composition Component attached", MessageType.Warning);
				if (Selection.activeGameObject != null && Selection.activeGameObject.GetComponent<BaseModule>() == null)
				{
					GUILayout.Space(8f);
					if (GUILayout.Button("Add a Sound Generator Composition to this GameObject"))
					{
						Selection.activeGameObject.AddComponent<Composition>();
					}
				}
				GUILayout.EndVertical();
				GUILayout.Space(20f);
				GUILayout.EndHorizontal();
				return;
			}
			if (autoCentre)
			{
				CentreComposition(autoZoom: true);
			}
			_zoomArea = new Rect(_iconBarWidth + 3f, 0f, (float)Screen.width - (_iconBarWidth + 3f), Screen.height);
			EditorZoomArea.Begin(_zoom, _zoomArea);
			BeginWindows();
			for (int i = 0; i < composition.modules.Count; i++)
			{
				if (!(composition.modules[i] != null))
				{
					continue;
				}
				_ = composition.modules[i].visualPlacementRect;
				Rect rect = GUI.Window(i, new Rect(composition.modules[i].visualPlacementRect.x, composition.modules[i].visualPlacementRect.y, composition.modules[i].visualPlacementRect.width + (float)GUI.skin.window.padding.left + (float)GUI.skin.window.padding.right, composition.modules[i].visualPlacementRect.height + (float)GUI.skin.window.padding.top + (float)GUI.skin.window.padding.bottom), DrawModuleWindow, global::DarkArtsStudios.SoundGenerator.DarkArtsStudios.Core.GUIUtility.TempContent(composition.modules[i].name));
				composition.modules[i].visualPlacementRect.x = rect.x;
				composition.modules[i].visualPlacementRect.y = rect.y;
				float num = 2f;
				foreach (BaseModule.Attribute attribute in composition.modules[i].attributes)
				{
					if (attribute.allowInput)
					{
						Rect position = new Rect(composition.modules[i].visualPlacementRect.xMin + (float)GUI.skin.window.padding.left - 16f, composition.modules[i].visualPlacementRect.y + (float)GUI.skin.window.padding.top + num, 16f, 16f);
						Color color = GUI.color;
						if (busyAttachingAttribute != null)
						{
							GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, 0.2f);
						}
						if (GUI.Button(position, "") && busyAttachingAttribute == null)
						{
							busyAttachingModule = composition.modules[i];
							busyAttachingAttribute = attribute;
							busyAttachingAttributeFrom = position;
						}
						GUI.color = color;
						if (!composition.hasModule(attribute.generator))
						{
							attribute.generator = null;
						}
						if (attribute.generator != null)
						{
							DrawNodeCurve(new Rect(attribute.generator.visualPlacementRect.xMax + (float)GUI.skin.window.padding.right + 14f, attribute.generator.visualPlacementRect.y + (float)GUI.skin.window.padding.top + attribute.generator.visualPlacementRect.height / 2f, 1f, 1f), new Rect(position.x + 1f, position.y + 8f, 1f, 1f), Color.black);
						}
					}
					num += BaseModuleEditor.AttributeHeight + 4f;
					if (attribute.type == BaseModule.Attribute.AttributeType.FREQUENCY)
					{
						num += BaseModuleEditor.AttributeHeight;
					}
				}
				if (composition.modules[i].GetType() != typeof(Output))
				{
					Rect position2 = new Rect(composition.modules[i].visualPlacementRect.xMax + (float)GUI.skin.window.padding.right, composition.modules[i].visualPlacementRect.y + (float)GUI.skin.window.padding.top + composition.modules[i].visualPlacementRect.height / 2f - 8f, 16f, 16f);
					Color color2 = GUI.color;
					if (busyAttachingAttribute == null || busyAttachingModule == composition.modules[i])
					{
						GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, 0.2f);
					}
					if (GUI.Button(position2, "") && busyAttachingAttribute != null && busyAttachingModule != composition.modules[i])
					{
						busyAttachingAttribute.generator = composition.modules[i];
						busyAttachingModule.dirty = true;
						busyAttachingAttribute = null;
						busyAttachingModule = null;
					}
					GUI.color = color2;
				}
			}
			EndWindows();
			if (busyAttachingAttribute != null)
			{
				if (Event.current.type == EventType.MouseDown && Event.current.button == 0)
				{
					if (busyAttachingAttribute != null && busyAttachingAttribute.generator != null)
					{
						busyAttachingAttribute.generator = null;
					}
					busyAttachingModule.dirty = true;
					busyAttachingAttribute = null;
					Repaint();
				}
				else
				{
					DrawNodeCurve(new Rect(Event.current.mousePosition.x, Event.current.mousePosition.y, 1f, 1f), new Rect(busyAttachingAttributeFrom.x + 1f, busyAttachingAttributeFrom.y + 8f, 1f, 1f), drawNodeCurveColour);
				}
			}
			EditorZoomArea.End();
			if (Event.current.type == EventType.ScrollWheel && _zoomArea.Contains(Event.current.mousePosition))
			{
				Vector2 mousePosition = Event.current.mousePosition;
				Vector2 delta = Event.current.delta;
				Vector2 b = ConvertScreenCoordsToZoomCoords(mousePosition);
				_ = (0f - delta.y) / 150f;
				_ = _zoom;
				if (delta.y < 0f)
				{
					_zoom *= _zoomSpeed;
				}
				else
				{
					_zoom /= _zoomSpeed;
				}
				_zoom = Mathf.Clamp(_zoom, 0.1f, 3.5f);
				Vector2 vector = ConvertScreenCoordsToZoomCoords(mousePosition) - b;
				foreach (BaseModule module in composition.modules)
				{
					if ((bool)module)
					{
						module.visualPlacementRect.x += vector.x;
						module.visualPlacementRect.y += vector.y;
					}
				}
				if (busyAttachingAttribute != null)
				{
					busyAttachingAttributeFrom = new Rect(busyAttachingAttributeFrom.x + vector.x, busyAttachingAttributeFrom.y + vector.y, busyAttachingAttributeFrom.width, busyAttachingAttributeFrom.height);
				}
				Event.current.Use();
			}
			if (Event.current.type == EventType.MouseDrag && (Event.current.button == 0 || Event.current.button == 2) && _zoomArea.Contains(Event.current.mousePosition))
			{
				Vector2 delta2 = Event.current.delta;
				delta2 /= _zoom;
				foreach (BaseModule module2 in composition.modules)
				{
					if ((bool)module2)
					{
						module2.visualPlacementRect.x += delta2.x;
						module2.visualPlacementRect.y += delta2.y;
					}
				}
				if (busyAttachingAttribute != null)
				{
					busyAttachingAttributeFrom = new Rect(busyAttachingAttributeFrom.x + delta2.x, busyAttachingAttributeFrom.y + delta2.y, busyAttachingAttributeFrom.width, busyAttachingAttributeFrom.height);
				}
				Event.current.Use();
			}
			if (Event.current.type == EventType.ContextClick)
			{
				lastMouseClick.x = Event.current.mousePosition.x;
				lastMouseClick.y = Event.current.mousePosition.y;
				GenericMenu genericMenu = new GenericMenu();
				List<Assembly> list = new List<Assembly>();
				list.Add(Assembly.GetAssembly(typeof(BaseModule)));
				Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
				foreach (Assembly item in assemblies)
				{
					if (!list.Contains(item))
					{
						list.Add(item);
					}
				}
				foreach (Assembly item2 in list)
				{
					if (item2 != null)
					{
						foreach (Type item3 in from moduleClass in item2.GetTypes()
							where moduleClass.IsClass && !moduleClass.IsAbstract && moduleClass.IsSubclassOf(typeof(BaseModule))
							select moduleClass)
						{
							try
							{
								string text = (string)item3.GetMethod("MenuEntry").Invoke(null, null);
								genericMenu.AddItem(new GUIContent(text), on: false, AddModule, item3);
							}
							catch (NullReferenceException)
							{
								Debug.LogError($"SoundGenerator Module Type {item3} Lacks a MenuEntry");
							}
						}
					}
				}
				genericMenu.ShowAsContext();
				Event.current.Use();
			}
			bool flag = false;
			foreach (BaseModule module3 in composition.modules)
			{
				if (!(module3 == null))
				{
					flag = module3.dirty;
					if (flag)
					{
						break;
					}
				}
			}
			if (!flag)
			{
				return;
			}
			for (int k = 0; k < composition.modules.Count; k++)
			{
				if (!(composition.modules[k] == null) && generatorEditors.ContainsKey(composition.modules[k].GetInstanceID()))
				{
					((BaseModuleEditor)generatorEditors[composition.modules[k].GetInstanceID()]).resetPreviewTexture();
				}
			}
			Repaint();
		}

		public static void RemoveModuleById(Composition composition, int id)
		{
			try
			{
				UnityEngine.Object.DestroyImmediate(composition.modules[id].gameObject, allowDestroyingAssets: true);
			}
			catch (NullReferenceException)
			{
			}
			composition.modules.RemoveAt(id);
			Component[] componentsInChildren = composition.GetComponentsInChildren(typeof(BaseModule), includeInactive: true);
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				BaseModule baseModule = (BaseModule)componentsInChildren[i];
				if (!composition.modules.Contains(baseModule))
				{
					UnityEngine.Object.DestroyImmediate(baseModule.gameObject, allowDestroyingAssets: true);
				}
			}
			string assetPath = AssetDatabase.GetAssetPath(composition);
			if (assetPath != "")
			{
				AssetDatabase.ImportAsset(assetPath, ImportAssetOptions.ForceUpdate | ImportAssetOptions.ForceSynchronousImport);
			}
		}

		private void RemoveModuleById(object id)
		{
			RemoveModuleById(composition, (int)id);
			Repaint();
		}

		private void OnSelectionChange()
		{
			Repaint();
		}

		private void DrawModuleWindow(int id)
		{
			Rect position = new Rect(composition.modules[id].visualPlacementRect.width - 7f, 1f, 16f, 16f);
			bool flag = GUI.Button(position, global::DarkArtsStudios.SoundGenerator.DarkArtsStudios.Core.GUIUtility.TempContent(Fugue.icon("cross-button"), "Remove this module"), GUI.skin.label);
			EditorGUIUtility.AddCursorRect(position, MouseCursor.Link);
			GUIContent content = (!composition.modules[id].showPreviewTexture) ? global::DarkArtsStudios.SoundGenerator.DarkArtsStudios.Core.GUIUtility.TempContent(Fugue.icon("system-monitor--plus"), "Show Sample Preview") : global::DarkArtsStudios.SoundGenerator.DarkArtsStudios.Core.GUIUtility.TempContent(Fugue.icon("system-monitor--minus"), "Hide Sample Preview");
			Rect position2 = new Rect(composition.modules[id].visualPlacementRect.width - 22f, 1f, 16f, 16f);
			bool flag2 = GUI.Button(position2, content, GUI.skin.label);
			EditorGUIUtility.AddCursorRect(position2, MouseCursor.Link);
			if (flag && UnityEditor.EditorUtility.DisplayDialog($"Remove {composition.modules[id].name} module?", "Are you sure?", "Remove", "Cancel"))
			{
				RemoveModuleById(id);
				return;
			}
			if (flag2)
			{
				composition.modules[id].showPreviewTexture = !composition.modules[id].showPreviewTexture;
			}
			Rect innerRect = new Rect(GUI.skin.window.padding.left, GUI.skin.window.padding.top, composition.modules[id].visualPlacementRect.width, composition.modules[id].visualPlacementRect.height);
			if (!generatorEditors.ContainsKey(composition.modules[id].GetInstanceID()))
			{
				generatorEditors[composition.modules[id].GetInstanceID()] = Editor.CreateEditor(composition.modules[id]);
			}
			Rect rect = ((BaseModuleEditor)generatorEditors[composition.modules[id].GetInstanceID()]).ModuleGUI(innerRect);
			if (Event.current.type == EventType.Repaint && (composition.modules[id].visualPlacementRect.width != rect.width || composition.modules[id].visualPlacementRect.height != rect.height))
			{
				composition.modules[id].visualPlacementRect.width = rect.width;
				composition.modules[id].visualPlacementRect.height = rect.height;
				Repaint();
			}
			GUI.DragWindow();
		}

		private void DrawNodeCurve(Rect start, Rect end, Color colour)
		{
			Vector3 vector = new Vector3(start.x + start.width, start.y + start.height / 2f, 0f);
			Vector3 vector2 = new Vector3(end.x, end.y + end.height / 2f, 0f);
			Vector3 startTangent = vector + Vector3.right * 50f;
			Vector3 endTangent = vector2 + Vector3.left * 50f;
			Color color = new Color(colour.r, colour.g, colour.b, 0.06f);
			for (int i = 0; i < 3; i++)
			{
				Handles.DrawBezier(vector, vector2, startTangent, endTangent, color, null, (i + 1) * 5);
			}
			Handles.DrawBezier(vector, vector2, startTangent, endTangent, colour, null, 1f);
		}
	}
}
