using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelInfo
{

    #region 数据成员
    //包名
    private string _panelName;
    public string PanelName
    {
        get
        {
            return _panelName;
        }

        set
        {
            _panelName = value;
        }
    }
    //窗体名
    private string _panelPath;
    public string PanelPath
    {
        get
        {
            return _panelPath;
        }

        set
        {
            _panelPath = value;
        }
    }
    //window类型
    private UIPanelTypes _uiPanelType;
    public UIPanelTypes UIPanelType
    {
        get
        {
            return _uiPanelType;
        }
        set
        {
            _uiPanelType = value;
        }
    }
    #endregion

    /// <summary>
    /// 获取当前Panel的名字
    /// </summary>
    /// <returns></returns>
    public string GetPanelName()
    {
        return _panelName;
    }

    /// <summary>
    /// 获取当前Panel的加载路径
    /// </summary>
    /// <returns></returns>
    public string GetPanelPath()
    {
        return _panelPath;
    }
}
