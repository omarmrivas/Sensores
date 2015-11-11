using UnityEngine;
using System.Collections;

public class IrAtras : MonoBehaviour {

    public void irAtras()
    {
        string escena = PlayerPrefs.GetString(Estado.EstadoAnterior, "");
        if (escena != "")
        {
            Application.LoadLevel(escena);
        }
    }

}
