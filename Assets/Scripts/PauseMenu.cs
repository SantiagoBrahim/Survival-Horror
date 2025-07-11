using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;


public class PauseMenu : MonoBehaviour
{
    private VisualElement root;
    private VisualElement PauseMenuContenedor;
    private Label EnPausa_Label;
    private Button Reanudar_Button;
    private Button VolverInicio_Button;

    private bool juegoPausado = false;

    void Start()
    {
        var uiDoc = GetComponent<UIDocument>();
        root = uiDoc.rootVisualElement;

        PauseMenuContenedor = root.Q<VisualElement>("PauseMenuContenedor");
        PauseMenuContenedor.style.display = DisplayStyle.None;

        root.Q<VisualElement>("Fondo").style.display = DisplayStyle.None;

        root.Q<Button>("Reanudar_Button").clicked += ContinuarJuego;

        root.Q<Button>("VolverInicio_Button").clicked += VolverInicio;


    }

     void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            if (!juegoPausado) { ContinuarJuego(); }
            else PausarJuego();

        }
    }

    void PausarJuego()
    {
        Time.timeScale = 0f;
        PauseMenuContenedor.style.display = DisplayStyle.Flex;



        root.Q<VisualElement>("Fondo").style.display= DisplayStyle.Flex;

        juegoPausado = true;

    }

    void ContinuarJuego()
    {
        Time.timeScale = 1f;
        PauseMenuContenedor.style.display = DisplayStyle.None;


        root.Q<VisualElement>("Fondo").style.display = DisplayStyle.None;

        juegoPausado = false;
    }

    void VolverInicio()
    {
        Time.timeScale = 1f;

        SceneManager.LoadScene("MainMenuDeluxe");

    }
}
