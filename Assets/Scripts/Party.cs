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

    private void UpdateCharacterView()
    {
        UpdateCharacter("knight", BackendGameData.Instance.UserGameData.knights, 0, 2);
        UpdateCharacter("magic", BackendGameData.Instance.UserGameData.magics, 3, 5);
        UpdateCharacter("heal", BackendGameData.Instance.UserGameData.heals, 6, 8);
    }

    private void UpdateCharacter(string characterType, int[] characterData, int startIdx, int endIdx)
    {
        for (int i = startIdx; i <= endIdx; i++)
        {
            int grade = i - startIdx; // 0, 1, 2

            if (characterData[grade] > 0)
            {
                characterImages[i].color = Color.white;
                questionMarks[i].gameObject.SetActive(false);
                characterNamesTexts[i].text = guild.characterNames[i];
                characterLevelsTexts[i].text = "Lv." + characterData[grade].ToString();
            }
        }
    }
}
