using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

// 뒤끝 SDK namespace 추가
using BackEnd;

public class Login : LoginBase
{
    [SerializeField]
    private Image imageID; // ID 필드 색상 변경

    [SerializeField]
    private TMP_InputField inputFieldID; // ID 필드 텍스트 정보 추출

    [SerializeField]
    private Image imagePW; // PW 필드 색상 변경

    [SerializeField]
    private TMP_InputField inputFieldPW; // PW 필드 텍스트 정보 추출

    [SerializeField]
    private Button btnLogin; // 로그인 버튼 (상호작용 가능 / 불가능)

    /// <summary>
    /// "로그인" 버튼을 눌렀을 때 호출
    /// </summary>
    public void OnClickLogin()
    {
        // 매개변수로 입력한 InputField UI의 색상과 Message 내용 초기화
        ResetUI(imageID, imagePW);

        // 필드 값이 비어있는지 체크
        if ( IsFieldDataEmpty(imageID, inputFieldID.text, "아이디") ) return;
        if ( IsFieldDataEmpty(imagePW, inputFieldPW.text, "비밀번호") ) return;

        // 로그인 버튼을 연타하지 못하도록 상호작용 비활성화
        btnLogin.interactable = false;

        // 서버에 로그인을 요청하는 동안 화면에 출력하는 내용 업데이트
        StartCoroutine(nameof(LoginProcess));

        // 뒤끝 서버 로그인 시도
        ResponseToLogin(inputFieldID.text, inputFieldPW.text);
    }

    /// <summary>
    /// 로그인 시도 후 서버로부터 전달받은 message를 기반으로 로직 처리
    /// </summary>
    private void ResponseToLogin(string ID, string PW)
    {
        // 서버에 로그인 요청 (비동기)
        Backend.BMember.CustomLogin(ID, PW, callback =>
        {
            StopCoroutine(nameof(LoginProcess));

            // 로그인 성공
            if (callback.IsSuccess())
            {
                SetMessage($"{inputFieldID.text}님, 환영합니다!");
            }
            // 로그인 실패
            else
            {
                // 로그인에 실패했을 때는 다시 로그인을 해야하기 때문에 "로그인" 버튼 상호작용 활성화
                btnLogin.interactable = true;

                string message = string.Empty;

                switch( int.Parse(callback.GetStatusCode()) )
                {
                    case 401: // 존재하지 않는 아이디, 잘못된 비밀번호
                        message = callback.GetMessage().Contains("customID") ? "존재하지 않는 아이디입니다." : "잘못된 비밀번호입니다.";
                        break;
                    case 403: // 유저 or 디바이스 차단
                        message = callback.GetMessage().Contains("user") ? "차단당한 유저입니다." : "차단당한 디바이스입니다.";
                        break;
                    case 410: // 탈퇴 진행중
                        message = "탈퇴중인 계정입니다.";
                        break;
                    default:
                        message = callback.GetMessage();
                        break;
                }

                // StatusCode 401에서 "잘못된 비밀번호입니다." 일 때
                if ( message.Contains("비밀번호") )
                {
                    GuideForIncorrectlyEnteredData(imagePW, message);
                }
                else
                {
                    GuideForIncorrectlyEnteredData(imageID, message);
                }
            }
        });
    }

    private IEnumerator LoginProcess()
    {
        float time = 0;

        while (true)
        {
            time += Time.deltaTime;

            SetMessage($"로그인 중입니다... {time:F1}");

            yield return null;
        }
    }
}