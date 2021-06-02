// ****************************************************
//     文件：UIWindowData_SO.cs
//     作者：积极向上小木木
//     邮箱：positivemumu@126.com
//     日期：2021/5/24 16:4:47
//     功能：UGUI框架窗口配置文件
// *****************************************************

using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace UGUIFramework
{
    public class UGUIWindowConfig : ScriptableObject
    {
        [ListDrawerSettings(ShowIndexLabels = true,ListElementLabelName = "GameObjectName")]
        public List<UGUIWindowInfo> UIWindowInfoList=new List<UGUIWindowInfo>();
    }
}

