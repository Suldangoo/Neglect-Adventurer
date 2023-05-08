using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Animator playerAnimator;
    private Animator enemyAnimator;

    float currTime;
    bool battle = false;

    void Start()
    {
        // 플레이어 오브젝트에서 Animator 컴포넌트 가져오기
        playerAnimator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (battle)
        {
            currTime += Time.deltaTime;
            if (currTime >= 0.4f)
            {
                playerAnimator.SetBool("Battle", false);
                currTime = 0;
                battle = false;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 충돌한 오브젝트의 태그가 "Monster"인지 확인
        if (collision.CompareTag("Monster"))
        {
            // 적 오브젝트에서 Animator 컴포넌트 가져오기
            enemyAnimator = collision.GetComponent<Animator>();

            // 플레이어 애니메이터의 Battle 파라미터를 true로 설정
            playerAnimator.SetBool("Battle", true);

            // 적 애니메이터의 Dead 파라미터를 true로 설정
            enemyAnimator.SetBool("Dead", true);

            battle = true;
        }
    }
}
