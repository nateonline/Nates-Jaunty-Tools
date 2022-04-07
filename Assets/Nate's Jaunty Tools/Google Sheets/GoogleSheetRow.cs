using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Globalization;
using UnityEngine;

namespace NatesJauntyTools.GoogleSheets
{
	public abstract class GoogleSheetRow
	{
		public abstract void Deserialize(List<object> rowValues);

		public abstract List<object> Serialize();
	}
}
