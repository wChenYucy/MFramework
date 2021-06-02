// ****************************************************
//     文件：UIWindowManagerWindowEditor.cs
//     作者：积极向上小木木
//     邮箱：positivemumu@126.com
//     日期：2021/5/24 16:6:37
//     功能：UGUI框架窗口配置文件编辑窗口
// *****************************************************

using System.Linq;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;

namespace UGUIFramework
{
    public class UGUIWindowConfigEditWindow : OdinMenuEditorWindow
    {

        [MenuItem("Tools/UGUIFramework/WindowConfig EditWindow")]
        public static void OpenWindow()
        {
            UGUIWindowConfigEditWindow windowConfigEditWindow = GetWindow<UGUIWindowConfigEditWindow>("WindowConfig EditWindow");
            windowConfigEditWindow.position = GUIHelper.GetEditorWindowRect().AlignCenter(800, 500);
            windowConfigEditWindow.autoRepaintOnSceneChange = true;
            windowConfigEditWindow.Show();
        }
        
        protected override OdinMenuTree BuildMenuTree()
        {
            OdinMenuTree tree = new OdinMenuTree();
            tree.Config.DrawSearchToolbar = true;
            foreach (UGUIWindowConfig uiWindowConfig in UIWindowConfigs)
            {
                tree.Add(uiWindowConfig.name,uiWindowConfig);
            }
            return tree;
        }

        protected override void OnBeginDrawEditors()
        {
            var selected = this.MenuTree.Selection.FirstOrDefault();
            var toolbarHeight = this.MenuTree.Config.SearchToolbarHeight;
            SirenixEditorGUI.BeginHorizontalToolbar(toolbarHeight);
            {
                if (selected != null)
                {
                    GUILayout.Label(selected.Name);
                }

                if (SirenixEditorGUI.ToolbarButton(new GUIContent("Create UGUIWindowConfig")))
                {
                    ScriptableObjectCreator.ShowDialog<UGUIWindowConfig>("Assets/UGUIFramework/GameData");
                }
            }
            SirenixEditorGUI.EndHorizontalToolbar();
        }

        public UGUIWindowConfig[] UIWindowConfigs
        {
            get
            {
                return AssetDatabase.FindAssets("t:UGUIWindowConfig")
                    .Select(guid => AssetDatabase.LoadAssetAtPath<UGUIWindowConfig>(AssetDatabase.GUIDToAssetPath(guid)))
                    .ToArray();
            }
        }
    }
}
