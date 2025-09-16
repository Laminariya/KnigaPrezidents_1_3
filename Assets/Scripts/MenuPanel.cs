using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class MenuPanel : MonoBehaviour
{
    
    public List<Button> Buttons = new List<Button>();
    public Button BackButton;
    
    public GameObject AllButtonsParent;
    public GameObject AllThemeParent;
    
    private List<ThemePanel> AllThemePanels = new List<ThemePanel>();
    private List<Button> AllButtons = new List<Button>();
    
    private List<AnimTextClass> AllAnimTexts = new List<AnimTextClass>();
    private GameManager _manager;

    public void Init()
    {
        _manager = GameManager.instance;
        AllAnimTexts = AllButtonsParent.GetComponentsInChildren<AnimTextClass>(true).ToList();
        AllThemePanels = AllThemeParent.GetComponentsInChildren<ThemePanel>().ToList();
        AllButtons = AllButtonsParent.GetComponentsInChildren<Button>().ToList();

        for (int i = 0; i < AllThemePanels.Count; i++)
        {
            var i1 = i;
            AllButtons[i].onClick.AddListener(()=>AllThemePanels[i1].Show(AllButtons[i1]));
            AllThemePanels[i].Init();
        }

        Hide();
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        foreach (var button in AllButtons)
        {
            button.image.color = new Color(button.image.color.r, button.image.color.g, button.image.color.b, 0);
        }
        gameObject.SetActive(false);
    }

    public void ChangeLang()
    {
        foreach (var animText in AllAnimTexts)
        {
            animText.ChangeLanguage(_manager.CurrentLang);
        }
    }

    public void OffAllButtons()
    {
        foreach (var button in Buttons)
        {
            button.enabled = false;
        }
    }


}
