using System;
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

    // --- 외부 오브젝트 변수
    UiManager UiManager => UiManager.Instance;

    InfiniteScrolling background;   // 백그라운드 스크롤링 오브젝트
    Animator playerAnimator;        // 플레이어 캐릭터 애니메이터

    [SerializeField] Monster monster; // 몬스터 프리팹
    [SerializeField] Party party;
    [SerializeField] String webURL;

    // --- 게임 변수
    [HideInInspector] public bool isStart = false;   // 시작 확인
    [HideInInspector] public bool isScroll = false;  // 스크롤 확인
    [HideInInspector] public bool isBattle = false;  // 전투 확인

    float currTime;             // 시간을 측정할 변수
    float backSpeed;            // 배경 스크롤링 속도
    float terrainSpeed;         // 지형 스크롤링 속도

    // --- 플레이어 변수

    public float power;         // 공격력
    public float attackSpeed;   // 공격속도
    public float scrollSpeed;   // 이동속도 / 스크롤링 속도

    void Start()
    {
        UiManager.SetStartUi(true); // 시작 UI 켜기
        UiManager.SetGameUi(false); // 게임 UI 끄기

        background = GameObject.Find("Background").GetComponent<InfiniteScrolling>(); // 백그라운드 오브젝트 할당
        playerAnimator = GameObject.Find("Knight").GetComponent<Animator>(); // 플레이어 애니메이션 할당

        SetScrollSpeed(); // 스크롤 속도 초기화
        SetScroll(false); // 스크롤 끄기
    }

    void Update()
    {
        // 전투 모드 돌입
        if (isBattle)
        {
            // 공격 속도 간격을 위한 시간 측정
            currTime += Time.deltaTime;

            if (currTime >= attackSpeed) // 공격 속도의 간격이 되었다면
            {
                if (monster.isLive)
                {
                    playerAnimator.SetTrigger("Attack"); // 공격 애니메이션 재생
                    monster.Damage(power); // 몬스터에게 데미지
                    currTime = 0; // 시간 초기화
                }
                else
                {
                    SetBattle(false);
                    currTime = 0; // 시간 초기화
                }
            }
        }
    }

    // 게임 시작 함수
    public void TouchStart() {
        // 게임 시작 터치 시
        UiManager.SetStartUi(false); // 시작 UI 끄기
        UiManager.SetLoginUi(true); // 로그인 UI 켜기
    }

    public void GameStart()
    {
        UpdateState(); // 유저의 스테이터스 갱신
        isStart = true; // 시작 상태 체크
        SetScroll(true); // 스크롤 시작
        monster.SetMonster(); // 몬스터 상태 초기화
        party.LoadAndEquipCharacters();
    }

    // 전투 시작 함수
    public void SetBattle(bool active)
    {
        SetScroll(!active); // 배틀 상태에 따라 스크롤 On / Off
        isBattle = active; // 배틀 상태 체크
    }

    /// <summary>
    /// ※ 밸런싱 ※ 업그레이드 스텟에 따른 스탯 가중치 코드
    /// </summary>
    public void UpdateState()
    {
        power = 10f + (BackendGameData.Instance.UserGameData.atkLv - 1) * 2;            // 공격력
        attackSpeed = 1f - (BackendGameData.Instance.UserGameData.atkLv - 1) * 0.05f;   // 공격속도
        scrollSpeed = 5f + (BackendGameData.Instance.UserGameData.dexLv - 1);           // 스크롤링 속도 ≒ 이동속도
    }

    public void SetScroll(bool active) {
        isScroll = active; // 스크롤 상태 체크
        playerAnimator.SetBool("Scroll", active); // 플레이어 달리기 애니메이션 시작
        background.speed = scrollSpeed * Convert.ToInt32(active);           // 땅 스크롤링
        background.backSpeed = backSpeed * Convert.ToInt32(active);         // 배경 스크롤링
        background.terrainSpeed = terrainSpeed * Convert.ToInt32(active);   // 지형 스크롤링
    }
    void SetScrollSpeed()
    {
        backSpeed = scrollSpeed / 20f;  // 배경 스크롤링 속도
        terrainSpeed = scrollSpeed / 2f; // 지형 스크롤링 속도
    }

    public void EightBitURL()
    {
        // 8비트 공식 홈페이지 이동
        Application.OpenURL(webURL);
    }
}
