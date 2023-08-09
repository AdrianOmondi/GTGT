#if UNITY_EDITOR
using System;
using UnityEditor;
using UnityEngine;

namespace SkillzSDK.Settings
{
	public static class SkillzPackageImportListener
	{
		private const string SkillzPackagePrefix = "skillz";

		[InitializeOnLoadMethod]
#pragma warning disable IDE0051 // Remove unused private members
		private static void OnProjectLoad()
#pragma warning restore IDE0051 // Remove unused private members
		{
			SkillzDebug.Log(SkillzDebug.Type.SKILLZ, "Project loaded. Listening for import of Skillz.", true);
			AssetDatabase.importPackageCompleted += OnImportPackageCompleted;
		}

		private static void OnImportPackageCompleted(string packageName)
		{
			if (!IsPackageSkillz(packageName))
			{
				return;
			}

			AssetDatabase.importPackageCompleted -= OnImportPackageCompleted;

			SkillzDebug.Log(SkillzDebug.Type.SKILLZ, "Skillz package was imported. Generating files required for Android...", true);
			AndroidFilesGenerator.GenerateFiles();
		}

		private static bool IsPackageSkillz(string packageName)
		{
			return packageName.StartsWith(SkillzPackagePrefix, StringComparison.InvariantCultureIgnoreCase);
		}
	}
}
#endif