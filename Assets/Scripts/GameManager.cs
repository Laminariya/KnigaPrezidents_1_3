using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    
    public static GameManager instance;

    public float SpeedAnimText;
    public float SpeedAnimColor;
    public MenuPanel MenuPanel;
    public StartPanel StartPanel;
    public ThemePanel ThemePanel;
    public ClientUDP ClientUdp;
    public GameObject Border;
    public int CurrentLang;
    
    public Button Lang_Uzb;
    public Button Lang_Rus;
    
    public List<LangTheme> LangRusThemes = new List<LangTheme>();
    public List<LangTheme> LangUzbThemes = new List<LangTheme>();

    private Coroutine _coroutineClickButton;
    
    private void Awake()
    {
        if(instance == null)
            instance = this;
    }

    void Start()
    {
        CurrentLang = 4;
        MenuPanel.Init();
        StartPanel.Init();
        ThemePanel.Init();
        ClientUdp.Init();
        Border.SetActive(false);
        Lang_Uzb.onClick.AddListener(OnUzb);
        Lang_Rus.onClick.AddListener(OnRus);
    }

    private void OnUzb()
    {
        CurrentLang = 0;
    }


    private void OnRus()
    {
        CurrentLang = 1;
    }

    public void ChangeLang()
    {
        MenuPanel.ChangeLang();
        ThemePanel.ChangeLang();
        StartPanel.ChangeLang();
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
