// ****************************************************
//     文件：UGUIWindowPreferencesWindowEditor.cs
//     作者：积极向上小木木
//     邮箱：positivemumu@126.com
//     日期：2021/5/27 23:3:47
//     功能：UGUI框架配置文件编辑窗口
// *****************************************************

using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using UnityEditor;

namespace UGUIFramework
{
    public class UGUIWindowPreferencesWindowEditor : OdinEditorWindow
    {
        [MenuItem("Tools/UGUIFramework/Preferences")]
        public static void OpenWindow()
        {
            UGUIWindowPreferencesWindowEditor window = GetWindow<UGUIWindowPreferencesWindowEditor>("UGUIFramework Preferences");
            window.position = GUIHelper.GetEditorWindowRect().AlignCenter(800, 500);
            window.autoRepaintOnSceneChange = true;
            window.Show();
        }
    
        protected override object GetTarget()
        {
            return UGUIFrameworkPreferences.Instance;
        }
    }
}

