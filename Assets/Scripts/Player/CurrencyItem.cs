using UnityEngine;

public class CurrencyItem : MonoBehaviour
{

    [SerializeField] int Value = 1;
    [SerializeField] float pickupRadius=2;

    void Update()
    {
        CheckPickup();
    }
    void OnPickup()
    {
        PlayerCurrency.instance.AddCurrency(Value);
        Destroy(gameObject);
    }
    protected virtual void CheckPickup()
    {
        Vector3 newpos = transform.position;
        newpos.y = Player.player.transform.position.y;
        if (Vector3.Distance(Player.player.transform.position, transform.position) < pickupRadius)
        {
            OnPickup();
        }
    }
}
