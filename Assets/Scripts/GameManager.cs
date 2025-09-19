using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    
    public static GameManager instance;

    public float SpeedAnimText;
    public MenuPanel MenuPanel;
    public StartPanel StartPanel;
    
    [HideInInspector] public ClientUDP ClientUdp;
    public GameObject Border;
    public int CurrentLang;
    
    public Button Lang_Uzb;
    public Button Lang_Rus;
    public Button b_Back;

    public GameObject HomePanelButton;
    public ThemePanel CurrentThemePanel;

    private Coroutine _coroutineClickButton;
    
    private void Awake()
    {
        if(instance == null)
            instance = this;
    }

    void Start()
    {
        ClientUdp = GetComponent<ClientUDP>();
        CurrentLang = 1;
        MenuPanel.Init();
        StartPanel.Init();
        ClientUdp.Init();
        Border.SetActive(false);
        Lang_Uzb.onClick.AddListener(OnUzb);
        Lang_Rus.onClick.AddListener(OnRus);
        HomePanelButton.SetActive(false);
        b_Back = HomePanelButton.GetComponentInChildren<Button>();
    }

    private void OnUzb()
    {
        CurrentLang = 0;
        
        OffButtonMenu();
        Lang_Uzb.image.DOFade(1f, 0.3f);
        Lang_Uzb.image.DOFade(0f, 0.3f).SetDelay(0.3f).OnComplete(StartShowCor);
    }

    private void OnRus()
    {
        CurrentLang = 1;
        
        OffButtonMenu();
        Lang_Rus.image.DOFade(1f, 0.3f);
        Lang_Rus.image.DOFade(0f, 0.3f).SetDelay(0.3f).OnComplete(StartShowCor);
        
    }

    private void StartShowCor()
    {
        if (!MenuPanel.gameObject.activeSelf)
        {
            MenuPanel.Show();
        }

        StartCoroutine(ChangeLangCoroutine());
    }

    IEnumerator ChangeLangCoroutine()
    {
        float progress = 0f;
        if (CurrentThemePanel != null && CurrentThemePanel.IsActive)
        {
            progress = 1f;
            while (progress > 0f)
            {
                progress -= Time.deltaTime * SpeedAnimText;
                foreach (var animText in CurrentThemePanel.textJuicersUzb)
                {
                    animText.SetProgress(progress);
                    animText.Update();
                }
                foreach (var animText in CurrentThemePanel.textJuicersRus)
                {
                    animText.SetProgress(progress);
                    animText.Update();
                }
                yield return null;
            }
            CurrentThemePanel.ChangeLang();
            progress = 0f;
            while (progress < 1f)
            {
                progress += Time.deltaTime * SpeedAnimText;
                foreach (var animText in CurrentThemePanel.textJuicersUzb)
                {
                    animText.SetProgress(progress);
                    animText.Update();
                }
                foreach (var animText in CurrentThemePanel.textJuicersRus)
                {
                    animText.SetProgress(progress);
                    animText.Update();
                }
                yield return null;
            }
            
            OnButtonMenu();
            yield break;
        }

        foreach (var animText in MenuPanel.AllAnimTexts)
        {
            animText.ChangeLanguage(CurrentLang);
            animText.textJuicer.SetProgress(0);
            animText.textJuicer.Update();
        }
        progress = 0f;
        while (progress < 1f)
        {
            progress += Time.deltaTime * SpeedAnimText;
            foreach (var animText in MenuPanel.AllAnimTexts)
            {
                animText.textJuicer.SetProgress(progress);
                animText.textJuicer.Update();
            }
            yield return null;
        }
        
        OnButtonMenu();
    }

    public void OffButtonMenu()
    {
        Lang_Rus.enabled = false;
        Lang_Uzb.enabled = false;
        b_Back.enabled = false;
    }

    public void OnButtonMenu()
    {
        Lang_Rus.enabled = true;
        Lang_Uzb.enabled = true;
        b_Back.enabled = true;
    }


    public void MySendMessage(string number)
    {
        string message =
            "{\"jsonrpc\":\"2.0\", \"id\":39, \"method\":\"Pixera.Compound.applyCueOnTimeline\", \"params\":{\"timelineName\":\"alphaarea1\", \"cueName\":\"";
        //RU0001
        switch (CurrentLang)
        {
            case 0:
            {
                message += "UZ";
                break;
            }
            case 1:
            {
                message += "RU";
                break;
            }
        }
        message += number;

        message += "\", \"blendDuration\":1}}";
        Debug.Log(message);
        ClientUdp.AddMessage(message);
    }

}

[Serializable]
public class LangTheme
{
    public Sprite Name;
    public Sprite Discription;
    public Sprite ScrollText;
    public List<string> Quotes = new List<string>();
}
