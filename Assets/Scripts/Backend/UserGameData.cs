[System.Serializable]
public class UserGameData
{
	public int gold;		// 골드 (무료 재화)
	public int diamond;		// 다이아 (유료 재화)

	public int atkLv;       // 검술 수련 레벨
	public int defLv;       // 방어 수련 레벨
	public int dexLv;       // 민첩 수련 레벨
	public int lukLv;       // 행운 수련 레벨

    // 보유 모험가 레벨
    public int[] knights = new int[3]; // 1성 ~ 3성 전사
    public int[] magics = new int[3];  // 1성 ~ 3성 마법사
    public int[] heals = new int[3];   // 1성 ~ 3성 힐러

	// 장착된 캐릭터들의 인덱스
	public int[] equippedCharacters = new int[3] { -1, -1, -1 }; // -1은 아무 캐릭터도 장착되지 않았음을 나타냄

	public void Reset()
	{
		gold = 0;
		diamond = 0;

		atkLv = 1;
		defLv = 1;
		dexLv = 1;
		lukLv = 1;

        System.Array.Clear(knights, 0, knights.Length);
        System.Array.Clear(magics, 0, magics.Length);
        System.Array.Clear(heals, 0, heals.Length);

		for (int i = 0; i < equippedCharacters.Length; i++)
		{
			equippedCharacters[i] = -1;
		}
	}
}