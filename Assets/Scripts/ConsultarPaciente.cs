using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class ConsultarPaciente : MonoBehaviour {
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
    private string ACTUALIZAR_url = "http://192.168.2.239/cgi-bin/actualizar_paciente.pl";
    
    // Use this for initialization
	void Start () {
		Identificacion.text = PlayerPrefs.GetString (Estado.PacienteCodigo);
		Nombre.text = PlayerPrefs.GetString (Estado.PacienteNombre);
		ApellidoP.text = PlayerPrefs.GetString (Estado.PacienteApellidoP);
		ApellidoM.text = PlayerPrefs.GetString (Estado.PacienteApellidoM);
		FechaNac.text = PlayerPrefs.GetString (Estado.PacienteFechaNac);
		Direccion.text = PlayerPrefs.GetString (Estado.PacienteDireccion);
		Telefono.text = PlayerPrefs.GetString (Estado.PacienteTelefono);
		Email.text = PlayerPrefs.GetString (Estado.PacienteEmail);
		NoParejas.text = PlayerPrefs.GetString (Estado.PacienteNoParejas);
		Area.value = Int32.Parse (PlayerPrefs.GetString (Estado.PacienteArea));
		Genero.value = Int32.Parse (PlayerPrefs.GetString (Estado.PacienteGenero));
		IVSA.text = PlayerPrefs.GetString (Estado.PacienteIVSA);
		IdMPF.value = Int32.Parse (PlayerPrefs.GetString (Estado.PacienteIdMPF));
	}

    public void llamar_actualizar()
    {
        StartCoroutine(actualizar());
    }
	
    private IEnumerator actualizar()
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

        WWW download = new WWW(ACTUALIZAR_url, form);

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
