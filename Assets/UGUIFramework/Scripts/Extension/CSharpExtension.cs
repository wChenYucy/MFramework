// ****************************************************
//     文件：CSharpExtension.cs
//     作者：积极向上小木木
//     邮箱：positivemumu@126.com
//     日期：2021/5/24 12:20:51
//     功能：UGUI框架拓展方法类
// *****************************************************

using System;
using System.IO;

public static class CSharpExtension
{
    public static string CreateDirIfNotExists(this string dirFullPath)
    {
        if (!Directory.Exists(dirFullPath))
        {
            Directory.CreateDirectory(dirFullPath);
        }

        return dirFullPath;
    }
    
    public static bool IsNotNull<T>(this T selfObj) where T : class
    {
        return null != selfObj;
    }
    
    public static string FillFormat(this string selfStr, params object[] args)
    {
        return string.Format(selfStr, args);
    }

    public static string ResourcesPath(this string selfStr)
    {
        int start = selfStr.LastIndexOf("Resources/", StringComparison.Ordinal) + "Resources/".Length;
        int length = selfStr.Length - start;
        return selfStr.Substring(start, length);
    }
}
