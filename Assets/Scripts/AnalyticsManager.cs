using UnityEngine;
using GameAnalyticsSDK;

namespace KTLib
{
    public class AnalyticsManager : MonoBehaviour
    {
        public void GameStart(int stageNo)
        {
            SendDesignEvent(new[] {"game_start", $"stage_no_{stageNo}"});
        }

        public void LevelStart(int stageNo)
        {
            GameAnalytics.NewProgressionEvent(GAProgressionStatus.Start, "stage_" + stageNo);
            SendDesignEvent(new[] {"level_start", $"stage_no_{stageNo}"});
        }

        public void ClearGame(int stageNo)
        {
            GameAnalytics.NewProgressionEvent(GAProgressionStatus.Complete, "stage" + stageNo);
            SendDesignEvent(new[] {"level_complete", $"stage_no_{stageNo}"});
        }

        public void FailGame(int stageNo)
        {
            GameAnalytics.NewProgressionEvent(GAProgressionStatus.Fail, "stage" + stageNo);
        }

        public void SendDesignEvent(string[] values)
        {
            var eventName = "";
            foreach (var value in values)
            {
                if (eventName.Length > 0)
                {
                    eventName += ":";
                }
                eventName += value;
            }
            GameAnalytics.NewDesignEvent(eventName);
        }

        public void SendDesignEvent(string[] events, float eventValue)
        {
            var eventName = "";
            foreach (var e in events)
            {
                if (eventName.Length > 0)
                {
                    eventName += ":";
                }
                eventName += e;
            }
            GameAnalytics.NewDesignEvent(eventName, eventValue);
        }
    }
}