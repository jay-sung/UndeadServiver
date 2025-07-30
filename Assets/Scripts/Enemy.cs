using System;
using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float Speed;
    public Rigidbody2D Target;
    public RuntimeAnimatorController[] AnimatorControllers;
    public float Health;
    public float MaxHealth;
    bool isAlive = true;
    Rigidbody2D rigidbody;
    SpriteRenderer spriteRenderer;
    Animator animator;
    WaitForFixedUpdate waitForFixedUpdate;
    Collider2D collider2D;

    void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        waitForFixedUpdate = new WaitForFixedUpdate();
        collider2D = GetComponent<Collider2D>();
    }

    void FixedUpdate()
    {
        if (!GameManager.Instance.IsLive)
            return;

        if (!isAlive || animator.GetCurrentAnimatorStateInfo(0).IsName("Hit"))
            return;
        Vector2 dirVec = (Target.position - rigidbody.position).normalized;
        Vector2 movement = Speed * Time.fixedDeltaTime * dirVec;
        rigidbody.position += movement;
    }

    void LateUpdate()
    {
        if (!GameManager.Instance.IsLive)
            return;
            
        if (!isAlive)
            return;

        if (Target.position.x < rigidbody.position.x)
        {
            spriteRenderer.flipX = true;
        }
        else
        {
            spriteRenderer.flipX = false;
        }
    }

    void OnEnable()
    {
        if (GameManager.Instance == null || GameManager.Instance.Player == null)
            return;
        
        Target = GameManager.Instance.Player.Rigidbody;
        isAlive = true;
        collider2D.enabled = true;
        rigidbody.simulated = true;
        spriteRenderer.sortingOrder = 2;
        Health = MaxHealth;
        rigidbody.simulated = true;
        animator.enabled = true;
        animator.SetBool("Dead", false);
    }

    public void Init(SpawnData data)
    {
        animator.runtimeAnimatorController = AnimatorControllers[data.SpawnType];
        Speed = data.Speed;
        MaxHealth = data.Health;
        Health = MaxHealth;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Bullet") || !isAlive)
            return;

        Health -= collision.GetComponent<Bullet>().Damage;
        StartCoroutine(KnockBack());

        if (Health > 0)
        {
            animator.SetTrigger("Hit");
        }
        else
        {
            isAlive = false;
            collider2D.enabled = false;
            rigidbody.simulated = false;
            spriteRenderer.sortingOrder = 1;
            animator.SetBool("Dead", true);
            StartCoroutine(DeadDelay());
            GameManager.Instance.Kill++;
            GameManager.Instance.GetExp();
        }
    }

    IEnumerator KnockBack()
    {
        yield return null;
        rigidbody.linearVelocity = Vector2.zero;
        Vector2 dirVec = (rigidbody.position - GameManager.Instance.Player.Rigidbody.position).normalized;
        rigidbody.AddForce(dirVec * 3f, ForceMode2D.Impulse);
    }

    IEnumerator DeadDelay()
    {
        yield return new WaitForSeconds(0.5f);
        gameObject.SetActive(false);
    }
}
