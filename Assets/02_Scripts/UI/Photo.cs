using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;

public class Photo : MonoBehaviour
{
    [SerializeField] private SpeciesData _species;
    
    private Button _button;

    private void OnEnable()
    {
        _button = this.GetRequiredComponent<Button>();
        _button.onClick.AddListener(OnClick);
    }

    private void OnClick()
    { 
        var chance = new Random().Next(0, 100);
        if (chance < 30.0F)
            AudioManager.Instance.PlaySFX(Customer.GetCustomerAngryVoice(_species));
        else if (chance < 70.0F)
            AudioManager.Instance.PlaySFX(Customer.GetCustomerHappyVoice(_species));
        else
            AudioManager.Instance.PlaySFX(Customer.GetCustomerLoveVoice(_species));
    }
}
