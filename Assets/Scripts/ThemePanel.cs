using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BrunoMikoski.TextJuicer;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;

public class ThemePanel : MonoBehaviour
{
    
    public Sprite FirstSpriteRus;
    public Sprite SecondSpriteRus;
    public Sprite FirstSpriteUzb;
    public Sprite SecondSpriteUzb;

    private GameObject rusPanel;
    private GameObject uzbPanel;
    private List<TMP_TextJuicer> textJuicersRus = new List<TMP_TextJuicer>();
    private List<TMP_TextJuicer> textJuicersUzb = new List<TMP_TextJuicer>();

    private Button b_DownRus;
    private Button b_DownUzb;
    private Button b_UpRus;
    private Button b_UpUzb;
    
    private ScrollRect ScrollViewRus;
    private ScrollRect ScrollViewUzb;

    private float _timer;
    private GameManager _manager;
    
    public void Init()
    {
        _manager = GameManager.instance;

        rusPanel = transform.GetChild(0).gameObject;
        rusPanel.SetActive(false);
        uzbPanel = transform.GetChild(1).gameObject;
        uzbPanel.SetActive(false);
        
        textJuicersRus = rusPanel.GetComponentsInChildren<TMP_TextJuicer>().ToList();
        textJuicersUzb = uzbPanel.GetComponentsInChildren<TMP_TextJuicer>().ToList();
        
        Button[] buttons = uzbPanel.GetComponentsInChildren<Button>();
        b_DownUzb = buttons[0];
        b_UpUzb = buttons[1];
        Button[] buttons2 = rusPanel.GetComponentsInChildren<Button>();
        b_DownRus = buttons2[0];
        b_UpRus = buttons2[1];
        
        ScrollViewRus = rusPanel.GetComponentInChildren<ScrollRect>(true);
        ScrollViewUzb = uzbPanel.GetComponentInChildren<ScrollRect>(true);
        
    }

    public void Show(Button button)
    {
        Debug.Log(name);
        _manager.MenuPanel.OffAllButtons();
        button.image.DOFade(1f, 0.3f);
        button.image.DOFade(0f, 0.3f).SetDelay(0.3f).OnComplete(StartShowCor);
    }

    private void StartShowCor()
    {
        StartCoroutine(ShowCoroutine());
    }

    IEnumerator ShowCoroutine()
    {
        if (_manager.CurrentLang == 0)
        {
            uzbPanel.SetActive(true);
        }

        if (_manager.CurrentLang == 1)
        {
            rusPanel.SetActive(true);
        }

        foreach (var juicer in textJuicersUzb)
        {
            juicer.SetProgress(0f);
            juicer.Update();
        }
        foreach (var juicer in textJuicersRus)
        {
            juicer.SetProgress(0f);
            juicer.Update();
        }
        
        float progress = 0f;
        while (progress<1f)
        {
            progress += Time.deltaTime*_manager.SpeedAnimText;
            foreach (var juicer in textJuicersUzb)
            {
                juicer.SetProgress(progress);
                juicer.Update();
            }
            foreach (var juicer in textJuicersRus)
            {
                juicer.SetProgress(progress);
                juicer.Update();
            }
            yield return null;
        }
    }


    private void Hide()
    {
        gameObject.SetActive(false);
    }

    public void ChangeLang()
    {
        if (GameManager.instance.CurrentLang == 0)
        {
            
        }
        else if (GameManager.instance.CurrentLang == 1)
        {
            
        }
    }
    
}
