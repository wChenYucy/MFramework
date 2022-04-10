// ****************************************************
//     文件：UIWindowManagerWindowEditor.cs
//     作者：积极向上小木木
//     邮箱：positivemumu@126.com
//     日期：2021/5/24 16:6:37
//     功能：UGUI框架Window类模板（来自QFramework）
// *****************************************************

using System;
using System.IO;

namespace UGUIFramework
{
    public class UGUIWindowTemplate
    {
        public static void Write(string name, string srcFilePath,string srcNamespace)
        {
            var scriptFile = srcFilePath;

            if (File.Exists(scriptFile))
            {
                return;
            }
            
            var writer = File.CreateText(scriptFile);

            var scriptNamespace = srcNamespace;
            if (!srcNamespace.IsNotNull()||srcNamespace.Equals(""))
            {
                scriptNamespace = "UGUIFramework";
            }
            
            var root = new RootCode()
                .Using("System")
                .Using("UnityEngine")
                .Using("UnityEngine.UI")
                .Using("UGUIFramework")
                .EmptyLine()
                .Namespace(scriptNamespace, ns =>
                {
                    ns.Custom("// Generate Id:{0}".FillFormat(Guid.NewGuid().ToString()));
                    ns.Class(name, "UGUIWindowBase", true, false, (classScope) =>
                    {
                        classScope.Custom("public const string Name = \"" + name + "\";");
                        classScope.EmptyLine();
                        
                        classScope.EmptyLine();
                        classScope.Custom("private " + name + "Data windowData = null;");

                        classScope.EmptyLine();
                        
                    });
                });

            var codeWriter = new FileCodeWriter(writer);
            root.Gen(codeWriter);
            codeWriter.Dispose();
        }
    }
}