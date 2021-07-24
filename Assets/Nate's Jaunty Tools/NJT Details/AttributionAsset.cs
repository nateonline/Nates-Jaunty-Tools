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
				new AttributionItem(
					"Inconsolata Font", 
					"",
					"https://github.com/googlefonts/Inconsolata",
					"Google Fonts", "https://fonts.google.com/attribution",
					"OFL 1.1", "https://github.com/googlefonts/Inconsolata/blob/main/OFL.txt"
				),
				new AttributionItem(
					"Google Sheets Logo", 
					"",
					"https://commons.wikimedia.org/wiki/File:Google_Sheets_logo_(2014-2020).svg",
					"Google", "https://www.google.com/sheets/about/",
					"None", "https://commons.wikimedia.org/wiki/File:Google_Sheets_logo_(2014-2020).svg"
				),
				new AttributionItem(
					"Font Awesome Icons", 
					"Original and modified icons from the free tier of Font Awesome",
					"https://fontawesome.com/",
					"Font Awesome", "https://fontawesome.com/license",
					"CC BY 4.0", "https://creativecommons.org/licenses/by/4.0/"
				)
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

		public AttributionItem(string name, string description, string sourceURL, string authorName, string authorURL, string licenseName, string licenseURL)
		{
			this.name = name;
			this.description = description;

			this.sourceURL = sourceURL;
			this.authorName = authorName;
			this.authorURL = authorURL;
			this.licenseName = licenseName;
			this.licenseURL = licenseURL;
		}
	}
}
