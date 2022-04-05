using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Runtime
{
	public static void Quit()
	{
#if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false;
#else
		Application.Quit();
#endif
	}

	public static void CopyToClipboard(string contents)
	{
		GUIUtility.systemCopyBuffer = contents;
	}
}
