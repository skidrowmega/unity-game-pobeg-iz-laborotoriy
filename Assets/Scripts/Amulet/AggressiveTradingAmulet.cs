using UnityEngine;

[CreateAssetMenu(fileName = "AggressiveTradingAmulet", menuName = "Roguelike/Amulets/AggressiveTradingAmulet")]

public class AggressiveTradingAmulet: AmuletSO
{
    [SerializeField] private float Discount = 0.2f;

    public int CalcPriceWithDiscount(int basePrice)
    {
        return (int)(basePrice / (1 + Discount));
    }
    /*public override void OnEquip()
    {
        for (int i = 0; i < ShopRoomScript.instance.itempool.Length; i++)
        {
            ShopRoomScript.instance.itempool[i].Value = (int)(ShopRoomScript.instance.itempool[i].Value / (1 + Discount));
        }
    }

    public override void OnUnequip()
    {
        for (int i = 0; i < ShopRoomScript.instance.itempool.Length; i++)
        {
            ShopRoomScript.instance.itempool[i].Value = (int)(ShopRoomScript.instance.itempool[i].Value * (1 + Discount));
        }
    }*/
}

