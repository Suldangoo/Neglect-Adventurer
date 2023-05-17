using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Monster : MonoBehaviour
{
    // ���Ͱ� �����Ǿ��ٸ� �������� ��ũ�Ѹ��Ǹ� �̵��ϴ� ��ũ��Ʈ
    // ���� ������ ������ ������ �����ϸ�, �� �ٱ����� �Ѿ�� �����θ� �ı��ϵ��� �Ѵ�.

    public float maxHp; // �ִ� HP
    public float hp;    // ���� HP

    GameObject gameManager; // ���ӸŴ��� ������Ʈ
    GameObject hpBar;       // HP�� ������Ʈ

    Animator anim; // ���� �ִϸ�����

    private void Start()
    {
        hp = maxHp; // HP �ʱ�ȭ

        // ������Ʈ �Ҵ�
        gameManager = GameObject.Find("GameManager");
        hpBar = GameObject.Find("HP");
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        // ���� HP�� HP�ٿ� �ݿ�
        hpBar.GetComponent<Image>().fillAmount = hp / maxHp;

        if (hp <= 0f)
        {
            // ü���� 0�� �� ��� ���
            anim.SetTrigger("Dead");
            transform.GetChild(0).gameObject.SetActive(false); // HP�� ��Ȱ��ȭ
            gameManager.GetComponent<GameManager>().EndBattle();
        }

        if (gameManager.GetComponent<GameManager>().isScroll)
        {
            // ��ũ�Ѹ� ���̶�� �������� ���Ͱ� �̵�
            this.transform.Translate(Vector3.left * gameManager.GetComponent<GameManager>().scrollSpeed * Time.deltaTime);
        }

        if (this.transform.position.x <= 1f)
        {
            // x��ǥ�� 1���� �Դٸ�, ���� ����
            gameManager.GetComponent<GameManager>().StartBattle();
        }

        if (this.transform.position.x <= -15f)
        {
            // x��ǥ�� -15���� ���ٸ�, ������Ʈ �ı�
            Destroy(gameObject);
        }
    }
}
