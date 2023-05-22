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
        // 속도 0으로 초기화
        speed = 0f;
        backSpeed = 0f;
        terrainSpeed = 0f;

        // 땅, 배경, 지형 스프라이트의 크기
        size = 26f;
        backSize = 30f;
        terrainSize = 11f;
    }

    private void Update()
    {
        // 땅 오브젝트 스크롤링
        foreach (GameObject obj in groundsObjects)
        {
            ScrollObject(obj, speed, size);
        }
        // 배경 오브젝트 스크롤링
        foreach (GameObject back in backObjects)
        {
            ScrollObject(back, backSpeed, backSize);
        }
        // 지형 오브젝트 스크롤링
        foreach (GameObject terrain in terrainObjects)
        {
            ScrollObject(terrain, terrainSpeed, terrainSize * 2);
        }
    }

    private void ScrollObject(GameObject obj, float scrollSpeed, float objSize)
    {
        // 오브젝트 스크롤링
        obj.transform.Translate(Vector3.left * scrollSpeed * Time.deltaTime);

        // 오브젝트가 맨 오른쪽 끝으로 갔을 때 위치를 초기화하고 한 칸 오른쪽으로 이동
        if (obj.transform.position.x <= -objSize)
        {
            Vector2 offset = new Vector2(objSize * 2, 0f);
            obj.transform.position = (Vector2)obj.transform.position + offset;
        }
    }
}