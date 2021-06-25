using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NatesJauntyTools.EditorDetails
{
	// Used for creating the asset if it's ever deleted
	// [CreateAssetMenu(menuName = "Nate's Cool Tools/Editor Details/Readme", fileName = "Read Me")] 
	public class ReadmeAsset : ScriptableObject
	{
		public string version_niceNumber;
		public string version_trueNumber;

		public List<ChangeLogItem> changeLog;

		public void ApplyData()
		{
			version_niceNumber = "0.0.1";
			version_trueNumber = "00.00.01";

			changeLog = new List<ChangeLogItem>()
			{
				new ChangeLogItem(ChangeType.Modified, "Created Nate's Jaunty Tools")
			};
		}
	}


	public enum ChangeType { Added, Deleted, Modified }


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
