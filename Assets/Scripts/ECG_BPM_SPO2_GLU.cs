using System;
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

    // URLs
    private static string CREAR_url = "http://192.168.2.239/cgi-bin/crear_exploracion.pl";
    private static string AGREGAR_url = "http://192.168.2.239/cgi-bin/agregar_lectura.pl";

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
        if (PlayerPrefs.HasKey(Estado.GLU) && PlayerPrefs.GetInt(Estado.GLU) != -1)
        {
            if (m != "")
                m += ", GLU=" + PlayerPrefs.GetInt(Estado.GLU) + " ";
            else m += "GLU=" + PlayerPrefs.GetInt(Estado.GLU) + " ";
        }

        Texto.text = m;

        Paciente.text = "Paciente: " + 
                        PlayerPrefs.GetString(Estado.PacienteNombre) + " " +
                        PlayerPrefs.GetString(Estado.PacienteApellidoP) + " " +
                        PlayerPrefs.GetString(Estado.PacienteApellidoM);
    }

    public void seleccionarSPO2()
    {
//        PlayerPrefs.SetString(Estado.EstadoAnterior, "Adquisicion");
        Estado.PushEscena("Adquisicion");
        Application.LoadLevel("SPO2");
    }

    public void seleccionarGLU()
    {
//        PlayerPrefs.SetString(Estado.EstadoAnterior, "Adquisicion");
        Estado.PushEscena("Adquisicion");
        Application.LoadLevel("GLU");
    }

    public static IEnumerator crearExploracion()
    {
        WWWForm form = new WWWForm();
        form.AddField(Estado.PacienteIdentificacion, PlayerPrefs.GetString(Estado.PacienteIdentificacion));
        form.AddField(Estado.MedicoId, PlayerPrefs.GetString(Estado.MedicoId));

        WWW download = new WWW(CREAR_url, form);

        // Wait until the download is done
        yield return download;

        if (!string.IsNullOrEmpty(download.error))
        {
            Debug.Log("Error downloading: " + download.error);
        }
        else
        {
            // Mostrar resultado
            Debug.Log("crearExploracion: " + download.text);
            if (download.text == "")
            {
                Debug.Log("Error en la creación de la exploracion...");
//                Error.text = "Error en la creación del paciente...";
            }
            else
            {
                string[] campos = download.text.Split(':');
                PlayerPrefs.SetInt(Estado.ExploracionId, Int32.Parse(campos[0]));
            }
        }
    }

    public void guardarLecturas()
    {
        int IdExploracion = PlayerPrefs.GetInt(Estado.ExploracionId);
        if (PlayerPrefs.HasKey(Estado.SPO2) && PlayerPrefs.GetInt(Estado.SPO2) != -1)
        {
            StartCoroutine(guardarLectura(IdExploracion, Estado.ID_SPO2, PlayerPrefs.GetInt(Estado.SPO2)));
        }
        if (PlayerPrefs.HasKey(Estado.RITMOCARDIACO) && PlayerPrefs.GetInt(Estado.RITMOCARDIACO) != -1)
        {
            StartCoroutine(guardarLectura(IdExploracion, Estado.ID_FREC_CARDIACA, PlayerPrefs.GetInt(Estado.RITMOCARDIACO)));
        }
        if (PlayerPrefs.HasKey(Estado.GLU) && PlayerPrefs.GetInt(Estado.GLU) != -1)
        {
            StartCoroutine(guardarLectura(IdExploracion, Estado.ID_GLU, PlayerPrefs.GetInt(Estado.GLU)));
        }
    }

    public IEnumerator guardarLectura(int IdExploracion, int IdVariable, object Valor)
    {
        Debug.Log("Guardando Lectura: " + IdExploracion + ", " + IdVariable + ", " + Valor);
        WWWForm form = new WWWForm();
        form.AddField("IdExploracion", IdExploracion.ToString());
        form.AddField("IdVariable", IdVariable.ToString());
        form.AddField("Valor", Valor.ToString());

        WWW download = new WWW(AGREGAR_url, form);

        // Wait until the download is done
        yield return download;

        if (!string.IsNullOrEmpty(download.error))
        {
            Debug.Log("Error downloading: " + download.error);
        }
        else
        {
            // Mostrar resultado
            Debug.Log(download.text);
        }
    }

}
