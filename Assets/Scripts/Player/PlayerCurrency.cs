using UnityEngine;
using UnityEngine.UI;

public class PlayerCurrency : MonoBehaviour
{



    [SerializeField] int Currency=0;

    public static PlayerCurrency instance;
    public Text AssosiatedTextbox;

    private void Awake()
    {
        instance = this;
        
    }
    private void Start()
    {
        UpdateTextbox();
    }
    public void SetCurrency(int currency)
    {
        Currency = currency;
        UpdateTextbox();
    }
    public int GetCurrency()
    {
        UpdateTextbox();
        return Currency;
    }
    public void LoadCurrency()
    {
        Currency = 0;
        UpdateTextbox();
    }
    public void AddCurrency(int currency)
    {
        Currency += currency;
        UpdateTextbox();
    }
    public void SubstractCurrency(int currency)
    {
        Currency -= currency;
        UpdateTextbox();
    }

    public void UpdateTextbox()
    {
        if (AssosiatedTextbox)
        {
            AssosiatedTextbox.text=Currency.ToString();
        }
    }
}
