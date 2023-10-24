using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Party : MonoBehaviour
{
    [SerializeField]
    private Image[] characterImages; // 캐릭터 스프라이트를 담는 배열
    [SerializeField]
    private Image[] questionMarks; // ? 이미지를 담는 배열
    [SerializeField]
    private TextMeshProUGUI[] characterNamesTexts; // 캐릭터 이름을 담는 배열
    [SerializeField]
    private TextMeshProUGUI[] characterLevelsTexts; // 캐릭터 레벨을 담는 배열
    [SerializeField]
    private Guild guild; // Guild script reference

    private void Start()
    {
        UpdateCharacterView();
    }

    private void OnEnable()
    {
        UpdateCharacterView();
    }


    private void UpdateCharacterView()
    {
        UpdateCharacter(BackendGameData.Instance.UserGameData.knights, 0); // 기사
        UpdateCharacter(BackendGameData.Instance.UserGameData.magics, 3);  // 법사
        UpdateCharacter(BackendGameData.Instance.UserGameData.heals, 6);  // 힐러
    }

    private void UpdateCharacter(int[] characterData, int startIdx)
    {
        for (int i = 0; i < 3; i++)
        {
            int idx = startIdx + i;

            if (characterData[i] > 0)
            {
                characterImages[idx].color = Color.white;
                questionMarks[idx].gameObject.SetActive(false);
                characterNamesTexts[idx].text = guild.characterNames[idx];
                characterLevelsTexts[idx].text = "Lv." + characterData[i].ToString();
            }
        }
    }
}
