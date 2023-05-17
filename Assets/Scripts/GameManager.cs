using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    // --- 싱글톤 패턴
    public static GameManager Instance{
        get {
            if (instance == null) {
                instance = FindObjectOfType<GameManager>();
            }
            return instance;
        }
    }
    private static GameManager instance;

    UiManager UiManager => UiManager.Instance;

    // --- 외부 오브젝트 변수
    public GameObject monsterPrefab;

    GameObject background;  // 백그라운드 스크롤링 오브젝트
    GameObject ground;      // 그라운드 스크롤링 오브젝트

    private Animator playerAnimator;
    private Animator enemyAnimator;

    // --- 게임 변수
    public bool isStart = false;   // 시작 확인
    public bool isScroll = false;  // 스크롤 확인
    float currTime = 0;         // 시간을 측정할 변수
    float backSpeed = 0.2f;
    float terrainSpeed = 2f;
    float groundSpeed = 5f;

    void Start()
    {
        UiManager.SetStartUi(true);
        UiManager.SetGameUi(false);

        background = GameObject.Find("Background");
        ground = GameObject.Find("Ground");

        SetScrollOff();
    }

    void Update()
    {
        if (isStart)
        {
            currTime += Time.deltaTime;

            if (currTime > 5)
            {
                GameObject monster = Instantiate(monsterPrefab);
                monster.transform.position = new Vector3(14, -2, 0);

                currTime = 0;
            }
        }
    }

    // 게임 시작 함수
    public void GameStart() {
        isStart = true;
        UiManager.SetStartUi(false);
        UiManager.SetGameUi(true);
        SetScrollOn();
    }

    // 전투 시작 함수
    public void StartBattle()
    {
        SetScrollOff();
    }

    public void SetScrollOn() {
        isScroll = true;
        background.GetComponent<InfiniteScrollingBackground>().backSpeed = this.backSpeed;
        background.GetComponent<InfiniteScrollingBackground>().terrainSpeed = this.terrainSpeed;
        ground.GetComponent<GroundScrolling>().speed = this.groundSpeed;
    }

    public void SetScrollOff() {
        isScroll = false;
        background.GetComponent<InfiniteScrollingBackground>().backSpeed = 0f;
        background.GetComponent<InfiniteScrollingBackground>().terrainSpeed = 0f;
        ground.GetComponent<GroundScrolling>().speed = 0f;
    }
}
