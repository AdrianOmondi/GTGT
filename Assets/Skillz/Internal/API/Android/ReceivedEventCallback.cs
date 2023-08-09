using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace SkillzSDK.Internal.API.Android
{
    class ReceivedEventCallback : AndroidJavaProxy
    {
        Action<string, Dictionary<string, string>> didReceiveEventCallback;
        public ReceivedEventCallback(Action<string, Dictionary<string, string>> callback)
            : base("com.skillz.ReceivedEventCallback")
        {
            didReceiveEventCallback = callback;
        }

        void didReceiveEvent(System.String eventName, System.String dataString)
        {
            if (didReceiveEventCallback == null) {
                return;
            }

            Dictionary<string, object> eventDataBase = MiniJSON.Json.Deserialize(dataString) as Dictionary<string, object>;

            Dictionary<string, string> eventDataReal = new Dictionary<string, string>();
            foreach (KeyValuePair<string, object> kvp in eventDataBase)
            {
                eventDataReal.Add(kvp.Key, kvp.Value.ToString());
            }

            didReceiveEventCallback(eventName, eventDataReal);
        }

        // for cases when the event data is null - apparently null java strings dont count as C# strings
        void didReceiveEvent(System.String eventName, AndroidJavaObject dataString)
        {
            if (didReceiveEventCallback == null) {
                return;
            }

            Dictionary<string, string> eventDataReal = null;
            // dataString really should be null if we're here and not in the other method, but just in
            // case, let's try to parse it
            if (dataString != null)
            {
                Dictionary<string, object> eventDataBase = MiniJSON.Json.Deserialize(dataString.Call<string>("toString")) as Dictionary<string, object>;

                eventDataReal = new Dictionary<string, string>();
                foreach (KeyValuePair<string, object> kvp in eventDataBase)
                {
                    eventDataReal.Add(kvp.Key, kvp.Value.ToString());
                }
            }

            didReceiveEventCallback(eventName, eventDataReal);
        }
    }
}
