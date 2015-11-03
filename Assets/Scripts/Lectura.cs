using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO.Ports;

public class Lectura : MonoBehaviour {
    SerialPort puerto;
    bool DETENER_BANDERA = false;

    bool ECG_LECTURA = false;

    byte REQ = 0x52;
    byte CFM = 0x43;
    byte IND = 0x69;

	byte GLU_START_MEASUREMENT = 0x00;
	byte GLU_ABORT_MEASUREMENT = 0x01;
	byte GLU_START_CALIBRATION = 0x02;
	byte GLU_BLOOD_DETECTED = 0x03;
	byte GLU_MEASUREMENT_COMPLETE_OK = 0x04;
	byte GLU_CALIBRATION_COMPLETE_OK = 0x05;
	
	byte BPM_START_MEASUREMENT = 0x06;
	byte BPM_ABORT_MEASUREMENT = 0x07;
	byte BPM_MEASUREMENT_COMPLETE_OK  = 0x08;
	byte BPM_MEASUREMENT_ERROR = 0x09;
	
	//leak test	
	byte BPM_START_LEAK_TEST = 0x0A;
	byte BPM_ABORT_LEAK_TEST = 0x0B;
	byte BPM_LEAK_TEST_COMPLETE = 0x0C;
	
	byte ECG_HEART_RATE_START_MEASUREMENT = 0x0D;
	byte ECG_HEART_RATE_ABORT_MEASUREMENT = 0x0E;
	byte ECG_HEART_RATE_MEASUREMENT_COMPLETE_OK = 0x0F;
	byte ECG_HEART_RATE_MEASUREMENT_ERROR = 0x10;
	byte ECG_HEART_BEAT_OCCURRED = 0x11;
	
	byte ECG_DIAGNOSTIC_MODE_START_MEASUREMENT = 0x12;
	byte ECG_DIAGNOSTIC_MODE_STOP_MEASUREMENT = 0x13;
	byte ECG_DIAGNOSTIC_MODE_NEW_DATA_READY = 0x14;
	
	byte TMP_READ_TEMPERATURE = 0x15;
	byte HGT_READ_HEIGHT = 0x16;
	byte WGT_READ_WEIGHT = 0x17;
	
	byte SPR_START_MEASUREMENT = 0x18;
	byte SPR_ABORT_MEASUREMENT = 0x19;
	byte SPR_MEASUREMENT_COMPLETE_OK = 0x1A;
	byte SPR_MEASUREMENT_ERROR = 0x1B;

	byte SPR_DIAGNOSTIC_MODE_START_MEASUREMENT = 0x1C;
	byte SPR_DIAGNOSTIC_MODE_STOP_MEASUREMENT = 0x1D;
	byte SPR_DIAGNOSTIC_MODE_NEW_DATA_READY = 0x1E;
	byte SPR_DIAGNOSTIC_MODE_MEASUREMENT_COMPLETE_OK = 0x1F;
	byte SPR_DIAGNOSTIC_MODE_MEASUREMENT_ERROR = 0x20;

	
	byte POX_START_MEASUREMENT = 0x21;
	byte POX_ABORT_MEASUREMENT = 0x22;
	byte POX_MEASUREMENT_COMPLETE_OK = 0x23;
	byte POX_MEASUREMENT_ERROR = 0x24;
	
	byte POX_DIAGNOSTIC_MODE_START_MEASUREMENT = 0x25;
	byte POX_DIAGNOSTIC_MODE_STOP_MEASUREMENT = 0x26;
	byte POX_DIAGNOSTIC_MODE_NEW_DATA_READY = 0x27;
		
	byte BPM_SEND_PRESSURE_VALUE_TO_PC = 0x28;
	byte BPM_DATA_READY = 0xFF;

// Códigos de ERROR
    byte ERROR_OK = 0x00;
    byte ERROR_BUSY = 0x01;
    byte ERROR_INVALID_OPCODE = 0x02;

	// Use this for initialization
	void Start () {
        string nombre_puerto = PlayerPrefs.GetString("Puerto", "no hay puerto");
        Debug.Log("Monitoreando puerto: " + nombre_puerto);
        puerto = new SerialPort(nombre_puerto, 115200, Parity.None, 8, StopBits.One);
//        puerto.DataReceived += new SerialDataReceivedEventHandler(recepcion_datos);
        puerto.WriteTimeout = 500;
        puerto.ReadTimeout = 500;

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
    }

    public void inicializarPloter()
    {
        //  Create a new graph named "ECG", with a range of 0 to 30000, colour red at position 100,100
        PlotManager.Instance.PlotCreate("ECG", 0, 30000, Color.red, new Vector2(100, 100));
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

    public int leer(string debug) {
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

    public static string ArrayToString(int[] arr)
    {
        string[] arr_str = new string[arr.Length];
        for (int i = 0; i < arr.Length; i++)
            arr_str[i] = arr[i].ToString();

        return string.Join(",", arr_str);
    }

}
