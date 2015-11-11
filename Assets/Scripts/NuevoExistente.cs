using UnityEngine;
using System.Collections;

public class NuevoExistente : MonoBehaviour {

	// Use this for initialization
    void Start()
    {
        PlayerPrefs.SetInt(Estado.SPO2, -1);
        PlayerPrefs.SetInt(Estado.RITMOCARDIACO, -1);
    }

    public void nuevo()
    {
        PlayerPrefs.SetString(Estado.EstadoAnterior, "Pacientes");
        Application.LoadLevel("NuevoPaciente");
    }

    public void existente()
    {
        PlayerPrefs.SetString(Estado.EstadoAnterior, "Pacientes");
        Application.LoadLevel("PacienteExistente");
    }

}
