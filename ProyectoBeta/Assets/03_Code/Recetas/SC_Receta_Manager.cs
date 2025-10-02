using UnityEngine;
using System.Collections.Generic;

public class SC_Receta_Manager : MonoBehaviour
{
    public static SC_Receta_Manager Instance;  // Singleton para acceso fácil.

    [SerializeField] private List<SC_Recetas> allRecipes;  // todas las recetas
 
    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

    }

    public SC_Recetas FindMatchingRecipe(List<string> ingredients)
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

    private bool IngredientsMatch(List<string> required, List<string> provided)
    {
        if (required.Count != provided.Count) return false;

        // Ordenamos para ignorar orden y comparamos sets.
        required.Sort();
        provided.Sort();
        for (int i = 0; i < required.Count; i++)
        {
            if (required[i] != provided[i]) return false;
        }
        return true;

 
    }
}