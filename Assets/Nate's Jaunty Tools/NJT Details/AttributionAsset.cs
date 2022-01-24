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
					name = "Google Sheets Logo",
					description = "",
					sourceURL = "https://commons.wikimedia.org/wiki/File:Google_Sheets_logo_(2014-2020).svg",
					authorName = "Google",
					authorURL = "https://www.google.com/sheets/about/",
					licenseName = "None",
					licenseURL = "https://commons.wikimedia.org/wiki/File:Google_Sheets_logo_(2014-2020).svg"
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
