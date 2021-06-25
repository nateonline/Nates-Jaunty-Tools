using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NatesJauntyTools
{
	[AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = true)]
	public class InspectorButton : Attribute
	{
		public bool editMode;

		public InspectorButton()
		{
			editMode = false;
		}

		public InspectorButton(bool editMode)
		{
			this.editMode = editMode;
		}
	}


	public class ReadOnlyAttribute : PropertyAttribute
	{
	}


	// [AttributeUsage(AttributeTargets.All, Inherited = false, AllowMultiple = true)]
	// public class ReadOnly : Attribute { public ReadOnly() { } }


	// [AttributeUsage(AttributeTargets.All, Inherited = false, AllowMultiple = true)]
	// public class PositionHandle : Attribute
	// {
	// 	public bool ignoreObjectPosition;

	// 	public PositionHandle()
	// 	{
	// 		ignoreObjectPosition = false;
	// 	}

	// 	public PositionHandle(bool ignoreObjectPosition)
	// 	{
	// 		this.ignoreObjectPosition = ignoreObjectPosition;
	// 	}
	// }
}
