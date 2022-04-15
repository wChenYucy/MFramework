// ****************************************************
//     文件：UGUIWindowPreferences.cs
//     作者：积极向上小木木
//     邮箱：positivemumu@126.com
//     日期：2021/5/27 23:4:18
//     功能：UGUI框架配置文件
// *****************************************************

using System.IO;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace UGUIFramework
{
    public class UGUIFrameworkPreferences : SerializedScriptableObject
{
    private static UGUIFrameworkPreferences instance;

    public static UGUIFrameworkPreferences Instance
    {
        get
        {
            if (instance == null)
            {
                UGUIFrameworkPreferences temp = FindUIWindowConfigOverview();

                if (temp == null)
                {
                    string path = "Assets/Editor/Config";
                    string name = "UGUIFrameworkConfig.asset";
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    UGUIFrameworkPreferences obj = ScriptableObject.CreateInstance<UGUIFrameworkPreferences>();
                    AssetDatabase.CreateAsset(obj, path + "/" + name);
                    AssetDatabase.Refresh();
                    instance = FindUIWindowConfigOverview();
                }
                else
                    instance = temp;
            }
            return instance;
        }
    }
    private static UGUIFrameworkPreferences FindUIWindowConfigOverview()
    {
        UGUIFrameworkPreferences[] temp = AssetDatabase.FindAssets("t:UGUIFrameworkPreferences")
            .Select(guid =>
                AssetDatabase.LoadAssetAtPath<UGUIFrameworkPreferences>(AssetDatabase.GUIDToAssetPath(guid)))
            .ToArray();
        if (temp == null || temp.Length == 0)
            return null;
        return temp[0];

    }
    
    [InfoBox("UGUIFramework配置信息:\n" +
             "第一次打开窗口时会自动创建一个配置文件，默认配置文件的路径为\"Assets/Editor/Config\"。名称为\"UGUIFrameworkConfig.asset\"。\n" +
             "配置文件可根据需要移动至任意文件夹，框架会自动寻找，但请注意不要更改配置文件名称。\n" +
             "推荐将配置文件移动至\"Editor/\"目录下，可以防止配置文件被Unity在构建输出程序时打包。")]
    [HideLabel]
    [ReadOnly]
    public string tips;

    [BoxGroup("代码生成信息：",CenterLabel = true)]
    [Tooltip("脚本程序集名称，不填写时默认值为:\"Assembly-CSharp\"。\n如果计划将生成的UI脚本放在自定义的程序集中，请在此处输入自定义程序集的名称")]
    public string ScriptAssembly = "Assembly-CSharp";
    
    [BoxGroup("代码生成信息：",CenterLabel = true)]
    [Tooltip("脚本命名空间，不填写时默认值为:\"UGUIFramework\"")]
    public string ScriptNamespace = "UGUIFramework";
    
    [BoxGroup("代码生成信息：",CenterLabel = true)]
    [Tooltip("脚本生成文件夹，不填写时默认值为:\"Assets/\"。拖拽文件夹自动赋值")]
    [FolderPath]
    public string ScriptsFolder = "Assets/Scripts/UI";

    [BoxGroup("代码生成信息：",CenterLabel = true)]
    [Tooltip("在脚本生成文件夹中生成一个与窗口预制体名相同的文件夹来存放与该窗口预制体相关的脚本文件")]
    public bool CreateScriptsFolder = false;

    [InfoBox("点击按钮将配置文件恢复为默认设置")]
    [Button(ButtonStyle.FoldoutButton,ButtonHeight = 30)]
    public void ResetPreferences()
    {
        ScriptAssembly = "Assembly-CSharp";
        ScriptNamespace = "UGUIFramework";
        ScriptsFolder = "Assets/Scripts/UI";
        CreateScriptsFolder = false;
    }
}
}

