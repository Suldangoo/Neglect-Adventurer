using BackEnd;
using UnityEngine;

public class GoldRankRegister : MonoBehaviour
{
	public void Process()
	{
		//UpdateMyRankData(newScore);
		UpdateMyBestRankData(BackendGameData.Instance.UserGameData.gold);
	}

	private void UpdateMyRankData(int newScore)
	{
		string rowInDate = string.Empty;

		// 랭킹 데이터를 업데이트하려면 게임 데이터에서 사용하는 데이터의 inDate 값이 필요
		var bro = Backend.GameData.GetMyData(Constants.USER_DATA_TABLE, new Where());

		if (!bro.IsSuccess())
		{
			Debug.LogError($"데이터 조회 중 문제가 발생했습니다 : {bro}");
			return;
		}

		Debug.Log($"데이터 조회에 성공했습니다 : {bro}");

		if (bro.FlattenRows().Count > 0)
		{
			rowInDate = bro.FlattenRows()[0]["inDate"].ToString();
		}
		else
		{
			Debug.LogError("데이터가 존재하지 않습니다.");
			return;
		}

		Param param = new Param()
			{
				{ "gold", newScore }
			};

		var ranking = Backend.URank.User.UpdateUserScore(Constants.GOLD_RANK_UUID, Constants.USER_DATA_TABLE, rowInDate, param);

		if (ranking.IsSuccess())
		{
			Debug.Log($"랭킹 등록에 성공했습니다 : {ranking}");
		}
		else
		{
			Debug.LogError($"랭킹 등록 중 오류가 발생했습니다 : {ranking}");
		}
	}

	private void UpdateMyBestRankData(int newScore)
	{
		var bro = Backend.URank.User.GetMyRank(Constants.GOLD_RANK_UUID);

		if (bro.IsSuccess())
		{
			// JSON 데이터 파싱 성공
			try
			{
				LitJson.JsonData rankDataJson = bro.FlattenRows();

				// 받아온 데이터의 개수가 0이면 데이터가 없는 것
				if (rankDataJson.Count <= 0)
				{
					Debug.LogWarning("데이터가 존재하지 않습니다.");
				}
				else
				{
					// 추가로 등록한 항목은 컬럼명을 그대로 사용
					int bestScore = int.Parse(rankDataJson[0]["score"].ToString());

					// 현재 점수가 최고 점수보다 높으면
					if (newScore > bestScore)
					{
						// 현재 점수를 새로운 최고 점수로 설정하고, 랭킹에 등록
						UpdateMyRankData(newScore);

						Debug.Log($"최고 점수 갱신 {bestScore} -> {newScore}");
					}
				}
			}
			// JSON 데이터 파싱 실패
			catch (System.Exception e)
			{
				// try-catch 에러 출력
				Debug.LogError(e);
			}
		}
		else
		{
			// 자신의 랭킹 정보가 존재하지 않을 때는 현재 점수를 새로운 랭킹으로 등록
			if (bro.GetMessage().Contains("userRank"))
			{
				UpdateMyRankData(newScore);

				Debug.Log($"새로운 랭킹 데이터 생성 및 등록 : {bro}");
			}
		}
	}
}

