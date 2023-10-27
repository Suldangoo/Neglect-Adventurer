using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 몬스터 정보 클래스
[System.Serializable]
public class MonsterData
{
    public string name;                  // 몬스터 이름 (예: "Goblin", "Dragon" 등)
    public float maxHp;                  // 최대 HP
    public int rewardGold;               // 기본 보상 골드
    public float power;                  // 공격력
    public AnimationClip idleAnimation;  // Idle 애니메이션 클립
    public AnimationClip attackAnimation;// Attack 애니메이션 클립
    public AnimationClip deathAnimation; // Death 애니메이션 클립
    public float positionOffset;         // 위치 보정용 변수
    public float hpBarOffsetX;           // HP바 x축 위치 보정용 변수
    public float hpBarOffsetY;           // HP바 y축 위치 보정용 변수
}

// 던전 관리 클래스
[System.Serializable]
public class Dungeon
{
    public string dungeonName;                // 던전 이름
    public List<MonsterData> monstersInDungeon;   // 해당 던전에 등장하는 몬스터 목록
}

public class Monster : MonoBehaviour
{
    GameManager GameManager => GameManager.Instance;

    [SerializeField] GameObject hpBar; // hp바 오브젝트
    [SerializeField] GameObject dmgText; // 데미지 텍스트 프리팹
    [SerializeField] GameObject goldDrop; // 골드 드랍 이펙트 프리팹
    [SerializeField] Canvas canvas; // 몬스터의 하위 캔버스

    public Player player;  // 플레이어
    public HpUI heart;     // 플레이어의 Hp
    public Image hpColor;  // HP바 오브젝트
    public Quest quest;   // 퀘스트 UI

    public float hp;        // 현재 HP
    public bool isLive;     // 몬스터의 생존 여부
    public bool isBattle;   // 몬스터의 배틀 여부
    float speed;            // 몬스터의 속도
    float hitDuration;      // 피격 효과 지속시간

    Animator anim;  // 몬스터 애니메이터
    SpriteRenderer sprite; // 스프라이트 렌더러
    ParticleSystem particle; // 파티클

    public List<MonsterData> dungeonMonsters; // 현재 등장 가능한 몬스터 리스트
    private MonsterData currentMonsterData;   // 현재 몬스터 데이터

    public List<Dungeon> dungeons;    // 전체 던전 목록
    private Dungeon currentDungeon;   // 현재 던전

    private void Start()
    {
        // 초기 던전 설정 (예: 첫 번째 던전으로 설정)
        SetCurrentDungeon(0);

        // 오브젝트 할당
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        particle = GetComponent<ParticleSystem>();

        StartCoroutine("HitEffect");

        SetRandomMonster(); // 게임 시작 시 랜덤 몬스터 설정
    }

    void Update()
    {
        // 스크롤링 중이라면 왼쪽으로 몬스터가 이동
        if (GameManager.isScroll)
        {
            transform.Translate(Vector2.left * speed * Time.deltaTime);
        }

        if (isLive && hp <= 0f)
        {
            // 체력이 0이 될 경우 사망
            Death();
        }

        // x좌표가 1까지 왔다면, 전투 시작
        if (isLive && transform.position.x < 1f && !isBattle)
        {
            StartCoroutine("Attacker");
            GameManager.SetBattle(true);
            isBattle = true;
        }

        // x좌표가 -15까지 갔다면, 리스폰
        if (transform.position.x <= -15f)
        {
            anim.SetTrigger("Respawn"); // 애니메이터 컨트롤
            SetMonster(); // 몬스터 리스폰
        }
    }

    // 던전 변경 메서드
    public void SetCurrentDungeon(int dungeonIndex)
    {
        if (dungeonIndex >= 0 && dungeonIndex < dungeons.Count)
        {
            currentDungeon = dungeons[dungeonIndex];
            // 현재 던전을 바꾸면 몬스터 목록도 바꿔줍니다.
            dungeonMonsters = currentDungeon.monstersInDungeon;
        }
        else
        {
            Debug.LogError("Invalid dungeon index!");
        }
    }

    public void SetRandomMonster()
    {
        int randomIndex = Random.Range(0, dungeonMonsters.Count); // 랜덤 인덱스 생성
        currentMonsterData = dungeonMonsters[randomIndex];       // 랜덤 몬스터 데이터 선택

        hp = currentMonsterData.maxHp;  // 최대 HP 설정
        hpColor.fillAmount = 1; // HP 바 이미지 채우기

        transform.position = new Vector3(Random.Range(15f, 30f), -3 + currentMonsterData.positionOffset, 0); // 몬스터 위치 랜덤하게 초기화 후 위치 보정
        canvas.GetComponent<RectTransform>().anchoredPosition = new Vector2(currentMonsterData.hpBarOffsetX, currentMonsterData.hpBarOffsetY);

        // 애니메이션 클립 설정
        AnimatorOverrideController overrideController = new AnimatorOverrideController(anim.runtimeAnimatorController);
        overrideController["Idle"] = currentMonsterData.idleAnimation;
        overrideController["Attack"] = currentMonsterData.attackAnimation;
        overrideController["Death"] = currentMonsterData.deathAnimation;
        anim.runtimeAnimatorController = overrideController;
    }

