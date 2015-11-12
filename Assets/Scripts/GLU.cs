using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.IO.Ports;
using System.Threading;

public class GLU : MonoBehaviour {
    enum estado { NO_ACTIVIDAD, CALCULANDO, CALCULADO };

    public BarraProgreso barra;
    public Text Mensaje;
    public Button Empezar;

    private SerialPort puerto;
    private float tiempoInicial = 0f;

    private estado estado_puerto = estado.NO_ACTIVIDAD;
    private int GLUv = -1;
//    private int RITMOCARDIACOv = -1;

    // Use this for initialization
    void Start()
    {
        Mensaje.text = "Presione boton empezar...";
        barra.deshabilitar();
        estado_puerto = estado.NO_ACTIVIDAD;
    }

    // Update is called once per frame
    void Update()
    {
        if (estado_puerto == estado.CALCULANDO)
            barra.progreso = (Time.time - tiempoInicial) / 10.0f;
        if (estado_puerto == estado.CALCULADO)
        {
            PlayerPrefs.SetInt(Estado.GLU, GLUv);
            barra.progreso = 1.0f;
            estado_puerto = estado.NO_ACTIVIDAD;
            Empezar.interactable = true;
            Mensaje.text = "GLU = " + GLUv;
        }
    }

    public void GLU_gui()
    {
        if ((puerto = Lectura.iniciarPuerto()) != null)
        {
            tiempoInicial = Time.time;
            barra.habilitar();
            Mensaje.text = "Calculando GLU. Por favor, espere...";
            estado_puerto = estado.CALCULANDO;
            Empezar.interactable = false;

            Thread oThread = new Thread(new ThreadStart(GLU_comando));
            oThread.Start();
        }
        else { Debug.Log("Puerto no iniciado...");  }
    }

    public bool confirmacion()
    {
        int cfm = Lectura.leer(puerto, "CFM: ");

        if (cfm == Lectura.CFM)
        {
            int request = Lectura.leer(puerto, "Request: ");
            int uno = Lectura.leer(puerto, "UNO 1: ");

            return request == Lectura.GLU_START_MEASUREMENT &&
                              uno == 1;
        }
        return false;
    }

    public void GLU_comando()
    {
        byte[] mensaje = new byte[] { Lectura.REQ, Lectura.GLU_START_MEASUREMENT};
        puerto.Write(mensaje, 0, 2);

        Debug.Log("Datos enviados para GLU...");

        if (confirmacion())
        {
            int ok = Lectura.leer(puerto, "OK: ");

            if (ok == Lectura.ERROR_OK)
            {
                GLU_lectura();
            }
        }
    }

    private void GLU_lectura()
    {
        Debug.Log("Empezando a leer bloque GLU...");

        int ind = Lectura.leer(puerto, "IND: ");
        int m_ready = Lectura.leer(puerto, "M READY: ");
        if (ind == Lectura.IND && m_ready == Lectura.GLU_BLOOD_DETECTED)
        {
            int length = Lectura.leer(puerto, "LENGTH: ");

            ind = Lectura.leer(puerto, "IND: ");
            m_ready = Lectura.leer(puerto, "M READY: ");
            length = Lectura.leer(puerto, "LENGTH: ");

            // leer desde el dato 0 hasta el dato n
            int[] bloque = Lectura.leerBloque(puerto, length);

            Debug.Log("Datos: " + Lectura.ArrayToString(bloque));

            if (length == 2)
            {
                Debug.Log("GLU: " + bloque[1]);
                GLUv = bloque[1];

                estado_puerto = estado.CALCULADO;

                Lectura.cerrarPuerto(puerto);
            }
        }
    }
}
