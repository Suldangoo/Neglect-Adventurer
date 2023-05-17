using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterMove : MonoBehaviour
{
    // ���Ͱ� �����Ǿ��ٸ� �������� ��ũ�Ѹ��Ǹ� �̵��ϴ� ��ũ��Ʈ
    // ���� ������ ������ ������ �����ϸ�, �� �ٱ����� �Ѿ�� �����θ� �ı��ϵ��� �Ѵ�.

    public float speed;
    GameObject gameManager;

    private void Start()
    {
        gameManager = GameObject.Find("GameManager");
    }

    void Update()
    {
        if (gameManager.GetComponent<GameManager>().isScroll)
        {
            this.transform.Translate(Vector3.left * speed * Time.deltaTime);
        }

        if (this.transform.position.x <= 1f)
        {
            gameManager.GetComponent<GameManager>().StartBattle();
        }

        if (this.transform.position.x <= -15f) // �� ������ ����� �ڱ� �ڽ� �ı�
        {
            Destroy(gameObject);
        }
    }
}
