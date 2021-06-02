using UnityEngine;
using UnityEngine.UI;
using UGUIFramework;

namespace UGUIFramework
{
	public partial class MainWindow
	{
		public override void OpenWindow(UGUIWindowInfo windowInfo)
		{
			base.OpenWindow(windowInfo);
			windowTransform = GetComponent<RectTransform>();
			//在此处添加初始化窗口代码
		}
		
		public override void ShowWindow()
		{
		}
		
		public override void HideWindow()
		{
		}
		
		public override void CloseWindow()
		{
			base.CloseWindow();
			//在此处添加销毁窗口代码
			
			windowData = null;
		}
	}
}
