using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Vector2 InputVec;
    public float Speed;
    public Scanner Scanner;
    public Hand[] Hands;

    public Rigidbody2D Rigidbody;
    SpriteRenderer SpriteRenderer;
    Animator Animator;


    void Awake()
    {
        Rigidbody = GetComponent<Rigidbody2D>();
        SpriteRenderer = GetComponent<SpriteRenderer>();
        Animator = GetComponent<Animator>();
        Scanner = GetComponent<Scanner>();
        Hands = GetComponentsInChildren<Hand>(true);
    }
    void Update()
    {
        if (!GameManager.Instance.IsLive)
            return;

        InputVec.x = Input.GetAxis("Horizontal");
        InputVec.y = Input.GetAxis("Vertical");
    }

    void FixedUpdate()
    {
        if (!GameManager.Instance.IsLive)
            return;

        Vector2 movement = Speed * Time.fixedDeltaTime * InputVec.normalized;
        Rigidbody.MovePosition(Rigidbody.position + movement);
    }

    void LateUpdate()
    {
        if (!GameManager.Instance.IsLive)
            return;

        Animator.SetFloat("Speed", InputVec.magnitude);

        if (InputVec.x != 0)
        {
            SpriteRenderer.flipX = InputVec.x < 0;
        }

    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (!collision.collider.CompareTag("Enemy"))
            return;

        GameManager.Instance.Health -= Time.deltaTime * 10;
        if (GameManager.Instance.Health <= 0)
        {
            for (int i = 2; i < transform.childCount; i++)
            {
                transform.GetChild(i).gameObject.SetActive(false);
            }

            Animator.SetTrigger("Dead");
            GameManager.Instance.GameOver();
        }
    }
}
