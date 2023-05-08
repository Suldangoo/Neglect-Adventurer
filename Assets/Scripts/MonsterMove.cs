using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterMove : MonoBehaviour
{
    // 몬스터가 생성되었다면 왼쪽으로 스크롤링되며 이동하는 스크립트

    public float speed;

    void Update()
    {
        this.transform.Translate(Vector3.left * speed * Time.deltaTime);

        if (this.transform.position.x <= -15f) // 맵 밖으로 벗어나면 자기 자신 파괴
        {
            Destroy(gameObject);
        }
    }
}
