using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ThemePanel : MonoBehaviour
{

    public Image NameImage;
    public Image DiscriptionImage;
    public Image ScrollTextImage;
    public TMP_Text QuotesText;
    public Button BackButton;
    
    private Scrollbar _scrollbar;
    private int _currentQuote;
    private Coroutine _coroutine;
    private int _currentNumberTheme;
    private LangTheme _currentTheme;

    private float _timer;
    
    public void Init()
    {
        // _scrollbar = GetComponentInChildren<Scrollbar>(true);
        // BackButton.onClick.AddListener(OnBack);
        // _currentQuote = 0;
        // _timer = Time.time;
        // Hide();
    }

    private void Update()
    {
        if (gameObject.activeSelf && Time.time - _timer > 5f)
        {
            _timer = Time.time;
            ChangeQuote();
        }
    }

    public void Show(int numberTheme)
    {
        _timer = Time.time;
        _currentNumberTheme = numberTheme;
        ChangeLang();
        gameObject.SetActive(true);
        
        ChangeQuote();
    }

    private void ChangeQuote()
    {
        if (_coroutine != null)
        {
            StopCoroutine(_coroutine);
        }

        _coroutine = StartCoroutine(ShowQuoteCoroutine());
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }

    private void OnBack()
    {
        
    }

    public void ChangeLang()
    {
        if (GameManager.instance.CurrentLang == 1)
        {
            _currentTheme = GameManager.instance.LangUzbThemes[_currentNumberTheme];
        }
        else if (GameManager.instance.CurrentLang == 4)
        {
            _currentTheme = GameManager.instance.LangRusThemes[_currentNumberTheme];
        }

        NameImage.sprite = _currentTheme.Name;
        NameImage.SetNativeSize();
        DiscriptionImage.sprite = _currentTheme.Discription;
        DiscriptionImage.SetNativeSize();
        ScrollTextImage.sprite = _currentTheme.ScrollText;
        ScrollTextImage.SetNativeSize();
    }

    IEnumerator ShowQuoteCoroutine()
    {
        int lenght = QuotesText.text.Length;
        string str = QuotesText.text;
        for (int i = lenght-1; i >= 0; i--)
        {
            QuotesText.text = str.Substring(0, i);
            yield return new WaitForSeconds(GameManager.instance.SpeedAnimText);
        }

        _currentQuote++;
        if(_currentQuote >= _currentTheme.Quotes.Count)
            _currentQuote = 0;
        
        string quote = (_currentQuote+1).ToString();
        if (quote.Length == 1)
            quote = "0" + quote;
        string numberVideo = "0" + _currentNumberTheme + quote;
        GameManager.instance.MySendMessage(numberVideo);
        
        lenght = _currentTheme.Quotes[_currentQuote].Length;
        for (int i = 0; i <= lenght; i++)
        {
            QuotesText.text = _currentTheme.Quotes[_currentQuote].Substring(0, i);
            yield return new WaitForSeconds(GameManager.instance.SpeedAnimText);
        }
        _timer = Time.time;
    }
}
