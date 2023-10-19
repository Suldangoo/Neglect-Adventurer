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


    // 뽑은 캐릭터의 이름과 등급을 출력할 TextMeshPro 컴포넌트


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
        int minIndex = 0, maxIndex = 6, grade1 = 1, grade2 = 2; // 1~2성 범위 설정
        float grade1Chance = 0.9f;
        PerformPick(minIndex, maxIndex, grade1, grade2, grade1Chance);
    }

    public void OnClickRareOnePick()
    {
        int minIndex = 3, maxIndex = 9, grade1 = 2, grade2 = 3; // 2~3성 범위 설정
        float grade1Chance = 0.9f;
        PerformPick(minIndex, maxIndex, grade1, grade2, grade1Chance);
    }

    private void PerformPick(int minIndex, int maxIndex, int grade1, int grade2, float grade1Chance)
    {
        (Sprite selectedSprite, int grade, int index) = PickCharacterInRange(minIndex, maxIndex, grade1, grade2, grade1Chance);
        resultImage.sprite = selectedSprite;
        SetFrame(grade);
        string starRepresentation = new string('*', grade);
        resultText.text = $"{starRepresentation} {characterNames[index]}";
    }

    public void OnClickNomalTenPick()
    {
        int minIndex = 0, maxIndex = 6, grade1 = 1, grade2 = 2; // 1~2성 범위 설정
        float grade1Chance = 0.9f;
        PerformTenPick(minIndex, maxIndex, grade1, grade2, grade1Chance);
    }

    public void OnClickRareTenPick()
    {
        int minIndex = 3, maxIndex = 9, grade1 = 2, grade2 = 3; // 2~3성 범위 설정
        float grade1Chance = 0.9f;
        PerformTenPick(minIndex, maxIndex, grade1, grade2, grade1Chance);
    }

    private void PerformTenPick(int minIndex, int maxIndex, int grade1, int grade2, float grade1Chance)
    {
        for (int i = 0; i < 10; i++)
        {
            (Sprite selectedSprite, int grade, int index) = PickCharacterInRange(minIndex, maxIndex, grade1, grade2, grade1Chance);
            resultImages[i].sprite = selectedSprite;
            prameImages[i].sprite = prameSprites[grade - 1]; // 프레임 설정

            string starRepresentation = new string('*', grade);
            resultTexts[i].text = $"{starRepresentation} {characterNames[index]}"; // 캐릭터 이름 출력
        }
    }

    private (Sprite, int, int) PickCharacterInRange(int minIndex, int maxIndex, int grade1, int grade2, float grade1Chance)
    {
        float randomValue = Random.value;
        int selectedIndex;

        if (randomValue <= grade1Chance)
        {
            selectedIndex = Random.Range(minIndex, minIndex + 3);
            return (characterSprites[selectedIndex], grade1, selectedIndex);
        }
        else
        {
            selectedIndex = Random.Range(minIndex + 3, maxIndex);
            return (characterSprites[selectedIndex], grade2, selectedIndex);
        }
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
}
