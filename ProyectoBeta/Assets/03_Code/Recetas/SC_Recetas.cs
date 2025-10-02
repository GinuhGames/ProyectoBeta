using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "Nueva Receta", menuName = "Crafteo/Receta")]
public class SC_Recetas : ScriptableObject
{
    public string recipeName;      // Nombre de la receta (para debug o UI).
    public Sprite icon;            // Icono para UI si lo necesitas.
    public GameObject resultPrefab; // El prefab que se instanciará como resultado.

    public List<string> requiredIngredients;  // Lista de itemIDs requeridos
 
}