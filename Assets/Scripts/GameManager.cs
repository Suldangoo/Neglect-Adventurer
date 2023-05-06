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
    GameObject background; // 백그라운드 스크롤링 오브젝트
    GameObject ground; // 그라운드 스크롤링 오브젝트

    public Animator knightAnim;

    // --- 게임 변수
    float backSpeed = 0.2f;
    float terrainSpeed = 2f;
    float groundSpeed = 5f;

    void Start()
    {
        UiManager.SetStartUi(true);
        UiManager.SetGameUi(false);

        background = GameObject.Find("Background");
        ground = GameObject.Find("Ground");

        background.GetComponent<InfiniteScrollingBackground>().backSpeed = 0f;
        background.GetComponent<InfiniteScrollingBackground>().terrainSpeed = 0f;
        ground.GetComponent<GroundScrolling>().speed = 0f;
    }

    void Update()
    {
        
    }

    public void GameStart() {
        knightAnim.SetBool("Start", true);
        UiManager.SetStartUi(false);
        UiManager.SetGameUi(true);

        background.GetComponent<InfiniteScrollingBackground>().backSpeed = this.backSpeed;
        background.GetComponent<InfiniteScrollingBackground>().terrainSpeed = this.terrainSpeed;
        ground.GetComponent<GroundScrolling>().speed = this.groundSpeed;
    }
}
