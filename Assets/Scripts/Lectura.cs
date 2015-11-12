using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO.Ports;

public class Lectura : MonoBehaviour {
    SerialPort puerto;
    bool DETENER_BANDERA = false;

    bool ECG_LECTURA = false;
    bool SPO2_LECTURA = false;

    public static readonly byte REQ = 0x52;
    public static readonly byte CFM = 0x43;
    public static readonly byte IND = 0x69;

    public static readonly byte GLU_START_MEASUREMENT = 0x00;
    public static readonly byte GLU_ABORT_MEASUREMENT = 0x01;
    public static readonly byte GLU_START_CALIBRATION = 0x02;
    public static readonly byte GLU_BLOOD_DETECTED = 0x03;
    public static readonly byte GLU_MEASUREMENT_COMPLETE_OK = 0x04;
    public static readonly byte GLU_CALIBRATION_COMPLETE_OK = 0x05;

    public static readonly byte BPM_START_MEASUREMENT = 0x06;
    public static readonly byte BPM_ABORT_MEASUREMENT = 0x07;
    public static readonly byte BPM_MEASUREMENT_COMPLETE_OK = 0x08;
    public static readonly byte BPM_MEASUREMENT_ERROR = 0x09;
	
	//leak test	
    public static readonly byte BPM_START_LEAK_TEST = 0x0A;
    public static readonly byte BPM_ABORT_LEAK_TEST = 0x0B;
    public static readonly byte BPM_LEAK_TEST_COMPLETE = 0x0C;

    public static readonly byte ECG_HEART_RATE_START_MEASUREMENT = 0x0D;
    public static readonly byte ECG_HEART_RATE_ABORT_MEASUREMENT = 0x0E;
    public static readonly byte ECG_HEART_RATE_MEASUREMENT_COMPLETE_OK = 0x0F;
    public static readonly byte ECG_HEART_RATE_MEASUREMENT_ERROR = 0x10;
    public static readonly byte ECG_HEART_BEAT_OCCURRED = 0x11;

    public static readonly byte ECG_DIAGNOSTIC_MODE_START_MEASUREMENT = 0x12;
    public static readonly byte ECG_DIAGNOSTIC_MODE_STOP_MEASUREMENT = 0x13;
    public static readonly byte ECG_DIAGNOSTIC_MODE_NEW_DATA_READY = 0x14;

    public static readonly byte TMP_READ_TEMPERATURE = 0x15;
    public static readonly byte HGT_READ_HEIGHT = 0x16;
    public static readonly byte WGT_READ_WEIGHT = 0x17;

    public static readonly byte SPR_START_MEASUREMENT = 0x18;
    public static readonly byte SPR_ABORT_MEASUREMENT = 0x19;
    public static readonly byte SPR_MEASUREMENT_COMPLETE_OK = 0x1A;
    public static readonly byte SPR_MEASUREMENT_ERROR = 0x1B;

    public static readonly byte SPR_DIAGNOSTIC_MODE_START_MEASUREMENT = 0x1C;
    public static readonly byte SPR_DIAGNOSTIC_MODE_STOP_MEASUREMENT = 0x1D;
    public static readonly byte SPR_DIAGNOSTIC_MODE_NEW_DATA_READY = 0x1E;
    public static readonly byte SPR_DIAGNOSTIC_MODE_MEASUREMENT_COMPLETE_OK = 0x1F;
    public static readonly byte SPR_DIAGNOSTIC_MODE_MEASUREMENT_ERROR = 0x20;


    public static readonly byte POX_START_MEASUREMENT = 0x21;
    public static readonly byte POX_ABORT_MEASUREMENT = 0x22;
    public static readonly byte POX_MEASUREMENT_COMPLETE_OK = 0x23;
    public static readonly byte POX_MEASUREMENT_ERROR = 0x24;

    public static readonly byte POX_DIAGNOSTIC_MODE_START_MEASUREMENT = 0x25;
    public static readonly byte POX_DIAGNOSTIC_MODE_STOP_MEASUREMENT = 0x26;
    public static readonly byte POX_DIAGNOSTIC_MODE_NEW_DATA_READY = 0x27;

