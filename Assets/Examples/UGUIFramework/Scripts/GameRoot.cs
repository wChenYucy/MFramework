// ****************************************************
//     文件：GameRoot.cs
//     作者：积极向上小木木
//     邮箱：positivemumu@126.com
//     日期：2021/5/29 20:24:32
//     功能：
// *****************************************************

using System;
using System.Collections;
using System.Collections.Generic;
using UGUIFramework;
using UnityEngine;

public class GameRoot : MonoBehaviour
{
    public UGUIWindowConfig mainConfig;
    // Start is called before the first frame update
    private void Awake()
    {
        UGUIWindowManager.Instance.InitUGUIFramework(mainConfig);
        UGUIWindowManager.Instance.OpenWindow<MainWindow>();
        //UGUIWindowManager.Instance.Stack.PushWindow<MainWindow>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            UGUIWindowManager.Instance.HideAllWindow(false,UGUIWindowTypes.PopupWindow);
            //UGUIWindowManager.Instance.Stack.PopAllWindow();
        }
    }
}
