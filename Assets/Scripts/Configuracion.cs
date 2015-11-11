using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.IO.Ports;

public class Configuracion : MonoBehaviour
{
    string[] puertos;
    [SerializeField] Dropdown puertos_gui;

    // Use this for initialization
    void Start()
    {
        puertos = obtenerPuertos();

        if (puertos.Length == 0)
        {
            Debug.Log("No se detectaron puertos abiertos...");
        }

        for (int i = 0; i < puertos.Length; i++)
        {
            Dropdown.OptionData item = new Dropdown.OptionData(puertos[i]);
            puertos_gui.options.Add(item);
        }

        Dropdown.OptionData[] datos = puertos_gui.options.ToArray();
        puertos = new string[datos.Length];
        for (int i = 0; i < datos.Length; i++)
            puertos[i] = datos[i].text;
    }

    string[] obtenerPuertos()
    {
        return SerialPort.GetPortNames();
        //		return new string[] {"COM1", "COM2", "COM3"};
    }

    public void Empezar()
    {
        int index = puertos_gui.value;

        PlayerPrefs.SetString("Puerto", puertos[index]);
        PlayerPrefs.SetString(Estado.EstadoAnterior, "Configuracion");

        Debug.Log("Puerto Seleccionado: " + puertos[index]);

        Application.LoadLevel("Pacientes");
    }
}
