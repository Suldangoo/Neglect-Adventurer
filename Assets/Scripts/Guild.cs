using System;
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
}