    public void SetMonster()
    {
        SetRandomMonster(); // 리스폰할 때마다 랜덤 몬스터로 변경

        hpBar.SetActive(true); // HP바 활성화
        isLive = true; // 생존 상태 체크
        isBattle = false; // 배틀 상태 체크 해제
        speed = GameManager.scrollSpeed; // 현재 스크롤 속도 반영
    }

    public void Attack()
    {
        anim.SetTrigger("Attack");
        SoundManager.Instance.PlaySound("monsterattack"); // 사운드 재생

        // 몬스터의 데미지 계산. 데미지 = 몬스터 공격력 - (내 방어력 + 동료 방어력) / 100
        float dam = currentMonsterData.power - (GameManager.defense + GameManager.party.EquippedCharacterStats("defense")) / 100f;
        dam = Mathf.Max(dam, 0);  // 만약 데미지가 음수라면 0으로 설정

        heart.SetHp(-dam);
        Debug.Log($"몬스터의 공격! {dam} 만큼의 피해를 입었다!");

        player.Damage();
    }

    public void Damage(float atk)
    {
        hp -= atk; // atk의 피해만큼 데미지를 입음
        hpColor.fillAmount = hp / currentMonsterData.maxHp; // 현재 HP를 HP바에 반영
        hitDuration = Time.time + 0.1f; // 히트 이펙트 반영시간을 0.1초 이후로 갱신
        particle.Clear(); // 현재 파티클 초기화
        particle.Play(); // 파티클 재생

        GameObject damageText = Instantiate(dmgText, canvas.transform); // 데미지 프리팹
        damageText.GetComponent<Text>().text = atk.ToString();
    }

    public void Death()
    {
        anim.SetTrigger("Dead");
        SoundManager.Instance.PlaySound("monsterdie"); // 사운드 재생
        hpBar.SetActive(false); // HP바 비활성화
        isLive = false; // 생존 상태 체크 해제
        isBattle = false; // 배틀 상태 체크 해제

        GameObject goldEffect = Instantiate(goldDrop, canvas.transform); // 골드 드랍 이펙트 프리팹 생성

        // 먼저 몬스터 고유의 보상값에서 플러스 마이너스 20%의 랜덤한 값을 계산합니다.
        float randomFactor = Random.Range(0.8f, 1.2f); // 80%(0.8)부터 120%(1.2)까지 랜덤한 값을 가져옵니다.
        int randomReward = Mathf.RoundToInt(currentMonsterData.rewardGold * randomFactor); // 랜덤한 요소를 반영하여 보상 값을 계산합니다.

        // 그런 다음 '운' 스탯에 따라 추가 보상을 계산합니다.
        int totalReward = randomReward + (randomReward / 100) * (BackendGameData.Instance.UserGameData.lukLv - 1);

        // 최종 보상을 유저에게 지급합니다.
        BackendGameData.Instance.UserGameData.gold += totalReward;
        BackendGameData.Instance.GameDataUpdate(); // 골드 지급 반영

        // 퀘스트 업데이트
        if (quest.GetCurrentQuestTypeAsString().Equals("KillMonsters"))
        {
            quest.UpQuestValue(1);
        }
        else if (quest.GetCurrentQuestTypeAsString().Equals("CollectGold"))
        {
            quest.UpQuestValue(totalReward);
        }

        speed = GameManager.scrollSpeed; // 현재 스크롤 속도 반영
        StopCoroutine("Attacker");
    }

    public void RunAway()
    {
        StopCoroutine("Attacker");  // 공격 코루틴 멈춤
        StartCoroutine(RunAwayCoroutine());  // 몬스터와 hpBar가 사라지는 코루틴 시작
    }

    private IEnumerator RunAwayCoroutine()
    {
        yield return new WaitForSeconds(3f);  // 3초 대기

        float fadeDuration = 2f;
        float initialTime = Time.time;

        // 몬스터와 hpBar가 천천히 사라지도록 함
        while (Time.time - initialTime < fadeDuration)
        {
            float t = (Time.time - initialTime) / fadeDuration;
            float alpha = 1 - t;

            if (GetComponent<SpriteRenderer>())
                GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, alpha);

            if (hpBar.GetComponent<Image>())
                hpBar.GetComponent<Image>().color = new Color(1, 1, 1, alpha);

            yield return null;
        }

        SetMonster();  // 새로운 몬스터 설정

        // 몬스터와 hpBar의 투명도 상태를 다시 정상으로 복구
        if (GetComponent<SpriteRenderer>())
            GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);

        if (hpBar.GetComponent<Image>())
            hpBar.GetComponent<Image>().color = new Color(1, 1, 1, 1);
    }

    IEnumerator Attacker()
    {
        while (true)
        {
            Attack();
            yield return new WaitForSeconds(3.0f);
        }
    }

    IEnumerator HitEffect()
    {
        // 코루틴으로 히트 이펙트 지속
        while (true)
        {
            // 히트 이펙트 반영시간까지 red 컬러 곱하기
            if (Time.time < hitDuration)
                sprite.color = Color.red;
            // 히트 이펙트 반영시간에 도달하면 white 컬러로 돌아오기
            else
                sprite.color = Color.white;

            yield return null;
        }
    }
}
