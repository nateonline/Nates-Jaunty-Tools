using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class NumberField : TextField
{
	public int IntValue => int.TryParse(Value, out int intValue) ? intValue : default(int);
	public decimal DecimalValue => decimal.TryParse(Value, out decimal decimalValue) ? decimalValue : default(decimal);
}
