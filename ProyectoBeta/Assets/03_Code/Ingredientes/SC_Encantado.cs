// SC_Encantado.cs
// Script para objetos encantados: permite que un objeto tenga un % de volverse encantado al inicio,
// instancia un sistema de partículas en el objeto, y emite partículas periódicamente en intervalos aleatorios
// entre un tiempo mínimo y máximo si está encantado. Desactivar el bool detiene el encantamiento.
// Los valores de probabilidad e intervalos se obtienen del SC_Item asociado via SC_Ingredientes.
using UnityEngine;
using System.Collections;

public class SC_Encantado : MonoBehaviour
{
    [Tooltip("Indica si el objeto está encantado (puede desactivarse para detener el encantamiento).")]
    [SerializeField] private bool estaEncantado = false;

    [Tooltip("Prefab del sistema de partículas para el efecto encantado (se instanciará en este objeto).")]
    [SerializeField] private ParticleSystem particulasPrefab;

    private ParticleSystem sistemaParticulas;
    private Coroutine rutinaParticulas;
    private SC_Item itemType;  // Referencia al SC_Item para obtener valores de encantamiento.

    private void Start()
    {
        // Obtener el SC_Item desde SC_Ingredientes (asumiendo que el objeto lo tiene).
        SC_Ingredientes ingredientes = GetComponent<SC_Ingredientes>();
        if (ingredientes != null)
        {
            itemType = ingredientes.itemType;
        }

        if (itemType != null)
        {
            // Determinar si el objeto se vuelve encantado según la probabilidad del SC_Item.
            if (Random.Range(0f, 100f) <= itemType.probabilidadEncantamiento)
            {
                estaEncantado = true;
            }
        }

        if (estaEncantado && particulasPrefab != null && itemType != null)
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
        else if (estaEncantado && rutinaParticulas == null && particulasPrefab != null && itemType != null)
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
            float intervaloAleatorio = Random.Range(itemType.intervaloMinimo, itemType.intervaloMaximo);
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