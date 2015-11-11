using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BuscarPaciente : MonoBehaviour {
    public InputField Identificacion;
    public InputField Nombre;
    public InputField ApellidoP;
    public InputField ApellidoM;
    public InputField FechaNac;

    public Text Error;

    public Toggle BuscarPorClave;
    public Toggle BuscarPorNombre;

    public Button Buscar;

    // URLs
    private string BUSCAR_url = "http://192.168.2.239/cgi-bin/buscar_paciente.pl";

    // Use this for initialization
    void Start()
    {
        Identificacion.interactable = false;
        Nombre.interactable = false;
        ApellidoP.interactable = false;
        ApellidoM.interactable = false;
        FechaNac.interactable = false;
        Buscar.interactable = false;
    }

    public void actualizarCampos()
    {
        if (BuscarPorClave.isOn)
        {
            Identificacion.interactable = true;
            Buscar.interactable = true;

            Nombre.interactable = false;
            ApellidoP.interactable = false;
            ApellidoM.interactable = false;
            FechaNac.interactable = false;
        }
 
        if (BuscarPorNombre.isOn)
        {
            Buscar.interactable = true;

            Nombre.interactable = true;
            ApellidoP.interactable = true;
            ApellidoM.interactable = true;
            FechaNac.interactable = true;

            Identificacion.interactable = false;
        }
    }

    public void llamar_buscar()
    {
        StartCoroutine(buscar());
    }

    private IEnumerator buscar()
    {
        WWWForm form = new WWWForm();
        if (BuscarPorClave.isOn)
        {
            form.AddField("Identificacion", Identificacion.text);
        }
        else
        {
            form.AddField("Nombre", Nombre.text);
            form.AddField("ApellidoP", ApellidoP.text);
            form.AddField("ApellidoM", ApellidoM.text);
            form.AddField("FechaNac", FechaNac.text);
        }

        WWW download = new WWW(BUSCAR_url, form);

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
            if (download.text == "")
            {
                Debug.Log("No se encontró el paciente...");
                Error.text = "No se encontró el paciente...";
            }
            else
            {
                string[] campos = download.text.Split(':');
                PlayerPrefs.SetString(Estado.PacienteIdentificacion, campos[0]);
                PlayerPrefs.SetString(Estado.PacienteApellidoP, campos[2]);
                PlayerPrefs.SetString(Estado.PacienteApellidoM, campos[3]);
                PlayerPrefs.SetString(Estado.PacienteNombre, campos[4]);

                Application.LoadLevel("Adquisicion");
            }
        }
    }
}
