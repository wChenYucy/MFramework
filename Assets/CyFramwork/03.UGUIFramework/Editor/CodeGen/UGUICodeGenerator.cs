// ****************************************************
//     文件：UIWindowManagerWindowEditor.cs
//     作者：积极向上小木木
//     邮箱：positivemumu@126.com
//     日期：2021/5/24 16:6:37
//     功能：UGUI框架脚本生成工具类（来自QFramework）
// *****************************************************
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor.Callbacks;

namespace UGUIFramework
{
	using UnityEngine;
	using UnityEditor;
	using System.IO;

	public class UGUICodeGenerator
	{
		[MenuItem("Assets/Create UGUIWindowCode")]
		public static void CreateUGUICode()
		{
			var objs = Selection.GetFiltered(typeof(GameObject), SelectionMode.Assets | SelectionMode.TopLevel);
			
			DoCreateCode(objs);
		}

		public static void DoCreateCode(Object[] objs)
		{
			var displayProgress = objs.Length > 1;
			if (displayProgress) EditorUtility.DisplayProgressBar("", "Create UIPrefab Code...", 0);
			for (var i = 0; i < objs.Length; i++)
			{
				mInstance.CreateCode(objs[i] as GameObject);
				if (displayProgress)
					EditorUtility.DisplayProgressBar("", "Create UIPrefab Code...", (float) (i + 1) / objs.Length);
			}

			AssetDatabase.Refresh();
			if (displayProgress) EditorUtility.ClearProgressBar();
		}
		

		private void CreateCode(GameObject obj)
		{
			if (obj.IsNotNull())
			{
				if (!PrefabUtility.IsPartOfPrefabAsset(obj))
				{
					return;
				}

				var clone = PrefabUtility.InstantiatePrefab(obj) as GameObject;
				if (null == clone)
				{
					return;
				}
				
				CreateUGUIWindowCode(obj);
				SavePrefabsPath(obj);
				Object.DestroyImmediate(clone);
			}
		}

		private void CreateUGUIWindowCode(GameObject uiPrefab)
		{
			if (null == uiPrefab)
				return;

			var behaviourName = uiPrefab.name;

			var strFilePath = UGUIFrameworkPreferences.Instance.ScriptsFolder.TrimStart('/');
			if (!strFilePath.StartsWith("Assets/"))
			{
				strFilePath = "Assets/"+strFilePath;
			}

			if (UGUIFrameworkPreferences.Instance.CreateScriptsFolder)
			{
				strFilePath = strFilePath.TrimEnd('/') + "/" + behaviourName;
			}
			strFilePath = strFilePath.CreateDirIfNotExists() + "/" + behaviourName + ".cs";
			if (File.Exists(strFilePath) == false)
			{
				UGUIWindowTemplate.Write(behaviourName,strFilePath,UGUIFrameworkPreferences.Instance.ScriptNamespace);
			}

			CreateUGUIWindowDesignerCode(behaviourName, strFilePath);
		}
		
		private void CreateUGUIWindowDesignerCode(string behaviourName, string uiPanelFilePath)
		{
			var dir = uiPanelFilePath.Replace(behaviourName + ".cs", "");
			UGUIWindowDesignerTemplate.Write(behaviourName,dir,UGUIFrameworkPreferences.Instance.ScriptNamespace);
			
			CreateUGUIWindowDataCode(behaviourName,dir);
		}

		private void CreateUGUIWindowDataCode(string behaviourName, string uiPanelFilePath)
		{
			UGUIWindowDataTemplate.Write(behaviourName,uiPanelFilePath,UGUIFrameworkPreferences.Instance.ScriptNamespace);
		}

		private void SavePrefabsPath(GameObject uiPrefab)
		{
			var prefabPath = AssetDatabase.GetAssetPath(uiPrefab);
			if (string.IsNullOrEmpty(prefabPath))
				return;

			var pathStr = EditorPrefs.GetString("AutoGenUIPrefabPath");
			if (string.IsNullOrEmpty(pathStr))
			{
				pathStr = prefabPath;
			}
			else
			{
				pathStr += ";" + prefabPath;
			}

			EditorPrefs.SetString("AutoGenUIPrefabPath", pathStr);
		}
		
		[DidReloadScripts]
		private static void AddComponentToPrefabs()
		{
			var pathStr = EditorPrefs.GetString("AutoGenUIPrefabPath");
			if (string.IsNullOrEmpty(pathStr))
				return;

			EditorPrefs.DeleteKey("AutoGenUIPrefabPath");
			//string assembly = "Assembly-CSharp";
			
			var paths = pathStr.Split(new[] {';'}, StringSplitOptions.RemoveEmptyEntries);
			var displayProgress = paths.Length > 3;
			if (displayProgress) EditorUtility.DisplayProgressBar("", "Serialize UIPrefab...", 0);
			
			for (var i = 0; i < paths.Length; i++)
			{
				var uiPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(paths[i]);
				string className = UGUIFrameworkPreferences.Instance.ScriptNamespace + "." + uiPrefab.name;
				Assembly assembly = null;
				string assemblyName = UGUIFrameworkPreferences.Instance.ScriptAssembly;
				if (!assemblyName.IsNotNull() || assemblyName.Equals(""))
					assemblyName = "Assembly-CSharp";
				foreach (var VARIABLE in AppDomain.CurrentDomain.GetAssemblies())
				{
					if (VARIABLE.FullName.StartsWith(assemblyName))
					{
						assembly = VARIABLE;
						break;
					}
				}
				Type t= assembly.GetType(className);
				if (t == null)
				{
					Debug.LogError("UGUIFrameworkError:未能在"+assemblyName+"程序集中找到"+className+"类，请在配置窗口检查程序集名称与命名空间名称是否正确并更正！");
					string classFolderPath = UGUIFrameworkPreferences.Instance.ScriptsFolder;
					if (UGUIFrameworkPreferences.Instance.CreateScriptsFolder)
						classFolderPath = classFolderPath + "/" + uiPrefab.name;
					List<string> deleteFiles = new List<string>();
					deleteFiles.Add(classFolderPath + "/" + uiPrefab.name + ".cs");
					deleteFiles.Add(classFolderPath + "/" + uiPrefab.name + ".cs.meta");
					deleteFiles.Add(classFolderPath + "/" + uiPrefab.name + ".Designer.cs");
					deleteFiles.Add(classFolderPath + "/" + uiPrefab.name + ".Designer.cs.meta");
					deleteFiles.Add(classFolderPath + "/" + uiPrefab.name + "Data.cs");
					deleteFiles.Add(classFolderPath + "/" + uiPrefab.name + "Data.cs.meta");
					foreach (var deleteFile in deleteFiles)
					{
						if(File.Exists(deleteFile))
							File.Delete(deleteFile);
					}

					if (UGUIFrameworkPreferences.Instance.CreateScriptsFolder && Directory.Exists(classFolderPath))
					{
						Directory.Delete(classFolderPath);
						File.Delete(classFolderPath + ".meta");
					}
				}
				else
				{
					if (!uiPrefab.GetComponent(t))
					{
						uiPrefab.AddComponent(t);
					}
				}
				
				if (displayProgress)
					EditorUtility.DisplayProgressBar("", "Serialize UIPrefab..." + uiPrefab.name, (float) (i + 1) / paths.Length);
			}

			AssetDatabase.SaveAssets();
			AssetDatabase.Refresh();
			if (displayProgress) EditorUtility.ClearProgressBar();
		}

		private static readonly UGUICodeGenerator mInstance = new UGUICodeGenerator();

		
		
		
	}
}