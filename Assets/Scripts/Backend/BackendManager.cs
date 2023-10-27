using UnityEngine;

// 뒤끝 SDK namespace 추가
using BackEnd;
using UnityEngine.UI;

public class BackendManager : MonoBehaviour
{
    public InputField input;

    private void Awake()
    {
        // Update() 메소드의 Backend.AsyncPoll() 호출을 위해 오브젝트를 파괴하지 않는다
        DontDestroyOnLoad(gameObject);

        // 뒤끝 서버 초기화
        BackendSetup();
    }

    private void BackendSetup()
    {
        var bro = Backend.Initialize(true); // 뒤끝 초기화

        // 뒤끝 초기화에 대한 응답값
        if (bro.IsSuccess())
        {
            Debug.Log("초기화 성공 : " + bro); // 성공일 경우 statusCode 204 Success
        }
        else
        {
            Debug.LogError("초기화 실패 : " + bro); // 실패일 경우 statusCode 400대 에러 발생 
        }
    }

    public void GetGoogleHash()
    {
        string googleHashKey = Backend.Utils.GetGoogleHash();

        if (!string.IsNullOrEmpty(googleHashKey))
        {
            Debug.Log(googleHashKey);
            if (input != null)
            {
                input.text = googleHashKey;
            }
        }
    }
}