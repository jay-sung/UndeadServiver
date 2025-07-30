using UnityEngine;

public class Weapon : MonoBehaviour
{
    public int Id;
    public float prefabId;
    public float Damage;
    public int Count;
    public float Speed;

    float timer = 0f;
    Player player;

    void Awake()
    {
        player = GameManager.Instance.Player;
    }

    public void LevelUp(float damage, int count)
    {
        Damage = damage;
        Count = count;
        if (Id == 0)
            Batch();
    }

    void Update()
    {
        if (!GameManager.Instance.IsLive)
            return;

        switch (Id)
        {
            case 0:
                transform.Rotate(0, 0, Speed * Time.deltaTime);
                break;
            default:
                timer += Time.deltaTime;
                if (timer > Speed)
                {
                    timer = 0f;
                    Fire();
                }
                break;
        }
    }

    public void Init(ItemData data)
    {
        name = "Weapon_" + data.name;
        transform.parent = player.transform;
        transform.localPosition = Vector3.zero;

        Id = data.itemId;
        Damage = data.baseDamage;
        Count = data.baseCount;

        for (int i = 0; i < GameManager.Instance.PoolManager.prefabs.Length; i++)
        {
            if (data.projectile == GameManager.Instance.PoolManager.prefabs[i])
            {
                prefabId = i;
                break;
            }
        }

        switch (Id)
        {
            case 0:
                Speed = 150;
                Batch();
                break;
            default:
                Speed = 0.3f;
                break;
        }

        Hand hand = player.Hands[(int)data.itemType];
        hand.spriteRenderer.sprite = data.hand;
        hand.gameObject.SetActive(true);

        player.BroadcastMessage("ApplyGear", SendMessageOptions.DontRequireReceiver);
    }

    void Batch()
    {
        for (int i = 0; i < Count; i++)
        {
            Transform bullet;
            if (i < transform.childCount)
            {
                bullet = transform.GetChild(i);
            }
            else
            {
                bullet = GameManager.Instance.PoolManager.GetObject((int)prefabId).transform;
            }
            bullet.parent = transform;

            bullet.localPosition = Vector3.zero;
            bullet.localRotation = Quaternion.identity;

            Vector3 rotVec = new Vector3(0, 0, i * (360f / Count));
            bullet.Rotate(rotVec);
            bullet.Translate(bullet.up * 1.5f, Space.World);
            bullet.GetComponent<Bullet>().Init(Damage, -1, Vector3.zero);
        }
    }

    void Fire()
    {
        if (player.Scanner.nearestTarget == null)
            return;

        Vector3 dirVec = (player.Scanner.nearestTarget.position - transform.position).normalized;

        Transform bullet = GameManager.Instance.PoolManager.GetObject((int)prefabId).transform;
        bullet.gameObject.SetActive(true);
        bullet.position = transform.position;
        bullet.rotation = Quaternion.FromToRotation(Vector3.up, dirVec);
        bullet.GetComponent<Bullet>().Init(Damage, Count, dirVec);
    }
}
