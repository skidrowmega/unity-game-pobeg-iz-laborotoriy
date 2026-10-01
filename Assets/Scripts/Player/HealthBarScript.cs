using UnityEngine;

public class HealthBarScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    CharacterController player;
    [SerializeField] RectTransform greenhealth;
    Vector3 startingposition;

    void Awake()
    {
        //greenhealth = GameObject.Find("/Green").GetComponent<RectTransform>();
        startingposition = greenhealth.transform.localPosition;
        player=PlayerHandler.player;
    }

    // Update is called once per frame
    void Update()
    {
        MatchSize();
    }

    void MatchSize()
    {
        greenhealth.transform.localScale = new Vector3(player.HealthPoints / 1f / player.MaxHealthpoints / 1f, .25f, 1);
        float xOffset = greenhealth.rect.width/2*greenhealth.transform.localScale.x-greenhealth.rect.width/2;
        Vector3 savedpos = startingposition;
        savedpos.x+= xOffset;
        greenhealth.transform.localPosition = savedpos;
    }


}
