using SkillzSDK;
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public sealed class SkillzDelegate : MonoBehaviour
{
	/// <summary>
	/// This method is called when a user starts a match from Skillz
	/// This method is required to impelement.
	/// </summary>
	private void OnMatchWillBegin(string matchInfoJsonString)
	{
		SkillzState.NotifyMatchWillBegin(matchInfoJsonString);
	}

	/// <summary>
	/// This method is called when a user exits the Skillz experience (via Menu -> Exit)
	/// This method is optional to impelement. This method is usually used only if your game has its own Main Menu.
	/// </summary>
	private void OnSkillzWillExit()
	{
		SkillzState.NotifySkillzWillExit();
	}

	/// <summary>
	/// This method is called when a user enters the Progression Room (via Menu -> Progression Room)
	/// This method is optional to implement. This method should only be implemented if your game has
	/// own player progression experience.
	/// </summary>
	private void OnProgressionRoomEnter()
	{
		SkillzState.NotifyProgressionRoomEnter();
	}

	private void OnEventReceived(string receivedEvent)
	{
		Dictionary<string, object> receivedEventDict = SkillzSDK.MiniJSON.Json.Deserialize(receivedEvent) as Dictionary<string, object>;

		string eventName = (string)receivedEventDict["eventName"];
		Dictionary<string, string> eventData = new Dictionary<string, string>();

		if (receivedEventDict["eventData"] is Dictionary<string, object>) {
			Dictionary<string, object> eventDataBase = receivedEventDict["eventData"] as Dictionary<string, object>;

			foreach (KeyValuePair<string, object> kvp in eventDataBase)
			{
				eventData.Add(kvp.Key, kvp.Value.ToString());
			}
		}

		SkillzEvents.RaiseOnEventReceived(eventName, eventData);
	}

    /// <summary>
    /// This method is called when a Deeplink is opened and the Unity app is not running. 
    /// </summary>
    public void LaunchSkillzFromDeeplink()
    {
        if (SkillzManager.ExistsInProject()) {
            SkillzManager.LaunchSkillz();
            return;
        }

        var types = AppDomain.CurrentDomain.GetTypesWithInterface(typeof(SkillzMatchDelegate));
        List<Type> controllerTypes = new List<Type>();
        foreach (var t in types)
        {
            if (!t.Name.Equals("SkillzMatchDelegate") && !t.Name.Equals("SkillzSyncDelegate") && !t.Name.Equals("SkillzExampleMatchDelegate"))
            {
                controllerTypes.Add(t);
            }
        }

        if (controllerTypes.Count <= 0)
        {
            Debug.LogError("One implementation of SkillzMatchDelegate must exist in the project!");
            return;
        }
        if (controllerTypes.Count > 1)
        {
            Debug.LogError("Only one implementation of SkillzMatchDelegate may exist in the project!");
            return;
        }

        SkillzMatchDelegate delegateInstance = (SkillzMatchDelegate)Activator.CreateInstance(controllerTypes[0]);
#pragma warning disable 612, 618
        SkillzCrossPlatform.LaunchSkillz(delegateInstance);
#pragma warning restore 612, 618
    }
}
