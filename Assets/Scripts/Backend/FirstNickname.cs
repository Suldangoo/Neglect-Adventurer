using UnityEngine;
using UnityEngine.UI;
using TMPro;
using BackEnd;

public class FirstNickname : LoginBase
{
    UiManager UiManager => UiManager.Instance; // UI 매니저 인스턴스

	[SerializeField]
	private	Image				imageNickname;		// 닉네임 필드 색상 변경
	[SerializeField]
	private	TMP_InputField		inputFieldNickname;	// 닉네임 필드 텍스트 정보 추출

	[SerializeField]
	private	Button				btnCreateNickname;  // "닉네임 설정" 버튼 (상호작용 가능/불가능)

	private void OnEnable()
	{
		// 닉네임 변경에 실패해 에러 메시지를 출력한 상태에서
		// 닉네임 변경 팝업을 닫았다가 열 수 있기 때문에 상태를 초기화
		ResetUI(imageNickname);
		SetMessage("닉네임을 입력하세요");
	}

	public void OnClickCreateNickname()
	{
		// 매개변수로 입력한 InputField UI의 색상과 Message 내용 초기화
		ResetUI(imageNickname);

		// 필드 값이 비어있는지 체크
		if ( IsFieldDataEmpty(imageNickname, inputFieldNickname.text, "닉네임") )	return;

        // "닉네임 변경" 버튼의 상호작용 비활성화
        btnCreateNickname.interactable = false;
		SetMessage("닉네임 변경중입니다..");

        // 뒤끝 서버 닉네임 변경 시도
        CreateNickname();
	}

	private void CreateNickname()
	{
		// 닉네임 설정
		var bro = Backend.BMember.CreateNickname(inputFieldNickname.text);

        // "닉네임 변경" 버튼의 상호작용 활성화
        btnCreateNickname.interactable = true;

		// 닉네임 변경 성공
		if (bro.IsSuccess())
		{
            UiManager.SetnicknameUi(false);
            UiManager.SetGameUi(true);
        }
		// 닉네임 변경 실패
		else
		{
			string message = string.Empty;

			switch (int.Parse(bro.GetStatusCode()))
			{
				case 400:   // 빈 닉네임 혹은 string.Empty, 20자 이상의 닉네임, 닉네임 앞/뒤에 공백이 있는 경우
					message = "닉네임이 비어있거나, 20자 이상 이거나, 앞/뒤에 공백이 있습니다.";
					break;
				case 409:   // 이미 중복된 닉네임이 있는 경우
					message = "이미 존재하는 닉네임입니다.";
					break;
				default:
					message = bro.GetMessage();
					break;
			}

			GuideForIncorrectlyEnteredData(imageNickname, message);
		}
	}
}

