using DarkArtsStudios.SoundGenerator.DarkArtsStudios.Core.Icons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace DarkArtsStudios.SoundGenerator.DarkArtsStudios.Core
{
	internal class AboutWindow : EditorWindow
	{
		private const int BANNER_WIDTH = 364;

		private const int BANNER_HEIGHT = 120;

		private const int FILLER_SIZE = 260;

		private const float OTHER_PRODUCT_BANNER_SCALE = 0.5f;

		private const int OTHER_PRODUCT_BANNER_WIDTH = 182;

		private const int OTHER_PRODUCT_BANNER_HEIGHT = 60;

		private static GUIStyle OtherProductDescriptionStyle;

		private List<ProductInformation> otherProducts;

		internal ProductInformation product;

		private static GUIStyle BannerStyle;

		private Vector2 productRangeScroll = Vector2.zero;

		internal static void OpenAboutWindow(string productName)
		{
			ProductInformation productInformation = ProductInformation.ByName(productName);
			if (productInformation == null)
			{
				throw new Exception($"Unable to locate product: {productName}");
			}
			AboutWindow window = EditorWindow.GetWindow<AboutWindow>(utility: true, productName, focus: true);
			window.product = productInformation;
			window.maxSize = new Vector2(364f, 500f);
			window.minSize = new Vector2(364f, 500f);
			window.Show();
			window.Focus();
		}

		private void OnGUI()
		{
			if (otherProducts == null)
			{
				otherProducts = ProductInformation.otherPoducts.OrderBy((ProductInformation a) => UnityEngine.Random.value + (float)((a.asset_store_id == -1) ? 1000 : 0)).ToList();
			}
			if (BannerStyle == null)
			{
				BannerStyle = new GUIStyle(GUI.skin.label)
				{
					alignment = TextAnchor.MiddleCenter,
					margin = new RectOffset(),
					padding = new RectOffset()
				};
			}
			if (OtherProductDescriptionStyle == null)
			{
				OtherProductDescriptionStyle = new GUIStyle(GUI.skin.label)
				{
					alignment = TextAnchor.MiddleCenter,
					margin = new RectOffset(),
					padding = new RectOffset(4, 4, 4, 4),
					wordWrap = true
				};
			}
			if (product == null)
			{
				Close();
				return;
			}
			EditorGUILayout.BeginVertical();
			if (product.banner != null)
			{
				Rect rect = GUILayoutUtility.GetRect(364f, 120f, BannerStyle);
				rect.y = 0f;
				EditorGUI.DrawPreviewTexture(rect, product.banner);
				EditorGUIUtility.AddCursorRect(rect, MouseCursor.Link);
				if (Event.current.type == EventType.MouseDown && Event.current.button == 0 && rect.Contains(Event.current.mousePosition))
				{
					Event.current.Use();
					if (product.asset_store_id == -1)
					{
						product.OpenProductWebsite("Primary Banner", "About Window");
					}
					else
					{
						product.OpenProductAssetStorePage("Primary Banner", "About Window");
					}
				}
			}
			Version version = Assembly.GetExecutingAssembly().GetName().Version;
			EditorGUILayout.BeginVertical();
			EditorGUILayout.LabelField($"Version: {version.ToString()}");
			EditorGUILayout.LabelField($"Built on: {new DateTime(2000, 1, 1).AddDays(version.Build).AddSeconds(version.MinorRevision * 2).ToString()}");
			EditorGUILayout.EndVertical();
			GUILayout.Space(16f);
			EditorGUILayout.BeginHorizontal();
			GUILayout.FlexibleSpace();
			if (EditorUtility.WebLink(GUIUtility.TempContent(" Questions & Support", Fugue.icon("mail--pencil"))))
			{
				product.HelpCentre();
			}
			GUILayout.FlexibleSpace();
			EditorGUILayout.EndHorizontal();
			GUILayout.Space(4f);
			EditorGUILayout.BeginHorizontal();
			GUILayout.FlexibleSpace();
			if (EditorUtility.WebLink(GUIUtility.TempContent($" {product.name} Website", Fugue.icon("globe"))))
			{
				product.OpenProductWebsite("Website Link", "About Window");
			}
			GUILayout.FlexibleSpace();
			EditorGUILayout.EndHorizontal();
			if (product.asset_store_id != -1)
			{
				GUILayout.Space(4f);
				EditorGUILayout.BeginHorizontal();
				GUILayout.FlexibleSpace();
				if (EditorUtility.WebLink(GUIUtility.TempContent("Please take the time to Rate & Review", Fugue.icon("thumb-up"))))
				{
					product.OpenProductAssetStorePage("Please Rate Review", "About Window");
				}
				GUILayout.FlexibleSpace();
				EditorGUILayout.EndHorizontal();
			}
			if (product.trello_development_roadmap_id != null)
			{
				GUILayout.Space(4f);
				EditorGUILayout.BeginHorizontal();
				GUILayout.FlexibleSpace();
				if (EditorUtility.WebLink(GUIUtility.TempContent("Development Roadmap", Fugue.icon("task--plus"))))
				{
					product.OpenTrelloDevelopmentRoadmap();
				}
				GUILayout.FlexibleSpace();
				EditorGUILayout.EndHorizontal();
			}
			GUILayout.Space(16f);
			GUILayout.FlexibleSpace();
			productRangeScroll = GUILayout.BeginScrollView(productRangeScroll);
			EditorGUILayout.BeginVertical(GUI.skin.box);
			GUILayout.Space(8f);
			EditorGUILayout.BeginHorizontal(GUILayout.Height(120f));
			Color color = GUI.color;
			bool flag = true;
			foreach (ProductInformation otherProduct in otherProducts)
			{
				if (flag && otherProducts.Count > 2)
				{
					GUILayout.Space(8f);
				}
				else
				{
					flag = false;
				}
				GUI.color = ((otherProduct.asset_store_id == -1) ? Color.gray : Color.white);
				EditorGUILayout.BeginVertical(GUILayout.Width(182f));
				Rect rect2 = GUILayoutUtility.GetRect(182f, 60f);
				if (otherProduct.banner != null)
				{
					EditorGUI.DrawPreviewTexture(rect2, otherProduct.banner);
				}
				GUILayout.Label(otherProduct.description, OtherProductDescriptionStyle, GUILayout.Width(182f));
				EditorGUILayout.EndVertical();
				Rect lastRect = GUILayoutUtility.GetLastRect();
				EditorGUIUtility.AddCursorRect(lastRect, MouseCursor.Link);
				if (Event.current.type == EventType.MouseDown && Event.current.button == 0 && lastRect.Contains(Event.current.mousePosition))
				{
					Event.current.Use();
					if (otherProduct.asset_store_id == -1)
					{
						otherProduct.OpenProductWebsite($"{otherProduct.name} Small Banner", "About Window");
					}
					else
					{
						otherProduct.OpenProductAssetStorePage($"{otherProduct.name} Small Banner", "About Window");
					}
				}
				if (otherProducts.Count > 2)
				{
					GUILayout.Space(8f);
				}
			}
			GUI.color = color;
			EditorGUILayout.EndHorizontal();
			EditorGUILayout.EndVertical();
			GUILayout.EndScrollView();
			EditorGUILayout.EndVertical();
		}
	}
}
