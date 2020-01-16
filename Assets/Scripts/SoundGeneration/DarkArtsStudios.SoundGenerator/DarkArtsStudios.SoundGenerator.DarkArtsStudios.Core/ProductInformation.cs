using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace DarkArtsStudios.SoundGenerator.DarkArtsStudios.Core
{
	internal class ProductInformation
	{
		internal const int UNPUBLISHED = -1;

		internal string name;

		internal string description;

		internal int asset_store_id;

		internal string trello_development_roadmap_id;

		internal static readonly IList<ProductInformation> allProducts = new ReadOnlyCollection<ProductInformation>(new List<ProductInformation>
		{
			new ProductInformation("GUI Generator", "Quickly & easily create high quality GUI art", 12037, "EdyWAvvj"),
			new ProductInformation("Screenshot Creator", "Create screenshots at any resolution", 8682, "yaPXxiob"),
			new ProductInformation("Sound Generator", "Runtime & In-Editor procedural audio", 12092, "c8LGXEQ7"),
			new ProductInformation("Sound Reactor", "Runtime audio events and reaction", -1, null),
			new ProductInformation("Sign Generator", "Procedurally create 3D signs & signposts", -1, null)
		});

		private static ProductInformation _primaryProduct = null;

		private Texture2D _banner;

		internal static List<ProductInformation> otherPoducts => allProducts.Where((ProductInformation p) => p != primaryProduct).ToList();

		internal string uniqueName => UniqueName(name);

		internal string bannerName => BannerName(name);

		internal string windowIconName => $"{name} Window Icon";

		internal static ProductInformation primaryProduct
		{
			get
			{
				return _primaryProduct;
			}
			set
			{
				_primaryProduct = value;
				if (_primaryProduct == null)
				{
					return;
				}
				string key = $"DarkArtsStudios.{_primaryProduct.uniqueName}.LastKnownVersion";
				string @string = EditorPrefs.GetString(key, "");
				string text = Assembly.GetExecutingAssembly().GetName().Version.ToString();
				if (@string != text)
				{
					EditorPrefs.SetString(key, text);
					if (@string == "")
					{
						UnityEngine.Debug.Log($"Installing {_primaryProduct.name} {text}");
					}
					else
					{
						UnityEngine.Debug.Log($"Upgrading {_primaryProduct.name} from {@string} to {text}");
					}
				}
			}
		}

		internal Texture2D banner
		{
			get
			{
				if (_banner == null)
				{
					if (primaryProduct == null)
					{
						return null;
					}
					_banner = ResourceUtility.TextureFromResource($"{uniqueName}-banner-small");
				}
				return _banner;
			}
		}

		internal static ProductInformation ByName(string productName)
		{
			ProductInformation result = null;
			foreach (ProductInformation allProduct in allProducts)
			{
				if (allProduct.name == productName)
				{
					result = allProduct;
				}
			}
			return result;
		}

		internal static string UniqueName(string productName)
		{
			return productName.Replace(" ", "-").ToLower();
		}

		internal static string BannerName(string productName)
		{
			return string.Format("{0}-{1}", UniqueName(productName), "banner-small");
		}

		internal ProductInformation(string name, string description, int asset_store_id, string trello_development_roadmap_id)
		{
			this.name = name;
			this.description = description;
			this.asset_store_id = asset_store_id;
			this.trello_development_roadmap_id = trello_development_roadmap_id;
		}

		internal static void OpenDarkArtsAssetStoreProducts(string productName)
		{
			Process.Start($"http://www.darkarts.co.za/unity-asset-store?utm_source=Unity&utm_medium=About%20Window&utm_campaign={UniqueName(productName)}");
		}

		internal void OpenDarkArtsAssetStoreProducts()
		{
			OpenDarkArtsAssetStoreProducts(name);
		}

		internal void EmailSupport()
		{
			Process.Start($"mailto:{uniqueName}@darkarts.co.za");
		}

		internal void HelpCentre()
		{
			Process.Start("http://darkarts.zendesk.com/hc");
		}

		internal void OpenTrelloDevelopmentRoadmap()
		{
			if (trello_development_roadmap_id != null)
			{
				Process.Start($"http://trello.com/b/{trello_development_roadmap_id}/development-roadmap");
			}
		}

		internal static string GoogleAnalytics(string medium, string campaign)
		{
			return string.Format("utm_source={0}&utm_medium={1}&utm_campaign={2}", primaryProduct.name.Replace(" ", "%20"), medium.Replace(" ", "%20"), campaign.Replace(" ", "%20"));
		}

		internal void OpenProductAssetStorePage(string medium, string campaign)
		{
			Process.Start($"com.unity3d.kharma:content/{asset_store_id}?{GoogleAnalytics(medium, campaign)}");
		}

		internal void OpenProductWebsite(string medium, string campaign)
		{
			Process.Start($"http://www.darkarts.co.za/{uniqueName}?{GoogleAnalytics(medium, campaign)}");
		}

		internal static void OpenAssetByGUID(string GUID)
		{
			Process.Start(System.IO.Path.GetFullPath(AssetDatabase.GUIDToAssetPath(GUID)));
		}

		internal static void OpenForumThread(string threadUniqueIdentifier)
		{
			Process.Start($"http://forum.unity3d.com/threads/{threadUniqueIdentifier}/");
		}
	}
}
