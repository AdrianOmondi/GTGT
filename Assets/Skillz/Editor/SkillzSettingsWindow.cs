#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using SkillzSDK;
using SkillzSDK.Settings;


public sealed class SkillzSettingsWindow : EditorWindow
{
	private const int MaxKeyLength = 255;
	private const int MaxValueLength = 1000;

	private GUIStyle HorizontalLine
	{
		get
		{
			if (horizontalLine != null)
			{
				return horizontalLine;
			}

			horizontalLine = new GUIStyle();
			horizontalLine.normal.background = Texture2D.whiteTexture;
			horizontalLine.margin = new RectOffset(0, 0, 4, 4);
			horizontalLine.fixedHeight = 1;

			return horizontalLine;
		}
	}

	private bool showGameSettings;
	private bool showCompanionSettings;

	private GUIStyle horizontalLine;
	private GUIStyle wrappedLabelStyle;

	[MenuItem("Skillz/Settings", priority = -2)]
#pragma warning disable IDE0051 // Remove unused private members
	private static void Open()
#pragma warning restore IDE0051 // Remove unused private members
	{
		var window = GetWindow<SkillzSettingsWindow>();
		window.titleContent.text = "Skillz Settings";
		window.minSize = new Vector2(300, 400);
		window.showGameSettings = true;
		window.showCompanionSettings = true;
		SkillzSettings.Instance = null; // force the settings to be loaded
		window.Show();
	}

	private void OnGUI()
	{
		InitializeStyles();
		DrawGameSettingsPane();
		DrawHorizontalLine();
		DrawCompanionSettings();
		SaveChanges();
	}

	private void InitializeStyles()
	{
		if (wrappedLabelStyle != null)
		{
			return;
		}

		wrappedLabelStyle = new GUIStyle(EditorStyles.label)
		{
			wordWrap = true,
			clipping = TextClipping.Overflow
		};
	}

	private void DrawGameSettingsPane()
	{
		showGameSettings = EditorGUILayout.Foldout(showGameSettings, "Game Settings");
		if (!showGameSettings)
		{
			return;
		}

		EditorGUI.indentLevel++;

		EditorGUILayout.LabelField("Settings for configuring your Skillz-based game.");
		EditorGUILayout.Space();

		SkillzSettings.Instance.GameID = EditorGUILayout.IntField("Game ID", SkillzSettings.Instance.GameID);
		SkillzSettings.Instance.Environment = (Environment)EditorGUILayout.EnumPopup("Skillz Environment", SkillzSettings.Instance.Environment);
		SkillzSettings.Instance.Orientation = (Orientation)EditorGUILayout.EnumPopup("Skillz Orientation", SkillzSettings.Instance.Orientation);
		SkillzSettings.Instance.IsDebugMode = EditorGUILayout.Toggle(
			new GUIContent(
				"Debug Logging",
				"Toggle extra Skillz SDK logging messages."
			),
			SkillzSettings.Instance.IsDebugMode
		);
		SkillzSettings.Instance.AllowSkillzExit = EditorGUILayout.Toggle(
			new GUIContent(
				"Allow Skillz to Exit",
				"Allows the user to exit the Skillz UI via the sidebar menu."
			),
			SkillzSettings.Instance.AllowSkillzExit
		);
		SkillzSettings.Instance.HasSyncBot = EditorGUILayout.Toggle(
			new GUIContent(
				"Has Synchronous Bot",
				"Determines if the game has support for synchronous gameplay on-boarding bots."
			),
			SkillzSettings.Instance.HasSyncBot
		);

		EditorGUI.indentLevel--;
	}

	private void DrawHorizontalLine()
	{
		var prevColor = GUI.color;
		GUI.color = Color.grey;

		GUILayout.Box(GUIContent.none, HorizontalLine);

		GUI.color = prevColor;
	}

	private new void SaveChanges()
	{
		if (!GUI.changed)
		{
			return;
		}

		SkillzDebug.Log(SkillzDebug.Type.SETTINGS, "Saving Skillz settings...");
		EditorUtility.SetDirty(SkillzSettings.Instance);
		AssetDatabase.SaveAssets();
	}

	private void DrawCompanionSettings()
	{
		showCompanionSettings = EditorGUILayout.Foldout(showCompanionSettings, "SIDEkick");
		if (!showCompanionSettings)
		{
			return;
		}

		EditorGUI.indentLevel++;

		EditorGUILayout.LabelField("Simulate the skillz environment in the Unity editor.");
		EditorGUILayout.Space();

		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField(new GUIContent(""), GUILayout.Width(40));

		bool LaunchButtonPressed = GUILayout.Button(new GUIContent("Launch Game", "Test your game using the Skillz Companion within Unity."),
																								GUILayout.Width(180),
																								GUILayout.Height(35));
		EditorGUILayout.EndHorizontal();
		EditorGUIUtility.labelWidth = 176;


		if (LaunchButtonPressed)
		{
			SkillzSettings.Instance.IsLaunching = true;
			EditorApplication.EnterPlaymode();
		}

		EditorGUILayout.Space();

		EditorGUIUtility.labelWidth = 180;

		SkillzSettings.Instance.MatchTypeTemplates = (MatchTypeTemplates)EditorGUILayout.ObjectField(new GUIContent("Match Types", "Simulate different tournament types in the Unity editor."),
																																																					SkillzSettings.Instance.MatchTypeTemplates,
																																																					typeof(MatchTypeTemplates),
																																																					false);

		SkillzSettings.Instance.MatchParametersTemplates = (MatchParametersTemplates)EditorGUILayout.ObjectField(new GUIContent("Match Parameter Templates", "Simulate different match parameters in the Unity editor."),
																																																					SkillzSettings.Instance.MatchParametersTemplates,
																																																					typeof(MatchParametersTemplates),
																																																					false);

		SkillzSettings.Instance.PlayerTemplates = (PlayerTemplates)EditorGUILayout.ObjectField(new GUIContent("Player Templates", "Simulate different players in the Unity editor."),
																																																					SkillzSettings.Instance.PlayerTemplates,
																																																					typeof(PlayerTemplates),
																																																					false);

		SkillzSettings.Instance.ProgressionResponsesTemplate = (ProgressionResponsesTemplate)EditorGUILayout.ObjectField(new GUIContent("Progression Responses", "Simulate responses to progresssion api calls in the Unity editor. (GetProgressionUserData and UpdateProgressionUserData)"),
																																																							SkillzSettings.Instance.ProgressionResponsesTemplate,
																																																							typeof(ProgressionResponsesTemplate),
																																																							false);

		SkillzSettings.Instance.SeasonsTemplate = (SeasonsTemplate)EditorGUILayout.ObjectField(new GUIContent("Seasons", "Simulate current, past, and future seasons."),
																																																							SkillzSettings.Instance.SeasonsTemplate,
																																																							typeof(SeasonsTemplate),
																																																							false);


		EditorGUI.indentLevel--;

	}
}
#endif