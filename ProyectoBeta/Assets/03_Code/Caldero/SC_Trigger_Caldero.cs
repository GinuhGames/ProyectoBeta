// Script que maneja el caldero: detecta ingredientes entrantes, almacena referencias, verifica recetas al activar el crafting, y controla indicadores visuales.
using System.Collections.Generic;
using UnityEngine;

public class SC_Trigger_Caldero : MonoBehaviour
{
    [SerializeField] private List<GameObject> currentObjects = new List<GameObject>();  // Objetos físicos dentro (visible y editable en Inspector).
    [SerializeField] private List<SC_Item> currentIngredients = new List<SC_Item>();     // Referencias a items para chequeo (visible y editable en Inspector).

    [Tooltip("Donde instanciar el resultado del crafting y reproducir las partículas.")]
    [SerializeField] private Transform spawnPoint;  // Donde instanciar el resultado y reproducir partículas.
    [Tooltip("Segundos de espera antes de instanciar el resultado.")]
    [SerializeField] private float craftDelay = 3f;  // Segundos de espera.
    [Tooltip("Set to true to simulate crafting button press.")]
    [SerializeField] private bool startCrafting = false;
    [Tooltip("Sistema de partículas para éxito en crafting (componente en la escena, reproducido en spawnPoint).")]
    [SerializeField] private ParticleSystem successParticles;
    [Tooltip("Sistema de partículas para fracaso en crafting (componente en la escena, reproducido en spawnPoint).")]
    [SerializeField] private ParticleSystem failureParticles;
    [Tooltip("GameObject que se muestra cuando hay ingredientes en el caldero (componente en la escena).")]
    [SerializeField] private GameObject ingredientsIndicator;
    [Tooltip("GameObject que se muestra durante el proceso de crafting (componente en la escena).")]
    [SerializeField] private GameObject craftingIndicator;

    private Dictionary<GameObject, bool> originalEncantadoStates = new Dictionary<GameObject, bool>();  // Almacena estados originales de encantado para restaurar al salir.

    private void Update()
    {
        if (startCrafting)
        {
            OnCraftButtonPressed();
            startCrafting = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        SC_Ingredientes item = other.GetComponent<SC_Ingredientes>();
        if (item != null && item.itemType != null)
        {
            // Evitar añadir el mismo GameObject más de una vez.
            if (currentObjects.Contains(other.gameObject))
            {
                Debug.Log($"El objeto {item.itemType.itemName} ya está en el caldero, ignorado.");
                return;
            }

            // Manejar encantamiento: pausar si está encantado.
            SC_Encantado encantado = other.GetComponent<SC_Encantado>();
            if (encantado != null)
            {
                originalEncantadoStates[other.gameObject] = encantado.estaEncantado;
                encantado.estaEncantado = false;
            }

            currentObjects.Add(other.gameObject);
            currentIngredients.Add(item.itemType);
            Debug.Log($"Añadido: {item.itemType.itemName}");

            // Activar indicador de ingredientes si es el primer ingrediente.
            if (currentIngredients.Count == 1 && ingredientsIndicator != null)
            {
                ingredientsIndicator.SetActive(true);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        SC_Ingredientes item = other.GetComponent<SC_Ingredientes>();
        if (item != null && currentObjects.Contains(other.gameObject))
        {
            // Restaurar encantamiento si aplica.
            SC_Encantado encantado = other.GetComponent<SC_Encantado>();
            if (encantado != null && originalEncantadoStates.ContainsKey(other.gameObject))
            {
                encantado.estaEncantado = originalEncantadoStates[other.gameObject];
                originalEncantadoStates.Remove(other.gameObject);
            }

            // Remover de las listas.
            int index = currentObjects.IndexOf(other.gameObject);
            currentObjects.RemoveAt(index);
            currentIngredients.RemoveAt(index);
            Debug.Log($"Removido: {item.itemType.itemName}");

            // Desactivar indicador si no hay más ingredientes.
            if (currentIngredients.Count == 0 && ingredientsIndicator != null)
            {
                ingredientsIndicator.SetActive(false);
            }
        }
    }

    private void OnCraftButtonPressed()
    {
        if (currentIngredients.Count == 0) return;

        // Activar indicador de crafting.
        if (craftingIndicator != null)
        {
            craftingIndicator.SetActive(true);
        }

        SC_Recetas matchingRecipe = SC_Receta_Manager.Instance.FindMatchingRecipe(currentIngredients);

        if (matchingRecipe != null)
        {
            // Borrar ingredientes (sin desactivar ingredientsIndicator aún).
            ClearCauldron();

            // Esperar unos segundos e instanciar.
            StartCoroutine(CraftAfterDelay(matchingRecipe));
        }
        else
        {
            // Fallo: Opcional, anima algo o devuelve items.
            Debug.Log("No coincide con ninguna receta.");

            // Reproducir partículas de fracaso.
            if (failureParticles != null)
            {
                failureParticles.Play();
            }

            // Desactivar indicadores.
            if (craftingIndicator != null)
            {
                craftingIndicator.SetActive(false);
            }
            if (ingredientsIndicator != null)
            {
                ingredientsIndicator.SetActive(false);
            }

            ClearCauldron();
        }
    }

    private System.Collections.IEnumerator CraftAfterDelay(SC_Recetas recipe)
    {
        yield return new WaitForSeconds(craftDelay);

        // Instanciar el resultado.
        Instantiate(recipe.resultPrefab, spawnPoint.position, Quaternion.identity);

        // Reproducir partículas de éxito.
        if (successParticles != null)
        {
            successParticles.Play();
        }

        // Desactivar indicadores.
        if (craftingIndicator != null)
        {
            craftingIndicator.SetActive(false);
        }
        if (ingredientsIndicator != null)
        {
            ingredientsIndicator.SetActive(false);
        }

        Debug.Log($"Creado: {recipe.recipeName}");
    }

    private void ClearCauldron()
    {
        foreach (var obj in currentObjects)
        {
            // Limpiar estados guardados si aplica.
            if (originalEncantadoStates.ContainsKey(obj))
            {
                originalEncantadoStates.Remove(obj);
            }
            Destroy(obj);
        }
        currentObjects.Clear();
        currentIngredients.Clear();
    }
}