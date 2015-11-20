using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class NuevoExistente : MonoBehaviour {
	public Toggle Exploracion;
	public Toggle Consulta;

	// Use this for initialization
    void Start()
    {
        PlayerPrefs.SetInt(Estado.SPO2, -1);
        PlayerPrefs.SetInt(Estado.RITMOCARDIACO, -1);
        PlayerPrefs.SetInt(Estado.GLU, -1);
    }

    public void nuevo()
    {
		if (Exploracion.isOn) {
			PlayerPrefs.SetString (Estado.EXPLORAR_CONSULTAR, Estado.EXPLORAR);
		} else {
			PlayerPrefs.SetString (Estado.EXPLORAR_CONSULTAR, Estado.CONSULTAR);
		}

		Estado.PushEscena ("Pacientes");
		Application.LoadLevel ("NuevoPaciente");
    }

    public void existente()
    {
		if (Exploracion.isOn) {
			PlayerPrefs.SetString (Estado.EXPLORAR_CONSULTAR, Estado.EXPLORAR);
		} else {
			PlayerPrefs.SetString (Estado.EXPLORAR_CONSULTAR, Estado.CONSULTAR);
		}
		Estado.PushEscena ("Pacientes");
		Application.LoadLevel ("PacienteExistente");
    }

}
