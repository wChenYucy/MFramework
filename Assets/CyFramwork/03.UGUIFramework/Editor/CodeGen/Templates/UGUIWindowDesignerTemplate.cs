// ****************************************************
//     文件：UIWindowManagerWindowEditor.cs
//     作者：积极向上小木木
//     邮箱：positivemumu@126.com
//     日期：2021/5/24 16:6:37
//     功能：UGUI框架Window.Designer类模板（来自QFramework）
// *****************************************************

namespace UGUIFramework
{
    using System.IO;

    public class UGUIWindowDesignerTemplate
    {
        public static void Write(string name, string scriptsFolder,string scriptsNamespace)
        {
            var scriptFile = scriptsFolder + "/{0}.Designer.cs".FillFormat(name);

            var writer = File.CreateText(scriptFile);

            var codeWriter = new FileCodeWriter(writer);
            
            if (!scriptsNamespace.IsNotNull()||scriptsNamespace.Equals(""))
            {
                scriptsNamespace = "UGUIFramework";
            }

            var rootCode = new RootCode()
                .Using("UnityEngine")
                .Using("UnityEngine.UI")
                .Using("UGUIFramework")
                .EmptyLine()
                .Namespace(scriptsNamespace, nsScope =>
                {
                    
                    nsScope.Class(name, null, true, false, classScope =>
                    {
                        classScope.CustomScope("public override void OpenWindow(UGUIWindowInfo windowInfo)", false,
                            function =>
                            {
                                function.Custom("base.OpenWindow(windowInfo);");
                                function.Custom("windowTransform = GetComponent<RectTransform>();");
                                function.Custom("//在此处添加初始化窗口代码");
                            });
                        classScope.EmptyLine();
                        classScope.CustomScope("public override void ShowWindow()", false,
                            function => { });
                        classScope.EmptyLine();
                        classScope.CustomScope("public override void HideWindow()", false,
                            function => { });
                        classScope.EmptyLine();
                        classScope.CustomScope("public override void CloseWindow()", false,
                            function =>
                            {
                                function.Custom("//在此处添加销毁窗口代码");
                                function.Custom("");
                                function.Custom("windowData = null;");
                            });

                    });
                });

            rootCode.Gen(codeWriter);
            codeWriter.Dispose();
        }
    }
}