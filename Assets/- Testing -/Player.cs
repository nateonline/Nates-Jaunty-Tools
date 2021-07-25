using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NatesJauntyTools.NetCode;

public class Player : MonoBehaviour
{
    float lastSent;
	BaseClient client;

    void Start()
    {
		client = FindObjectOfType<BaseClient>();
    }

    void Update()
    {
        if (Time.time - lastSent > 1.0f)
		{
			PlayerPosition pos = new PlayerPosition(
				1, 
				transform.position.x, 
				transform.position.y, 
				transform.position.z
			);
			client.SendToServer(pos);
			lastSent = Time.time;
		}
    }
}
