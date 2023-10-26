using UnityEngine;

public class InfiniteScrolling : MonoBehaviour
{
    public DungeonManager dungeonManager; // DungeonManager에 대한 참조 추가

    [SerializeField] protected GameObject[] backObjects;
    [SerializeField] protected GameObject[] terrainObjects1;
    [SerializeField] protected GameObject[] terrainObjects2;
    [SerializeField] protected GameObject[] groundsObjects;

    [HideInInspector] public float speed;

    public float size; // 땅 오브젝트 크기
    public float backSize; // 배경 오브젝트 크기
    public float terrainSize; // 지형 오브젝트 크기

    private void Start()
    {
        // 속도 0으로 초기화
        speed = 0f;
    }

    private void Update()
    {
        // DungeonManager로부터 speed 가져오기
        float speed = dungeonManager.speed;

        // 땅 오브젝트 스크롤링
        foreach (GameObject obj in groundsObjects)
        {
            ScrollObject(obj, speed, size);
        }
        // 배경 오브젝트 스크롤링
        foreach (GameObject back in backObjects)
        {
            ScrollObject(back, speed / 20f, backSize);
        }
        // 지형 오브젝트 스크롤링
        foreach (GameObject terrain in terrainObjects1)
        {
            ScrollObject(terrain, speed / 8f, terrainSize);
        }
        // 지형2 오브젝트 스크롤링
        foreach (GameObject terrain in terrainObjects2)
        {
            ScrollObject(terrain, speed / 2f, terrainSize);
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