// ScriptableObject para tipos de items: define datos comunes para cada tipo de ingrediente (usado en recetas y componentes).
using UnityEngine;

[CreateAssetMenu(fileName = "Nuevo Item", menuName = "Crafteo/Item")]
public class SC_Item : ScriptableObject
{
    [Tooltip("Nombre �nico del item (usado para comparaci�n).")]
    public string itemName; // Nombre �nico para identificaci�n y display.
    [Tooltip("Icono opcional para UI.")]
    public Sprite icon; // Icono opcional.
}