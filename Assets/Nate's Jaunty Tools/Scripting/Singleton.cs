using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace NatesJauntyTools
{
	/// <summary> Used for easy setup for Singletons. </summary>
	/// <remarks> Currently the Singleton class utilizes the Awake MonoBehavior function for initialization, which means using the Awake function will override the automatic initialization. Use "PostInitialize" instead.</remarks>
	public abstract class Singleton<T> : Script where T : Singleton<T>
	{
		/// <summary> Singleton Instance Variable. </summary>
		public static T _;
		protected bool isPersistent = true;

		protected void Awake()
		{
			PreInitialize();
			InitializeSingleton();
			PostInitialize();

			SceneManager.sceneLoaded += OnSceneLoaded;
		}

		protected virtual void PreInitialize() { }

		protected void InitializeSingleton()
		{
			if (_ == null)
			{
				_ = this as T;
				transform.SetParent(null);
				if (isPersistent) { DontDestroyOnLoad(gameObject); }
			}
			else
			{
				Destroy(gameObject);
			}
		}

		protected virtual void PostInitialize() { }

		protected virtual void OnSceneLoaded(Scene scene, LoadSceneMode mode) { }
	}
}
