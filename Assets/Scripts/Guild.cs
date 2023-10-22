using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Guild : MonoBehaviour
{
    GameManager GameManager => GameManager.Instance; // 게임 매니저 인스턴스

    [SerializeField]
    private TextMeshProUGUI textGold;
    [SerializeField]
    private TextMeshProUGUI textDiamond;

    // 캐릭터 이미지 배열
    public Sprite[] characterSprites;

    // 캐릭터 프레임 이미지 배열
    public Sprite[] prameSprites;

    // 캐릭터 이름 배열
    public string[] characterNames;

    // --- 1번 뽑기
    public Image resultImage; // 캐릭터 이미지
    public Image prameImage; // 프레임 이미지
    public TextMeshProUGUI resultText; // 캐릭터 이름

    // --- 10번 뽑기
    public Image[] resultImages;
    public Image[] prameImages;
    public TextMeshProUGUI[] resultTexts; // 캐릭터 이름

    // 각 등급의 색상
    private readonly Color32 colorGrade1 = new Color32(255, 255, 255, 255); // FFFFFF
    private readonly Color32 colorGrade2 = new Color32(153, 255, 153, 255); // 99FF99
    private readonly Color32 colorGrade3 = new Color32(255, 249, 141, 255); // FFF98D

    private const int COST_NORMAL_ONE_PICK = 50000; // 1회 뽑기 비용
    private const int COST_RARE_ONE_PICK = 100;  // 희귀 1회 뽑기 비용
    private const int COST_NORMAL_TEN_PICK = 450000;  // 10회 뽑기 비용 (가격 할인 포함)
    private const int COST_RARE_TEN_PICK = 900; // 희귀 10회 뽑기 비용 (가격 할인 포함)

    // 뽑기 설정 구조체
    struct GachaSetting
    {
        public int minIndex;
        public int maxIndex;
        public int grade1;
        public int grade2;
        public float grade1Chance;

        public GachaSetting(int min, int max, int g1, int g2, float chance)
        {
            minIndex = min;
            maxIndex = max;
            grade1 = g1;
            grade2 = g2;
            grade1Chance = chance;
        }
    }

    private void Awake()
    {
        // 업그레이드 UI가 켜지면 수치 갱신
        modifyState(0);
    }

    public void modifyState(int value)
    {
        textGold.text = BackendGameData.Instance.UserGameData.gold.ToString("N0"); // 골드 표시 갱신
        textDiamond.text = BackendGameData.Instance.UserGameData.diamond.ToString("N0"); // 골드 표시 갱신
    }

    public void OnClickNomalOnePick()
    {
        if (BackendGameData.Instance.UserGameData.gold >= COST_NORMAL_ONE_PICK)
        {
            // 골드 감소
            BackendGameData.Instance.UserGameData.gold -= COST_NORMAL_ONE_PICK;
            // 뽑기 로직
            GachaSetting setting = new GachaSetting(0, 6, 1, 2, 0.9f);
            PerformPick(setting, 1);

            // UI 갱신
            modifyState(0);

            // 1개 뽑기 UI 활성화
            UiManager.Instance.SetOneUi(true);
        }
        else
        {
            Debug.Log("Not enough gold for normal one pick.");
        }
    }

    public void OnClickRareOnePick()
    {
        if (BackendGameData.Instance.UserGameData.diamond >= COST_RARE_ONE_PICK)
        {
            // 다이아 감소
            BackendGameData.Instance.UserGameData.diamond -= COST_RARE_ONE_PICK;

            // 뽑기 로직
            GachaSetting setting = new GachaSetting(3, 9, 2, 3, 0.9f);
            PerformPick(setting, 1);

            // UI 갱신
            modifyState(0);

            // 1개 뽑기 UI 활성화
            UiManager.Instance.SetOneUi(true);
        }
        else
        {
            Debug.Log("Not enough diamonds for rare one pick.");
        }
    }

    public void OnClickNomalTenPick()
    {
        if (BackendGameData.Instance.UserGameData.gold >= COST_NORMAL_TEN_PICK)
        {
            // 골드 감소
            BackendGameData.Instance.UserGameData.gold -= COST_NORMAL_TEN_PICK;

            // 뽑기 로직
            GachaSetting setting = new GachaSetting(0, 6, 1, 2, 0.9f);
            PerformPick(setting, 10);

            // UI 갱신
            modifyState(0);

            // 10개 뽑기 UI 활성화
            UiManager.Instance.SetTenUi(true);
        }
        else
        {
            Debug.Log("Not enough gold for normal ten pick.");
        }
    }

    public void OnClickRareTenPick()
    {
        if (BackendGameData.Instance.UserGameData.diamond >= COST_RARE_TEN_PICK)
        {
            // 다이아 감소
            BackendGameData.Instance.UserGameData.diamond -= COST_RARE_TEN_PICK;

            // 뽑기 로직
            GachaSetting setting = new GachaSetting(3, 9, 2, 3, 0.9f);
            PerformPick(setting, 10);

            // UI 갱신
            modifyState(0);

            // 10개 뽑기 UI 활성화
            UiManager.Instance.SetTenUi(true);
        }
        else
        {
            Debug.Log("Not enough diamonds for rare ten pick.");
        }
    }


    private void PerformPick(GachaSetting setting, int times)
    {
        for (int i = 0; i < times; i++)
        {
            (Sprite selectedSprite, int grade, int index) = PickCharacterInRange(setting);

            if (times == 1)
            {
                resultImage.sprite = selectedSprite;
                resultText.text = FormatCharacterText(grade, index);
                SetTextColorBasedOnGrade(resultText, grade);
                prameImage.sprite = prameSprites[grade - 1];
            }
            else
            {
                resultImages[i].sprite = selectedSprite;
                resultTexts[i].text = FormatCharacterText(grade, index);
                SetTextColorBasedOnGrade(resultTexts[i], grade);
                prameImages[i].sprite = prameSprites[grade - 1];
            }
        }
    }

    private (Sprite, int, int) PickCharacterInRange(GachaSetting setting)
    {
        float randomValue = Random.value;
        int selectedIndex;

        if (randomValue <= setting.grade1Chance)
        {
            selectedIndex = Random.Range(setting.minIndex, setting.minIndex + 3);
        }
        else
        {
            selectedIndex = Random.Range(setting.minIndex + 3, setting.maxIndex);
        }

        UpdateCharacterData(selectedIndex);

        int grade = (randomValue <= setting.grade1Chance) ? setting.grade1 : setting.grade2;
        return (characterSprites[selectedIndex], grade, selectedIndex);
    }

    private void UpdateCharacterData(int characterIndex)
    {
        // 클래스별로 (기사, 마법사, 힐러) 3개씩 캐릭터가 있으므로
        int classIndex = characterIndex % 3;
        int grade = characterIndex / 3; // 등급 (0-based. 즉, 0: 1성, 1: 2성, 2: 3성)

        switch (classIndex)
        {
            case 0: // 기사
                BackendGameData.Instance.UserGameData.knights[grade]++;
                break;
            case 1: // 마법사
                BackendGameData.Instance.UserGameData.magics[grade]++;
                break;
            case 2: // 힐러
                BackendGameData.Instance.UserGameData.heals[grade]++;
                break;
        }

        // 변수 갱신
        BackendGameData.Instance.GameDataUpdate();
    }


    private void SetFrame(int grade)
    {
        switch (grade)
        {
            case 1:
                prameImage.sprite = prameSprites[0]; // 1성 프레임
                break;
            case 2:
                prameImage.sprite = prameSprites[1]; // 2성 프레임
                break;
            case 3:
                prameImage.sprite = prameSprites[2]; // 3성 프레임
                break;
            default:
                Debug.LogWarning($"Unexpected grade: {grade}");
                break;
        }
    }

    private string FormatCharacterText(int grade, int index)
    {
        string starRepresentation = new string('★', grade);
        return $"{starRepresentation} {characterNames[index]}";
    }

    private void SetTextColorBasedOnGrade(TextMeshProUGUI textComponent, int grade)
    {
        switch (grade)
        {
            case 1:
                textComponent.color = colorGrade1;
                break;
            case 2:
                textComponent.color = colorGrade2;
                break;
            case 3:
                textComponent.color = colorGrade3;
                break;
            default:
                Debug.LogWarning($"Unexpected grade: {grade}");
                break;
        }
    }
}
