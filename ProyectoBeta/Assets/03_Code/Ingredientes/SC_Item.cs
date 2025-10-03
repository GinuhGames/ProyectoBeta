// SC_Item.cs
// ScriptableObject para tipos de items: define datos comunes para cada tipo de ingrediente (usado en recetas y componentes).
using UnityEngine;

[CreateAssetMenu(fileName = "Nuevo Item", menuName = "Crafteo/Item")]
public class SC_Item : ScriptableObject
{
    [Tooltip("Nombre único del item (usado para comparación).")]
    public string itemName; // Nombre único para identificación y display.
    [Tooltip("Icono opcional para UI.")]
    public Sprite icon; // Icono opcional.

    [Tooltip("Probabilidad (0-100%) de que el objeto se vuelva encantado al inicio.")]
    [SerializeField] public float probabilidadEncantamiento = 50f;

    [Tooltip("Tiempo mínimo en segundos entre emisiones de partículas (solo si encantado).")]
    [SerializeField] public float intervaloMinimo = 1f;

    [Tooltip("Tiempo máximo en segundos entre emisiones de partículas (solo si encantado).")]
    [SerializeField] public float intervaloMaximo = 3f;
}