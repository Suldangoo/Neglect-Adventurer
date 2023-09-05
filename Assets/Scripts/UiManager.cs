using UnityEngine;

// UI 활성화 / 비활성화 제어 클래스
public class UiManager : MonoBehaviour
{
    // 싱글톤 패턴
    public static UiManager Instance{
        get {
            if (instance == null) {
                instance = FindObjectOfType<UiManager>();
            }
            return instance;
        }
    }
    private static UiManager instance;

    [SerializeField]
    GameObject startUI;     // 게임 시작 UI

    [SerializeField]
    GameObject gameUI;      // 인게임 UI

    [SerializeField]
    GameObject settingUI;   // 랭킹 UI

    [SerializeField]
    GameObject guildUI;   // 길드 UI

    [SerializeField]
    GameObject partyUI;   // 동료 UI

    [SerializeField]
    GameObject upgradeUI;   // 수련 UI

    [SerializeField]
    GameObject dungeonUI;   // 던전 UI

    [SerializeField]
    GameObject questUI;   // 임무 UI

    [SerializeField]
    GameObject rankingUI;   // 랭킹 UI

    [SerializeField]
    GameObject oneUI;   // 단차 UI

    [SerializeField]
    GameObject tenUI;   // 10연차 UI

    public void SetStartUi(bool active)
    {
        // 게임 시작 UI 켜고 끄기
        startUI.SetActive(active);
    }

    public void SetGameUi(bool active)
    {
        // 인게임 UI 켜고 끄기
        gameUI.SetActive(active);
    }

    public void SetSettingUi(bool active)
    {
        // 인게임 UI 켜고 끄기
        settingUI.SetActive(active);
    }

    public void SetGuildUi(bool active)
    {
        // 인게임 UI 켜고 끄기
        guildUI.SetActive(active);
    }

    public void SetPartyUi(bool active)
    {
        // 인게임 UI 켜고 끄기
        partyUI.SetActive(active);
    }

    public void SetDungeonUi(bool active)
    {
        // 인게임 UI 켜고 끄기
        dungeonUI.SetActive(active);
    }

    public void SetQuestUi(bool active)
    {
        // 인게임 UI 켜고 끄기
        questUI.SetActive(active);
    }

    public void SetRankingUi(bool active)
    {
        // 인게임 UI 켜고 끄기
        rankingUI.SetActive(active);
    }

    public void SetUpgradeUi(bool active)
    {
        // 인게임 UI 켜고 끄기
        upgradeUI.SetActive(active);
    }

    public void SetOneUi(bool active)
    {
        // 인게임 UI 켜고 끄기
        oneUI.SetActive(active);
    }

    public void SetTenUi(bool active)
    {
        // 인게임 UI 켜고 끄기
        tenUI.SetActive(active);
    }
}
