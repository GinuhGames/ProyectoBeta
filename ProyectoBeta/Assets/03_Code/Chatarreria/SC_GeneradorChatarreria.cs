using System.Collections.Generic;
using UnityEngine;

// Resumen: Este script genera un mapa aleatorio al inicio del juego para una chatarrería. Toma una lista de puntos de spawn (GameObjects vacíos) y una lista de prefabs. De forma aleatoria, selecciona un subconjunto de estos puntos y instancia prefabs aleatorios en ellos, asegurando que al menos un número mínimo especificado de prefabs aparezca para evitar un mapa vacío. Los prefabs instanciados se agrupan como hijos de un GameObject contenedor especificado. Es escalable, permitiendo agregar más puntos y prefabs fácilmente desde el Inspector de Unity.

public class SC_GeneradorChatarreria : MonoBehaviour
{
    // Lista de puntos de spawn: GameObjects vacíos donde potencialmente se instanciarán prefabs.
    [Tooltip("Arrastra aquí los GameObjects vacíos que servirán como puntos de spawn para los prefabs. Puedes añadir tantos como desees para definir las posiciones posibles.")]
    public List<Transform> puntosDeSpawn;

    // Lista de prefabs a instanciar: Objetos prefabricados que se colocarán aleatoriamente.
    [Tooltip("Arrastra aquí los prefabs que se instanciarán aleatoriamente en los puntos de spawn. Añade diferentes prefabs para mayor variedad en el mapa.")]
    public List<GameObject> prefabs;

    // Número mínimo de prefabs que deben aparecer en el mapa.
    [Tooltip("Especifica el número mínimo de prefabs que deben instanciarse (mínimo 0, máximo igual al número de puntos de spawn). Esto asegura que el mapa no quede vacío.")]
    public int minimoPrefabs = 1;

    // GameObject contenedor donde se agruparán los prefabs instanciados.
    [Tooltip("Arrastra aquí el GameObject vacío que actuará como padre de todos los prefabs instanciados. Si no se asigna, los prefabs se instanciarán sin un padre específico.")]
    public GameObject contenedorPrefabs;

    void Start()
    {
        // Verificar que haya puntos y prefabs configurados.
        if (puntosDeSpawn.Count == 0 || prefabs.Count == 0)
        {
            Debug.LogWarning("No hay puntos de spawn o prefabs configurados en SC_GeneradorChatarreria.");
            return;
        }

        // Validar que el número mínimo de prefabs sea válido.
        if (minimoPrefabs < 0 || minimoPrefabs > puntosDeSpawn.Count)
        {
            Debug.LogWarning($"El valor de minimoPrefabs ({minimoPrefabs}) no es válido. Debe estar entre 0 y {puntosDeSpawn.Count}. Ajustando a un valor válido.");
            minimoPrefabs = Mathf.Clamp(minimoPrefabs, 0, puntosDeSpawn.Count);
        }

        // Calcular el número de puntos a ocupar aleatoriamente, asegurando al menos el mínimo especificado.
        // Se elige un número entre minimoPrefabs y el total de puntos disponibles.
        int numPuntosAOcupar = Random.Range(minimoPrefabs, puntosDeSpawn.Count + 1);

        // Mezclar la lista de puntos para seleccionar aleatoriamente sin repetir.
        // Creamos una copia de la lista para no modificar la original.
        List<Transform> puntosMezclados = new List<Transform>(puntosDeSpawn);
        MezclarLista(puntosMezclados);

        // Instanciar prefabs en los primeros 'numPuntosAOcupar' puntos de la lista mezclada.
        for (int i = 0; i < numPuntosAOcupar; i++)
        {
            // Elegir un prefab aleatorio de la lista.
            GameObject prefabAleatorio = prefabs[Random.Range(0, prefabs.Count)];

            // Instanciar el prefab en la posición y rotación del punto de spawn, con el contenedor como padre.
            // Si no hay contenedor asignado, se instancia sin padre.
            Instantiate(prefabAleatorio, puntosMezclados[i].position, puntosMezclados[i].rotation, contenedorPrefabs != null ? contenedorPrefabs.transform : null);
        }
    }

    // Método auxiliar para mezclar una lista genérica (Fisher-Yates shuffle).
    // Esto asegura aleatoriedad en la selección de puntos.
    private void MezclarLista<T>(List<T> lista)
    {
        for (int i = lista.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            T temp = lista[i];
            lista[i] = lista[j];
            lista[j] = temp;
        }
    }
}