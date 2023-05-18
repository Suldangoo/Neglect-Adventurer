using UnityEngine;

public class InfiniteScrolling : MonoBehaviour
{
    [SerializeField] protected GameObject[] backObjects; // 배경 오브젝트들
    [SerializeField] protected GameObject[] terrainObjects; // 지형 오브젝트들
    [SerializeField] protected GameObject[] groundsObjects; // 땅 오브젝트들

    public float speed; // 땅 오브젝트 스크롤 속도
    public float backSpeed; // 배경 오브젝트 스크롤 속도
    public float terrainSpeed; // 지형 오브젝트 스크롤 속도
    
    public float size; // 땅 오브젝트 크기
    public float backSize; // 배경 오브젝트 크기
    public float terrainSize; // 지형 오브젝트 크기

    private void Start()
    {
        speed = 0f;
        backSpeed = 0f;
        terrainSpeed = 0f;

        size = 26f;
        backSize = 30f;
        terrainSize = 11f;
    }

    private void Update()
    {
        // 땅 오브젝트 스크롤링
        foreach (GameObject obj in groundsObjects)
        {
            obj.transform.Translate(Vector3.left * speed * Time.deltaTime);

            // 땅 오브젝트가 맨 오른쪽 끝으로 갔을 때 위치를 초기화하고 한 칸 오른쪽으로 이동
            if (obj.transform.position.x <= -size)
            {
                Vector2 offset = new Vector2(size * 2, 0f);
                obj.transform.position = (Vector2)obj.transform.position + offset;
            }
        }

        // 배경 오브젝트 스크롤링
        foreach (GameObject back in backObjects)
        {
            back.transform.Translate(Vector3.left * backSpeed * Time.deltaTime);

            // 배경 오브젝트가 맨 오른쪽 끝으로 갔을 때 위치를 초기화하고 한 칸 오른쪽으로 이동
            if (back.transform.position.x <= -backSize)
            {
                Vector2 offset = new Vector2(backSize * 2, 0f);
                back.transform.position = (Vector2)back.transform.position + offset;
            }
        }

        // 지형 오브젝트 스크롤링
        foreach (GameObject terrain in terrainObjects)
        {
            terrain.transform.Translate(Vector3.left * terrainSpeed * Time.deltaTime);
            
            // 지형 오브젝트가 맨 오른쪽 끝으로 갔을 때 위치를 초기화하고 한 칸 오른쪽으로 이동
            if (terrain.transform.position.x <= -terrainSize * 2)
            {
                Vector2 offset = new Vector2(terrainSize * 4, 0f);
                terrain.transform.position = (Vector2)terrain.transform.position + offset;
            }
        }
    }
}