using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Status : MonoBehaviour
{
	[SerializeField] TMP_Text statusLabel;

	public void Set(string message)
	{
		gameObject.SetActive(true);
		statusLabel.text = message;
	}

	public void Clear()
	{
		gameObject.SetActive(false);
		statusLabel.text = "";
	}
}
