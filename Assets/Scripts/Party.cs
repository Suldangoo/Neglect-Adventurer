using UnityEngine;
using UnityEngine.UI;
using TMPro;

// 캐릭터의 스텟을 나타내는 구조체
public struct CharacterStats
{
    public int Attack;
    public int Defense;
    public int Recovery;
    public float RecoveryTime;

    public static CharacterStats operator +(CharacterStats a, CharacterStats b)
    {
        return new CharacterStats
        {
            Attack = a.Attack + b.Attack,
            Defense = a.Defense + b.Defense,
            Recovery = a.Recovery + b.Recovery,
            RecoveryTime = a.RecoveryTime + b.RecoveryTime
        };
    }
}

public class Party : MonoBehaviour
{
    // 게임매니저 호출
    GameManager GameManager => GameManager.Instance;

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

    [SerializeField]
    private GameObject equipButton; // 장착 버튼 참조

    // 캐릭터의 세부 정보 표시를 위한 변수들
    [SerializeField] private Image detailedCharacterImage;
    [SerializeField] private TextMeshProUGUI detailedCharacterNameText;
    [SerializeField] private TextMeshProUGUI detailedCharacterLevelText;
    [SerializeField] private TextMeshProUGUI detailedCharacterGradePositionText;
    [SerializeField] private TextMeshProUGUI detailedCharacterEffectText;
    [SerializeField] private TextMeshProUGUI detailedCharacterDescriptionText;

    // 각 캐릭터의 설명
    [SerializeField, TextArea(1, 3)]
    private string[] characterDescriptions;

    [SerializeField]
    private Image[] equippedCharacterImages; // 장착된 캐릭터들의 이미지들 (3개)
    [SerializeField]
    private Image[] onGameEquippedCharacterImages; // 게임화면에 갱신되어야 할 장착된 캐릭터의 이미지들


    private int[] equippedCharacterIndices = { -1, -1, -1 }; // 현재 장착된 캐릭터의 인덱스 (최대 3명)

    // 토글 배열 (9개의 캐릭터에 대한 토글)
    [SerializeField] private Toggle[] characterToggles;

    [System.Serializable]
    public class CharacterStat
    {
        public int baseAttack;
        public int baseDefense;
        public float baseRecovery;
        public float baseRecoveryTime;
        public int attackPerLevel;
        public int defensePerLevel;
        public float recoveryTimePerLevel;
    }

    // 외부에서 참조 가능한 배열
    public CharacterStat[] characterStats;

    // 해당 캐릭터의 공격력을 얻는 메서드
    public int GetCharacterAttack(int characterIndex, int level)
    {
        return characterStats[characterIndex].baseAttack + (characterStats[characterIndex].attackPerLevel * (level - 1));
    }

    // 해당 캐릭터의 방어력을 얻는 메서드
    public int GetCharacterDefense(int characterIndex, int level)
    {
        return characterStats[characterIndex].baseDefense + (characterStats[characterIndex].defensePerLevel * (level - 1));
    }

    // 해당 캐릭터의 회복력을 얻는 메서드
    public float GetCharacterRecovery(int characterIndex)
    {
        return characterStats[characterIndex].baseRecovery;
    }

    // 해당 캐릭터의 회복 시간을 얻는 메서드
    public float GetCharacterRecoveryTime(int characterIndex, int level)
    {
        return characterStats[characterIndex].baseRecoveryTime - (characterStats[characterIndex].recoveryTimePerLevel * (level - 1));
    }

    // 처음 파티 창을 열었을 때 토글 선택되어있는 캐릭터 탭의 디테일을 표시하는 메서드
    private void OnEnable()
    {
        UpdateCharacterView();
        if (characterToggles[0].isOn)
        {
            UpdateDetailedView(0);
        }
    }

