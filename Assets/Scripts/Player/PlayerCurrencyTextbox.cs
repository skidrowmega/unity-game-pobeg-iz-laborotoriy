using UnityEngine;
using UnityEngine.UI;

public class PlayerCurrencyTextbox : MonoBehaviour
{

    private void Start()
    {
        PlayerCurrency.instance.AssosiatedTextbox = this.GetComponent<Text>();
    }

}
