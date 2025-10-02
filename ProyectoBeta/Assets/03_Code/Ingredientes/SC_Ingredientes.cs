// Script componente para ingredientes: referencia al tipo de item (ScriptableObject) para identificación.
using UnityEngine;

public class SC_Ingredientes : MonoBehaviour
{
    [Tooltip("Referencia al ScriptableObject que define este tipo de item.")]
    public SC_Item itemType;  // Referencia al SO del item.
}