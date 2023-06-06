using UnityEngine;
using UnityEngine.UI;

public class QuestUI : MonoBehaviour
{
    public Image questSlide;    // 퀘스트 진행바
    public Text questText;      // 퀘스트 텍스트

    int value;              // 퀘스트 진행률
    int maxValue;           // 최대 퀘스트 진행률

    void Start()
    {
        // 초기화
        maxValue = 100;
        value = 0;
    }

    public void setQuestValue(int val)
    {
        value += val;
        questSlide.fillAmount = (float) value / maxValue;
        questText.text = "몬스터 " + value.ToString() + " / " + maxValue.ToString() + " 마리 토벌!";
    }
}
