using UnityEngine;
using UnityEngine.UI;

public class GoldText : MonoBehaviour
{
    GameManager GameManager => GameManager.Instance; // ���� �Ŵ��� �ν��Ͻ�
    [SerializeField] Text text; // ��� �ؽ�Ʈ ������Ʈ
    
    void Update()
    {
        text.text = string.Format("{0:#,###}", GameManager.gold).ToString();
    }
}
