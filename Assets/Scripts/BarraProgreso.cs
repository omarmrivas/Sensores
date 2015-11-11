using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BarraProgreso : MonoBehaviour {
    public Texture2D barra_progreso_vacio = null;
    public Texture2D barra_progreso_lleno = null;
    public Text progresoTexto;

    private Vector2 pos;
    private Vector2 size;

    public float progreso = 0f;

    void OnGUI()
    {
        GUI.DrawTexture(new Rect(pos.x, pos.y, size.x, size.y), barra_progreso_vacio);
        GUI.DrawTexture(new Rect(pos.x, pos.y, size.x * Mathf.Clamp01(progreso), size.y), barra_progreso_lleno);
        progresoTexto.text = (int)(Mathf.Clamp01(progreso) * 100.0f) + "%";
    }

    // Use this for initialization
    void Start()
    {
        RectTransform r = GetComponent<RectTransform>();
        pos = new Vector2(r.rect.x + r.transform.position.x, Screen.height + r.rect.y - r.transform.position.y);
        size = new Vector2(r.rect.width, r.rect.height);
    }

    public void habilitar()
    {
        this.enabled = true;
        progresoTexto.enabled = true;
    }

    public void deshabilitar()
    {
        this.enabled = false;
        progresoTexto.enabled = false;
    }

    // Update is called once per frame
/*    void Update()
    {
        progreso = Time.time * 0.05f;
    }*/
}