    public static readonly byte BPM_SEND_PRESSURE_VALUE_TO_PC = 0x28;
    public static readonly byte BPM_DATA_READY = 0xFF;

// Códigos de ERROR
    public static readonly byte ERROR_OK = 0x00;
    public static readonly byte ERROR_BUSY = 0x01;
    public static readonly byte ERROR_INVALID_OPCODE = 0x02;

// URLs
    string SPO2_url = "http://192.168.2.239/cgi-bin/SPO2.pl";

	// Use this for initialization
	void Start () {
        string nombre_puerto = PlayerPrefs.GetString(Estado.PUERTO, "no hay puerto");
        Debug.Log("Monitoreando puerto: " + nombre_puerto);
        puerto = new SerialPort(nombre_puerto, 115200, Parity.None, 8, StopBits.One);
//        puerto.DataReceived += new SerialDataReceivedEventHandler(recepcion_datos);
        puerto.WriteTimeout = 10000;
        puerto.ReadTimeout = 10000;

        puerto.Open();
        inicializarPloter();
//        DETENER_gui();
	}

    // Update is called once per frame
    void Update()
    {
        if (DETENER_BANDERA)
            DETENER();

        if (ECG_LECTURA)
        {
            ECG_lectura();
        }

        if (SPO2_LECTURA)
        {
            SPO2_lectura();
        }
    }

    public void inicializarPloter()
    {
        //  Create a new graph named "ECG", with a range of 0 to 30000, colour red at position 100,100
        PlotManager.Instance.PlotCreate("ECG", 0, 30000, Color.red, new Vector2(100, 100));
        PlotManager.Instance.PlotCreate("SPO2", 0, 30000, Color.red, new Vector2(100, 100));
    }

    public bool DETENER_comando()
    {
        Debug.Log("Datos enviados para detener ECG...");

        byte[] mensaje = new byte[] { REQ, ECG_DIAGNOSTIC_MODE_STOP_MEASUREMENT };
        puerto.Write(mensaje, 0, 2);

        int cfm = leer("CFM: ");

        if (cfm == CFM)
        {
            int stop = leer("STOP: ");
            int length = leer("Length: ");
            return stop == ECG_DIAGNOSTIC_MODE_STOP_MEASUREMENT;
        }
        return false;
    }

    public void DETENER_gui()
    {
        DETENER_BANDERA = true;
    }

    public void DETENER()
    {
        if (DETENER_comando())
        {
            ECG_LECTURA = false;
            DETENER_BANDERA = false;
            Debug.Log("Dispositivo detenido correctamente...");
        } else Debug.Log("Dispositivo no detenido!");
    }

    public void ECG_comando()
    {
        byte[] mensaje = new byte[] { REQ, ECG_DIAGNOSTIC_MODE_START_MEASUREMENT };
        //        byte[] mensaje = new byte[] { REQ, ECG_HEART_RATE_START_MEASUREMENT };
        puerto.Write(mensaje, 0, 2);

        Debug.Log("Datos enviados para ECG...");

        if (confirmacion())
        {
            int ind = leer("IND: ");

            if (ind == IND)
                ECG_LECTURA = true;
            else ECG_LECTURA = false;
        }
    }

    public void SPO2_comando()
    {
        byte[] mensaje = new byte[] { REQ, POX_START_MEASUREMENT };
        puerto.Write(mensaje, 0, 2);

        Debug.Log("Datos enviados para SPO2...");

        if (confirmacion())
        {
            int ind = leer("IND (Packet Type): ");

            if (ind == IND)
                SPO2_LECTURA = true;
            else SPO2_LECTURA = false;
        }
    }

    public int leer(string debug)
    {
        int i = puerto.ReadByte();
        string i_str = String.Format("{0,10:G} {0,10:X}", i);
        Debug.Log(debug + i_str);
        return i;
    }

    public static int leer(SerialPort puerto, string debug)
    {
        int i = puerto.ReadByte();
        string i_str = String.Format("{0,10:G} {0,10:X}", i);
        Debug.Log(debug + i_str);
        return i;
    }

    public int[] leerBloque(int n)
    {
        int[] bloque = new int[n];
        for (int i = 0; i < bloque.Length; i++)
            bloque[i] = puerto.ReadByte();
        return bloque;
    }

