// Script para objetos encantados: permite que un objeto tenga un % de volverse encantado al inicio,
// instancia un sistema de part�culas en el objeto, y emite part�culas peri�dicamente en intervalos aleatorios
// entre un tiempo m�nimo y m�ximo si est� encantado. Desactivar el bool detiene el encantamiento.
using UnityEngine;
using System.Collections;

public class SC_Encantado : MonoBehaviour
{
    [Tooltip("Indica si el objeto est� encantado (puede desactivarse para detener el encantamiento).")]
    [SerializeField] private bool estaEncantado = false;

    [Tooltip("Probabilidad (0-100%) de que el objeto se vuelva encantado al inicio.")]
    [SerializeField] private float probabilidadEncantamiento = 50f;

    [Tooltip("Tiempo m�nimo en segundos entre emisiones de part�culas (solo si encantado).")]
    [SerializeField] private float intervaloMinimo = 1f;

    [Tooltip("Tiempo m�ximo en segundos entre emisiones de part�culas (solo si encantado).")]
    [SerializeField] private float intervaloMaximo = 3f;

    [Tooltip("Prefab del sistema de part�culas para el efecto encantado (se instanciar� en este objeto).")]
    [SerializeField] private ParticleSystem particulasPrefab;

    private ParticleSystem sistemaParticulas;
    private Coroutine rutinaParticulas;

    private void Start()
    {
        // Determinar si el objeto se vuelve encantado seg�n la probabilidad
        if (Random.Range(0f, 100f) <= probabilidadEncantamiento)
        {
            estaEncantado = true;
        }

        if (estaEncantado && particulasPrefab != null)
        {
            // Instanciar el sistema de part�culas como hijo de este objeto
            sistemaParticulas = Instantiate(particulasPrefab, transform.position, Quaternion.identity, transform);
            rutinaParticulas = StartCoroutine(EmitirParticulas());
        }
    }

    private void Update()
    {
        // Si se desactiva el bool, detener las part�culas
        if (!estaEncantado && rutinaParticulas != null)
        {
            StopCoroutine(rutinaParticulas);
            rutinaParticulas = null;
            if (sistemaParticulas != null)
            {
                Destroy(sistemaParticulas.gameObject);
            }
        }
        // Si se reactiva el bool y no hay rutina activa, reiniciar
        else if (estaEncantado && rutinaParticulas == null && particulasPrefab != null)
        {
            if (sistemaParticulas == null)
            {
                sistemaParticulas = Instantiate(particulasPrefab, transform.position, Quaternion.identity, transform);
            }
            rutinaParticulas = StartCoroutine(EmitirParticulas());
        }
    }

    private IEnumerator EmitirParticulas()
    {
        while (estaEncantado)
        {
            if (sistemaParticulas != null)
            {
                sistemaParticulas.Emit(5); // Emite 5 part�culas (ajusta seg�n el efecto deseado)
            }
            float intervaloAleatorio = Random.Range(intervaloMinimo, intervaloMaximo);
            yield return new WaitForSeconds(intervaloAleatorio);
        }
    }

    private void OnDestroy()
    {
        // Limpiar la rutina si el objeto es destruido
        if (rutinaParticulas != null)
        {
            StopCoroutine(rutinaParticulas);
        }
    }
}