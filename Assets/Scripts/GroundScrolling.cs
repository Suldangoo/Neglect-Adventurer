using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundScrolling : MonoBehaviour
{
    [SerializeField] protected GameObject[] objects; // 땅 오브젝트들

    public float speed; // 땅 오브젝트 스크롤 속도
    public float size; // 땅 오브젝트 크기

    private void Update()
    {
        // 배경 오브젝트 스크롤링
        foreach (GameObject obj in objects)
        {
            obj.transform.Translate(Vector3.left * speed * Time.deltaTime);

            // 배경 오브젝트가 맨 오른쪽 끝으로 갔을 때 위치를 초기화하고 한 칸 오른쪽으로 이동
            if (obj.transform.position.x <= -size)
            {
                Vector2 offset = new Vector2(size * 2, 0f);
                obj.transform.position = (Vector2)obj.transform.position + offset;
            }
        }
    }
}
