// ScriptableObject para recetas: define los ingredientes requeridos (ScriptableObjects), resultado y otros datos.
using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "Nueva Receta", menuName = "Crafteo/Receta")]
public class SC_Recetas : ScriptableObject
{
    [Tooltip("Nombre de la receta para debug o UI.")]
    public string recipeName;      // Nombre de la receta (para debug o UI).
    [Tooltip("Icono para UI si es necesario.")]
    public Sprite icon;            // Icono para UI si lo necesitas.
    [Tooltip("Prefab del objeto resultado a instanciar.")]
    public GameObject resultPrefab; // El prefab que se instanciará como resultado.

    [Tooltip("Lista de ScriptableObjects de items requeridos (arrastra los items SO aquí, repite para cantidades).")]
    public List<SC_Item> requiredIngredients;  // Lista de referencias a items SO requeridos.
}