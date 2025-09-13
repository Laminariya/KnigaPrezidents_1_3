using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartPanel : MonoBehaviour
{
    
    public Button StartButton;
    
    public void Init()
    {
        
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
