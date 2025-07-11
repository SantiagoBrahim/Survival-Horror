using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
//using MySql.Data.MySqlClient;


public class WinMenu : MonoBehaviour
{
    public UIDocument document;
    public Timer timer;

    private void OnEnable()
    {
        VisualElement root = document.rootVisualElement;

        Label tiempoFinalLabel = root.Q<Label>("Tiempo_Label");

        if (tiempoFinalLabel != null && timer != null)
        {
            tiempoFinalLabel.text = $"Escapaste en:{timer.GetFormattedTime()}";
        }
    }


}
