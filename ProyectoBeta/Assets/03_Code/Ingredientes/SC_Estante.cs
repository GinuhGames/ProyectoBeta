// Script para triggers de estantes: detecta entrada de objetos, coloca el objeto en el centro, hace kinematic, activa bool objetoEnEstante, y actualiza texto de precio mínimo basado en si está encantado o no.
// Al salir, revierte cambios y limpia el texto. Asume un solo objeto por estante.
using UnityEngine;
using TMPro;

public class SC_Estante : MonoBehaviour
{
    [Tooltip("Indica si hay un objeto colocado en este estante.")]
    [SerializeField] private bool objetoEnEstante = false;

    [Tooltip("TextMeshPro para mostrar el precio mínimo (asigna en Inspector, componente en la escena o hijo).")]
    [SerializeField] private TextMeshPro textoPrecio;

    private GameObject currentObject;  // Referencia al objeto actual en el estante.
    private Rigidbody currentRb;       // Referencia al Rigidbody del objeto.

    private void OnTriggerEnter(Collider other)
    {
        // Asumir que el objeto tiene SC_Ingredientes (como en el sistema de crafting).
        SC_Ingredientes item = other.GetComponent<SC_Ingredientes>();
        if (item != null && item.itemType != null && !objetoEnEstante)
        {
            currentObject = other.gameObject;
            currentRb = currentObject.GetComponent<Rigidbody>();

            // Teleportar al centro del trigger.
            currentObject.transform.position = transform.position;

            // Hacer kinematic si tiene Rigidbody.
            if (currentRb != null)
            {
                currentRb.isKinematic = true;
            }

            objetoEnEstante = true;

            // Obtener precio mínimo basado en si está encantado.
            SC_Encantado encantado = currentObject.GetComponent<SC_Encantado>();
            float precioMin = (encantado != null && encantado.estaEncantado) ? item.itemType.en_precioMinimo : item.itemType.precioMinimo;

            // Actualizar texto.
            if (textoPrecio != null)
            {
                textoPrecio.SetText(precioMin.ToString());
            }

            Debug.Log($"Objeto {item.itemType.itemName} colocado en estante con precio {precioMin}.");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == currentObject && objetoEnEstante)
        {
            // Revertir kinematic.
            if (currentRb != null)
            {
                currentRb.isKinematic = false;
            }

            objetoEnEstante = false;

            // Limpiar texto.
            if (textoPrecio != null)
            {
                textoPrecio.SetText("");
            }

            currentObject = null;
            currentRb = null;

            Debug.Log($"Objeto removido del estante.");
        }
    }
}