using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace NatesJauntyTools
{
	public class TabSystem : Script
	{
		[Header("Buttons")]
		[SerializeField] Transform buttonParent;
		[SerializeField] GameObject buttonPrefab;

		[Header("Tabs")]
		[SerializeField] Transform tabParent;
		[SerializeField] List<Tab> tabs = new List<Tab>();
		[SerializeField, ReadOnly] Tab currentTab;


		void Awake() => InitializeTabs();

		void InitializeTabs()
		{
			tabs = tabParent.GetComponentsInChildren<Tab>(includeInactive: true).ToList();
			foreach (Tab tab in tabs)
			{
				tab.gameObject.SetActive(false);
				SmartButton tabButton = Instantiate(buttonPrefab, buttonParent).GetComponent<SmartButton>();
				tabButton.Text = tab.name;
				tabButton.OnClick.AddListener(() => NavigateTo(tab));
			}
			if (tabs.Count > 0) { NavigateTo(tabs[0]); }
		}

		public void NavigateTo(Tab tab)
		{
			if (currentTab)
			{
				currentTab.gameObject.SetActive(false);
				currentTab.OnClose();
			}

			currentTab = tab;

			currentTab.gameObject.SetActive(true);
			currentTab.OnOpen();
		}
	}
}
