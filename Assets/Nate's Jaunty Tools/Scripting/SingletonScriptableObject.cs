using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NatesJauntyTools
{
	/// <summary> Create a ScriptableObject class that extends this class, use your own CreateAssetMenu, then create exactly one instance of your scriptable object <summary>
	public abstract class SingletonScriptableObject<T> : ScriptableObject where T : ScriptableObject
	{
		private static T _local = null;

		public static T _
		{
			get
			{
				if (_local == null)
				{
					T[] results = Resources.FindObjectsOfTypeAll<T>();
					if (results.Length == 0)
					{
						Debug.LogError($"ScriptableObjectSingleton: Didn't find any {typeof(T)} in the project!");
						return null;
					}
					else if (results.Length > 1)
					{
						Debug.LogError($"ScriptableObjectSingleton: Found more than one {typeof(T)} in the project!");
						return null;
					}

					_local = results[0];
					_local.hideFlags = HideFlags.DontUnloadUnusedAsset;
				}

				return _local;
			}
		}
	}
}
