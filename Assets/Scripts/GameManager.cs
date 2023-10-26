using System;
using System.Collections;
using TMPro;
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
    [SerializeField] String webURL;
    [SerializeField] HpUI heart;
    [SerializeField] public Party party;
    [SerializeField] public Quest quest;
    [SerializeField] private Image[] heartImages; // 하트 이미지 5개
    [SerializeField] private TextMeshProUGUI ReviveCount; // 사망 카운트

    // --- 게임 변수
    [HideInInspector] public bool isStart = false;   // 시작 확인
    [HideInInspector] public bool isScroll = false;  // 스크롤 확인
    [HideInInspector] public bool isBattle = false;  // 전투 확인
    [HideInInspector] public bool isDead = false;  // 전투 확인

    float currTime;             // 시간을 측정할 변수
    float backSpeed;            // 배경 스크롤링 속도
    float terrainSpeed;         // 지형 스크롤링 속도
    private Coroutine healerRecoveryRoutine;

    // --- 플레이어 변수

    public float power;         // 공격력
    public float defense;       // 방어력
    public float heal;          // 회복력
    public float healtime;      // 회복시간
    public float attackSpeed;   // 공격속도
    public float scrollSpeed;   // 이동속도 / 스크롤링 속도
    private float accumulatedDistance = 0f; // 이동 퀘스트를 위한 달린거리 계산

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
        if (isBattle) // 전투 상태라면
        {
            // 공격 속도 간격을 위한 시간 측정
            currTime += Time.deltaTime;

            if (currTime >= attackSpeed) // 공격 속도의 간격이 되었다면
            {
                if (monster.isLive)
                {
                    // 내 데미지 계산. 데미지 = 내 공격력 + 동료 공격력
                    float dam = power + party.EquippedCharacterStats("attack");

                    playerAnimator.SetTrigger("Attack"); // 공격 애니메이션 재생
                    monster.Damage(dam); // 몬스터에게 데미지
                    currTime = 0; // 시간 초기화
                }
                else
                {
                    SetBattle(false);
                    currTime = 0; // 시간 초기화
                }
            }
        }
        else if (!isDead) // 달리고 있는 상태라면
        {
            if (quest.GetCurrentQuestTypeAsString().Equals("RunMeters"))
            {
                accumulatedDistance += scrollSpeed * Time.deltaTime;

                if (accumulatedDistance >= 1)
                {
                    quest.UpQuestValue((int)accumulatedDistance);
                    accumulatedDistance -= (int)accumulatedDistance;
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
        party.LoadAndEquipCharacters(); // 저장되어있는 장착 캐릭터들 불러오기
        UpdateHealerRecoveryRoutine(); // 힐러 코루틴 시작
    }

    // 전투 시작 함수
    public void SetBattle(bool active)
    {
        SetScroll(!active); // 배틀 상태에 따라 스크롤 On / Off
        isBattle = active; // 배틀 상태 체크
    }

    private IEnumerator ReviveCountdown()
    {
        int timeLeft = 10;
        while (timeLeft > 0)
        {
            ReviveCount.text = timeLeft.ToString(); // 카운트 다운 텍스트 업데이트
            yield return new WaitForSeconds(1); // 1초 대기
            timeLeft--; // 카운트 다운
        }
        PlayerRevive(); // 카운트 다운이 끝나면 부활 메소드 호출
    }

    public void PlayerDead()
    {
        playerAnimator.SetTrigger("Dead"); // 사망 애니메이션 켜기
        isDead = true; // 사망상태 켜기
        isBattle = false; // 배틀 끄기
        SetScroll(false); // 스크롤 중지
        monster.RunAway(); // 몬스터 도망 연출
        UiManager.SetDeadUi(true);

        StartCoroutine(ReviveCountdown()); // 카운트 다운 시작
    }

    public void PlayerRevive()
    {
        playerAnimator.SetTrigger("Revive"); // 원래 애니메이션으로 돌리기
        isDead = false; // 사망상태 끄기
        SetScroll(true); // 스크롤 켜기
        heart.SetHp(5); // HP 회복
        UiManager.SetDeadUi(false);
    }

    // 힐러 코루틴의 상태를 업데이트하는 메서드
    public void UpdateHealerRecoveryRoutine()
    {
        if (party.isEquippedHealer())
        {
            if (healerRecoveryRoutine == null)
            {
                healerRecoveryRoutine = StartCoroutine(HealerRecoveryRoutine());
            }
        }
        else
        {
            if (healerRecoveryRoutine != null)
            {
                StopCoroutine(healerRecoveryRoutine);
                healerRecoveryRoutine = null;
            }
        }
    }

    // 회복 코루틴
    private IEnumerator HealerRecoveryRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(party.EquippedCharacterStats("recoverytime"));
            heart.SetHp(party.EquippedCharacterStats("recovery"));
            PlayRecoveryAnimation();
        }
    }

    // 하트에 회복 연출 실행
    public void PlayRecoveryAnimation()
    {
        foreach (Image heartImage in heartImages)
        {
            StartCoroutine(FadeFromGreen(heartImage));
        }
    }

    // 초록색에서 원래 색상으로 부드럽게 바꾸는 코루틴
    private IEnumerator FadeFromGreen(Image heartImage)
    {
        Color originalColor = Color.white;
        Color greenColor = new Color(0.6f, 1f, 0.486f); // 99FF7C
        float duration = 1.0f;

        // 바로 초록색으로 설정
        heartImage.color = greenColor;

        float elapsed = 0;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float normalizedTime = elapsed / duration;

            // 초록색에서 원래 색상으로 부드럽게 바뀌게 설정
            heartImage.color = Color.Lerp(greenColor, originalColor, normalizedTime);

            yield return null;
        }

        heartImage.color = originalColor; // 마지막으로 원래 색상으로 확실히 설정
    }

    /// <summary>
    /// ※ 밸런싱 ※ 업그레이드 스텟에 따른 스탯 가중치 코드
    /// </summary>
    public void UpdateState()
    {
        power = 10f + (BackendGameData.Instance.UserGameData.atkLv - 1) * 2;            // 공격력
        attackSpeed = 1f - (BackendGameData.Instance.UserGameData.atkLv - 1) * 0.05f;   // 공격속도
        defense = 0f + (BackendGameData.Instance.UserGameData.defLv - 1) * 2;           // 방어력
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

    // 8비트 홈페이지 이동
    public void EightBitURL()
    {
        // 8비트 공식 홈페이지 이동
        Application.OpenURL(webURL);
    }

    // 게임 종료
    public void ExitGame()
    {
        Application.Quit();
    }
}
