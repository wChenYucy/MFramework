using System;
using UnityEngine;
using UnityEngine.UI;
using UGUIFramework;

namespace UGUIFramework
{
	// Generate Id:919d34ff-0069-4221-9cde-03026ca845e9
	public partial class MainWindow : UGUIWindowBase
	{
		public const string Name = "MainWindow";

		private MainWindowData windowData = null;

		public void OnBagButtonDown()
		{
			UGUIWindowManager.Instance.OpenWindow<BagWindow>();
			//UGUIWindowManager.Instance.Stack.PushWindow<BagWindow>();
		}
		public void OnEquipmentButtonDown()
		{
			UGUIWindowManager.Instance.OpenWindow<EquipmentWindow>();
			//UGUIWindowManager.Instance.Stack.PushWindow<EquipmentWindow>();
		}
		public void OnStatusButtonDown()
		{
			UGUIWindowManager.Instance.OpenWindow<StatusWindow>();
			//UGUIWindowManager.Instance.Stack.PushWindow<StatusWindow>();
		}
	}
}
