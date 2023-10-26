using UnityEngine;

public class DungeonManager : MonoBehaviour
{
    public InfiniteScrolling[] dungeons; // 던전 맵 배열
    private int currentDungeonIndex = 0; // 현재 던전의 번호

    public float speed; // 수시로 변하는 스크롤링 속도

    // 시작 시 현재 던전으로 던전 선택
    private void Start()
    {
        for (int i = 0; i < dungeons.Length; i++)
        {
            dungeons[i].gameObject.SetActive(i == currentDungeonIndex);
        }
    }

    // 던전 변경 메서드
    public void SetDungeon(int dungeonIndex)
    {
        if (dungeonIndex < 0 || dungeonIndex >= dungeons.Length)
            return;

        dungeons[currentDungeonIndex].gameObject.SetActive(false);
        currentDungeonIndex = dungeonIndex;
        dungeons[currentDungeonIndex].gameObject.SetActive(true);
    }
}