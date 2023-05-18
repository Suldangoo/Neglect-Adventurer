using UnityEngine;
using UnityEngine.UI;

public class Monster : MonoBehaviour
{
    GameManager GameManager => GameManager.Instance;

    Image hpBar;    // HP�� ������Ʈ
    Animator anim;  // ���� �ִϸ�����

    public float maxHp;     // �ִ� HP
    public float hp;        // ���� HP
    public bool isLive;     // ������ ���� ����
    float speed;            // ������ �ӵ�

    private void Start()
    {
        // ������Ʈ �Ҵ�
        hpBar = GameObject.Find("HP").GetComponent<Image>();
        anim = GetComponent<Animator>();

        hp = maxHp; // HP �ʱ�ȭ
        isLive = true;
        speed = GameManager.scrollSpeed;
    }

    void Update()
    {
        // ���� HP�� HP�ٿ� �ݿ�
        hpBar.fillAmount = hp / maxHp;

        if (GameManager.isScroll)
        {
            // ��ũ�Ѹ� ���̶�� �������� ���Ͱ� �̵�
            transform.Translate(Vector2.left * speed * Time.deltaTime);
        }

        if (isLive && hp <= 0f)
        {
            // ü���� 0�� �� ��� ���
            anim.SetTrigger("Dead");
            transform.GetChild(0).gameObject.SetActive(false); // HP�� ��Ȱ��ȭ
            isLive = false;
        }

        if (isLive && transform.position.x < 1f)
        {
            // x��ǥ�� 1���� �Դٸ�, ���� ����
            GameManager.SetBattle(true);
        }

        if (transform.position.x <= -15f)
        {
            // x��ǥ�� -15���� ���ٸ�, ������
            hp = maxHp;
            transform.position = new Vector3(15, -2, 0);
            transform.GetChild(0).gameObject.SetActive(true); // HP�� Ȱ��ȭ
            isLive = true;
            anim.SetTrigger("Respawn");
        }
    }
}
