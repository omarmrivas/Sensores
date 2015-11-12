using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class AgregarPaciente : MonoBehaviour {
    public InputField   Identificacion;
    public InputField   Nombre;
    public InputField   ApellidoP;
    public InputField   ApellidoM;
    public InputField   FechaNac;
    public InputField   Direccion;
    public InputField   Telefono;
    public InputField   Email;
    public InputField   NoParejas;
    public Dropdown     Area;
    public Dropdown     Genero;
    public InputField   IVSA;
    public Dropdown     IdMPF;

    public Text         Error;

    // URLs
    private string AGREGAR_url = "http://192.168.2.239/cgi-bin/agregar_paciente.pl";
    
    // Use this for initialization
	void Start () {
	}

    public void llamar_agregar()
    {
        StartCoroutine(agregar());
    }
	
    private IEnumerator agregar()
    {
        WWWForm form = new WWWForm();
        form.AddField("Identificacion", Identificacion.text);
        form.AddField("Nombre", Nombre.text);
        form.AddField("ApellidoP", ApellidoP.text);
        form.AddField("ApellidoM", ApellidoM.text);
        form.AddField("FechaNac", FechaNac.text);
        form.AddField("Direccion", Direccion.text);
        form.AddField("Telefono", Telefono.text);
        form.AddField("Email", Email.text);
        form.AddField("NoParejas", NoParejas.text);
        form.AddField("Area", Area.value);
        form.AddField("Genero", Genero.value);
        form.AddField("IVSA", IVSA.text);
        form.AddField("IdMPF", IdMPF.value);

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
            if (download.text == "")
            {
                Debug.Log("Error en la creación del paciente...");
                Error.text = "Error en la creación del paciente...";
            }
            else
            {
                string[] campos = download.text.Split(':');
                PlayerPrefs.SetString(Estado.PacienteIdentificacion, campos[0]);
                yield return StartCoroutine(ECG_BPM_SPO2_GLU.crearExploracion());

                Application.LoadLevel("Adquisicion");
            }
        }
    }
}
