using UnityEngine;

public class DamageText : MonoBehaviour
{
    public void DestroyEvent()
    {
        // 애니메이션 재생이 끝나면 오브젝트 파괴
        Destroy(gameObject);
    }
}