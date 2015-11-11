using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ECG_BPM_SPO2_GLU : MonoBehaviour {
    public Button ECG;
    public Button BPM;
    public Button SPO2;
    public Button GLU;

    public Text Paciente;
    public Text Texto;

	// Use this for initialization
	void Start () {
        ECG.interactable = false;
        BPM.interactable = false;
        SPO2.interactable = true;
        GLU.interactable = true;

        actualizarTexto();
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    private void actualizarTexto()
    {
        string m = "";
        if (PlayerPrefs.HasKey(Estado.SPO2) && PlayerPrefs.GetInt(Estado.SPO2) != -1)
        {
            m += "SPO2=" + PlayerPrefs.GetInt(Estado.SPO2) + " ";
        }
        if (PlayerPrefs.HasKey(Estado.RITMOCARDIACO) && PlayerPrefs.GetInt(Estado.RITMOCARDIACO) != -1)
        {
            if (m != "")
                m += ", Ritmo Cardiaco="+PlayerPrefs.GetInt(Estado.RITMOCARDIACO) + " ";
            else m += "Ritmo Cardiaco=" + PlayerPrefs.GetInt(Estado.RITMOCARDIACO) + " ";
        }

        Texto.text = m;

        Paciente.text = "Paciente: " + 
                        PlayerPrefs.GetString(Estado.PacienteNombre) + " " +
                        PlayerPrefs.GetString(Estado.PacienteApellidoP) + " " +
                        PlayerPrefs.GetString(Estado.PacienteApellidoM);
    }

    public void seleccionarSPO2()
    {
        PlayerPrefs.SetString(Estado.EstadoAnterior, "Adquisicion");
        Application.LoadLevel("SPO2");
    }

    public void seleccionarGLU()
    {
        PlayerPrefs.SetString(Estado.EstadoAnterior, "Adquisicion");
        Application.LoadLevel("GLU");
    }
}
