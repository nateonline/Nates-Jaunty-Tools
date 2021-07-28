using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Networking.Transport;
using Unity.Collections;

namespace NatesJauntyTools.NetCode
{
	public class BaseHost : MonoBehaviour
	{
		[ReadOnly] public bool IsInitialized;

		// TODO: Have this class find or create both components, initialize them, and handle the updates and shutdowns. Will also expose all of the public functions
	}
}
