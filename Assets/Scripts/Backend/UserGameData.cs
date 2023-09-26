[System.Serializable]
public class UserGameData
{
	public int level;		// 총합 전투력
	public int gold;		// 골드 (무료 재화)
	public int jewel;       // 다이아 (유료 재화)

	//public int atkLv;       // 검술 수련 레벨
	//public int defLv;       // 방어 수련 레벨
	//public int dexLv;       // 민첩 수련 레벨
	//public int lukLv;       // 행운 수련 레벨

	public void Reset()
	{
		level = 100;
		gold = 0;
		jewel = 0;

		//atkLv = 1;
		//defLv = 1;
		//dexLv = 1;
		//lukLv = 1;
	}
}


/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BackEnd;
*/