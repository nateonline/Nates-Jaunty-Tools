using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NatesJauntyTools.Details
{
	// Used for creating the asset if it's ever deleted
	// [CreateAssetMenu(menuName = "Nate's Jaunty Tools/Editor Details/Readme", fileName = "Read Me")] 
	public class ReadmeAsset : ScriptableObject
	{
		public string versionNumber;
		public int[] VersionSections
		{
			get
			{
				string[] stringSections = versionNumber.Split('.');
				int[] intSections = new int[stringSections.Length];
				for (int i = 0; i < stringSections.Length; i++) { intSections[i] = int.Parse(stringSections[i]); }
				return intSections;
			}
		}

		public string unityVersion;

		public List<ChangeLogItem> changeLog;

		public void ApplyData()
		{
			versionNumber = "2022.05.12";
			unityVersion = "2020.3.10f1";

			changeLog = new List<ChangeLogItem>()
			{
				new ChangeLogItem(ChangeType.Modified, "Fixed naming of Subscriber.Unubscribe() missing the \"s\" after \"Un\""),
				new ChangeLogItem(ChangeType.Added, "Added SubscriberManager for easy subscriber handling in new projects"),
			};
		}
	}


	public enum ChangeType { Added, Deleted, Modified, Spacer }


	[System.Serializable]
	public class ChangeLogItem
	{
		public ChangeType type;
		public string description;

		public ChangeLogItem(ChangeType type, string description)
		{
			this.type = type;
			this.description = description;
		}

		public ChangeLogItem(ChangeType type)
		{
			this.type = type;
		}

		public override string ToString()
		{
			switch (type)
			{
				case ChangeType.Added: return $"+  {description}";
				case ChangeType.Deleted: return $"-  {description}";
				case ChangeType.Modified: return $"*  {description}";
				default: return $"{description}";
			}
		}
	}
}
