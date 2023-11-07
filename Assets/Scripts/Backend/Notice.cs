using UnityEngine;
using BackEnd;
using TMPro;

public class Notice : MonoBehaviour
{
    public TextMeshProUGUI noticeText;

    private void Start()
    {
        // 뒤끝 서버에서 공지 사항 가져오기
        Backend.Notice.GetTempNotice(callback =>
        {
            // JSON 파싱
            string jsonString = callback;
            NoticeData noticeData = JsonUtility.FromJson<NoticeData>(jsonString);

            // 공지 사항 내용을 TMPro 텍스트에 설정
            noticeText.text = noticeData.contents.Replace("\\n", "\n");
        });
    }
}

[System.Serializable]
public class NoticeData
{
    public bool isUse;
    public string contents;
}
