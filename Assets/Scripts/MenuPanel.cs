using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
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
    
    public List<AnimTextClass> AllAnimTexts = new List<AnimTextClass>();
    private GameManager _manager;
    private Button _backButton;

    public void Init()
    {
        Debug.Log("Init");
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

        foreach (var animText in AllAnimTexts)
        {
            animText.Init();
        }

        _backButton = _manager.HomePanelButton.GetComponentInChildren<Button>(true);
        _backButton.onClick.AddListener(OnBackHome);

        Hide();
    }

    public void Show()
    {
        _manager.HomePanelButton.SetActive(true);
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        foreach (var button in AllButtons)
        {
            button.image.color = new Color(button.image.color.r, button.image.color.g, button.image.color.b, 0);
        }
        _manager.HomePanelButton.SetActive(false);
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
    
    public void OnAllButtons()
    {
        foreach (var button in Buttons)
        {
            button.enabled = true;
        }
    }

    private void OnBackHome()
    {
        OffAllButtons();
        _manager.OffButtonMenu();
        _backButton.image.DOFade(1f, 0.3f);
        _backButton.image.DOFade(0f, 0.3f).SetDelay(0.3f).OnComplete(GoBack);
    }

    private void GoBack()
    {
        if (_manager.CurrentThemePanel != null && _manager.CurrentThemePanel.IsActive)
        {
            _manager.CurrentThemePanel.Hide();
            StartCoroutine(ShowMenu());
        }
        else
        {
            Hide();
            OnAllButtons();
            _manager.OnButtonMenu();
        }
    }

    IEnumerator ShowMenu()
    {
        Debug.Log(_manager.SpeedAnimText);
        foreach (var animText in AllAnimTexts)
        {
            animText.textJuicer.SetProgress(0f);
            animText.textJuicer.Update();
            animText.ChangeLanguage(_manager.CurrentLang);
        }

        float progress = 0f;
        while (progress < 1f)
        {
            progress += Time.deltaTime * _manager.SpeedAnimText;
            foreach (var animText in AllAnimTexts)
            {
                animText.textJuicer.SetProgress(progress);
                animText.textJuicer.Update();
            }
            yield return null;
        }

        _manager.OnButtonMenu();
        OnAllButtons();
        _manager.OnButtonMenu();
    }

}
