using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Monster : MonoBehaviour
{
    GameManager GameManager => GameManager.Instance;

    Image hpColor;  // HP바 오브젝트
    Animator anim;  // 몬스터 애니메이터
    SpriteRenderer sprite; // 스프라이트 렌더러
    ParticleSystem particle; // 파티클

    [SerializeField] GameObject hpBar; // hp바 오브젝트
    [SerializeField] GameObject hpSprite; // hp 스프라이트 오브젝트
    [SerializeField] GameObject dmgText; // 데미지 텍스트 프리팹
    [SerializeField] GameObject goldDrop; // 골드 드랍 이펙트 프리팹
    [SerializeField] Canvas canvas; // 몬스터의 하위 캔버스

    public float maxHp;     // 최대 HP
    public float hp;        // 현재 HP
    public bool isLive;     // 몬스터의 생존 여부

    int reward = 1000;      // 몬스터의 보상 골드
    float speed;            // 몬스터의 속도
    float hitDuration;      // 피격 효과 지속시간

    private void SetMonster()
    {
        isLive = true; // 생존 체크
        hp = maxHp; // HP 초기화
        hpColor.fillAmount = hp / maxHp; // 현재 HP를 HP바에 반영
        speed = GameManager.scrollSpeed; // 현재 스크롤 속도 반영
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
        isLive = false; // 생존 상태 False
        GameObject goldEffect = Instantiate(goldDrop, canvas.transform); // 골드 드랍 이펙트 프리팹 생성
        GameManager.gold += reward + (reward / 100) * (GameManager.lukLv - 1); // 보상 골드 지급
        speed = GameManager.scrollSpeed; // 현재 스크롤 속도 반영
    }

    private void Start()
    {
        // 오브젝트 할당
        hpColor = hpSprite.GetComponent<Image>();
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        particle = GetComponent<ParticleSystem>();

        SetMonster();
        StartCoroutine(HitEffect());
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
        if (isLive && transform.position.x < 1f)
        {
            GameManager.SetBattle(true);
        }

        // x좌표가 -15까지 갔다면, 리스폰
        if (transform.position.x <= -15f)
        {
            SetMonster(); // 몬스터 리스폰
            transform.position = new Vector3(15, -2, 0); // 위치 초기화
            hpBar.SetActive(true); // HP바 활성화
            anim.SetTrigger("Respawn"); // 애니메이터 컨트롤
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
