using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NatesJauntyTools;
using UnityEngine.Events;
using TMPro;

public abstract class InputField : Script
{
	public abstract string Value { get; set; }
	public abstract void Submit();
	public bool Interactible;
}
