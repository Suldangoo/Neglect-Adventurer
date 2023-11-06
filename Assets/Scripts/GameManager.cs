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

    [SerializeField] DungeonManager background;   // 백그라운드 스크롤링 오브젝트
    [SerializeField] Animator playerAnimator;        // 플레이어 캐릭터 애니메이터
    [SerializeField] Monster monster; // 몬스터 프리팹
    [SerializeField] HpUI heart;

    [SerializeField] TextMeshProUGUI[] goldTexts; // 골드 텍스트들
    [SerializeField] TextMeshProUGUI[] diamondTexts; // 다이아 텍스트들

    [SerializeField] string webURL;
    [SerializeField] public Party party;
    [SerializeField] public Quest quest;
    [SerializeField] private Image[] heartImages; // 하트 이미지 5개
    [SerializeField] private TextMeshProUGUI ReviveCount; // 사망 카운트

    // --- 게임 변수
    public bool isStart = false;   // 시작 확인
    public bool isScroll = false;  // 스크롤 확인
    public bool isBattle = false;  // 전투 확인
    public bool isDead = false;  // 전투 확인

    private Coroutine healerRecoveryRoutine;
    private Coroutine battleRoutine;

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

        SetScroll(false); // 스크롤 끄기
    }

    void Update()
    {
        if (!isDead && !isBattle) // 달리고 있는 상태라면
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

    // 로그인 후 게임 시작
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

        if (active && battleRoutine == null)
        {
            battleRoutine = StartCoroutine(BattleRoutine()); // 배틀 상태라면, 배틀 코루틴 시작
        }
        else if (!active && battleRoutine != null)
        {
            StopCoroutine(battleRoutine); // 배틀 상태가 아니라면, 배틀 코루틴 종료
            battleRoutine = null;
        }
    }

    // 사망 시 부활 카운트다운 코루틴
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

    // 플레이어 사망 메서드
    public void PlayerDead()
    {
        playerAnimator.SetTrigger("Dead"); // 사망 애니메이션 켜기
        SoundManager.Instance.PlaySound("dead"); // 사운드 재생
        isDead = true; // 사망상태 켜기
        isBattle = false; // 배틀 끄기
        SetScroll(false); // 스크롤 중지
        monster.RunAway(); // 몬스터 도망 연출
        UiManager.SetDeadUi(true); // 사망 UI 켜기

        if (healerRecoveryRoutine != null)
        {
            StopCoroutine(healerRecoveryRoutine); // 힐러 코루틴 종료
        }

        StopCoroutine(battleRoutine); // 배틀 코루틴 종료
        battleRoutine = null;

        StartCoroutine(ReviveCountdown()); // 부활 카운트 다운 시작
    }

    // 플레이어 부활 메서드
    public void PlayerRevive()
    {
        playerAnimator.SetTrigger("Revive"); // 원래 애니메이션으로 돌리기
        SoundManager.Instance.PlaySound("revive"); // 사운드 재생
        isDead = false; // 사망상태 끄기
        SetScroll(true); // 스크롤 켜기
        heart.SetHp(5); // HP 회복
        UpdateHealerRecoveryRoutine(); // 힐러 코루틴 시작
        UiManager.SetDeadUi(false); // 부활 UI 켜기
    }

    // 전투 코루틴
    IEnumerator BattleRoutine()
    {
        while (true)
        {
            // 시작 부분에 대기
            yield return new WaitForSeconds(attackSpeed);

            if (monster.isLive)
            {
                // 내 데미지 계산. 데미지 = 내 공격력 + 동료 공격력
                float dam = power + party.EquippedCharacterStats("attack");

                playerAnimator.SetTrigger("Attack"); // 공격 애니메이션 재생
                SoundManager.Instance.PlaySound("attack"); // 사운드 재생
                monster.Damage(dam); // 몬스터에게 데미지
            }
            else
            {
                SetBattle(false);
            }
        }
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
            SoundManager.Instance.PlaySound("heal"); // 사운드 재생
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
        attackSpeed = 1f - (BackendGameData.Instance.UserGameData.atkLv - 1) * 0.005f;   // 공격속도
        defense = 0f + (BackendGameData.Instance.UserGameData.defLv - 1) * 2;           // 방어력
        scrollSpeed = 5f + (BackendGameData.Instance.UserGameData.dexLv - 1) * 0.3f;    // 스크롤링 속도 ≒ 이동속도
    }

    // 스크롤 켜고 끄는 메서드
    public void SetScroll(bool active) {
        isScroll = active; // 스크롤 상태 체크
        playerAnimator.SetBool("Scroll", active); // 플레이어 달리기 애니메이션 시작
        background.speed = scrollSpeed * Convert.ToInt32(active);           // 땅 스크롤링
    }

    // 재화 새로고침
    public void RefreshCurrency()
    {
        // 골드 텍스트 갱신
        foreach (TextMeshProUGUI goldText in goldTexts)
        {
            goldText.text = BackendGameData.Instance.UserGameData.gold.ToString("N0");
        }

        // 다이아 텍스트 갱신
        foreach (TextMeshProUGUI diamondText in diamondTexts)
        {
            diamondText.text = BackendGameData.Instance.UserGameData.diamond.ToString("N0");
        }
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
