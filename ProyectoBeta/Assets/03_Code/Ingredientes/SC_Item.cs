// ScriptableObject para tipos de items: define datos comunes para cada tipo de ingrediente (usado en recetas y componentes).
using UnityEngine;

[CreateAssetMenu(fileName = "Nuevo Item", menuName = "Crafteo/Item")]
public class SC_Item : ScriptableObject
{
    [Tooltip("Nombre único del item (usado para comparación).")]
    public string itemName; // Nombre único para identificación y display.
    [Tooltip("Icono opcional para UI.")]
    public Sprite icon; // Icono opcional.
}