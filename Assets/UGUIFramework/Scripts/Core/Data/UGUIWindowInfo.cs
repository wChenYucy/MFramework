// ****************************************************
//     文件：UIWindowBaseData.cs
//     作者：积极向上小木木
//     邮箱：positivemumu@126.com
//     日期：2021/5/24 14:31:29
//     功能：UGUI框架窗口信息类
// *****************************************************

using Sirenix.OdinInspector;
using UnityEngine;

namespace UGUIFramework
{

    [System.Serializable]
    public class UGUIWindowInfo
    {
        [VerticalGroup("Split")]
        [HorizontalGroup("Split/One", MarginRight = 30f, LabelWidth = 130,Width = 0.6f)]
        [Tooltip("窗口预制体名称")]
        public string GameObjectName;
            
        [HorizontalGroup("Split/One", LabelWidth = 130,Width = 0.4f)]
        [Tooltip("窗体类型：\n1.弹出窗体，可以调整显示顺序的窗体。\n2.底部静态窗体，永远处于最底部的窗体。\n3.顶部静态窗体，永远处于最底部的窗体。")]
        public UGUIWindowTypes WindowType;
        
        [HorizontalGroup("Split/Two", MarginRight = 30f, LabelWidth = 130,Width = 0.6f)]
        [EnableIf("UseAssetBundle")]
        [Tooltip("预制体保存的AssetBundle文件的名称")]
        public string AssetBundleName;

        [HorizontalGroup("Split/Two", LabelWidth = 130,Width = 0.4f)]
        [Tooltip("是否启用AssetBundle加载资源，默认不启用（采用Resources.Load方法加载资源）")]
        public bool UseAssetBundle;
        
        [HorizontalGroup("Split/Three", MarginRight = 30f, LabelWidth = 130,Width = 0.6f)]
        [EnableIf("UseAssetBundle")]
        [Tooltip("预制体保存的AssetBundle文件的路径。拖拽文件或文件夹可以直接赋值")] [FilePath]
        public string AssetBundlePath;

        [HorizontalGroup("Split/Three", LabelWidth = 130,Width = 0.4f)]
        [Tooltip("预制体保存的AssetBundle文件的路径。拖拽文件或文件夹可以直接赋值")]
        public bool DontClearOnChange;
        
        [HorizontalGroup("Line2", LabelWidth = 130)]
        [DisableIf("UseAssetBundle")]
        [Tooltip("预制体存放路径（使用Resources.Load加载，请将预制体放在Resources文件夹下）。拖拽文件或文件夹可以直接赋值")] [FolderPath]
        public string PrefabsPath;
        
        [HideInInspector]
        public UGUIWindowBase window;
    }
}
