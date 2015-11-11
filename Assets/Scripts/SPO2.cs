using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.IO.Ports;
using System.Threading;

public class SPO2 : MonoBehaviour {
    enum estado {NO_ACTIVIDAD, CALCULANDO, CALCULADO};

    public BarraProgreso barra;
    public Text Mensaje;
    public Button Empezar;

    private SerialPort puerto;
    private float tiempoInicial = 0f;

    private estado estado_puerto = estado.NO_ACTIVIDAD;
    private int SPO2v = -1;
    private int RITMOCARDIACOv = -1;

	// Use this for initialization
	void Start () {
        Mensaje.text = "Presione boton empezar...";
        barra.deshabilitar();
        estado_puerto = estado.NO_ACTIVIDAD;
	}
	
	// Update is called once per frame
	void Update () {
        if (estado_puerto == estado.CALCULANDO)
            barra.progreso = (Time.time - tiempoInicial) / 10.0f;
        if (estado_puerto == estado.CALCULADO)
        {
            PlayerPrefs.SetInt(Estado.SPO2, SPO2v);
            PlayerPrefs.SetInt(Estado.RITMOCARDIACO, RITMOCARDIACOv);
            barra.progreso = 1.0f;
            estado_puerto = estado.NO_ACTIVIDAD;
            Empezar.interactable = true;
            Mensaje.text = "SPO2 = " + SPO2v + "\n" +
                           "Ritmo Cardiaco = " + RITMOCARDIACOv;
        }
	}

    public void SPO2_gui()
    {
        if ((puerto = Lectura.iniciarPuerto()) != null)
        {
            tiempoInicial = Time.time;
            barra.habilitar();
            Mensaje.text = "Calculando SPO2. Por favor, espere...";
            estado_puerto = estado.CALCULANDO;
            Empezar.interactable = false;

            Thread oThread = new Thread(new ThreadStart(SPO2_comando));
            oThread.Start();
        }
    }

    public void SPO2_comando()
    {
        byte[] mensaje = new byte[] { Lectura.REQ, Lectura.POX_START_MEASUREMENT };
        puerto.Write(mensaje, 0, 2);

        Debug.Log("Datos enviados para SPO2...");

        if (Lectura.confirmacion(puerto))
        {
            int ind = Lectura.leer(puerto, "IND (Packet Type): ");

            if (ind == Lectura.IND)
            {
                SPO2_lectura();
            }
        }
    }

    private void SPO2_lectura()
    {
        Debug.Log("Empezando a leer bloque SPO2...");

        int command_opcode = Lectura.leer(puerto, "Command Opcode: ");
        if (command_opcode == Lectura.POX_MEASUREMENT_COMPLETE_OK)
        {
            int length = Lectura.leer(puerto, "LENGTH: ");

            // leer desde el dato 0 hasta el dato n
            int[] bloque = Lectura.leerBloque(puerto, length);

            Debug.Log("Datos: " + Lectura.ArrayToString(bloque));

            if (length == 2)
            {
                Debug.Log("SPO2: " + bloque[0]);
                Debug.Log("Heart Rate: " + bloque[1]);
                SPO2v = bloque[0];
                RITMOCARDIACOv = bloque[1];

                estado_puerto = estado.CALCULADO;

                Lectura.cerrarPuerto(puerto);
//                StartCoroutine(SPO2_DB(10, 1, bloque[0]));
//                StartCoroutine(SPO2_DB(10, 2, bloque[1]));
            }
        }
    }
}
