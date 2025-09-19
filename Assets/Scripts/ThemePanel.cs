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
using Image = UnityEngine.UI.Image;

public class ThemePanel : MonoBehaviour
{
    
    public Sprite FirstSpriteRus;
    public Sprite SecondSpriteRus;
    public Sprite FirstSpriteUzb;
    public Sprite SecondSpriteUzb;

    private GameObject rusPanel;
    private GameObject uzbPanel;
    public GameObject rusPopap;
    public GameObject uzbPopap;
    public List<TMP_TextJuicer> textJuicersRus = new List<TMP_TextJuicer>();
    public List<TMP_TextJuicer> textJuicersUzb = new List<TMP_TextJuicer>();

    [HideInInspector] public bool IsActive;
    
    private Button b_DownRus;
    private Button b_DownUzb;
    private Button b_UpRus;
    private Button b_UpUzb;
    
    private ScrollRect ScrollViewRus;
    private ScrollRect ScrollViewUzb;

    private float _timer;
    private GameManager _manager;
    private Image _imageRu;
    private Image _imageUz;
    
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
        
        b_DownRus.onClick.AddListener(ShowPopap);
        b_DownUzb.onClick.AddListener(ShowPopap);
        b_UpUzb.onClick.AddListener(HidePopap);
        b_UpRus.onClick.AddListener(HidePopap);
        
        ScrollViewRus = rusPanel.GetComponentInChildren<ScrollRect>(true);
        ScrollViewUzb = uzbPanel.GetComponentInChildren<ScrollRect>(true);
        
        //Debug.Log(rusPanel.name + " - " + uzbPanel.name);
        _imageRu = rusPanel.GetComponent<Image>();
        _imageUz = uzbPanel.GetComponent<Image>();
        
        
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
        _imageUz.sprite = FirstSpriteUzb;
        _imageRu.sprite = FirstSpriteRus;

        rusPopap.SetActive(false);
        uzbPopap.SetActive(false);
        b_DownRus.gameObject.SetActive(true);
        b_DownUzb.gameObject.SetActive(true);
        b_UpUzb.gameObject.SetActive(false);
        b_UpRus.gameObject.SetActive(false);
    }

    private void ShowPopap()
    {
        _imageUz.sprite = SecondSpriteUzb;
        _imageRu.sprite = SecondSpriteRus;

        rusPopap.SetActive(true);
        uzbPopap.SetActive(true);
        b_DownRus.gameObject.SetActive(false);
        b_DownUzb.gameObject.SetActive(false);
        b_UpUzb.gameObject.SetActive(true);
        b_UpRus.gameObject.SetActive(true);
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
