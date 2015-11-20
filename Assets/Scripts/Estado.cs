using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Estado : MonoBehaviour {
    // Seguimiento de escenas
	public static readonly string EXPLORAR_CONSULTAR = "EXPLORAR_CONSULTAR";
	public static readonly string EXPLORAR = "EXPLORAR";
	public static readonly string CONSULTAR = "CONSULTAR";

    private static Stack<string> escenas = new Stack<string>();
    public static void PushEscena(string escena)
    {
        escenas.Push(escena);
    }
    public static string PopEscena()
    {
        return escenas.Pop();
    }

    // Manejo de puertos
    public static readonly string PUERTO = "PUERTO";
    public static readonly string SINPUERTO = "SINPUERTO";

    // Variables
    public static readonly string SPO2 = "SPO2";
    public static readonly string RITMOCARDIACO = "RITMOCARDIACO";
    public static readonly string GLU = "GLU";

    public static readonly int ID_SPO2 = 0;
    public static readonly int ID_TAS = 1;
    public static readonly int ID_TAD = 2;
    public static readonly int ID_FREC_CARDIACA= 3;
    public static readonly int ID_FREC_RESPIRATORIA = 4;
    public static readonly int ID_TEMPERATURA = 5;
    public static readonly int ID_TALLA = 6;
    public static readonly int ID_PESO = 7;
    public static readonly int ID_GLU = 8;

    // Estado Anterior
    public static readonly string EstadoAnterior = "EstadoAnterior";

    // Ingresar Médico
    public static readonly string MedicoEmail = "MedicoEmail";
    public static readonly string MedicoPassword = "MedicoPassword";
    public static readonly string MedicoId = "MedicoId";
    public static readonly string MedicoNombre = "MedicoNombre";
    public static readonly string MedicoApellidoP = "MedicoApellidoP";
    public static readonly string MedicoApellidoM = "MedicoApellidoM";

    // Agregar Paciente
    public static readonly string PacienteIdentificacion = "PacienteIdentificacion";
	public static readonly string PacienteCodigo = "PacienteCodigo";
	public static readonly string PacienteNombre = "PacienteNombre";
    public static readonly string PacienteApellidoP = "PacienteApellidoP";
    public static readonly string PacienteApellidoM = "PacienteApellidoM";
    public static readonly string PacienteFechaNac = "PacienteFechaNac";
    public static readonly string PacienteDireccion = "PacienteDireccion";
    public static readonly string PacienteTelefono = "PacienteTelefono";
    public static readonly string PacienteEmail = "PacienteEmail";
    public static readonly string PacienteNoParejas = "PacienteNoParejas";
    public static readonly string PacienteArea = "PacienteArea";
    public static readonly string PacienteGenero = "PacienteGenero";
    public static readonly string PacienteIVSA = "PacienteIVSA";
    public static readonly string PacienteIdMPF = "PacienteIdMPF";

    // Exploracion Física
    public static readonly string ExploracionId = "ExploracionId";

}
