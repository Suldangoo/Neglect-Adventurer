using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterMove : MonoBehaviour
{
    // 몬스터가 생성되었다면 왼쪽으로 스크롤링되며 이동하는 스크립트
    // 전투 영역에 들어오면 전투를 시작하며, 맵 바깥으로 넘어가면 스스로를 파괴하도록 한다.

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

        if (this.transform.position.x <= -15f) // 맵 밖으로 벗어나면 자기 자신 파괴
        {
            Destroy(gameObject);
        }
    }
}
