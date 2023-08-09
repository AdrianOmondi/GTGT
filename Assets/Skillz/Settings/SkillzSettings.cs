using System;
using UnityEngine;

namespace SkillzSDK.Settings
{
	[Serializable]
	public sealed class SkillzSettings : ScriptableObject
	{
		public const int MaxMatchParameters = 10;

		private static SkillzSettings instance;
		private static SettingsLoader settingsLoader;

		public static SkillzSettings Instance
		{
			get
			{
				return instance != null ? instance : (instance = SettingsLoader.Load());
			}
      set
      {
				instance = value;
      }
		}

		[HideInInspector]
		[SerializeField]
		public int GameID;

		[HideInInspector]
		[SerializeField]
		public Environment Environment;

		[HideInInspector]
		[SerializeField]
		public Orientation Orientation;

		[HideInInspector]
		[SerializeField]
		public bool IsDebugMode;

		[HideInInspector]
		[SerializeField]
		public bool AllowSkillzExit;

		[HideInInspector]
		[SerializeField]
		public bool HasSyncBot;
		
		[HideInInspector]
		[SerializeField]
		public MatchTypeTemplates MatchTypeTemplates;

		[HideInInspector]
		[SerializeField]
		public MatchParametersTemplates MatchParametersTemplates;

		[HideInInspector]
		[SerializeField]
		public PlayerTemplates PlayerTemplates;

		[HideInInspector]
		[SerializeField]
		public ProgressionResponsesTemplate ProgressionResponsesTemplate;

		[HideInInspector]
		[SerializeField]
		public SeasonsTemplate SeasonsTemplate;

		[HideInInspector]
		[SerializeField]
		public string Score;

		[HideInInspector]
		[SerializeField]
		public bool IsLeavingProgressionRoom;

		[HideInInspector]
		[SerializeField]
		public bool IsLaunching;

		[HideInInspector]
		[SerializeField]
		public bool hasSetSIDEkickDefaults;

		private static SettingsLoader SettingsLoader
		{
			get
			{
				if (settingsLoader == null)
				{
#if UNITY_EDITOR
					settingsLoader = new EditorSettingsLoader();
#else
					settingsLoader = new RuntimeSettingsLoader();
#endif
				}

				return settingsLoader;
			}
		}

		private SkillzSettings()
		{
			IsDebugMode = false;
			AllowSkillzExit = true;
			HasSyncBot = false;
		}
	}
}