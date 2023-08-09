#if UNITY_EDITOR
using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using SkillzSDK;
using SkillzSDK.Settings;

public sealed class SkillzSupportWindow : EditorWindow
{

	private GUIStyle linkStyle;
	private GUIStyle titleStyle;
	private GUIStyle textStyle;
	private GUIStyle horizontalLine;

	private Color titleColor = new Color(.8f, .8f, .8f);
	private Color lineColor = new Color(.5f, .5f, .5f);

	[MenuItem("Skillz/Support", priority = -1)]

	private static void Open()
	{
		var window = GetWindow<SkillzSupportWindow>();
		window.titleContent.text = "Support";
		window.minSize = new Vector2(300, 400);

		window.Show();
	}

	private void OnGUI()
	{
		InitializeStyles();
		Links();
	}

	private void Links()
	{

		EditorGUILayout.Space();
		EditorGUILayout.Space();

		EditorGUILayout.LabelField("Developer Resources", titleStyle);

		DrawHorizontalLine();

		EditorGUI.indentLevel++;
		EditorGUILayout.Space();

		EditorGUILayout.TextField("<a href=\"https://docs.skillz.com/\">Skillz Developer Documentation</a>", linkStyle);
		EditorGUILayout.TextField("<a href=\"https://developers.skillz.com/\">Skillz Developer Console</a>", linkStyle);

		EditorGUI.indentLevel--;
		EditorGUILayout.Space();

		EditorGUILayout.LabelField("Support", titleStyle);

		DrawHorizontalLine();

		EditorGUI.indentLevel++;
		EditorGUILayout.Space();

		EditorGUILayout.TextField("<a href=\"https://developers.skillz.com/support/contact_us\">Report A Bug</a>", linkStyle);
		EditorGUILayout.TextField("<a href=\"https://developers.skillz.com/support/contact_us\">Contact Support</a>", linkStyle);

		EditorGUI.indentLevel--;
		EditorGUILayout.Space();

		EditorGUILayout.LabelField("About", titleStyle);

		DrawHorizontalLine();

		EditorGUI.indentLevel++;
		EditorGUILayout.Space();

		EditorGUILayout.LabelField("Version: 29.1.35", textStyle);
	}

	private void InitializeStyles()
	{
		//style for links
		linkStyle = new GUIStyle()
		{
			richText = true,
			fontSize = 16,
			fontStyle = FontStyle.Bold,

		};

		//style for title labels
		titleStyle = new GUIStyle()
		{
			fontSize = 18,
			fontStyle = FontStyle.Bold
		};
		titleStyle.normal.textColor = titleColor;

		//style for labels
		textStyle = new GUIStyle()
		{
			fontSize = 14,
			fontStyle = FontStyle.Bold
		};
		textStyle.normal.textColor = titleColor;

		//style for horizonal lines
		horizontalLine = new GUIStyle();
		horizontalLine.normal.background = Texture2D.whiteTexture;
		horizontalLine.margin = new RectOffset(0, 0, 4, 4);
		horizontalLine.fixedHeight = 1;
	}

	
	private void DrawHorizontalLine()
	{
		var prevColor = GUI.color;
		GUI.color = lineColor;

		GUILayout.Box(GUIContent.none, horizontalLine);

		GUI.color = prevColor;
	}

}
#endif