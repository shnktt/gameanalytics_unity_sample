using System.Collections;
using GameAnalyticsSDK;
using UnityEngine;

namespace KTLib
{
    public class RemoteConfigManager : MonoBehaviour
    {
        public delegate void Delegate();
        public Delegate delegateDidLoad;
        
        enum kStatus
        {
            None,
            Loading,
            Loaded,
        }
        kStatus _status = kStatus.None;
        private Coroutine _timeOutCoroutine;

        private void Awake()
        {
            GameAnalytics.OnRemoteConfigsUpdatedEvent += Completed;
        }

        public void Fetch()
        {
            if (_status == kStatus.Loading)
            {
                return;
            }
            if (_timeOutCoroutine != null)
            {
                StopCoroutine(_timeOutCoroutine);
                _timeOutCoroutine = null;
            }
            _timeOutCoroutine = StartCoroutine(TimeOut());
            GameAnalytics.RemoteConfigsUpdated();
        }

        private void Completed()
        {
            if (_timeOutCoroutine != null)
            {
                StopCoroutine(_timeOutCoroutine);
                _timeOutCoroutine = null;
            }
            _status = kStatus.Loaded;
            delegateDidLoad?.Invoke();
        }

        private void FailedFetched()
        {
            _timeOutCoroutine = null;
            _status = kStatus.Loaded;
            delegateDidLoad?.Invoke();
        }

        IEnumerator TimeOut()
        {
            yield return new WaitForSeconds(5.0f);
            FailedFetched();
        }

        public int GetValue(string key, int def)
        {
            var v = GameAnalytics.GetRemoteConfigsValueAsString(key, def.ToString());
            if (int.TryParse(v, out var result))
            {
                return result;
            }

            return def;
        }
        
        public string GetValue(string key, string def)
        {
            return GameAnalytics.GetRemoteConfigsValueAsString(key, def.ToString());
        }
        
        public bool GetValue(string key, bool def)
        {
            var v = GameAnalytics.GetRemoteConfigsValueAsString(key, def.ToString());
            if (int.TryParse(v, out var i))
            {
                return i == 1;
            }
            if (bool.TryParse(v, out var result))
            {
                return result;
            }
            return def;
        }

        void OnDestroy()
        {
            GameAnalytics.OnRemoteConfigsUpdatedEvent -= Completed;
        }
    }
}
