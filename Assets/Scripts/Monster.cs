using UnityEngine;
using UnityEngine.UI;

public class Monster : MonoBehaviour
{
    GameManager GameManager => GameManager.Instance;

    Image hpBar;    // HP바 오브젝트
    Animator anim;  // 몬스터 애니메이터

    public float maxHp;     // 최대 HP
    public float hp;        // 현재 HP
    public bool isLive;     // 몬스터의 생존 여부
    float speed;            // 몬스터의 속도

    private void Start()
    {
        // 오브젝트 할당
        hpBar = GameObject.Find("HP").GetComponent<Image>();
        anim = GetComponent<Animator>();

        hp = maxHp; // HP 초기화
        isLive = true;
        speed = GameManager.scrollSpeed;
    }

    void Update()
    {
        // 현재 HP를 HP바에 반영
        hpBar.fillAmount = hp / maxHp;

        if (GameManager.isScroll)
        {
            // 스크롤링 중이라면 왼쪽으로 몬스터가 이동
            transform.Translate(Vector2.left * speed * Time.deltaTime);
        }

        if (isLive && hp <= 0f)
        {
            // 체력이 0이 될 경우 사망
            anim.SetTrigger("Dead");
            transform.GetChild(0).gameObject.SetActive(false); // HP바 비활성화
            isLive = false;
        }

        if (isLive && transform.position.x < 1f)
        {
            // x좌표가 1까지 왔다면, 전투 시작
            GameManager.SetBattle(true);
        }

        if (transform.position.x <= -15f)
        {
            // x좌표가 -15까지 갔다면, 리스폰
            hp = maxHp;
            transform.position = new Vector3(15, -2, 0);
            transform.GetChild(0).gameObject.SetActive(true); // HP바 활성화
            isLive = true;
            anim.SetTrigger("Respawn");
        }
    }
}
