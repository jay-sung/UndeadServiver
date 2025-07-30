using UnityEngine;

public class Gear : MonoBehaviour
{
    public ItemData.ItemType Type;
    public float Rate;

    public void Init(ItemData data)
    {
        name = "Gear_" + Type.ToString();
        transform.parent = GameManager.Instance.Player.transform;
        transform.localPosition = Vector3.zero;

        Type = data.itemType;
        Rate = data.damages[0];
        ApplyGear();
    }

    public void LevelUp(float rate)
    {
        Rate += rate;
        if (Rate > 1f)
            Rate = 1f;
        ApplyGear();
    }

    void ApplyGear()
    {
        switch (Type)
        {
            case ItemData.ItemType.Glove:
                RateUp();
                break;
            case ItemData.ItemType.Shoe:
                SpeedUp();
                break;
            case ItemData.ItemType.Heal:
                // Healing logic can be added here
                break;
            default:
                break;
        }
    }

    void RateUp()
    {
        Weapon[] weapons = transform.parent.GetComponentsInChildren<Weapon>();

        foreach (var weapon in weapons)
        {
            switch (weapon.Id)
            {
                case 0:
                    weapon.Speed = 150 + (150 * Rate);
                    break;
                default:
                    weapon.Speed = 0.5f + (1f - Rate);
                    break;
            }
        }
    }

    void SpeedUp()
    {
        float speed = 3f;
        GameManager.Instance.Player.Speed = speed + (speed * Rate);
    }
}
