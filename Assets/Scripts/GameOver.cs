using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GameOver : MonoBehaviour
{

    private UIDocument _document;

    private Button _TryAgain;

    private Button _GoMenu;

    private void Awake()
    {
        _document = GetComponent<UIDocument>();

        _TryAgain = _document.rootVisualElement.Q("TryAgainButton") as Button;
        _GoMenu = _document.rootVisualElement.Q("GoMenuButton") as Button;

        _TryAgain.RegisterCallback<ClickEvent>(onTryAgainClick);
        _GoMenu.RegisterCallback<ClickEvent>(onGoMenuClick);
    }

    private void onTryAgainClick (ClickEvent e)
    {
        Debug.Log("Se apretó Jugar de Nuevo");
        
    }

    private void onGoMenuClick(ClickEvent e)
    {
        Debug.Log("Se apretó Volver al Menu");
        //SceneManager.LoadScene(MainMenu);
    }


}
