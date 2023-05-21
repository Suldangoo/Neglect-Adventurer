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
    GameObject UpgradeUI;   // 수련 UI

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

    public void SetUpgradeUi(bool active)
    {
        // 인게임 UI 켜고 끄기
        UpgradeUI.SetActive(active);
    }
}
