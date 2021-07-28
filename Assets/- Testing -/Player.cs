using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
	public Client client;
	public Rigidbody rb;
	public float speed = 1f;

	void Update()
	{
		if (Input.GetKey(KeyCode.W)) { rb.AddForce(Vector3.forward * speed * Time.deltaTime); }
		if (Input.GetKey(KeyCode.S)) { rb.AddForce(Vector3.forward * speed * Time.deltaTime); }
		if (Input.GetKey(KeyCode.A)) { rb.AddForce(Vector3.forward * speed * Time.deltaTime); }
		if (Input.GetKey(KeyCode.D)) { rb.AddForce(Vector3.forward * speed * Time.deltaTime); }

		client.SendToServer(new PlayerPosition(client.PlayerID, transform.position.x, transform.position.y, transform.position.z));
	}
}
