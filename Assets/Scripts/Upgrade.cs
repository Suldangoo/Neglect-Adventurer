using System.Collections;
using System.Collections.Generic;
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
        atkLevel.text = "Lv. " + GameManager.atkLv.ToString();
        atk.text = "현재 공격력 : " + GameManager.power.ToString();
        atkSpeed.text = "현재 공격속도 : " + GameManager.attackSpeed.ToString();
        atkCost.text = "1,000";

        dexLevel.text = "Lv. " + GameManager.dexLv.ToString();
        dex.text = "현재 이동속도 : " + GameManager.scrollSpeed.ToString();
        dexCost.text = "1,000";
    }

    public void upgradeAtk()
    {
        GameManager.atkLv += 1;
        GameManager.power += 1f;
        GameManager.attackSpeed -= 0.05f;
        GameManager.gold -= 1000;

        modifyState();
    }

    public void upgradeDex()
    {
        GameManager.dexLv += 1;
        GameManager.scrollSpeed += 1f;
        GameManager.gold -= 1000;

        modifyState();
    }
}
