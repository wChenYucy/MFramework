// ****************************************************
//     文件：UIWindowManagerWindowEditor.cs
//     作者：积极向上小木木
//     邮箱：positivemumu@126.com
//     日期：2021/5/24 16:6:37
//     功能：UGUI框架WindowData类模板（来自QFramework）
// *****************************************************

using System.IO;

namespace UGUIFramework
{
    public class UGUIWindowDataTemplate
    {
        
        public static void Write(string name, string scriptsFolder, string scriptsNamespace)
        {
            var scriptFile = scriptsFolder + "/{0}Data.cs".FillFormat(name);
            
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
                    nsScope.Class(name + "Data", null, false, false, classScope =>
                    {
                        classScope.Custom("protected "+name+" window;");
                        classScope.EmptyLine();
                        classScope.CustomScope("public "+name + "Data("+name+" window)", false, (function) =>
                        {
                            function.Custom("this.window = window;");
                        });
                    });
                });
            
            rootCode.Gen(codeWriter);
            codeWriter.Dispose();
        }
    }
}