    public static int[] leerBloque(SerialPort puerto, int n)
    {
        int[] bloque = new int[n];
        for (int i = 0; i < bloque.Length; i++)
            bloque[i] = puerto.ReadByte();
        return bloque;
    }

    public static SerialPort iniciarPuerto()
    {
        if (PlayerPrefs.HasKey(Estado.PUERTO))
        {
            string nombre_puerto = PlayerPrefs.GetString(Estado.PUERTO);
            Debug.Log("Abriendo puerto: " + nombre_puerto);
            SerialPort puerto = new SerialPort(nombre_puerto, 115200, Parity.None, 8, StopBits.One);
            puerto.WriteTimeout = 15000;
            puerto.ReadTimeout = 15000;

            puerto.Open();
            return puerto;
        }

        return null;
    }

    public static void cerrarPuerto(SerialPort puerto)
    {
        Debug.Log("Cerrando puerto... ");
        puerto.Close();
    }

    public bool confirmacion()
    {
        int cfm = leer("CFM: ");

        if (cfm == CFM) {
            int start = leer("START: ");
            int length = leer("Length: ");
            int error = leer("Error: ");

            return error == ERROR_OK;
        }
        return false;
    }

    public static bool confirmacion(SerialPort puerto)
    {
        int cfm = Lectura.leer(puerto, "CFM: ");

        if (cfm == Lectura.CFM)
        {
            int start = Lectura.leer(puerto, "START: ");
            int length = Lectura.leer(puerto, "Length: ");
            int error = Lectura.leer(puerto, "Error: ");

            return error == Lectura.ERROR_OK;
        }
        return false;
    }
	
    void ECG_lectura()
    {
        Debug.Log("Empezando a leer bloque...");

        int m_ready = leer("M_READY: ");
        if (m_ready == ECG_DIAGNOSTIC_MODE_NEW_DATA_READY)
        {
            int length = leer("LENGTH: ");

            int packetid_hi = leer("Packet HI: ");
            int packetid_lo = leer("Packet LO: ");

            // leer desde el dato 2 hasta el dato n
            int[] bloque = leerBloque(length - 2 + 1);

            int[] datos = new int[bloque.Length / 2];
            for (int i = 0; i < bloque.Length; i += 2)
            {
                datos[i / 2] = (bloque[i] << 8) + (bloque[i + 1]);
                if (datos[i / 2] < 30000 &
                    datos[i / 2] > 0)
                    PlotManager.Instance.PlotAdd("ECG", datos[i / 2]);
            }

            Debug.Log("Datos: " + ArrayToString(datos));
            Debug.Log("Último dato: " + bloque[bloque.Length - 1]);
//            ECG_LECTURA = false;
        }
        else ECG_LECTURA = false;
    }

    void SPO2_lectura()
    {
        Debug.Log("Empezando a leer bloque SPO2...");

        int command_opcode = leer("Command Opcode: ");
        if (command_opcode == POX_MEASUREMENT_COMPLETE_OK)
        {
            int length = leer("LENGTH: ");

            // leer desde el dato 0 hasta el dato n
            int[] bloque = leerBloque(length);

            Debug.Log("Datos: " + ArrayToString(bloque));

            if (length == 2)
            {
                Debug.Log("SPO2: " + bloque[0]);
                Debug.Log("Heart Rate: " + bloque[1]);
                StartCoroutine(SPO2_DB(10, 1, bloque[0]));
                StartCoroutine(SPO2_DB(10, 2, bloque[1]));
            }

            SPO2_LECTURA = false;
        }
        else SPO2_LECTURA = false;
    }

    public IEnumerator SPO2_DB(int IdExploracion, int IdVariable, int Valor)
    {
        WWWForm form = new WWWForm();
        form.AddField("IdExploracion", IdExploracion.ToString());
        form.AddField("IdVariable", IdVariable.ToString());
        form.AddField("Valor", Valor.ToString());

        WWW download = new WWW(SPO2_url, form);

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
//        return null;
    }

    public static string ArrayToString(int[] arr)
    {
        string[] arr_str = new string[arr.Length];
        for (int i = 0; i < arr.Length; i++)
            arr_str[i] = arr[i].ToString();

        return string.Join(",", arr_str);
    }

}
