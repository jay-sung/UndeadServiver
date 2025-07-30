using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float Damage;
    public int Per;
    public float Speed = 15f;

    Rigidbody2D Rigidbody;

    public void Init(float dataDamage, int dataPer, Vector3 dir)
    {
        Damage = dataDamage;
        Per = dataPer;

        if (Per > -1)
        {
            Rigidbody.linearVelocity = dir * Speed;
        }
    }

    void Awake()
    {
        Rigidbody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        Dead();
    }

    void Dead()
    {
        Transform target = GameManager.Instance.Player.transform;
        Vector3 targetPos = target.position;
        float dir = Vector3.Distance(targetPos, transform.position);
        if (dir > 20f)
            gameObject.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Enemy") || Per == -1)
            return;

        Per--;

        if (Per == -1)
        {
            Rigidbody.linearVelocity = Vector2.zero;
            gameObject.SetActive(false);
        }
    }
}
