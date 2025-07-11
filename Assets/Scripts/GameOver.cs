using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using Cursor = UnityEngine.Cursor;

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

    private void Start()
    {
        Cursor.visible = true;           // Hace visible el cursor
        Cursor.lockState = CursorLockMode.None;  // Desbloquea el cursor
    }

    private void onTryAgainClick (ClickEvent e)
    {
        Debug.Log("Se apretó Jugar de Nuevo");
        SceneManager.LoadScene("Laberinto");
        
    }

    private void onGoMenuClick(ClickEvent e)
    {
        Debug.Log("Se apretó Volver al Menu");
        //SceneManager.LoadScene(MainMenu);
    }


}
