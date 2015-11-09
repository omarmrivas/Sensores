using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class IngresarMedico : MonoBehaviour {
    public InputField Email;
    public InputField Password;
    public Text Error;

    // URLs
    private string MEDICOS_url = "http://192.168.2.239/cgi-bin/medicos.pl";

    public void Start()
    {
        PlayerPrefs.DeleteAll();
    }

    public void llamar_ingresar()
    {
        StartCoroutine(ingresar());
    }

    public IEnumerator ingresar()
    {
        WWWForm form = new WWWForm();
        form.AddField("Email", Email.text);
        form.AddField("Password", Password.text);

        WWW download = new WWW(MEDICOS_url, form);

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
                Debug.Log("Error en Correo o contraseña...");
                Error.text = "Error en Correo o contraseña...";
            }
            else
            {
                string[] campos = download.text.Split(':');
                PlayerPrefs.SetString(Estado.MedicoEmail, Email.text);
                PlayerPrefs.SetString(Estado.MedicoPassword, Password.text);
                PlayerPrefs.SetString(Estado.MedicoId, campos[0]);
                PlayerPrefs.SetString(Estado.MedicoNombre, campos[1]);
                PlayerPrefs.SetString(Estado.MedicoApellidoP, campos[2]);
                PlayerPrefs.SetString(Estado.MedicoApellidoM, campos[3]);
            }
        }
    }
}
