// Script manager singleton: carga y verifica coincidencias de recetas basadas en ingredientes proporcionados.
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class SC_Receta_Manager : MonoBehaviour
{
    public static SC_Receta_Manager Instance;  // Singleton para acceso fácil.

    [Tooltip("Lista de todas las recetas disponibles (arrastra las ScriptableObjects aquí).")]
    [SerializeField] private List<SC_Recetas> allRecipes;  // todas las recetas

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public SC_Recetas FindMatchingRecipe(List<SC_Item> ingredients)
    {
        foreach (var recipe in allRecipes)
        {
            if (IngredientsMatch(recipe.requiredIngredients, ingredients))
            {
                return recipe;
            }
        }
        return null;  // No coincide con ninguna.
    }

    private bool IngredientsMatch(List<SC_Item> required, List<SC_Item> provided)
    {
        if (required.Count != provided.Count) return false;

        // Ordenamos por nombre del item para ignorar orden y comparamos.
        var reqSorted = required.OrderBy(r => r.itemName).ToList();
        var provSorted = provided.OrderBy(p => p.itemName).ToList();

        for (int i = 0; i < reqSorted.Count; i++)
        {
            if (reqSorted[i].itemName != provSorted[i].itemName) return false;
        }
        return true;
    }
}