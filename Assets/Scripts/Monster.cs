using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Monster : MonoBehaviour
{
    GameManager GameManager => GameManager.Instance;

    Image hpColor;  // HP�� ������Ʈ
    Animator anim;  // ���� �ִϸ�����
    SpriteRenderer sprite; // ��������Ʈ ������
    ParticleSystem particle; // ��ƼŬ

    [SerializeField] GameObject hpBar; // hp�� ������Ʈ
    [SerializeField] GameObject hpSprite; // hp ��������Ʈ ������Ʈ
    [SerializeField] GameObject dmgText; // ������ �ؽ�Ʈ ������
    [SerializeField] Canvas canvas; // ������ ���� ĵ����

    public float maxHp;     // �ִ� HP
    public float hp;        // ���� HP
    public bool isLive;     // ������ ���� ����
    float speed;            // ������ �ӵ�
    float hitDuration;      // �ǰ� ȿ�� ���ӽð�

    private void SetMonster()
    {
        isLive = true; // ���� üũ
        hp = maxHp; // HP �ʱ�ȭ
        hpColor.fillAmount = hp / maxHp; // ���� HP�� HP�ٿ� �ݿ�
        speed = GameManager.scrollSpeed; // ���� ��ũ�� �ӵ� �ݿ�
    }

    public void Damage(float atk)
    {
        hp -= atk; // atk�� ���ظ�ŭ �������� ����
        hpColor.fillAmount = hp / maxHp; // ���� HP�� HP�ٿ� �ݿ�
        hitDuration = Time.time + 0.1f; // ��Ʈ ����Ʈ �ݿ��ð��� 0.1�� ���ķ� ����
        particle.Clear(); // ���� ��ƼŬ �ʱ�ȭ
        particle.Play(); // ��ƼŬ ���
        GameObject damageText = Instantiate(dmgText, canvas.transform);
        damageText.GetComponent<Text>().text = atk.ToString();
    }

    private void Start()
    {
        // ������Ʈ �Ҵ�
        hpColor = hpSprite.GetComponent<Image>();
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        particle = GetComponent<ParticleSystem>();

        SetMonster();
        StartCoroutine(HitEffect());
    }

    void Update()
    {
        // ��ũ�Ѹ� ���̶�� �������� ���Ͱ� �̵�
        if (GameManager.isScroll)
        {
            transform.Translate(Vector2.left * speed * Time.deltaTime);
        }

        if (isLive && hp <= 0f)
        {
            // ü���� 0�� �� ��� ���
            anim.SetTrigger("Dead");
            hpBar.SetActive(false); // HP�� ��Ȱ��ȭ
            isLive = false;
        }

        // x��ǥ�� 1���� �Դٸ�, ���� ����
        if (isLive && transform.position.x < 1f)
        {
            GameManager.SetBattle(true);
        }

        // x��ǥ�� -15���� ���ٸ�, ������
        if (transform.position.x <= -15f)
        {
            SetMonster(); // ���� ������
            transform.position = new Vector3(15, -2, 0); // ��ġ �ʱ�ȭ
            hpBar.SetActive(true); // HP�� Ȱ��ȭ
            anim.SetTrigger("Respawn"); // �ִϸ����� ��Ʈ��
        }
    }

    IEnumerator HitEffect()
    {
        // �ڷ�ƾ���� ��Ʈ ����Ʈ ����
        while (true)
        {
            // ��Ʈ ����Ʈ �ݿ��ð����� red �÷� ���ϱ�
            if (Time.time < hitDuration)
                sprite.color = Color.red;
            // ��Ʈ ����Ʈ �ݿ��ð��� �����ϸ� white �÷��� ���ƿ���
            else
                sprite.color = Color.white;

            yield return null;
        }
    }
}
