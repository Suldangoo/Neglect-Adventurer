using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Monster : MonoBehaviour
{
    GameManager GameManager => GameManager.Instance;

    public Player player;  // 플레이어
    public HpUI heart;     // 플레이어의 Hp
    public Image hpColor;  // HP바 오브젝트
    public Quest quest;   // 퀘스트 UI

    Animator anim;  // 몬스터 애니메이터
    SpriteRenderer sprite; // 스프라이트 렌더러
    ParticleSystem particle; // 파티클

    [SerializeField] GameObject hpBar; // hp바 오브젝트
    [SerializeField] GameObject dmgText; // 데미지 텍스트 프리팹
    [SerializeField] GameObject goldDrop; // 골드 드랍 이펙트 프리팹
    [SerializeField] Canvas canvas; // 몬스터의 하위 캔버스

    public float maxHp;     // 최대 HP
    public float hp;        // 현재 HP
    public bool isLive;     // 몬스터의 생존 여부
    public bool isBattle;   // 몬스터의 배틀 여부

    int reward = 1000;      // 몬스터의 보상 골드
    float power = 0.5f;     // 몬스터의 공격력
    float speed;            // 몬스터의 속도
    float hitDuration;      // 피격 효과 지속시간

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
            SetMonster(); // 몬스터 리스폰
            transform.position = new Vector3(Random.Range(15f, 30f), -3, 0); // 몬스터 위치 랜덤하게 초기화
            hpBar.SetActive(true); // HP바 활성화
            anim.SetTrigger("Respawn"); // 애니메이터 컨트롤
        }
    }

    private void Start()
    {
        // 오브젝트 할당
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        particle = GetComponent<ParticleSystem>();

        StartCoroutine("HitEffect");
    }

    public void SetMonster()
    {
        isLive = true; // 생존 상태 체크
        isBattle = false; // 배틀 상태 체크 해제
        hp = maxHp; // HP 초기화
        hpColor.fillAmount = hp / maxHp; // 현재 HP를 HP바에 반영
        speed = GameManager.scrollSpeed; // 현재 스크롤 속도 반영
    }

    public void Attack()
    {
        anim.SetTrigger("Attack");

        // 몬스터의 데미지 계산. 데미지 = 몬스터 공격력 - (내 방어력 + 동료 방어력) / 100
        float dam = power - (GameManager.defense + GameManager.party.EquippedCharacterStats("defense")) / 100f;
        dam = Mathf.Max(dam, 0);  // 만약 데미지가 음수라면 0으로 설정

        heart.SetHp(-dam);
        Debug.Log($"몬스터의 공격! {dam} 만큼의 피해를 입었다!");

        player.Damage();
    }

    public void Damage(float atk)
    {
        hp -= atk; // atk의 피해만큼 데미지를 입음
        hpColor.fillAmount = hp / maxHp; // 현재 HP를 HP바에 반영
        hitDuration = Time.time + 0.1f; // 히트 이펙트 반영시간을 0.1초 이후로 갱신
        particle.Clear(); // 현재 파티클 초기화
        particle.Play(); // 파티클 재생
        GameObject damageText = Instantiate(dmgText, canvas.transform);
        damageText.GetComponent<Text>().text = atk.ToString();
    }

    public void Death()
    {
        anim.SetTrigger("Dead");
        hpBar.SetActive(false); // HP바 비활성화
        isLive = false; // 생존 상태 체크 해제
        isBattle = false; // 배틀 상태 체크 해제
        GameObject goldEffect = Instantiate(goldDrop, canvas.transform); // 골드 드랍 이펙트 프리팹 생성

        int goldReward = reward + (reward / 100) * (BackendGameData.Instance.UserGameData.lukLv - 1); // 보상 골드 설정
        BackendGameData.Instance.UserGameData.gold += goldReward; // 보상 골드 지급
        BackendGameData.Instance.GameDataUpdate(); // 골드 지급 반영

        // 퀘스트 업데이트
        if (quest.GetCurrentQuestTypeAsString().Equals("KillMonsters"))
        {
            quest.UpQuestValue(1);
        }
        else if (quest.GetCurrentQuestTypeAsString().Equals("CollectGold"))
        {
            quest.UpQuestValue(goldReward);
        }

        speed = GameManager.scrollSpeed; // 현재 스크롤 속도 반영
        StopCoroutine("Attacker");
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
