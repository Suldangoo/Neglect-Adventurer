using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterMove : MonoBehaviour
{
    // ���Ͱ� �����Ǿ��ٸ� �������� ��ũ�Ѹ��Ǹ� �̵��ϴ� ��ũ��Ʈ

    public float speed;

    void Update()
    {
        this.transform.Translate(Vector3.left * speed * Time.deltaTime);

        if (this.transform.position.x <= -15f) // �� ������ ����� �ڱ� �ڽ� �ı�
        {
            Destroy(gameObject);
        }
    }
}
