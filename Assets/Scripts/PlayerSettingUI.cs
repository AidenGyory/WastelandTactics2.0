using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSettingUI : MonoBehaviour
{
    public enum FactionColours
    {
        Blue,
        Red,
        Yellow,
        Green,
    }

    [SerializeField] PlayerSettingsTemplate settings;
    [SerializeField] Image HeroProfile;
    [SerializeField] Image FactionLogo;
    [SerializeField] Image FactionButton;
    [SerializeField] Image background;
    [SerializeField] Sprite[] LogoIcons;
    [SerializeField] TMP_Text FactionName;
    [SerializeField] TMP_Text HeroName;
    [Space]
    [SerializeField] Color[] BlueFaction;  
    [SerializeField] Color[] RedFaction;
    [SerializeField] Color[] YellowFaction;
    [SerializeField] Color[] GreenFaction;
    [SerializeField] FactionColours color; 


    private void Start()
    {
        UpdateUI(); 
    }
    public void UpdateUI()
    {
        FactionLogo.sprite = LogoIcons[(int)settings.faction]; 

        Color _primary = settings.primaryColour;
        Color _secondary = settings.secondaryColour;

        background.color = _primary;
        FactionLogo.color = _secondary;
        FactionButton.color = _secondary;
        FactionName.color = _secondary;
        HeroName.color = _secondary; 
    }

    public void ToggleColour()
    {
        int index = (int)color;

        index++;  

        if(index >= LogoIcons.Length)
        {
            index = 0; 
        }

        color = (FactionColours)index;  

        switch (color)
        {
            case FactionColours.Blue:
                settings.primaryColour = BlueFaction[0]; 
                settings.secondaryColour = BlueFaction[1];
                break;
            case FactionColours.Red:
                settings.primaryColour = RedFaction[0];
                settings.secondaryColour = RedFaction[1];
                break;
            case FactionColours.Yellow:
                settings.primaryColour = YellowFaction[0];
                settings.secondaryColour = YellowFaction[1];
                break;
            case FactionColours.Green:
                settings.primaryColour = GreenFaction[0];
                settings.secondaryColour = GreenFaction[1];
                break;
            default:
                break;
        }
        UpdateUI(); 
    }
}
