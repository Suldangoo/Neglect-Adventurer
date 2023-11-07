using BackEnd;
using GooglePlayGames.BasicApi;
using GooglePlayGames;
using UnityEngine;

public class GoogleLogin : MonoBehaviour
{
    UiManager UiManager => UiManager.Instance; // UI 매니저 인스턴스

    // GPGS 로그인 
    void Start()
    {
        // GPGS 플러그인 설정
        PlayGamesClientConfiguration config = new PlayGamesClientConfiguration
            .Builder()
            .RequestServerAuthCode(false)
            .RequestEmail()
            .RequestIdToken()
            .Build();
        PlayGamesPlatform.InitializeInstance(config);
        PlayGamesPlatform.DebugLogEnabled = true;
        PlayGamesPlatform.Activate();
    }

    public void OnClickLogin()
    {
        // 이미 로그인 된 경우
        if (Social.localUser.authenticated == true)
        {
            GPGSLogin();
        }
        else
        {
            Social.localUser.Authenticate((bool success) => {
                if (success)
                {
                    GPGSLogin();
                }
                else
                {
                    // 로그인 실패
                    Debug.Log("Login failed for some reason");
                }
            });
        }
    }

    // 구글 토큰 받아옴
    public string GetTokens()
    {
        if (PlayGamesPlatform.Instance.localUser.authenticated)
        {
            string _IDtoken = PlayGamesPlatform.Instance.GetIdToken();
            return _IDtoken;
        }
        else
        {
            Debug.Log("접속되어 있지 않습니다. PlayGamesPlatform.Instance.localUser.authenticated :  fail");
            return null;
        }
    }

    private void GPGSLogin()
    {
        // 페더레이션 유저의 뒤끝 가입 여부 확인
        var checkUserResult = Backend.BMember.CheckUserInBackend(GetTokens(), FederationType.Google);

        if (checkUserResult.IsClientRequestFailError()) // 클라이언트의 일시적인 네트워크 끊김 시
        {
            Debug.Log("네트워크가 불안정합니다. 다시 시도해보세요.");
        }
        else if (checkUserResult.IsServerError()) // 서버의 이상 발생 시
        {
            Debug.Log("현재 서버가 불안정합니다. 나중에 다시 시도해보세요.");
        }
        else if (checkUserResult.IsMaintenanceError()) // 서버 상태가 '점검'일 시
        {
            Debug.Log("서버가 점검중입니다.");

            UiManager.SetStartUi(false);
            UiManager.SetnotificationUi(true);

            return;
        }
        else if (checkUserResult.IsBadAccessTokenError())
        {
            var checkToken = Backend.BMember.RefreshTheBackendToken();

            if (checkToken.IsSuccess() == false && checkToken.IsDeviceBlockError())
            {
                Debug.Log("해당 디바이스는 차단되었습니다.");
            }
        }

        // 로그인 (회원가입)
        BackendReturnObject bro = Backend.BMember.AuthorizeFederation(GetTokens(), FederationType.Google, "GPGS");

        if (checkUserResult.GetStatusCode() == "200")
        {
            // 이미 가입한 유저
            UiManager.SetStartUi(false);
            UiManager.SetGameUi(true);
        }
        else
        {
            // 가입하지 않은 유저
            BackendGameData.Instance.GameDataInsert();

            UiManager.SetStartUi(false);
            UiManager.SetnicknameUi(true);
        }
    }
}
