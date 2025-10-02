using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;  // Si usas botón UI.

public class SC_Trigger_Caldero : MonoBehaviour
{
    private List<GameObject> currentObjects = new List<GameObject>();  // Objetos físicos dentro.
    private List<string> currentIngredients = new List<string>();     // IDs para chequeo.

    [SerializeField] private Button craftButton;  // Botón para craft (asigna en inspector).
    [SerializeField] private Transform spawnPoint;  // Donde instanciar el resultado.
    [SerializeField] private float craftDelay = 3f;  // Segundos de espera.

    private void Start()
    {
        if (craftButton != null)
        {
            craftButton.onClick.AddListener(OnCraftButtonPressed);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        SC_Ingredientes item = other.GetComponent<SC_Ingredientes>();
        if (item != null)
        {
            currentObjects.Add(other.gameObject);
            currentIngredients.Add(item.itemID);
            // Opcional: Desactivar physics o mover a una posición fija dentro del caldero.
            other.GetComponent<Rigidbody>().isKinematic = true;  // Para que no se mueva.
            Debug.Log($"Añadido: {item.itemID}");
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
            ClearCauldron();  // O no, dependiendo de tu diseño.
        }
    }

    private System.Collections.IEnumerator CraftAfterDelay(SC_Recetas recipe)
    {
        yield return new WaitForSeconds(craftDelay);

        // Instanciar el resultado.
        Instantiate(recipe.resultPrefab, spawnPoint.position, Quaternion.identity);
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