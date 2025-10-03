// Script para objetos encantados: permite que un objeto tenga un % de volverse encantado al inicio,
// instancia un sistema de partículas en el objeto, y emite partículas periódicamente en intervalos aleatorios
// entre un tiempo mínimo y máximo si está encantado. Desactivar el bool detiene el encantamiento.
using UnityEngine;
using System.Collections;

public class SC_Encantado : MonoBehaviour
{
    [Tooltip("Indica si el objeto está encantado (puede desactivarse para detener el encantamiento).")]
    [SerializeField] private bool estaEncantado = false;

    [Tooltip("Probabilidad (0-100%) de que el objeto se vuelva encantado al inicio.")]
    [SerializeField] private float probabilidadEncantamiento = 50f;

    [Tooltip("Tiempo mínimo en segundos entre emisiones de partículas (solo si encantado).")]
    [SerializeField] private float intervaloMinimo = 1f;

    [Tooltip("Tiempo máximo en segundos entre emisiones de partículas (solo si encantado).")]
    [SerializeField] private float intervaloMaximo = 3f;

    [Tooltip("Prefab del sistema de partículas para el efecto encantado (se instanciará en este objeto).")]
    [SerializeField] private ParticleSystem particulasPrefab;

    private ParticleSystem sistemaParticulas;
    private Coroutine rutinaParticulas;

    private void Start()
    {
        // Determinar si el objeto se vuelve encantado según la probabilidad
        if (Random.Range(0f, 100f) <= probabilidadEncantamiento)
        {
            estaEncantado = true;
        }

        if (estaEncantado && particulasPrefab != null)
        {
            // Instanciar el sistema de partículas como hijo de este objeto
            sistemaParticulas = Instantiate(particulasPrefab, transform.position, Quaternion.identity, transform);
            rutinaParticulas = StartCoroutine(EmitirParticulas());
        }
    }

    private void Update()
    {
        // Si se desactiva el bool, detener las partículas
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
                sistemaParticulas.Emit(5); // Emite 5 partículas (ajusta según el efecto deseado)
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