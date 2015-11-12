using UnityEngine;
using System.Collections;

public class IrAtras : MonoBehaviour {

    public void irAtras()
    {
//        string escena = PlayerPrefs.GetString(Estado.EstadoAnterior, "");
        string escena = Estado.PopEscena();
        Application.LoadLevel(escena);
    }

}
