using System;
using UnityEngine;

public class openbloknot : MonoBehaviour
{

    [SerializeField] GameObject toggleenabled;

    [SerializeField] float toggleDistance;

    [SerializeField] Trigger callbackfunctiontocall;


    private void Start()
    {
        if(callbackfunctiontocall)callbackfunctiontocall.AddCallback(DestroyLoaded);
    }

    private void Update()
    {
        if(Vector3.Distance(Player.player.transform.position, transform.position) <= toggleDistance)
        {
            if (!toggleenabled.activeSelf)
            {
                toggleenabled.SetActive(true);
            }
        }
        else
        {
            if (toggleenabled.activeSelf)
            {
                toggleenabled.SetActive(false);
            }
        }
    }
    public void DestroyLoaded()
    {
        Destroy(toggleenabled);
        Destroy(gameObject);
    }






}