    // 처음 게임을 실행했을 때 장착한 모험가들의 데이터를 불러오는 메서드
    public void LoadAndEquipCharacters()
    {
        // 불러온 데이터에 따라 캐릭터 장착하기
        for (int i = 0; i < BackendGameData.Instance.UserGameData.equippedCharacters.Length; i++)
        {
            int characterIndex = BackendGameData.Instance.UserGameData.equippedCharacters[i];

            if (characterIndex != -1)
            {
                // 이미지 할당하기
                equippedCharacterImages[i].sprite = guild.characterSprites[characterIndex];
                onGameEquippedCharacterImages[i].sprite = guild.characterSprites[characterIndex];
                equippedCharacterImages[i].enabled = true;
                onGameEquippedCharacterImages[i].enabled = true;
                equippedCharacterIndices[i] = characterIndex;
            }
            else
            {
                equippedCharacterImages[i].enabled = false;
                onGameEquippedCharacterImages[i].enabled = false;
            }
        }

        GameManager.UpdateHealerRecoveryRoutine(); // 힐러 코루틴 업데이트
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

    private string GetGradePositionTextByIndex(int index)
    {
        string[] positions = { "기사", "법사", "힐러" };
        string[] grades = { "★", "★★", "★★★" };

        int positionIndex = index / 3; // 0, 1, 2에 대해서 각각 0(기사), 1(법사), 2(힐러)
        int gradeIndex = index % 3; // 0, 1, 2에 대해서 각각 1성, 2성, 3성

        return grades[gradeIndex] + " " + positions[positionIndex];
    }

    private string GetCharacterEffectText(int characterIndex, int[] characterDataArray)
    {
        int gradeIndex = characterIndex % 3; // 0, 1, 2에 대해서 각각 1성, 2성, 3성
        int level = characterDataArray[gradeIndex];

        string effectText = "";

        int attack = GetCharacterAttack(characterIndex, level);
        if (attack > 0)
        {
            effectText += $"공격력 + {attack}\n";
        }

        int defense = GetCharacterDefense(characterIndex, level);
        if (defense > 0)
        {
            effectText += $"방어력 + {defense}\n";
        }

        float recovery = GetCharacterRecovery(characterIndex);
        if (recovery > 0)
        {
            effectText += $"회복력: {recovery}\n";
        }

        float recoveryTime = GetCharacterRecoveryTime(characterIndex, level);
        if (recoveryTime > 0)
        {
            effectText += $"회복시간: {recoveryTime}초\n";
        }

        return effectText;
    }

    public void UpdateDetailedView(int characterIndex)
    {
        int[] characterDataArray;

        if (characterIndex >= 0 && characterIndex < 3) // 기사
        {
            characterDataArray = BackendGameData.Instance.UserGameData.knights;
        }
        else if (characterIndex >= 3 && characterIndex < 6) // 법사
        {
            characterDataArray = BackendGameData.Instance.UserGameData.magics;
        }
        else if (characterIndex >= 6 && characterIndex < 9) // 힐러
        {
            characterDataArray = BackendGameData.Instance.UserGameData.heals;
        }
        else
        {
            HideDetailedView();
            return;
        }

        int gradeIndex = characterIndex % 3; // 0, 1, 2에 대해서 각각 1성, 2성, 3성

        if (characterDataArray[gradeIndex] > 0) // 해당 등급의 캐릭터를 얻었는지 확인
        {
            detailedCharacterImage.sprite = guild.characterSprites[characterIndex];
            detailedCharacterImage.color = Color.white;

            detailedCharacterNameText.text = guild.characterNames[characterIndex];
            detailedCharacterGradePositionText.text = GetGradePositionTextByIndex(characterIndex);
            detailedCharacterEffectText.text = GetCharacterEffectText(characterIndex, characterDataArray);
            detailedCharacterDescriptionText.text = characterDescriptions[characterIndex];

            detailedCharacterLevelText.text = "Lv." + characterDataArray[gradeIndex].ToString();

            // 장착 버튼 활성화
            equipButton.SetActive(true);
        }
        else
        {
            HideDetailedView();
        }
    }

    // 아무것도 선택하지 않았을 경우 세부 정보를 숨기는 함수
    private void HideDetailedView()
    {
        detailedCharacterImage.color = new Color(1, 1, 1, 0); // 투명
        detailedCharacterNameText.text = "";
        detailedCharacterLevelText.text = "";
        detailedCharacterGradePositionText.text = "";
        detailedCharacterEffectText.text = "";
        detailedCharacterDescriptionText.text = "";

        // 장착 버튼 비활성화
        equipButton.SetActive(false);
    }

    // 장착 캐릭터에 힐러가 존재하는지 확인하는 메서드
    public bool isEquippedHealer()
    {
        return System.Array.Exists(equippedCharacterIndices, index => index >= 6 && index < 9);
    }

    // 캐릭터 장착 버튼 클릭 메서드
    public void OnEquipButtonClicked()
    {
        int selectedCharacterIndex = GetCurrentSelectedToggleIndex();
        if (selectedCharacterIndex != -1) // 선택된 캐릭터가 있으면
        {
            EquipCharacter(selectedCharacterIndex);
        }
    }

    // 캐릭터 장착 메서드
    private void EquipCharacter(int characterIndex)
    {
        // 이미 장착된 캐릭터인지 확인
        if (System.Array.Exists(equippedCharacterIndices, index => index == characterIndex))
        {
            Debug.Log("This character is already equipped.");
            return;
        }

        // 만약 장착하려는 캐릭터가 힐러이고, 이미 장착된 캐릭터 중에 힐러가 있다면 장착을 거부
        if (characterIndex >= 6 && characterIndex < 9 && isEquippedHealer())
        {
            Debug.Log("A healer is already equipped.");
            return;
        }

        // 장착 가능한 빈 슬롯을 찾기
        int emptySlot = System.Array.FindIndex(equippedCharacterIndices, index => index == -1);
        if (emptySlot != -1)
        {
            equippedCharacterImages[emptySlot].sprite = guild.characterSprites[characterIndex];
            onGameEquippedCharacterImages[emptySlot].sprite = guild.characterSprites[characterIndex];
            equippedCharacterImages[emptySlot].enabled = true; // 이미지를 활성화
            onGameEquippedCharacterImages[emptySlot].enabled = true;
            equippedCharacterIndices[emptySlot] = characterIndex;

            GameManager.UpdateHealerRecoveryRoutine(); // 힐러 코루틴 업데이트

            // 백엔드 데이터에 반영
            BackendGameData.Instance.UserGameData.equippedCharacters[emptySlot] = characterIndex;

            // 백엔드 데이터 업데이트 요청
            BackendGameData.Instance.GameDataUpdate();
        }
        else
        {
            Debug.Log("All slots are occupied.");
        }
    }

    // 장착한 캐릭터 리셋
    public void ResetEquippedCharacters()
    {
        for (int i = 0; i < 3; i++)
        {
            equippedCharacterImages[i].enabled = false; // 이미지를 비활성화
            onGameEquippedCharacterImages[i].enabled = false;
            equippedCharacterIndices[i] = -1;

            // 백엔드 데이터에 반영
            BackendGameData.Instance.UserGameData.equippedCharacters[i] = -1;
        }
        GameManager.UpdateHealerRecoveryRoutine(); // 힐러 코루틴 업데이트

        // 백엔드 데이터 업데이트 요청
        BackendGameData.Instance.GameDataUpdate();
    }
    
    // 어떤 캐릭터 탭의 토글이 켜져있는지 검색
    private int GetCurrentSelectedToggleIndex()
    {
        for (int i = 0; i < characterToggles.Length; i++)
        {
            if (characterToggles[i].isOn)
            {
                return i;
            }
        }
        return -1; // 선택된 토글이 없는 경우
    }

    // 원하는 인덱스의 캐릭터 레벨을 알아오는 메서드
    private int GetCharacterLevel(int characterIndex)
    {
        if (characterIndex >= 0 && characterIndex < 3) // 기사
        {
            return BackendGameData.Instance.UserGameData.knights[characterIndex];
        }
        else if (characterIndex >= 3 && characterIndex < 6) // 법사
        {
            return BackendGameData.Instance.UserGameData.magics[characterIndex - 3];
        }
        else if (characterIndex >= 6 && characterIndex < 9) // 힐러
        {
            return BackendGameData.Instance.UserGameData.heals[characterIndex - 6];
        }
        else
        {
            Debug.LogError($"Invalid character index: {characterIndex}");
            return 0; // 기본 값 반환
        }
    }

    // 현재 장착한 캐릭터들의 스탯을 리턴하거나 디버그하는 메서드
    public float EquippedCharacterStats(string type)
    {
        int totalAttack = 0;
        int totalDefense = 0;
        float totalRecovery = 0;
        float totalRecoveryTime = 0;

        foreach (int characterIndex in equippedCharacterIndices)
        {
            // 장착되지 않은 캐릭터는 건너뛰기
            if (characterIndex == -1)
                continue;

            // 해당 캐릭터의 레벨 가져오기
            int level = GetCharacterLevel(characterIndex);

            totalAttack += GetCharacterAttack(characterIndex, level);
            totalDefense += GetCharacterDefense(characterIndex, level);

            // 힐러에 대한 회복력과 회복 시간을 가져옵니다.
            if (characterIndex >= 6 && characterIndex <= 8) // 힐러의 인덱스 범위
            {
                totalRecovery = GetCharacterRecovery(characterIndex);
                totalRecoveryTime = GetCharacterRecoveryTime(characterIndex, level);
            }
        }

        switch (type.ToLower())
        {
            case "debug":
                Debug.Log("현재 장착하고 있는 캐릭터들의 총합 스탯");
                Debug.Log($"증가 공격력 : {totalAttack}");
                Debug.Log($"증가 방어력 : {totalDefense}");
                if (totalRecovery > 0) // 힐러가 장착되어 있으면
                {
                    Debug.Log($"회복력 : {totalRecovery}");
                    Debug.Log($"회복 시간 : {totalRecoveryTime}초");
                }
                return 0;

            case "attack":
                return totalAttack;

            case "defense":
                return totalDefense;

            case "recovery":
                return totalRecovery;

            case "recoverytime":
                return totalRecoveryTime;

            default:
                Debug.LogError("Invalid type provided for GetEquippedCharacterStats!");
                return 0;
        }
    }

    // 현재 장착한 캐릭터들의 스탯을 디버그하는 메서드
    public void OnClickDebugStats()
    {
        EquippedCharacterStats("debug");
    }
}
