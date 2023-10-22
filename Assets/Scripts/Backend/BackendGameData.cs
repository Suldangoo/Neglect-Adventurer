using UnityEngine;
using BackEnd;
using UnityEngine.Events;

public class BackendGameData
{
	[System.Serializable]
	public class GameDataLoadEvent : UnityEvent { }
	public GameDataLoadEvent onGameDataLoadEvent = new GameDataLoadEvent();
	public class GameDataUpdateEvent : UnityEvent { }
	public GameDataUpdateEvent onGameDataUpdateEvent = new GameDataUpdateEvent();

	private	static	BackendGameData	instance = null;
	public	static	BackendGameData	Instance
	{
		get
		{
			if ( instance == null )
			{
				instance = new BackendGameData();
			}

			return instance;
		}
	}

	private	UserGameData userGameData = new UserGameData();
	public	UserGameData UserGameData => userGameData;

	private	string gameDataRowInDate = string.Empty;

	/// <summary>
	/// 뒤끝 콘솔 테이블에 새로운 유저 정보 추가
	/// </summary>
	public void GameDataInsert()
	{
		// 유저 정보를 초기값으로 설정
		userGameData.Reset();
		
		// 테이블에 추가할 데이터로 가공
		Param param = new Param()
		{
			{ "level",		userGameData.level },
			{ "gold",		userGameData.gold },
			{ "diamond",	userGameData.diamond },
			{ "atkLv",      userGameData.atkLv },
			{ "defLv",      userGameData.defLv },
			{ "dexLv",      userGameData.dexLv },
			{ "lukLv",      userGameData.lukLv }
		};

        for (int i = 0; i < 3; i++)
        {
            param.Add($"knight{i + 1}", userGameData.knights[i]);
            param.Add($"magic{i + 1}", userGameData.magics[i]);
            param.Add($"heal{i + 1}", userGameData.heals[i]);
        }

        // 첫 번째 매개변수는 뒤끝 콘솔의 "게임 정보 관리" 탭에 생성한 테이블 이름
        var data = Backend.GameData.Insert("USER_DATA", param);

		// 게임 정보 추가에 성공했을 때
		if (data.IsSuccess())
		{
			// 게임 정보의 고유값
			gameDataRowInDate = data.GetInDate();

			Debug.Log($"게임 정보 데이터 삽입에 성공했습니다. : {data}");
		}
		// 실패했을 때
		else
		{
			Debug.LogError($"게임 정보 데이터 삽입에 실패했습니다. : {data}");
		}
	}

	/// <summary>
	/// 뒤끝 콘솔 테이블에서 유저 정보를 불러올 때 호출
	/// </summary>
	public void GameDataLoad()
	{
		var data = Backend.GameData.GetMyData("USER_DATA", new Where());

		// 게임 정보 불러오기에 성공했을 때
		if (data.IsSuccess())
		{
			Debug.Log($"게임 정보 데이터 불러오기에 성공했습니다. : {data}");

			// JSON 데이터 파싱 성공
			try
			{
				LitJson.JsonData gameDataJson = data.FlattenRows();

				// 받아온 데이터의 개수가 0이면 데이터가 없는 것
				if (gameDataJson.Count <= 0)
				{
					Debug.LogWarning("데이터가 존재하지 않습니다.");
				}
				else
				{
					// 불러온 게임 정보의 고유값
					gameDataRowInDate = gameDataJson[0]["inDate"].ToString();
					// 불러온 게임 정보를 userData 변수에 저장
					userGameData.level = int.Parse(gameDataJson[0]["level"].ToString());
					userGameData.gold = int.Parse(gameDataJson[0]["gold"].ToString());
					userGameData.diamond = int.Parse(gameDataJson[0]["diamond"].ToString());
					userGameData.atkLv = int.Parse(gameDataJson[0]["atkLv"].ToString());
					userGameData.defLv = int.Parse(gameDataJson[0]["defLv"].ToString());
					userGameData.dexLv = int.Parse(gameDataJson[0]["dexLv"].ToString());
					userGameData.lukLv = int.Parse(gameDataJson[0]["lukLv"].ToString());

                    for (int i = 0; i < 3; i++)
                    {
                        userGameData.knights[i] = int.Parse(gameDataJson[0][$"knight{i + 1}"].ToString());
                        userGameData.magics[i] = int.Parse(gameDataJson[0][$"magic{i + 1}"].ToString());
                        userGameData.heals[i] = int.Parse(gameDataJson[0][$"heal{i + 1}"].ToString());
                    }

                    onGameDataLoadEvent?.Invoke();
				}
			}
			// JSON 데이터 파싱 실패
			catch (System.Exception e)
			{
				// 유저 정보를 초기값으로 설정
				userGameData.Reset();
				// try-catch 에러 출력
				Debug.LogError(e);
			}
		}
		// 실패했을 때
		else
		{
			Debug.LogError($"게임 정보 데이터 불러오기에 실패했습니다. : {data}");
		}
	}

	/// <summary>
	/// 뒤끝 콘솔 테이블에 있는 유저 데이터 갱신
	/// </summary>
	public void GameDataUpdate()
	{
		if (userGameData == null)
		{
			Debug.LogError("서버에서 다운받거나 새로 삽입한 데이터가 존재하지 않습니다." +
						   "Insert 혹은 Load를 통해 데이터를 생성해주세요.");
			return;
		}

		Param param = new Param()
		{
			{ "level",      userGameData.level },
			{ "gold",       userGameData.gold },
			{ "diamond",    userGameData.diamond },
			{ "atkLv",      userGameData.atkLv },
			{ "defLv",      userGameData.defLv },
			{ "dexLv",      userGameData.dexLv },
			{ "lukLv",      userGameData.lukLv }
		};

        for (int i = 0; i < 3; i++)
        {
            param.Add($"knight{i + 1}", userGameData.knights[i]);
            param.Add($"magic{i + 1}", userGameData.magics[i]);
            param.Add($"heal{i + 1}", userGameData.heals[i]);
        }

        // 게임 정보의 고유값(gameDataRowInDate)이 없으면 에러 메시지 출력
        if (string.IsNullOrEmpty(gameDataRowInDate))
		{
			Debug.LogError($"유저의 inDate 정보가 없어 게임 정보 데이터 수정에 실패했습니다.");
		}
		// 게임 정보의 고유값이 있으면 테이블에 저장되어 있는 값 중 inDate 컬럼의 값과
		// 소유하는 유저의 owner_inDate가 일치하는 row를 검색하여 수정하는 UpdateV2() 호출
		else
		{
			Debug.Log($"{gameDataRowInDate}의 게임 정보 데이터 수정을 요청합니다.");

			var bro = Backend.GameData.UpdateV2("USER_DATA", gameDataRowInDate, Backend.UserInDate, param);

			if (bro.IsSuccess())
			{
				Debug.Log($"게임 정보 데이터 수정에 성공했습니다. : {bro}");

				onGameDataUpdateEvent?.Invoke();
			}
			else
			{
				Debug.LogError($"게임 정보 데이터 수정에 실패했습니다. : {bro}");
			}
		}
	}
}

