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
    GameObject guildUI;   // 길드 UI

    [SerializeField]
    GameObject upgradeUI;   // 수련 UI

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

    public void SetGuildUi(bool active)
    {
        // 인게임 UI 켜고 끄기
        guildUI.SetActive(active);
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
