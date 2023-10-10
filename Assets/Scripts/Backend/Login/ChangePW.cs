using UnityEngine;
using UnityEngine.UI;
using TMPro;
using BackEnd;

public class ChangePW : LoginBase
{
	[SerializeField]
	private	Image				imageOld;			// 현재 비밀번호 필드 색상 변경
	[SerializeField]
	private	TMP_InputField		inputFieldOld;      // 현재 비밀번호 필드 텍스트 정보 추출
	[SerializeField]
	private Image				imageNew;           // 새 비밀번호 필드 색상 변경
	[SerializeField]
	private TMP_InputField		inputFieldNew;      // 새 비밀번호 필드 텍스트 정보 추출
	[SerializeField]
	private	Image				imageNewCheck;      // 새 비밀번호 확인 필드 색상 변경
	[SerializeField]
	private	TMP_InputField		inputFieldNewCheck; // 새 비밀번호 확인 필드 텍스트 정보 추출

	[SerializeField]
	private	Button				btnChangePW;		// "비밀번호 변경" 버튼 (상호작용 가능/불가능)

	public void OnClickChangePW()
	{
		// 매개변수로 입력한 InputField UI의 색상과 Message 내용 초기화
		ResetUI(imageOld, imageNew, imageNewCheck);

		// 필드 값이 비어있는지 체크
		if ( IsFieldDataEmpty(imageOld, inputFieldOld.text, "현재 비밀번호") )				return;
		if ( IsFieldDataEmpty(imageNew, inputFieldNew.text, "새 비밀번호") )				return;
		if ( IsFieldDataEmpty(imageNewCheck, inputFieldNewCheck.text, "새 비밀번호 확인"))	return;

		// 비밀번호 확인이 같은지 확인
		if (!inputFieldNew.text.Equals(inputFieldNewCheck.text))
		{
			GuideForIncorrectlyEnteredData(imageNewCheck, "비밀번호가 일치하지 않습니다.");
			return;
		}

		// "비밀번호 변경" 버튼의 상호작용 비활성화
		btnChangePW.interactable = false;
		SetMessage("비밀번호 변경중입니다.");

		// 뒤끝 서버 비밀번호 변경 시도
		ChangeCustomPW();
	}

	/// <summary>
	/// 비밀번호를 교체하기 위해 서버로부터 전달받은 message를 기반으로 로직 처리
	/// </summary>
	private void ChangeCustomPW()
	{
		// 현재 비밀번호와 새 비밀번호를 서버로 전송
		var bro = Backend.BMember.UpdatePassword(inputFieldOld.text, inputFieldNew.text);

		// "비밀번호 변경" 버튼 상호작용 활성화
		btnChangePW.interactable = true;

		// 비밀번호 변경 성공
		if (bro.IsSuccess())
		{
			SetMessage($"비밀번호가 변경되었습니다.");
		}
		// 비밀번호 변경 실패
		else
		{
			string message = string.Empty;

			switch (int.Parse(bro.GetStatusCode()))
			{
				case 400:   // 현재 비밀번호가 틀린 경우
					message = "현재 비밀번호가 틀립니다.";
					break;
				default:
					message = bro.GetMessage();
					break;
			}

			if (message.Contains("비밀번호"))
			{
				GuideForIncorrectlyEnteredData(imageOld, message);
			}
			else
			{
				SetMessage(message);
			}
		}
	}
}

