using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using NatesJauntyTools;

public class SubscriberManager : Script
{
	List<Subscriber> subscribers = new List<Subscriber>();


	public void Setup()
	{
		foreach (Subscriber subscriber in FindObjectsOfType<MonoBehaviour>(includeInactive: true).OfType<Subscriber>())
		{
			Register(subscriber);
		}
	}

	public void Shutdown()
	{
		foreach (Subscriber subscriber in subscribers)
		{
			subscriber.Unsubscribe();
		}
	}

	public void Register(Subscriber subscriber)
	{
		subscriber.Subscribe();
		subscribers.Add(subscriber);
	}
}
