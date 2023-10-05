using UnityEngine;

public class LobbyScenario : MonoBehaviour
{
	GameManager GameManager => GameManager.Instance; // 게임 매니저 인스턴스

	[SerializeField]
	private	UserInfo user;

	private void Awake()
	{
		user.GetUserInfoFromBackend();
	}

    private void Start()
    {
		BackendGameData.Instance.GameDataLoad();
		GameManager.GameStart();
	}
}