using UnityEngine;

public class WeaponDurabilityBar : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    Player player;
    [SerializeField] RectTransform durability;
    Vector3 startingposition;

    Weapon weapon;

    [SerializeField] bool IsMelee=false;

    void Awake()
    {
        //greenhealth = GameObject.Find("/Green").GetComponent<RectTransform>();
        startingposition = durability.transform.localPosition;

    }
    private void Start()
    {
        player = Player.player;
        if (IsMelee) weapon = player.GetComponent<WeaponMelee>();
        else weapon = player.GetComponent<WeaponRanged>();
        weapon.AssociatedUI= this;

    }

    // Update is called once per frame
    void Update()
    {
        MatchSize();
    }

    void MatchSize()
    {
        //greenhealth.transform.localScale = new Vector3(player.HealthPoints / 1f / player.MaxHealthpoints / 1f, .25f, 1);
        durability.transform.localScale = new Vector3(weapon.Durability / 1f / weapon.MaxDurability / 1f, .25f, 1);
        float xOffset = durability.rect.width / 2 * durability.transform.localScale.x - durability.rect.width / 2;
        Vector3 savedpos = startingposition;
        savedpos.x += xOffset;
        durability.transform.localPosition = savedpos;
    }

}
