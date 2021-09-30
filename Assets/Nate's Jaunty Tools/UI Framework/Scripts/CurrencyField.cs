using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class CurrencyField : NumberField
{
	public TMP_Text currencySymbol;

	public override string Value
	{
		get { return base.Value; }
		set { tmpInputField.text = string.Format("{0:#.00}", DecimalValue / 100m); }
	}
}
