using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace SkillzSDK.Internal.API.Android
{
    class CurrentSeasonCallback : AndroidJavaProxy
    {
        Action<Season> successCallback;
        Action<string> failureCallback;
        public CurrentSeasonCallback(Action<Season> success, Action<string> failure)
            : base("com.skillz.progression.CurrentSeasonCallback")
        {
            successCallback = success;
            failureCallback = failure;
        }

        void success(AndroidJavaObject dataObj)
        {
            if (dataObj == null)
            {
                if (successCallback != null)
                {
                    SkillzDebug.Log(SkillzDebug.Type.SKILLZ, $"GetCurrentSeason() Success callback, Season: 'null'");
                    successCallback(null);
                }
                return;
            }
            string jsonString = dataObj.Get<string>("jsonString");
            Dictionary<string, object> seasonDict = MiniJSON.Json.Deserialize(jsonString) as Dictionary<string, object>;
            Season data = new Season(seasonDict);
            if (successCallback != null)
            {
                SkillzDebug.Log(SkillzDebug.Type.SKILLZ, $"GetCurrentSeason() Success callback, Season: '{SkillzDebug.Format(data)}'");
                successCallback(data);
            }
        }

        void failure(string errorMessage)
        {
            if (failureCallback != null)
            {
                SkillzDebug.Log(SkillzDebug.Type.SKILLZ, $"GetCurrentSeason() Failure Callback: {errorMessage}");
                failureCallback(errorMessage);
            }
        }
    }
}
