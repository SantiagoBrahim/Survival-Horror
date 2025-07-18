using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    
    public Text textoTiempo;

    private float tiempoTranscurrido = 0f;

    public string tiempoFormateado;

    private bool contando = true;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
    void Update()
    {
        if (contando && SceneManager.GetActiveScene().name != "WinMenu")
        {
            tiempoTranscurrido += Time.deltaTime;

            int minutos = Mathf.FloorToInt(tiempoTranscurrido / 60F);
            int segundos = Mathf.FloorToInt(tiempoTranscurrido % 60F);
            int milisegundos = Mathf.FloorToInt((tiempoTranscurrido * 1000) % 1000);

            textoTiempo.text = string.Format("{0:00}:{1:00},{2:000}", minutos, segundos, milisegundos);

            
        }
        else if(SceneManager.GetActiveScene().name != "WinMenu")
        {
            contando = false;
        }
    }

    public void OnDestroy()
    {
        contando = false;
    }

    public float ObtenerTiempoFinal()
    {
        return tiempoTranscurrido;
    }

    public string GetFormattedTime()

    {

        int minutos = Mathf.FloorToInt(tiempoTranscurrido / 60F);
        int segundos = Mathf.FloorToInt(tiempoTranscurrido % 60F);
        int milisegundos = Mathf.FloorToInt((tiempoTranscurrido * 1000) % 1000);
        return minutos.ToString("00") + ":" + segundos.ToString("00") + "." + milisegundos.ToString("00"); 
    }
}
