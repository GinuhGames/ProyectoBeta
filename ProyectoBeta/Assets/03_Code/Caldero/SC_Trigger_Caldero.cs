// Script que maneja el caldero: detecta ingredientes entrantes, almacena referencias y verifica recetas al activar el crafting.
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

            currentObjects.Add(other.gameObject);
            currentIngredients.Add(item.itemType);
            Debug.Log($"Añadido: {item.itemType.itemName}");
        }
    }

    private void OnCraftButtonPressed()
    {
        if (currentIngredients.Count == 0) return;

        SC_Recetas matchingRecipe = SC_Receta_Manager.Instance.FindMatchingRecipe(currentIngredients);

        if (matchingRecipe != null)
        {
            // Borrar ingredientes.
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

            ClearCauldron();  // O no, dependiendo de tu diseño.
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

        Debug.Log($"Creado: {recipe.recipeName}");
    }

    private void ClearCauldron()
    {
        foreach (var obj in currentObjects)
        {
            Destroy(obj);
        }
        currentObjects.Clear();
        currentIngredients.Clear();
    }
}