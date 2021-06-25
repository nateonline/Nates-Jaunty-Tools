using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NatesJauntyTools
{
	/// <summary> Used for easy setup for Singletons. </summary>
	/// <remarks> Currently the Singleton class utilizes the Awake MonoBehavior function for initialization, which means using the Awake function will override the automatic initialization. Use "SingletonAwake" instead.</remarks>
	public abstract class Singleton<T> : Script where T : Singleton<T>
	{
		/// <summary> Singleton Instance Variable. </summary>
		public static T _;

		/// <summary> Controls if the singleton object persists between scenes. </summary>
		// public bool isPersistant;


		public virtual void Awake()
		{
			InitializeSingleton();
			SingletonAwake();
		}

		protected abstract void SingletonAwake();


		public void InitializeSingleton()
		{
			_ = this as T;

			// if (isPersistant)
			// {
			// 	if (_ == null)
			// 	{
			// 		transform.SetParent(null);
			// 		DontDestroyOnLoad(gameObject);
			// 	}
			// 	else
			// 	{
			// 		Destroy(gameObject);
			// 	}
			// }
		}
	}
}
