using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary> Allows MonoBehaviours to subscribe and unsubscribe from events, regardless of if they're active in the scene </summary>
public interface Subscriber
{
	public void Subscribe();
	public void Unubscribe();
}

public static class SubscriberHelper
{
	public static List<Subscriber> GetSubscribersInScene()
	{
		return GameObject.FindObjectsOfType<MonoBehaviour>(includeInactive: true).OfType<Subscriber>().ToList();
	}
}
