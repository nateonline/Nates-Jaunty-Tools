using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// <summary> Allows MonoBehaviours to subscribe and unsubscribe from events, regardless of if they're active in the scene </summary>
public interface Subscriber
{
	public void Subscribe();
	public void Unsubscribe();
}
