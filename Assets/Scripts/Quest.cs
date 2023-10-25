using UnityEngine;
using UnityEngine.UI;

public class Quest : MonoBehaviour
{
    public GameObject NotQuest; // 퀘스트를 수주하고 있지 않을 때 UI
    public GameObject NowQuest; // 퀘스트를 수주하고 있을 때 UI
    public GameObject onQuestGuide; // 퀘스트를 수주중일 때 나타나는 가이드
    public Image questSlide;    // 퀘스트 진행바
    public Text reward;         // 리워드 변수
    public Text questText;      // 퀘스트 텍스트

    private int questGoal; // 퀘스트 목표 값
    private int currentQuestValue; // 현재 진행 상태
    private int questReward; // 퀘스트 수행 시 받을 보상

    // 임무 수주 UI에 있는 퀘스트 수주 패널
    public GameObject[] QuestPanels; // QuestPanel 오브젝트 3개
    public Text[] QuestNameTexts;    // 3개의 퀘스트 이름 텍스트
    public Text[] RewardTexts;       // 3개의 보상 텍스트

    // 퀘스트 정보를 저장하는 구조체
    [System.Serializable]
    public struct QuestData
    {
        public QuestType questType;
        public int questGoal;
        public int reward;
    }

    // 생성된 퀘스트를 저장할 배열
    private QuestData[] offeredQuests = new QuestData[3];

    // 퀘스트의 타입
    public enum QuestType
    {
        KillMonsters,
        CollectGold,
        RunMeters
    }

    private QuestType currentQuestType;

    void Start()
    {
        // 현재 퀘스트 진행도 초기화
        currentQuestValue = 0;

        // 게임 시작 시 퀘스트 3개를 랜덤으로 제공하는 메소드를 호출
        OfferRandomQuests();
    }

    // 지금 수주중인 퀘스트 타입을 리턴하는 메서드
    public string GetCurrentQuestTypeAsString()
    {
        return currentQuestType.ToString();
    }

    // 랜덤으로 퀘스트 3개를 제공하는 메소드
    public void OfferRandomQuests()
    {
        for (int i = 0; i < 3; i++)
        {
            QuestType randomQuestType = (QuestType)Random.Range(0, 3);
            QuestData newQuest = GenerateRandomQuest(randomQuestType);

            // Update UI
            QuestNameTexts[i].text = GetQuestName(newQuest);
            RewardTexts[i].text = newQuest.reward.ToString("N0");
            QuestPanels[i].SetActive(true);

            offeredQuests[i] = newQuest;
        }
    }

    // 랜덤한 퀘스트 생성 메서드
    private QuestData GenerateRandomQuest(QuestType questType)
    {
        QuestData questData;
        questData.questType = questType;

        switch (questType)
        {
            case QuestType.KillMonsters:
                questData.questGoal = Random.Range(3, 21) * 10;
                questData.reward = questData.questGoal / 2;
                break;
            case QuestType.CollectGold:
                questData.questGoal = Random.Range(5, 51) * 10000;
                questData.reward = questData.questGoal / 10000;
                break;
            case QuestType.RunMeters:
                questData.questGoal = Random.Range(2, 16) * 100;
                questData.reward = questData.questGoal / 10;
                break;
            default:
                questData.questGoal = 0;
                questData.reward = 0;
                break;
        }

        return questData;
    }

    // 퀘스트 이름 생성 메서드
    private string GetQuestName(QuestData questData)
    {
        switch (questData.questType)
        {
            case QuestType.KillMonsters:
                return "몬스터 " + questData.questGoal.ToString("N0") + "마리 토벌";
            case QuestType.CollectGold:
                return questData.questGoal.ToString("N0") + " 골드 획득";
            case QuestType.RunMeters:
                return questData.questGoal.ToString("N0") + " 미터 달리기";
            default:
                return "";
        }
    }

    // 퀘스트 수주 메소드
    public void AcceptQuest(int questIndex)
    {
        currentQuestType = offeredQuests[questIndex].questType;
        questGoal = offeredQuests[questIndex].questGoal;
        questReward = offeredQuests[questIndex].reward;

        NotQuest.SetActive(false);
        NowQuest.SetActive(true);
        onQuestGuide.SetActive(true);
        foreach (GameObject panel in QuestPanels)
        {
            panel.SetActive(false);
        }

        UpdateQuestUI();
    }

    // 퀘스트의 진행 상태를 업데이트하는 메소드
    public void UpQuestValue(int value)
    {
        currentQuestValue += value;
        UpdateQuestUI();

        if (currentQuestValue >= questGoal)
        {
            // 퀘스트 완료
            CompleteQuest();
        }
    }

    void UpdateQuestUI()
    {
        questSlide.fillAmount = (float)currentQuestValue / questGoal;

        string progressText = ""; // 진행 상황을 나타내는 텍스트

        // 퀘스트의 종류에 따라 진행 상황 텍스트 업데이트
        switch (currentQuestType)
        {
            case QuestType.KillMonsters:
                progressText = $"몬스터 {currentQuestValue:N0} / {questGoal:N0} 마리 토벌";
                break;
            case QuestType.CollectGold:
                progressText = $"{currentQuestValue:N0} / {questGoal:N0} 골드 획득";
                break;
            case QuestType.RunMeters:
                progressText = $"{currentQuestValue:N0} / {questGoal:N0} 미터 달리기";
                break;
        }

        questText.text = progressText; // Text 컴포넌트에 진행 상황 텍스트 설정
        reward.text = questReward.ToString("N0"); // 보상 텍스트 설정
    }

    // 퀘스트 완료 메소드
    void CompleteQuest()
    {
        // 보상 지급 및 완료 연출
        // TODO: 보상 지급 로직 추가
    }
}
