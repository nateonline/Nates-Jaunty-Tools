using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NatesJauntyTools.Details
{
	// https://wiki.creativecommons.org/wiki/best_practices_for_attribution


	// Used for creating the asset if it's ever deleted
	// [CreateAssetMenu(menuName = "Nate's Jaunty Tools/Editor Details/Attribution", fileName = "Attribution")] 
	public class AttributionAsset : ScriptableObject
	{
		public List<AttributionItem> attributionItems;

		public void ApplyData()
		{
			attributionItems = new List<AttributionItem>()
			{
				new AttributionItem()
				{
					name = "Inconsolata Font",
					description = "",
					sourceURL = "https://github.com/googlefonts/Inconsolata",
					authorName = "Google Fonts",
					authorURL = "https://fonts.google.com/attribution",
					licenseName = "OFL 1.1",
					licenseURL = "https://github.com/googlefonts/Inconsolata/blob/main/OFL.txt"
				},
				new AttributionItem()
				{
					name = "Font Awesome Icons",
					description = "Original and modified icons from the free tier of Font Awesome",
					sourceURL = "https://fontawesome.com/",
					authorName = "Font Awesome",
					authorURL = "https://fontawesome.com/license",
					licenseName = "CC BY 4.0",
					licenseURL = "https://creativecommons.org/licenses/by/4.0/"
				},
				new AttributionItem()
				{
					name = "Controller & Keyboard Prompts",
					description = "",
					sourceURL = "https://thoseawesomeguys.com/prompts/",
					authorName = "Those Awesome Guys",
					authorURL = "https://thoseawesomeguys.com/",
					licenseName = "Creative Commons 0 (CC0)",
					licenseURL = "https://creativecommons.org/share-your-work/public-domain/cc0/"
				}
			};
		}
	}


	[System.Serializable]
	public class AttributionItem
	{
		public string name;
		public string description;

		public string sourceURL;
		public string authorName;
		public string authorURL;
		public string licenseName;
		public string licenseURL;
	}
}
