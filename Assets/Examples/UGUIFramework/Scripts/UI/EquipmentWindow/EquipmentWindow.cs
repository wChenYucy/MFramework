using System;
using UnityEngine;
using UnityEngine.UI;
using UGUIFramework;

namespace UGUIFramework
{
	// Generate Id:fa00ad63-cae7-4c5c-884b-5c0e4527e85a
	public partial class EquipmentWindow : UGUIWindowBase
	{
		public const string Name = "EquipmentWindow";
		
		
		private EquipmentWindowData windowData = null;
		
		public void OnCloseButtonDown()
		{
			UGUIWindowManager.Instance.HideWindow<EquipmentWindow>();
			//BackWindow();
		}
	}
}
