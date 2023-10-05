using System;
using UnityEngine;
using UnityEngine.UI;

public class Upgrade : MonoBehaviour
{
    GameManager GameManager => GameManager.Instance; // 게임 매니저 인스턴스

    [SerializeField] Text atkLevel; // 검술 레벨
    [SerializeField] Text atk;      // 검술 공격력
    [SerializeField] Text atkSpeed; // 검술 공격속도
    [SerializeField] Text atkCost;  // 검술 수련비용

    [SerializeField] Text defLevel; // 방어 레벨
    [SerializeField] Text def;      // 방어력
    [SerializeField] Text defCost;  // 방어 수련비용

    [SerializeField] Text dexLevel; // 민첩 레벨
    [SerializeField] Text dex;      // 민첩력
    [SerializeField] Text dexCost;  // 민첩 수련비용

    [SerializeField] Text lukLevel; // 행운 레벨
    [SerializeField] Text luk;      // 행운력
    [SerializeField] Text lukCost;  // 행운 수련비용

    private void Awake()
    {
        // 업그레이드 UI가 켜지면 수치 갱신
        modifyState();
    }

    public void modifyState()
    {
        // 소수점 변수들 보정
        GameManager.power = Rounds(GameManager.power, 0);
        GameManager.attackSpeed = Rounds(GameManager.attackSpeed, 2);
        GameManager.scrollSpeed = Rounds(GameManager.scrollSpeed, 0);

        GameManager.UpdateState(); // 실제 플레이어 스탯 업데이트
        BackendGameData.Instance.GameDataUpdate(); // 서버에 플레이어 스탯 갱신

        // 검술 단련 텍스트 갱신
        atkLevel.text = "Lv. " + BackendGameData.Instance.UserGameData.atkLv.ToString();
        atk.text = "현재 공격력 : " + GameManager.power.ToString();
        atkSpeed.text = "현재 공격속도 : " + GameManager.attackSpeed.ToString();
        atkCost.text = (1000 * BackendGameData.Instance.UserGameData.atkLv).ToString("#,##0");

        // 방어 단련 텍스트 갱신
        defLevel.text = "Lv. " + BackendGameData.Instance.UserGameData.defLv.ToString();
        def.text = "현재 방어력 : " + ((BackendGameData.Instance.UserGameData.defLv - 1) * 10).ToString().ToString();
        defCost.text = (1000 * BackendGameData.Instance.UserGameData.defLv).ToString("#,##0");

        // 민첩 단련 텍스트 갱신
        dexLevel.text = "Lv. " + BackendGameData.Instance.UserGameData.dexLv.ToString();
        dex.text = "현재 이동속도 : " + GameManager.scrollSpeed.ToString();
        dexCost.text = (1000 * BackendGameData.Instance.UserGameData.dexLv).ToString("#,##0");

        // 행운 단련 텍스트 갱신
        lukLevel.text = "Lv. " + BackendGameData.Instance.UserGameData.lukLv.ToString();
        luk.text = "골드획득량 + " + (BackendGameData.Instance.UserGameData.lukLv - 1).ToString() + "%";
        lukCost.text = (1000 * BackendGameData.Instance.UserGameData.lukLv).ToString("#,##0");
    }

    public float Rounds(float tmp, int cnt)
    {
        return (float)Math.Round(tmp, cnt);
    }

    public void upgradeAtk()
    {
        if (BackendGameData.Instance.UserGameData.gold >= 1000 * BackendGameData.Instance.UserGameData.atkLv)
        {
            BackendGameData.Instance.UserGameData.gold -= 1000 * BackendGameData.Instance.UserGameData.atkLv;
            BackendGameData.Instance.UserGameData.atkLv += 1;
            modifyState();
        }
    }

    public void upgradeDef()
    {
        if (BackendGameData.Instance.UserGameData.gold >= 1000 * BackendGameData.Instance.UserGameData.defLv)
        {
            BackendGameData.Instance.UserGameData.gold -= 1000 * BackendGameData.Instance.UserGameData.defLv;
            BackendGameData.Instance.UserGameData.defLv += 1;
            modifyState();
        }
    }

    public void upgradeDex()
    {
        if (BackendGameData.Instance.UserGameData.gold >= 1000 * BackendGameData.Instance.UserGameData.dexLv)
        {
            BackendGameData.Instance.UserGameData.gold -= 1000 * BackendGameData.Instance.UserGameData.dexLv;
            BackendGameData.Instance.UserGameData.dexLv += 1;
            modifyState();
        }
    }

    public void upgradeLuk()
    {
        if (BackendGameData.Instance.UserGameData.gold >= 1000 * BackendGameData.Instance.UserGameData.lukLv)
        {
            BackendGameData.Instance.UserGameData.gold  -= 1000 * BackendGameData.Instance.UserGameData.lukLv;
            BackendGameData.Instance.UserGameData.lukLv += 1;
            modifyState();
        }
    }
}
