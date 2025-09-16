using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    public GameObject HomePanelButton;
    

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
        
    }

    private void OnUzb()
    {
        CurrentLang = 0;
        if (!MenuPanel.gameObject.activeSelf)
        {
            MenuPanel.Show();
        }
    }

    private void OnRus()
    {
        CurrentLang = 1;
        if (!MenuPanel.gameObject.activeSelf)
        {
            MenuPanel.Show();
        }
    }

    public void ChangeLang()
    {
        MenuPanel.ChangeLang();
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
