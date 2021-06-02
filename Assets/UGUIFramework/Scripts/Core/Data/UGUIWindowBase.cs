// ****************************************************
//     文件：UIWindowBase.cs
//     作者：积极向上小木木
//     邮箱：positivemumu@126.com
//     日期：2021/5/24 14:55:30
//     功能：UGUI框架基类
// *****************************************************

using UnityEngine;
using UnityEngine.EventSystems;

namespace UGUIFramework
{

    public abstract class UGUIWindowBase : MonoBehaviour,IPointerDownHandler
    {
        [HideInInspector]
        public UGUIWindowInfo windowInfo;

        [HideInInspector] 
        public UGUIWindowStates windowState;

        [HideInInspector]
        public RectTransform windowTransform;


        #region 生命周期函数

        public virtual void OpenWindow(UGUIWindowInfo windowInfo)
        {
            this.windowInfo = windowInfo;
            this.windowInfo.window = this;
            windowState = UGUIWindowStates.Open;
            windowTransform = GetComponent<RectTransform>(); 
        }

        public abstract void ShowWindow();

        public abstract void HideWindow();

        public virtual void CloseWindow()
        {
            
        }

        #endregion

        public void BackWindow(bool closeFrontWindow = false)
        {
            UGUIWindowManager.Instance.Stack.PopWindow(windowInfo.GameObjectName,closeFrontWindow);
        }

        public void SetWindowAsFocusWindow()
        {
            UGUIWindowManager.Instance.SetWindowAsFocusWindow(windowInfo.GameObjectName);
        }
        
        public void OnPointerDown(PointerEventData eventData)
        {
            SetWindowAsFocusWindow();
        }

        public void Hide()
        {
            if (windowState == UGUIWindowStates.Hide) 
                return;
            windowState = UGUIWindowStates.Hide;
            HideWindow();
        }

        public void Show()
        {
            if (windowState == UGUIWindowStates.Open) 
                return;
            windowState = UGUIWindowStates.Open;
            ShowWindow();
        }
    }
}