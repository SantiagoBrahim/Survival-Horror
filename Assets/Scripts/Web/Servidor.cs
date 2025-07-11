using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Servidor", menuName = "ChickenJockey/Servidor", order = 1)]
public class Servidor : ScriptableObject
{
    public string servidor;
    public Servicio[] servicio;
}

[System.Serializable]
public class Servicio
{
    public string nombre;
    public string URL;
    public string[] parametros;
}