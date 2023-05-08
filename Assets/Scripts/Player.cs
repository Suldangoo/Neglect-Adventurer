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
        // �÷��̾� ������Ʈ���� Animator ������Ʈ ��������
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
        // �浹�� ������Ʈ�� �±װ� "Monster"���� Ȯ��
        if (collision.CompareTag("Monster"))
        {
            // �� ������Ʈ���� Animator ������Ʈ ��������
            enemyAnimator = collision.GetComponent<Animator>();

            // �÷��̾� �ִϸ������� Battle �Ķ���͸� true�� ����
            playerAnimator.SetBool("Battle", true);

            // �� �ִϸ������� Dead �Ķ���͸� true�� ����
            enemyAnimator.SetBool("Dead", true);

            battle = true;
        }
    }
}
