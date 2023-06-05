using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    SpriteRenderer sprite; // 스프라이트 렌더러
    float hitDuration;      // 피격 효과 지속시간

    void Start()
    {
        // 오브젝트 할당
        sprite = GetComponent<SpriteRenderer>();

        StartCoroutine("HitEffect");
    }

    void Update()
    {
        
    }

    IEnumerator HitEffect()
    {
        // 코루틴으로 히트 이펙트 지속
        while (true)
        {
            // 히트 이펙트 반영시간까지 red 컬러 곱하기
            if (Time.time < hitDuration)
                sprite.color = Color.red;
            // 히트 이펙트 반영시간에 도달하면 white 컬러로 돌아오기
            else
                sprite.color = Color.white;

            yield return null;
        }
    }
}
