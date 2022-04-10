using System;
using UnityEngine;
using UnityEngine.UI;
using UGUIFramework;

namespace UGUIFramework
{
	// Generate Id:b91e5a9d-f4d1-48dd-9400-8ea9ccd74663
	public partial class StatusWindow : UGUIWindowBase
	{
		public const string Name = "StatusWindow";
		
		
		private StatusWindowData windowData = null;
		
		public void OnCloseButtonDown()
		{
			UGUIWindowManager.Instance.HideWindow<StatusWindow>();
			//BackWindow();
		}
	}
}
