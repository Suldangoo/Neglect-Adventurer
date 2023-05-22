using UnityEngine;
using UnityEngine.UI;

public class GoldText : MonoBehaviour
{
    GameManager GameManager => GameManager.Instance; // 게임 매니저 인스턴스
    [SerializeField] Text text; // 골드 텍스트 컴포넌트
    
    void Update()
    {
        text.text = string.Format("{0:#,###}", GameManager.gold).ToString();
    }
}
