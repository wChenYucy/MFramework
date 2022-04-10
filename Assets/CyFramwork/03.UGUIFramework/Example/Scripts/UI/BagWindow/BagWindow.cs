using System;
using UnityEngine;
using UnityEngine.UI;
using UGUIFramework;

namespace UGUIFramework
{
	// Generate Id:7109f054-bb67-40e1-99ee-0dfed202e975
	public partial class BagWindow : UGUIWindowBase
	{
		public const string Name = "BagWindow";
		
		
		private BagWindowData windowData = null;


		public void OnCloseButtonDown()
		{
			UGUIWindowManager.Instance.HideWindow<BagWindow>();
			//BackWindow();
		}
	}
}
