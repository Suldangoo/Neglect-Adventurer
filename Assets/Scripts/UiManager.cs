using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// UI 활성화 / 비활성화 제어 클래스
public class UiManager : MonoBehaviour
{
    // 싱글톤 패턴
    public static UiManager Instancec{
        get {
            if (instance == null) {
                instance = FindObjectOfType<UiManager>();
            }
            return instance;
        }
    }
    private static UiManager instance;

    [SerializeField]
    GameObject startUI;

    [SerializeField]
    GameObject[] inGameUis;

    public void SetStartUi(bool active)
    {
        startUI.SetActive(active);
    }

    public void SetInGameUIs(bool active)
    {
        foreach (GameObject go in inGameUis) { go.SetActive(active); }
    }
}
