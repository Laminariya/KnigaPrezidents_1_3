using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class MenuPanel : MonoBehaviour
{
    
    public List<Button> Buttons = new List<Button>();
    public Button BackButton;
    public Sprite Rus;
    public Sprite Uzb;
    
    private Image _image;
    private int _index;

    public void Init()
    {
        // for (int i = 0; i < Buttons.Count; i++)
        // {
        //     int j = i;
        //     Buttons[i].onClick.AddListener(() => OnClickTheme(Buttons[j],j));
        // }
        //
        // _image = GetComponent<Image>();
        // BackButton.onClick.AddListener(OnBack);
        // ChangeLang();
        // Hide();
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    private void OnBack()
    {
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    private void OnClickTheme(Button button,int theme)
    {
        _index = theme;
    }

    private void ShowTheme()
    {
        int theme = _index;
        GameManager.instance.ThemePanel.Show(theme);
    }

    public void ChangeLang()
    {
        if (GameManager.instance.CurrentLang == 1)
        {
            _image.sprite = Uzb;
        }
        else if (GameManager.instance.CurrentLang == 4)
        {
            _image.sprite = Rus;
        }
    }


}
