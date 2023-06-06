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
    
    public void modifyState()
    {
        // 소수점 변수들 보정
        GameManager.power = Rounds(GameManager.power, 0);
        GameManager.attackSpeed = Rounds(GameManager.attackSpeed, 2);
        GameManager.scrollSpeed = Rounds(GameManager.scrollSpeed, 0);

        // 검술 단련 텍스트 갱신
        atkLevel.text = "Lv. " + GameManager.atkLv.ToString();
        atk.text = "현재 공격력 : " + GameManager.power.ToString();
        atkSpeed.text = "현재 공격속도 : " + GameManager.attackSpeed.ToString();
        atkCost.text = (1000 * GameManager.atkLv).ToString("#,##0");

        // 방어 단련 텍스트 갱신
        defLevel.text = "Lv. " + GameManager.defLv.ToString();
        def.text = "현재 방어력 : " + ((GameManager.defLv - 1) * 10).ToString().ToString();
        defCost.text = (1000 * GameManager.defLv).ToString("#,##0");

        // 민첩 단련 텍스트 갱신
        dexLevel.text = "Lv. " + GameManager.dexLv.ToString();
        dex.text = "현재 이동속도 : " + GameManager.scrollSpeed.ToString();
        dexCost.text = (1000 * GameManager.dexLv).ToString("#,##0");

        // 행운 단련 텍스트 갱신
        lukLevel.text = "Lv. " + GameManager.lukLv.ToString();
        luk.text = "골드획득량 + " + (GameManager.lukLv - 1).ToString() + "%";
        lukCost.text = (1000 * GameManager.lukLv).ToString("#,##0");
    }

    public float Rounds(float tmp, int cnt)
    {
        return (float)Math.Round(tmp, cnt);
    }

    public void upgradeAtk()
    {
        if (GameManager.gold >= 1000 * GameManager.atkLv)
        {
            GameManager.gold -= 1000 * GameManager.atkLv;
            GameManager.atkLv += 1;
            GameManager.power += 1f;
            GameManager.attackSpeed -= 0.05f;

            modifyState();
        }
    }

    public void upgradeDef()
    {
        if (GameManager.gold >= 1000 * GameManager.defLv)
        {
            GameManager.gold -= 1000 * GameManager.defLv;
            GameManager.defLv += 1;

            modifyState();
        }
    }

    public void upgradeDex()
    {
        if (GameManager.gold >= 1000 * GameManager.dexLv)
        {
            GameManager.gold -= 1000 * GameManager.dexLv;
            GameManager.dexLv += 1;
            GameManager.scrollSpeed += 1f;

            modifyState();
        }
    }

    public void upgradeLuk()
    {
        if (GameManager.gold >= 1000 * GameManager.lukLv)
        {
            GameManager.gold -= 1000 * GameManager.lukLv;
            GameManager.lukLv += 1;

            modifyState();
        }
    }
}
