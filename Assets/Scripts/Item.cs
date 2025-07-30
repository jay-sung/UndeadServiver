using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour
{
    public ItemData Data;
    public int Level;
    public Weapon Weapon;
    public Gear Gear;

    Image icon;
    Text textLevel;
    Text textName;
    Text textDesc;

    void Awake()
    {
        icon = GetComponentsInChildren<Image>()[1];
        icon.sprite = Data.itemIcon;

        Text[] texts = GetComponentsInChildren<Text>();
        textLevel = texts[0];
        textName = texts[1];
        textDesc = texts[2];
        textName.text = Data.itemName;
    }

    void OnEnable()
    {
        textLevel.text = string.Format("Lv.{0}", Level);

        switch (Data.itemType)
        {
            case ItemData.ItemType.Melee:
            case ItemData.ItemType.Range:
                if (Level < Data.damages.Length)
                    textDesc.text = string.Format(Data.itemDesc, Data.damages[Level] * 100, Data.counts[Level]);
                break;
            case ItemData.ItemType.Glove:
            case ItemData.ItemType.Shoe:
                if (Level < Data.damages.Length)
                    textDesc.text = string.Format(Data.itemDesc, Data.damages[Level] * 100);
                break;
            default:
                textDesc.text = Data.itemDesc;
                break;
        }  
    }

    public void OnClick()
    {
        switch (Data.itemType)
        {
            case ItemData.ItemType.Melee:
            case ItemData.ItemType.Range:
                if (Level == 0)
                {
                    GameObject newWeapon = new GameObject();
                    Weapon = newWeapon.AddComponent<Weapon>();
                    Weapon.Init(Data);
                }
                else
                {
                    float nextDamage = Data.damages[Level];
                    int nextCount = 0;

                    nextDamage += Data.baseDamage * Data.damages[Level];
                    nextCount += Data.counts[Level];

                    Weapon.LevelUp(nextDamage, nextCount);
                }
                Level++;
                break;
            case ItemData.ItemType.Glove:
            case ItemData.ItemType.Shoe:
                if (Level == 0)
                {
                    GameObject newGear = new GameObject();
                    Gear = newGear.AddComponent<Gear>();
                    Gear.Init(Data);
                }
                else
                {
                    float nextRate = Data.damages[Level];
                    Gear.LevelUp(nextRate);
                }
                Level++;
                break;
            case ItemData.ItemType.Heal:
                GameManager.Instance.Health += GameManager.Instance.MaxHealth;
                break;
            default:
                break;
        }

        if (Level == Data.damages.Length)
        {
            GetComponent<Button>().interactable = false;
        }
    }
}
