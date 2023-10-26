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
    GameObject loginUI;     // 로그인 UI

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

    [SerializeField]
    GameObject deadUI;   // 사망 UI

    public void SetStartUi(bool active)
    {
        startUI.SetActive(active);
    } // 게임 시작 UI 켜고 끄기

    public void SetLoginUi(bool active)
    {
        loginUI.SetActive(active);
    } // 로그인 UI 켜고 끄기

    public void SetGameUi(bool active)
    {
        gameUI.SetActive(active);
    } // 인게임 UI 켜고 끄기

    public void SetSettingUi(bool active)
    {
        settingUI.SetActive(active);
    } // 설정 UI 켜고 끄기

    public void SetGuildUi(bool active)
    {
        guildUI.SetActive(active);
    } // 길드 UI 켜고 끄기

    public void SetPartyUi(bool active)
    {
        partyUI.SetActive(active);
    } // 동료 UI 켜고 끄기

    public void SetDungeonUi(bool active)
    {
        dungeonUI.SetActive(active);
    } // 던전 UI 켜고 끄기

    public void SetQuestUi(bool active)
    {
        questUI.SetActive(active);
    } // 퀘스트 UI 켜고 끄기

    public void SetRankingUi(bool active)
    {
        rankingUI.SetActive(active);
    } // 랭킹 UI 켜고 끄기

    public void SetUpgradeUi(bool active)
    {
        upgradeUI.SetActive(active);
    } // 업그레이드 UI 켜고 끄기

    public void SetOneUi(bool active)
    {
        oneUI.SetActive(active);
    } // 1개 뽑기 UI 켜고 끄기

    public void SetTenUi(bool active)
    {
        tenUI.SetActive(active);
    } // 10개 뽑기 UI 켜고 끄기

    public void SetDeadUi(bool active)
    {
        deadUI.SetActive(active);
    } // 사망 스크린 UI 켜고 끄기
}
