using UnityEngine;

public class HealthBarScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    CharacterController player;
    [SerializeField] RectTransform greenhealth;

    void Awake()
    {
        //greenhealth = GameObject.Find("/Green").GetComponent<RectTransform>();
        player=GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        MatchSize();
    }

    void MatchSize()
    {
        greenhealth.transform.localScale = new Vector3(player.HealthPoints / 1f / player.MaxHealthpoints / 1f, .25f, 1);
/*        float changedXpos = -greenhealth.rect.width * (1/greenhealth.transform.localScale.x) / 4;
        Vector3 savedpos= greenhealth.rect.position;
        savedpos.x = changedXpos;
        greenhealth.transform.position=savedpos;*/
    }


}
