using UnityEngine;
using TMPro;

public class GameDataViewer : MonoBehaviour
{
	[SerializeField]
	private	TextMeshProUGUI	textNickname;
	[SerializeField]
	private TextMeshProUGUI textDiamond;
	[SerializeField]
	private TextMeshProUGUI textGold;

	private void Awake()
	{
		BackendGameData.Instance.onGameDataLoadEvent.AddListener(UpdateGameData);
		BackendGameData.Instance.onGameDataUpdateEvent.AddListener(UpdateGameData);
	}
	public void UpdateNickname()
	{
		// 닉네임이 없으면 gamer_id를 출력하고, 닉네임이 있으면 닉네임 출력
		textNickname.text = UserInfo.Data.nickname == null ?
							UserInfo.Data.gamerId : UserInfo.Data.nickname;
	}

	/*public void UpdateGameData()
	{
		textLevel.text = $"{BackendGameData.Instance.UserGameData.level}";
		textGold.text = $"{BackendGameData.Instance.UserGameData.gold}";
	}*/
	public void UpdateGameData()
	{
		textGold.text = BackendGameData.Instance.UserGameData.gold.ToString("N0");
		textDiamond.text = BackendGameData.Instance.UserGameData.diamond.ToString("N0");
	}
}