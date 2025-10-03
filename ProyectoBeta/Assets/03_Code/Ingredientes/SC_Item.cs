// SC_Item.cs
// ScriptableObject para tipos de items: define datos comunes para cada tipo de ingrediente (usado en recetas y componentes).
using UnityEngine;

[CreateAssetMenu(fileName = "Nuevo Item", menuName = "Crafteo/Item")]
public class SC_Item : ScriptableObject
{
    [Tooltip("Nombre �nico del item (usado para comparaci�n).")]
    public string itemName; // Nombre �nico para identificaci�n y display.
    [Tooltip("Icono opcional para UI.")]
    public Sprite icon; // Icono opcional.

    [Tooltip("Probabilidad (0-100%) de que el objeto se vuelva encantado al inicio.")]
    [SerializeField] public float probabilidadEncantamiento = 50f;

    [Tooltip("Tiempo m�nimo en segundos entre emisiones de part�culas (solo si encantado).")]
    [SerializeField] public float intervaloMinimo = 1f;

    [Tooltip("Tiempo m�ximo en segundos entre emisiones de part�culas (solo si encantado).")]
    [SerializeField] public float intervaloMaximo = 3f;
}