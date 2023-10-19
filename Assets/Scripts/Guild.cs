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

    // 결과를 출력할 Image 컴포넌트
    public Image resultImage;

    // 캐릭터 등급에 따른 프레임 Image 컴포넌트
    public Image prameImage;

    // 캐릭터 이름 배열
    public string[] characterNames;

    // 뽑은 캐릭터의 이름과 등급을 출력할 TextMeshPro 컴포넌트
    public TextMeshProUGUI resultText;

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
        (Sprite selectedSprite, int grade, int index) = PickCharacter();
        resultImage.sprite = selectedSprite;

        // 캐릭터 등급에 따라 프레임 설정
        SetFrame(grade);

        // 캐릭터 이름과 등급을 TextMeshPro에 출력
        string starRepresentation = new string('*', grade);
        resultText.text = $"{starRepresentation} {characterNames[index]}";
    }

    private (Sprite, int, int) PickCharacter()
    {
        // 0 ~ 1 사이의 랜덤한 숫자 생성
        float randomValue = Random.value;
        int selectedIndex;

        if (randomValue <= 0.9f)
        {
            // 1성 캐릭터 뽑기 (0 ~ 2 인덱스)
            selectedIndex = Random.Range(0, 3);
            return (characterSprites[selectedIndex], 1, selectedIndex);
        }
        else
        {
            // 2성 캐릭터 뽑기 (3 ~ 5 인덱스)
            selectedIndex = Random.Range(3, 6);
            return (characterSprites[selectedIndex], 2, selectedIndex);
        }
    }

    private void SetFrame(int grade)
    {
        if (grade == 1)
        {
            prameImage.sprite = prameSprites[0]; // 1성 프레임
        }
        else if (grade == 2)
        {
            prameImage.sprite = prameSprites[1]; // 2성 프레임
        }
    }

}
