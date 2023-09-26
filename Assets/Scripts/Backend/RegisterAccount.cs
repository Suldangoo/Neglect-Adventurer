using UnityEngine;
using UnityEngine.UI;
using TMPro;
using BackEnd;

public class RegisterAccount : LoginBase
{
    [SerializeField]
    private Image imageEmail; // 이메일 필드 색상 변경

    [SerializeField]
    private TMP_InputField inputFieldEmail; // 이메일 필드 텍스트 정보 추출

    [SerializeField]
    private Image imageID; // ID 필드 색상 변경

    [SerializeField]
    private TMP_InputField inputFieldID; // ID 필드 텍스트 정보 추출

    [SerializeField]
    private Image imagePW; // PW 필드 색상 변경

    [SerializeField]
    private TMP_InputField inputFieldPW; // PW 필드 텍스트 정보 추출

    [SerializeField]
    private Image imageConfirmPW; // PW 확인 필드 색상 변경

    [SerializeField]
    private TMP_InputField inputFieldConfirmPW; // PW 확인 필드 텍스트 정보 추출

    [SerializeField]
    private Button btnRegister; // 계정 생성 버튼 (상호작용 가능 / 불가능)

    /// <summary>
    /// "계정 생성" 버튼을 눌렀을 때 호출
    /// </summary>
    public void OnClickRegisterAccount()
    {
        // 매개변수로 입력한 InputField UI의 색상과 Message 내용 초기화
        ResetUI(imageEmail, imageID, imagePW, imageConfirmPW);

        // 필드 값이 비어있는지 체크
        if (IsFieldDataEmpty(imageEmail, inputFieldEmail.text, "이메일")) return;
        if (IsFieldDataEmpty(imageID, inputFieldID.text, "아이디")) return;
        if (IsFieldDataEmpty(imagePW, inputFieldPW.text, "비밀번호")) return;
        if (IsFieldDataEmpty(imageConfirmPW, inputFieldConfirmPW.text, "비밀번호 확인")) return;

        // 비밀번호와 비밀번호 확인의 내용이 다를 때
        if ( !inputFieldPW.text.Equals(inputFieldConfirmPW.text) )
        {
            GuideForIncorrectlyEnteredData(imageConfirmPW, "비밀번호가 일치하지 않습니다.");
            return;
        }

        // 메일 형식 검사
        if ( !inputFieldEmail.text.Contains("@") )
        {
            GuideForIncorrectlyEnteredData(imageEmail, "메일 형식이 잘못되었습니다.");
            return;
        }

        // 회원가입 버튼을 연타하지 못하도록 상호작용 비활성화
        btnRegister.interactable = false;
        SetMessage("계정 생성중입니다...");

        // 뒤끝 서버 계정 생성 시도
        CustomSignUp(inputFieldID.text, inputFieldPW.text);
    }

    /// <summary>
    /// 계정 생성 시도 후 서버로부터 전달받은 message를 기반으로 로직 처리
    /// </summary>
    /// <param name="id"></param>
    /// <param name="pw"></param>
    public void CustomSignUp(string id, string pw)
    {
        // "회원가입" 버튼 상호작용 활성화
        btnRegister.interactable = true;

        var bro = Backend.BMember.CustomSignUp(id, pw);

        if (bro.IsSuccess())
        {
            var Register = Backend.BMember.UpdateCustomEmail(inputFieldEmail.text);

            if (Register.IsSuccess())
            {
                SetMessage($"회원가입 성공! {inputFieldID.text}님이 신규 모험가가 되었습니다.");
            }
        }
        else
        {
            string message = string.Empty;

            switch ( int.Parse(bro.GetStatusCode()) )
            {
                case 409: // 중복된 customId가 존재하는 경우
                    message = "이미 존재하는 아이디입니다.";
                    break;
                case 403: // 차단당안 디바이스일 경우
                case 401: // 프로젝트의 상태가 점검중일 경우
                case 400: // 디바이스의 정보가 null일 경우
                default:
                    message = bro.GetMessage();
                    break;
            }

            if ( message.Contains("아이디") )
            {
                GuideForIncorrectlyEnteredData(imageID, message);
            }
            else
            {
                SetMessage(message);
            }
        }
    }
}
