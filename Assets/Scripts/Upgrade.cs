using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Upgrade : MonoBehaviour
{
    GameManager GameManager => GameManager.Instance; // ���� �Ŵ��� �ν��Ͻ�

    [SerializeField] Text atkLevel; // �˼� ����
    [SerializeField] Text atk;      // �˼� ���ݷ�
    [SerializeField] Text atkSpeed; // �˼� ���ݼӵ�
    [SerializeField] Text atkCost;  // �˼� ���ú��

    [SerializeField] Text defLevel; // ��� ����
    [SerializeField] Text def;      // ����
    [SerializeField] Text defCost;  // ��� ���ú��

    [SerializeField] Text dexLevel; // ��ø ����
    [SerializeField] Text dex;      // ��ø��
    [SerializeField] Text dexCost;  // ��ø ���ú��

    [SerializeField] Text lukLevel; // ��� ����
    [SerializeField] Text luk;      // ����
    [SerializeField] Text lukCost;  // ��� ���ú��

    public void modifyState()
    {
        atkLevel.text = "Lv. " + GameManager.atkLv.ToString();
        atk.text = "���� ���ݷ� : " + GameManager.power.ToString();
        atkSpeed.text = "���� ���ݼӵ� : " + GameManager.attackSpeed.ToString();
        atkCost.text = "1,000";

        dexLevel.text = "Lv. " + GameManager.dexLv.ToString();
        dex.text = "���� �̵��ӵ� : " + GameManager.scrollSpeed.ToString();
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
