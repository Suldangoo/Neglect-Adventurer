using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Monster : MonoBehaviour
{
    // 몬스터가 생성되었다면 왼쪽으로 스크롤링되며 이동하는 스크립트
    // 전투 영역에 들어오면 전투를 시작하며, 맵 바깥으로 넘어가면 스스로를 파괴하도록 한다.

    public float maxHp; // 최대 HP
    public float hp;    // 현재 HP

    GameObject gameManager; // 게임매니저 오브젝트
    GameObject hpBar;       // HP바 오브젝트

    Animator anim; // 몬스터 애니메이터

    private void Start()
    {
        hp = maxHp; // HP 초기화

        // 오브젝트 할당
        gameManager = GameObject.Find("GameManager");
        hpBar = GameObject.Find("HP");
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        // 현재 HP를 HP바에 반영
        hpBar.GetComponent<Image>().fillAmount = hp / maxHp;

        if (hp <= 0f)
        {
            // 체력이 0이 될 경우 사망
            anim.SetTrigger("Dead");
            transform.GetChild(0).gameObject.SetActive(false); // HP바 비활성화
            gameManager.GetComponent<GameManager>().EndBattle();
        }

        if (gameManager.GetComponent<GameManager>().isScroll)
        {
            // 스크롤링 중이라면 왼쪽으로 몬스터가 이동
            this.transform.Translate(Vector3.left * gameManager.GetComponent<GameManager>().scrollSpeed * Time.deltaTime);
        }

        if (this.transform.position.x <= 1f)
        {
            // x좌표가 1까지 왔다면, 전투 시작
            gameManager.GetComponent<GameManager>().StartBattle();
        }

        if (this.transform.position.x <= -15f)
        {
            // x좌표가 -15까지 갔다면, 오브젝트 파괴
            Destroy(gameObject);
        }
    }
}
