// ****************************************************
//     文件：UGUIWindowStack.cs
//     作者：积极向上小木木
//     邮箱：positivemumu@126.com
//     日期：2021/5/31 16:18:52
//     功能：UGUIFramework栈结构类
// *****************************************************

using System;
using System.Collections.Generic;
using UnityEngine;

namespace UGUIFramework
{
    public class UGUIWindowStack
    {
        private UGUIWindowManager manager;
        private Dictionary<string, UGUIWindowBase> windowIndex;
        private Stack<UGUIWindowBase> mUIStack;

        public UGUIWindowStack(UGUIWindowManager manager)
        {
            this.manager = manager;
            windowIndex = new Dictionary<string, UGUIWindowBase>();
            mUIStack = new Stack<UGUIWindowBase>();
        }
        
        /// <summary>
        /// 初始化函数
        /// </summary>
        public void InitStack()
        {
            List<string> removeKeyList = new List<string>();
            foreach (var VARIABLE in windowIndex)
            {
                UGUIWindowBase temp = VARIABLE.Value;
                if (!temp.windowInfo.DontClearOnChange)
                {
                    removeKeyList.Add(VARIABLE.Key);
                    temp.Hide();
                    manager.CloseWindow(temp);
                }
            }

            foreach (var VARIABLE in removeKeyList)
            {
                windowIndex.Remove(VARIABLE);
            }
        }
        
        /// <summary>
        /// 打开窗体
        /// </summary>
        /// <param name="hidePreWindow">是否隐藏现在显示的窗体</param>
        /// <typeparam name="T">要打开的窗体类型</typeparam>
        /// <returns>打开的窗体对象</returns>
        public T PushWindow<T>(bool hidePreWindow = false) where T : UGUIWindowBase
        {
            return PushWindow(typeof(T)) as T;
        }

        /// <summary>
        /// 打开窗体
        /// </summary>
        /// <param name="windowType">要打开的窗体类型</param>
        /// <param name="hidePreWindow">要打开的窗体类型是否隐藏现在显示的窗体</param>
        /// <returns>打开的窗体对象</returns>
        public UGUIWindowBase PushWindow(Type windowType,bool hidePreWindow = false)
        {
            var window = GetWindowByName(windowType.Name);

            if (window == null)
            {
                window = manager.CreateWindow(windowType);
                if (window != null)
                {
                    windowIndex.Add(windowType.Name,window);
                    
                }
                else
                {
                    return null;
                }
            }
            window.Show();
            mUIStack.Push(window);
            manager.SetWindowAsLastWindow(window);
            return window;
        }
        
        /// <summary>
        /// 关闭窗体
        /// </summary>
        /// <param name="windowName">窗体名称</param>
        /// <param name="destroyWindow">是否销毁窗体</param>
        public void PopWindow(string windowName, bool destroyWindow = false) 
        {
            Debug.Log(mUIStack.Count);
            if (mUIStack.Contains(GetWindowByName(windowName)))
            {
                UGUIWindowBase temp = mUIStack.Pop();
                while (temp.windowInfo.GameObjectName != windowName)
                {
                    temp.Hide();
                    if (destroyWindow)
                    {
                        temp.CloseWindow();
                        manager.CloseWindow(temp);
                        windowIndex.Remove(temp.windowInfo.GameObjectName);
                    }
                    temp = mUIStack.Pop();
                }
                temp.Hide();
                if (destroyWindow)
                {
                    temp.CloseWindow();
                    manager.CloseWindow(temp);
                    windowIndex.Remove(temp.windowInfo.GameObjectName);
                }
            }
            else
            {
                Debug.LogError("UGUIFrameworkStackError: 名称为"+windowName+"的窗口未打开，无法隐藏或关闭！");
            }
        }

        public void PopAllWindow(bool destroyWindow = false)
        {
            while (mUIStack.Count > 0) 
            {
                UGUIWindowBase temp = mUIStack.Pop();
                temp.Hide();
                if (destroyWindow)
                {
                    temp.CloseWindow();
                    manager.CloseWindow(temp);
                    windowIndex.Remove(temp.windowInfo.GameObjectName);
                }
            }
        }
        
        private UGUIWindowBase GetWindowByName(string windowName)
        {
            if (windowIndex.ContainsKey(windowName))
            {
                return windowIndex[windowName];
            }
            return null;
        }
        

        // public void Push<T>() where T : UGUIWindowBase
        // {
        //     Push(UIKit.GetPanel<T>());
        // }
        //
        // public void Push(UGUIWindowBase window)
        // {
        //     if (window != null)
        //     {
        //         mUIStack.Push(view.Info);
        //         view.Close();
        //
        //         var panelSearchKeys = PanelSearchKeys.Allocate();
        //
        //         panelSearchKeys.GameObjName = view.Transform.name;
        //
        //         UIManager.Instance.RemoveUI(panelSearchKeys);
        //
        //         panelSearchKeys.Recycle2Cache();
        //     }
        // }
        //
        // public void Pop()
        // {
        //     var previousPanelInfo = mUIStack.Pop();
        //
        //     var panelSearchKeys = PanelSearchKeys.Allocate();
        //
        //     panelSearchKeys.GameObjName = previousPanelInfo.GameObjName;
        //     panelSearchKeys.Level = previousPanelInfo.Level;
        //     panelSearchKeys.UIData = previousPanelInfo.UIData;
        //     panelSearchKeys.AssetBundleName = previousPanelInfo.AssetBundleName;
        //     panelSearchKeys.PanelType = previousPanelInfo.PanelType;
        //
        //     UIManager.Instance.OpenUI(panelSearchKeys);
        //     
        //     panelSearchKeys.Recycle2Cache();
        // }
    }
}

