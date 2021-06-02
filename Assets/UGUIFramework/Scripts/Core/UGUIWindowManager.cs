// ****************************************************
//     文件：UGUIWindowManager.cs
//     作者：积极向上小木木
//     邮箱：positivemumu@126.com
//     日期：2021/5/31 13:24:15
//     功能：UGUIFramework核心管理类
// *****************************************************

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UGUIFramework
{

    public class UGUIWindowManager : MonoBehaviour
    {
        private static UGUIWindowManager instance;

        public static UGUIWindowManager Instance
        {
            get
            {
                if (instance == null)
                {
                    GameObject go = GameObject.Find("ScriptsHolder");
                    if (go == null)
                    {
                        go = Instantiate(new GameObject(), Vector3.zero, Quaternion.identity);
                        go.name = "ScriptsHolder";
                        instance = go.AddComponent<UGUIWindowManager>();
                    }
                    else
                    {
                        instance = go.AddComponent<UGUIWindowManager>();
                    }

                    instance.windowIndex = new Dictionary<Type, UGUIWindowBase>();
                    instance.cacheWindowInfos = new List<UGUIWindowInfo>();
                    instance.Stack = new UGUIWindowStack(instance);
                }

                return instance;
            }
        }

        private List<UGUIWindowInfo> cacheWindowInfos;
        private Dictionary<Type, UGUIWindowBase> windowIndex;
        public UGUIWindowStack Stack;
        private Transform UGUICanvas;
        
        /// <summary>
        /// 初始化UGUI框架
        /// </summary>
        /// <param name="windowConfig">配置文件</param>
        public void InitUGUIFramework(UGUIWindowConfig windowConfig)
        {
            UGUICanvas = null;
            if (!GameObject.Find("UGUICanvas"))
            {
                Debug.LogError("UGUIFrameworkInitError: 未找到名称为UGUICanvas的物体，请将预制体拖放到场景中");
                return;
            }
            UGUICanvas = GameObject.Find("UGUICanvas").transform;
            
            //清除上一个配置
            Stack.InitStack();
            
            //清空窗口信息缓存列表
            for (int i = 0; i < cacheWindowInfos.Count;)
            {
                if (!cacheWindowInfos[i].DontClearOnChange)
                {
                    cacheWindowInfos.RemoveAt(i);
                }
                else
                {
                    i++;
                }
            }
            //清空索引字典
            List<Type> removeKeyList = new List<Type>();
            foreach (var VARIABLE in windowIndex)
            {
                UGUIWindowBase temp = VARIABLE.Value;
                if (!temp.windowInfo.DontClearOnChange)
                {
                    removeKeyList.Add(VARIABLE.Key);
                    temp.Hide();
                    CloseWindow(temp);
                }
            }

            foreach (var VARIABLE in removeKeyList)
            {
                windowIndex.Remove(VARIABLE);
            }

            if (windowConfig == null)
            {
                Debug.LogError("UGUIFrameworkInitError: 配置文件读取失败，配置文件为空！");
                return;
            }
            
            //加载配置文件
            foreach (var windowInfo in windowConfig.UIWindowInfoList)
            {
                if (GetUGUIWindowInfoByName(windowInfo.GameObjectName) != null)
                {
                    Debug.LogWarning("UGUIFrameworkInitWarning: 尝试加载"+windowInfo.GameObjectName+"窗体时发现该窗体已被缓存，窗体未被加载。");
                }
                else
                {
                    cacheWindowInfos.Add(windowInfo);
                }
            }
        }
        
        /// <summary>
        /// 打开窗口
        /// </summary>
        /// <typeparam name="T">窗口类型</typeparam>
        /// <returns>打开的窗口对象</returns>
        public T OpenWindow<T>() where T : UGUIWindowBase
        {
            return OpenWindow(typeof(T)) as T;
        }

        /// <summary>
        /// 打开窗口
        /// </summary>
        /// <param name="windowType">窗口类型</param>
        /// <returns>打开的窗口对象</returns>
        public UGUIWindowBase OpenWindow(Type windowType)
        {
            var window = GetWindowByType(windowType);

            if (window == null)
            {
                window = CreateWindow(windowType);
                if (window != null)
                {
                    windowIndex.Add(windowType,window);
                }
                else
                {
                    return null;
                }
            }
            
            window.Show();
            SetWindowAsLastWindow(window);
            return window;
        }
        
        /// <summary>
        /// 关闭窗口
        /// </summary>
        /// <typeparam name="T">窗口类型</typeparam>
        public void HideWindow<T>() where T : UGUIWindowBase
        {
            HideWindow(typeof(T));
        }

        /// <summary>
        /// 隐藏窗口
        /// </summary>
        /// <param name="windowType">窗口类型</param>
        public void HideWindow(Type windowType)
        {
            var window = GetWindowByType(windowType);
            if (window != null)
            {
                window.Hide();
            }
            else
            {
                Debug.LogWarning("UGUIFrameworkHideWindowWarning: 窗口"+windowType.Name+"未打开，无法关闭！");
            }
        }

        /// <summary>
        /// 关闭并销毁窗口
        /// </summary>
        /// <typeparam name="T">窗口类型</typeparam>
        public void CloseWindow<T>() where T : UGUIWindowBase
        {
            CloseWindow(typeof(T));
        }

        /// <summary>
        /// 关闭并销毁窗口
        /// </summary>
        /// <param name="windowType">窗口类型</param>
        public void CloseWindow(Type windowType)
        {
            var window = GetWindowByType(windowType);
            if (window != null)
            {
                window.Hide();
                CloseWindow(window);
                windowIndex.Remove(windowType);
            }
            else
            {
                Debug.LogWarning("UGUIFrameworkCloseWindowWarning: 窗口"+windowType.Name+"未打开，无法关闭！");
            }
        }
        
        /// <summary>
        /// 隐藏某一类或所有窗口
        /// </summary>
        /// <param name="hideAllWindow">是否隐藏所有窗口，当为false时第二个参数生效</param>
        /// <param name="windowType">要隐藏的窗口类型，默认值为弹出窗口(PopupWindow)</param>
        public void HideAllWindow(bool hideAllWindow, UGUIWindowTypes windowType = UGUIWindowTypes.PopupWindow)
        {
            if (hideAllWindow)
            {
                foreach (var VARIABLE in windowIndex)
                {
                    VARIABLE.Value.Hide();
                }
            }
            else
            {
                foreach (var VARIABLE in windowIndex)
                {
                    if (VARIABLE.Value.windowInfo.WindowType == windowType)
                    {
                        VARIABLE.Value.Hide();
                    }
                }
            }
        }

        /// <summary>
        /// 关闭并销毁某一类或所有窗口
        /// </summary>
        /// <param name="hideAllWindow">是否隐藏所有窗口，当为false时第二个参数生效</param>
        /// <param name="windowType">要隐藏的窗口类型，默认值为弹出窗口(PopupWindow)</param>
        public void CloseAllWindow(bool hideAllWindow, UGUIWindowTypes windowType = UGUIWindowTypes.PopupWindow)
        {
            List<Type> removeKeyList = new List<Type>();
            if (hideAllWindow)
            {
                foreach (var VARIABLE in windowIndex)
                {
                    VARIABLE.Value.Hide();
                    CloseWindow(VARIABLE.Value);
                    removeKeyList.Add(VARIABLE.Key);
                }
            }
            else
            {
                foreach (var VARIABLE in windowIndex)
                {
                    if (VARIABLE.Value.windowInfo.WindowType == windowType)
                    {
                        VARIABLE.Value.Hide();
                        CloseWindow(VARIABLE.Value);
                        removeKeyList.Add(VARIABLE.Key);
                    }
                }
            }

            foreach (var VARIABLE in removeKeyList)
            {
                windowIndex.Remove(VARIABLE);
            }
        }

        //系统调用方法
        public UGUIWindowBase CreateWindow(Type windowType)
        {
            UGUIWindowInfo windowInfo= GetUGUIWindowInfoByName(windowType.Name);
            if (windowInfo != null)
            {
                if (!windowInfo.UseAssetBundle)
                {
                    string path = windowInfo.PrefabsPath.ResourcesPath() + "/" + windowInfo.GameObjectName;
                    GameObject windowPrefabs = Resources.Load<GameObject>(path);
                    GameObject window = null;
                    if (windowPrefabs != null)
                    {
                        switch (windowInfo.WindowType)
                        {
                            case UGUIWindowTypes.BottomStaticWindow:
                                window = Instantiate(windowPrefabs, UGUICanvas.GetChild(0));
                                break;
                            case UGUIWindowTypes.PopupWindow:
                                window = Instantiate(windowPrefabs, UGUICanvas.GetChild(1));
                                break;
                            case UGUIWindowTypes.TopStaticWindow:
                                window = Instantiate(windowPrefabs, UGUICanvas.GetChild(2));
                                break;
                        }
                        
                        window.name = windowInfo.GameObjectName;
                        UGUIWindowBase prefabsWindowBase = window.GetComponent<UGUIWindowBase>();
                        prefabsWindowBase.OpenWindow(windowInfo);
                        return prefabsWindowBase;
                    }
                    else
                    {
                        Debug.LogError("UGUIFrameworkCreateWindowError: 路径" + windowInfo.PrefabsPath + "下不存在窗体预制体" + windowInfo.GameObjectName);
                    }
                }
                else
                {
                    Debug.LogError("UGUIFrameworkCreateWindowError: 路径"+windowInfo.PrefabsPath+"下不存在窗体预制体"+windowInfo.GameObjectName);
                }
            }
            else
            {
                Debug.LogError("UGUIFrameworkCreateWindowError: 未找到名称为："+windowType.Name+"的窗体信息！");
            }

            return null;
        }
        
        public void SetWindowAsLastWindow(UGUIWindowBase windowBase)
        {
            switch (windowBase.windowInfo.WindowType)
            {
                case UGUIWindowTypes.BottomStaticWindow:
                    windowBase.windowTransform.SetSiblingIndex(UGUICanvas.GetChild(0).childCount - 1);
                    break;
                case UGUIWindowTypes.PopupWindow:
                    windowBase.windowTransform.SetSiblingIndex(UGUICanvas.GetChild(1).childCount - 1);
                    break;
                case UGUIWindowTypes.TopStaticWindow:
                    windowBase.windowTransform.SetSiblingIndex(UGUICanvas.GetChild(2).childCount - 1);
                    break;
            }
        }

        public void CloseWindow(UGUIWindowBase window)
        {
            window.CloseWindow();
            Destroy(window.gameObject);
        }
        
        /// <summary>
        /// 将窗口设置为焦点窗口（同级别下最靠前的窗口）
        /// </summary>
        /// <param name="windowName"></param>
        public void SetWindowAsFocusWindow(string windowName)
        {
            UGUIWindowInfo windowInfo = GetUGUIWindowInfoByName(windowName);
            if(windowInfo!=null&&windowInfo.window!=null)
                SetWindowAsLastWindow(windowInfo.window);
        }
        
        private UGUIWindowInfo GetUGUIWindowInfoByName(string windowName)
        {
            foreach (var windowInfo in cacheWindowInfos)
            {
                if (windowInfo.GameObjectName == windowName)
                    return windowInfo;
            }

            return null;
        }

        private UGUIWindowBase GetWindowByType(Type windowType)
        {
            if (windowIndex.ContainsKey(windowType))
            {
                return windowIndex[windowType];
            }
            return null;
        }
    }
}
