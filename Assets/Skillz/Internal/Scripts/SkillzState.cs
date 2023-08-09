using System;
using System.Collections.Generic;
using SkillzSDK.Internal.API;
using SkillzSDK.Settings;
using UnityEngine;

namespace SkillzSDK
{
	public static class SkillzState
	{
		private const string SkillzDelegateName = "SkillzDelegate";

		private static SkillzMatchDelegate asyncDelegate;

		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
#pragma warning disable IDE0051 // Remove unused private members
		private static void OnAfterFirstSceneLoaded()
#pragma warning restore IDE0051 // Remove unused private members
		{
			// This method will be invoked only once, which is after
			// the initial scene of the game has been loaded.

			InitializeSkillz();
			InitializeSkillzDelegate();
		}

		public static void SetAsyncDelegate(SkillzMatchDelegate skillzDelegate)
		{
			asyncDelegate = skillzDelegate;
		}

		public static void NotifyMatchWillBegin(string matchInfoJson)
		{
			Dictionary<string, object> matchInfoDict = DeserializeJSONToDictionary(matchInfoJson);
			SkillzDebug.Log(SkillzDebug.Type.SKILLZ, $"Match info: {matchInfoJson}");

			try
			{
				Match matchInfo = new Match(matchInfoDict);

				if (asyncDelegate != null)
				{
					SkillzDebug.Log(SkillzDebug.Type.SKILLZ, "Calling SkillzMatchDelegate OnMatchWillBegin() callback");
					asyncDelegate.OnMatchWillBegin(matchInfo);
        }
        else
        {
					SkillzDebug.Log(SkillzDebug.Type.SKILLZ, "Calling SkillzEvents.OnMatchWillBegin event");
					SkillzEvents.RaiseOnMatchWillBegin(matchInfo);
        }
			}
			catch (Exception e)
			{
				SkillzDebug.LogError(SkillzDebug.Type.SKILLZ, $"Error retrieving the match info! Error={e}");
				throw;
			}
		}

		public static void NotifySkillzWillExit()
		{
			if (asyncDelegate != null)
			{
				SkillzDebug.Log(SkillzDebug.Type.SKILLZ, "Calling SkillzMatchDelegate OnSkillzWillExit() callback");
				asyncDelegate.OnSkillzWillExit();
      }
      else
      {
				SkillzDebug.Log(SkillzDebug.Type.SKILLZ, "Calling SkillzEvents.OnSkillzWillExit event");
				SkillzEvents.RaiseOnSkillzWillExit();
      }
		}

		public static void NotifyProgressionRoomEnter()
		{
			if (asyncDelegate != null)
			{
				SkillzDebug.Log(SkillzDebug.Type.SKILLZ, "Calling SkillzMatchDelegate OnProgressionRoomEnter() callback");
				asyncDelegate.OnProgressionRoomEnter();
      }
      else
      {
				SkillzDebug.Log(SkillzDebug.Type.SKILLZ, "Calling SkillzEvents.OnProgressionRoomEnter event");
				SkillzEvents.RaiseOnProgressionRoomEnter();
      }
		}

		private static void InitializeSkillz()
		{
			SkillzDebug.Log(SkillzDebug.Type.SKILLZ, "Skillz Debug Logging Mode Enabled. To disbale go to Skillz->Settings then deselect the 'Debug Logging' option.");
			SkillzDebug.Log(SkillzDebug.Type.SKILLZ, $"Initializing Skillz");
			SkillzDebug.Log(SkillzDebug.Type.SKILLZ, $"Game ID: {SkillzSettings.Instance.GameID}");
			SkillzDebug.Log(SkillzDebug.Type.SKILLZ, $"Environment: {SkillzSettings.Instance.Environment}");
			SkillzDebug.Log(SkillzDebug.Type.SKILLZ, $"Orientation: {SkillzSettings.Instance.Orientation}");

			SkillzCrossPlatform.Initialize(
				SkillzSettings.Instance.GameID,
				SkillzSettings.Instance.Environment,
				SkillzSettings.Instance.Orientation
			);
		}

		private static void InitializeSkillzDelegate()
		{
			// We are adding a SkillzDelegate game object to the start
			// scene for historical reasons.
			// In previous releases, devs would add an instance via a
			// Skillz -> Generate Delegate menu item.
			//
			// Although most of SkillzDelegate's functionality has been
			// moved to here, the iOS and Android SDKs call the
			// UnitySendMessage to invoke it as a callback when a match
			// begins and ends.
			//
			// Unfortunately, UnitySendMessage can only call a method on a
			// game object that is present in a scene, so we must live
			// with loading an instance in the startup scene.

			SkillzDebug.Log(SkillzDebug.Type.SKILLZ, "Initializing SkillzDelegate");

			var gameObject = new GameObject();
			gameObject.name = SkillzDelegateName;

			gameObject.AddComponent<SkillzDelegate>();

#if UNITY_IOS || UNITY_ANDROID
			UnityEngine.Object.DontDestroyOnLoad(gameObject);
#endif
		}

		private static Dictionary<string, object> DeserializeJSONToDictionary(string jsonString)
		{
			return MiniJSON.Json.Deserialize(jsonString) as Dictionary<string, object>;
		}
	}
}