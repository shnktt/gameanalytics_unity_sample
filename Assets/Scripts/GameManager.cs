using GameAnalyticsSDK;
using KTLib;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private AnalyticsManager _analyticsManager;
    [SerializeField] private RemoteConfigManager _remoteConfigManager;
    [SerializeField] private Text _textABTestingId;
    [SerializeField] private Text _textABTestingVariantId;
    [SerializeField] private Text _textInt;
    [SerializeField] private Text _textBoolInt;
    [SerializeField] private Text _textBoolString;
    [SerializeField] private Text _textString;
    [SerializeField] private Text _textStageNo;

    enum kKey
    {
        Int,
        Bool_int,
        Bool_String,
        String
    }

    private int _stageNo = 1;

    private void Awake()
    {
        GameAnalyticsSDK.GameAnalytics.Initialize();
        _remoteConfigManager.delegateDidLoad += Fetched;
        _textStageNo.text = $"Stage {_stageNo}";
    }
    
    public void OnClickFetch()
    {
        _textInt.text = "";
        _textBoolInt.text = "";
        _textBoolString.text = "Loading...";
        _textString.text = "";
        _remoteConfigManager.Fetch();
    }

    void Fetched()
    {
        _textABTestingId.text = "ABTestingId: " + GameAnalytics.GetABTestingId();
        _textABTestingVariantId.text = "ABTestingVariantId: " + GameAnalytics.GetABTestingVariantId();
        _textInt.text = "Int: " + _remoteConfigManager.GetValue(kKey.Int.ToString().ToLower(), -1).ToString();
        _textBoolInt.text = "BoolInt: " + _remoteConfigManager.GetValue(kKey.Bool_int.ToString().ToLower(), false).ToString();
        _textBoolString.text = "BoolString: " + _remoteConfigManager.GetValue(kKey.Bool_String.ToString().ToLower(), false).ToString();
        _textString.text = "String: " + _remoteConfigManager.GetValue(kKey.String.ToString().ToLower(), "none");
    }

    public void OnClickGameStart()
    {
        _analyticsManager.GameStart(_stageNo);
    }
    public void OnClickLevelStart()
    {
        _analyticsManager.LevelStart(_stageNo);
    }
    public void OnClickClear()
    {
        _analyticsManager.ClearGame(_stageNo);
        _stageNo++;
        _textStageNo.text = $"Stage {_stageNo}";
    }
    public void OnClickGameOver()
    {
        _analyticsManager.FailGame(_stageNo);
    }
}
