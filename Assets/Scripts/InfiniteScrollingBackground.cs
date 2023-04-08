using UnityEngine;

public class InfiniteScrollingBackground : MonoBehaviour
{
    public float scrollSpeed = 1.0f; // 배경 스크롤 속도
    public float tileSize = 18.0f; // 이미지 타일 크기

    private Vector3 startPosition;

    private void Start()
    {
        startPosition = transform.position;
    }

    private void Update()
    {
        float newPosition = Mathf.Repeat(Time.time * scrollSpeed, tileSize);
        transform.position = startPosition + Vector3.left * newPosition;

        // 첫 번째 이미지가 왼쪽으로 일정 수준 이동하면 두 번째 이미지를 첫 번째 이미지 오른쪽에 붙임
        if (transform.position.x < startPosition.x - tileSize)
        {
            transform.position = startPosition;
        }
    }
}
