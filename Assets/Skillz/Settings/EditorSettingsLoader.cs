#if UNITY_EDITOR
using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace SkillzSDK.Settings
{
	internal sealed class EditorSettingsLoader : SettingsLoader
	{

		private const string RootAssetsFolder = "Assets";
		private const string ResourcesFolder = "Resources";
		private const string AssetExt = ".asset";
		private const string SimulatorTemplatesPath = "Assets/Skillz/SIDEkick/";

		private static string BaseResourcesFolder
		{
			get
			{
				return Path.Combine(RootAssetsFolder, ResourcesFolder);
			}
		}

		private static string AssetFolder
		{
			get
			{
				return Path.Combine(BaseResourcesFolder, SkillzFolder);
			}
		}

		private static string AssetPath
		{
			get
			{
				return Path.Combine(AssetFolder, string.Concat(SkillzAssetName, AssetExt));
			}
		}

		protected override bool AssetExists
		{
			get
			{
				return AssetDatabase
					.FindAssets($"t:{typeof(SkillzSettings)}")
					.Any(assetGuid =>
					{
						// HACK: GUIDToAssetPath returns a path with `/` delimiters regardless of platform (re: Windows vs Mac OS)
						var normalizedAssetPath = AssetPath.Replace(@"\", @"/");

						return string.Compare(AssetDatabase.GUIDToAssetPath(assetGuid), normalizedAssetPath, StringComparison.InvariantCulture) == 0;
					});
			}
		}

		protected override SkillzSettings CreateSettings()
		{
			var skillzSettings = ScriptableObject.CreateInstance<SkillzSettings>();

			if (!AssetDatabase.IsValidFolder(BaseResourcesFolder))
			{
				SkillzDebug.LogWarning(SkillzDebug.Type.SETTINGS, $"'{BaseResourcesFolder}' is missing, Skillz will create it.");

				var baseResourcesFolder = AssetDatabase.GUIDToAssetPath(AssetDatabase.CreateFolder(RootAssetsFolder, ResourcesFolder));
				SkillzDebug.Log(SkillzDebug.Type.SETTINGS, $"Created asset folder '{baseResourcesFolder}'", true);
			}

			var skillzAssetsFolder = AssetFolder;
			if (!AssetDatabase.IsValidFolder(skillzAssetsFolder))
			{
				skillzAssetsFolder = AssetDatabase.GUIDToAssetPath(AssetDatabase.CreateFolder(BaseResourcesFolder, SkillzFolder));
				SkillzDebug.Log(SkillzDebug.Type.SETTINGS, $"Created asset folder '{skillzAssetsFolder}'", true);
			}

			SetSIDEkickDefaults(skillzSettings);

			SkillzDebug.Log(SkillzDebug.Type.SETTINGS, $"Creating Skillz settings at '{AssetPath}'", true);
			AssetDatabase.CreateAsset(skillzSettings, AssetPath);

			return skillzSettings;
		}

		public void SaveSIDEkickDefaults(SkillzSettings skillzSettings)
		{
			if (skillzSettings.hasSetSIDEkickDefaults)
			{
				return;
			}

			SetSIDEkickDefaults(skillzSettings);

			skillzSettings.hasSetSIDEkickDefaults = true;

			EditorUtility.SetDirty(skillzSettings);
			AssetDatabase.SaveAssets();
		}

		private void SetSIDEkickDefaults(SkillzSettings skillzSettings)
    {
			SkillzDebug.Log(SkillzDebug.Type.SETTINGS, "Setting default SIDEkick Templates...", true);

			string path = SimulatorTemplatesPath + "Default Match Types.asset";
			skillzSettings.MatchTypeTemplates = (MatchTypeTemplates)AssetDatabase.LoadAssetAtPath(path, typeof(MatchTypeTemplates));

			path = SimulatorTemplatesPath + "Default Match Parameters.asset";
			skillzSettings.MatchParametersTemplates = (MatchParametersTemplates)AssetDatabase.LoadAssetAtPath(path, typeof(MatchParametersTemplates));

			path = SimulatorTemplatesPath + "Default Players.asset";
			skillzSettings.PlayerTemplates = (PlayerTemplates)AssetDatabase.LoadAssetAtPath(path, typeof(PlayerTemplates));

			path = SimulatorTemplatesPath + "Default Progression Responses.asset";
			skillzSettings.ProgressionResponsesTemplate = (ProgressionResponsesTemplate)AssetDatabase.LoadAssetAtPath(path, typeof(ProgressionResponsesTemplate));

			path = SimulatorTemplatesPath + "Default Seasons.asset";
			skillzSettings.SeasonsTemplate = (SeasonsTemplate)AssetDatabase.LoadAssetAtPath(path, typeof(SeasonsTemplate));
		}

		protected override SkillzSettings LoadSettings()
		{
			try
			{
				SkillzDebug.Log(SkillzDebug.Type.SETTINGS, $"Loading Skillz Settings from '{AssetPath}'", true);
				SkillzSettings skillzSettings = AssetDatabase.LoadAssetAtPath<SkillzSettings>(AssetPath);
				SaveSIDEkickDefaults(skillzSettings);
				return skillzSettings;
			}
			catch (Exception exception)
			{
				SkillzDebug.LogWarning(SkillzDebug.Type.SETTINGS, $"Failed to load the Skillz Settings from '{AssetPath}'!");
				SkillzDebug.LogWarning(SkillzDebug.Type.SETTINGS, exception);

				AssetDatabase.DeleteAsset(AssetPath);

				return CreateSettings();
			}
		}
	}
}
#endif