using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class StartPanel : MonoBehaviour
{
    
    public List<AnimTextClass> AllAnimTexts = new List<AnimTextClass>();
    
    public void Init()
    {
        AllAnimTexts = GetComponentsInChildren<AnimTextClass>(true).ToList();
        foreach (var animText in AllAnimTexts)
        {
            animText.Init();
        }
        
    }

    private void OnStart()
    {
        GameManager.instance.MenuPanel.Show();
    }

    public void ChangeLang()
    {
        
    }

    private void OnArab()
    {
        
    }

    private void OnEng()
    {
        
    }


}
