using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class MainMenuDeluxe : MonoBehaviour
{

    private VisualElement root;
    private VisualElement MenuContenedor;

    // Start is called before the first frame update
    void Start()
    {
        var uiDoc = GetComponent<UIDocument>();
        root = uiDoc.rootVisualElement;

        MenuContenedor = root.Q<VisualElement>("MenuContenedor");

        root.Q<Button>("Jugar_Button").clicked += IniciarJuego;
        root.Q<Button>("Salir_Button").clicked += SalirJuego;
    }



    // Update is called once per frame
    void Update()
    {
        
    }

    void IniciarJuego()
    {
        Debug.Log("Se inicio el juego");
        SceneManager.LoadScene("Laberinto");
    }

    void SalirJuego()
    {
        Debug.Log("Salir del juego");

        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
