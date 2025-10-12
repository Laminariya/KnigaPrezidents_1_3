using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BrunoMikoski.TextJuicer;
using DG.Tweening;
using UnityEngine;
using Button = UnityEngine.UI.Button;

public class ThemePanel : MonoBehaviour
{

    private GameObject rusPanel;
    private GameObject uzbPanel;
    public List<TMP_TextJuicer> textJuicersRus = new List<TMP_TextJuicer>();
    public List<TMP_TextJuicer> textJuicersUzb = new List<TMP_TextJuicer>();

    [HideInInspector] public bool IsActive;
    
    public Button b_DownRus;
    public Button b_DownUzb;
    public Button b_UpRus;
    public Button b_UpUzb;

    private float _timer;
    private GameManager _manager;

    public float StartY;
    public float EndY;
    public float StartY_Uzb;
    public float EndY_Uzb;
    public float Heght;
    public RectTransform RusPanel;
    public RectTransform UzbPanel;
    public Transform ButtonPointRus;
    public Transform ButtonPointUzb;
    public int NumberSend;
    
    public void Init()
    {
        _manager = GameManager.instance;
        
        rusPanel = transform.GetChild(0).gameObject;
        rusPanel.SetActive(false);
        uzbPanel = transform.GetChild(1).gameObject;
        uzbPanel.SetActive(false);
        
        textJuicersRus = rusPanel.GetComponentsInChildren<TMP_TextJuicer>().ToList();
        textJuicersUzb = uzbPanel.GetComponentsInChildren<TMP_TextJuicer>().ToList();
        
        b_DownRus.onClick.AddListener(ShowPopap);
        b_DownUzb.onClick.AddListener(ShowPopap);
        b_UpUzb.onClick.AddListener(HidePopap);
        b_UpRus.onClick.AddListener(HidePopap);
    }

    private void Update()
    {
        if (IsActive)
        {
            b_DownRus.transform.position = ButtonPointRus.position;
            b_DownUzb.transform.position = ButtonPointUzb.position;
        }
    }

    public void Show(Button button)
    {
        IsActive = true;
        //Debug.Log(name);
        _manager.CurrentThemePanel = this;
        _manager.MenuPanel.OffAllButtons();
        button.image.DOFade(1f, 0.3f);
        button.image.DOFade(0f, 0.3f).SetDelay(0.3f).OnComplete(StartShowCor);
        HidePopap();
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
            progress += Time.deltaTime * _manager.SpeedAnimText;
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
            //Debug.Log("ShowTheme");
            yield return null;
        }
    }

    public void Hide()
    {
        IsActive = false;
        uzbPanel.SetActive(false);
        rusPanel.SetActive(false);
    }

    private void HidePopap()
    {
        b_UpUzb.enabled = false;
        b_UpRus.enabled = false;
        b_DownRus.enabled = false;
        b_DownUzb.enabled = false;
        
        RusPanel?.DOLocalMove(new Vector3(RusPanel.localPosition.x, StartY, 0), 0.5f);
        RusPanel?.DOSizeDelta(new Vector2(RusPanel.sizeDelta.x, 0), 0.5f).OnComplete(ActivateUpButton);
        UzbPanel?.DOLocalMove(new Vector3(UzbPanel.localPosition.x, StartY_Uzb, 0), 0.5f);
        UzbPanel?.DOSizeDelta(new Vector2(UzbPanel.sizeDelta.x, 0), 0.5f).OnComplete(ActivateUpButton);
        
        b_UpUzb.gameObject.SetActive(false);
        b_UpRus.gameObject.SetActive(false);
    }

    private void ShowPopap()
    {
        _manager.MySendMessage("0"+NumberSend+"01");
        b_UpUzb.enabled = false;
        b_UpRus.enabled = false;
        b_DownRus.enabled = false;
        b_DownUzb.enabled = false;

        RusPanel?.DOLocalMove(new Vector3(RusPanel.localPosition.x, EndY, 0), 0.5f);
        RusPanel?.DOSizeDelta(new Vector2(RusPanel.sizeDelta.x, Heght), 0.5f).OnComplete(ActivateUpButton);
        UzbPanel?.DOLocalMove(new Vector3(UzbPanel.localPosition.x, EndY_Uzb, 0), 0.5f);
        UzbPanel?.DOSizeDelta(new Vector2(UzbPanel.sizeDelta.x, Heght), 0.5f).OnComplete(ActivateUpButton);
        
        b_UpUzb.gameObject.SetActive(true);
        b_UpRus.gameObject.SetActive(true);
    }

    private void ActivateUpButton()
    {
        b_UpUzb.enabled = true;
        b_UpRus.enabled = true;
        b_DownRus.enabled = true;
        b_DownUzb.enabled = true;
    }

    public void ChangeLang()
    {
        if (GameManager.instance.CurrentLang == 0)
        {
            uzbPanel.SetActive(true);
            rusPanel.SetActive(false);
        }
        else if (GameManager.instance.CurrentLang == 1)
        {
            uzbPanel.SetActive(false);
            rusPanel.SetActive(true);
        }
    }
    
}
