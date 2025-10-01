using System.Collections.Generic;
using UnityEngine;

// Resumen: Este script genera un mapa aleatorio al inicio del juego para una chatarrer�a. Toma una lista de puntos de spawn (GameObjects vac�os) y una lista de prefabs. De forma aleatoria, selecciona un subconjunto de estos puntos y instancia prefabs aleatorios en ellos, asegurando que al menos un n�mero m�nimo especificado de prefabs aparezca para evitar un mapa vac�o. Los prefabs instanciados se agrupan como hijos de un GameObject contenedor especificado. Es escalable, permitiendo agregar m�s puntos y prefabs f�cilmente desde el Inspector de Unity.

public class SC_GeneradorChatarreria : MonoBehaviour
{
    // Lista de puntos de spawn: GameObjects vac�os donde potencialmente se instanciar�n prefabs.
    [Tooltip("Arrastra aqu� los GameObjects vac�os que servir�n como puntos de spawn para los prefabs. Puedes a�adir tantos como desees para definir las posiciones posibles.")]
    public List<Transform> puntosDeSpawn;

    // Lista de prefabs a instanciar: Objetos prefabricados que se colocar�n aleatoriamente.
    [Tooltip("Arrastra aqu� los prefabs que se instanciar�n aleatoriamente en los puntos de spawn. A�ade diferentes prefabs para mayor variedad en el mapa.")]
    public List<GameObject> prefabs;

    // N�mero m�nimo de prefabs que deben aparecer en el mapa.
    [Tooltip("Especifica el n�mero m�nimo de prefabs que deben instanciarse (m�nimo 0, m�ximo igual al n�mero de puntos de spawn). Esto asegura que el mapa no quede vac�o.")]
    public int minimoPrefabs = 1;

    // GameObject contenedor donde se agrupar�n los prefabs instanciados.
    [Tooltip("Arrastra aqu� el GameObject vac�o que actuar� como padre de todos los prefabs instanciados. Si no se asigna, los prefabs se instanciar�n sin un padre espec�fico.")]
    public GameObject contenedorPrefabs;

    void Start()
    {
        // Verificar que haya puntos y prefabs configurados.
        if (puntosDeSpawn.Count == 0 || prefabs.Count == 0)
        {
            Debug.LogWarning("No hay puntos de spawn o prefabs configurados en SC_GeneradorChatarreria.");
            return;
        }

        // Validar que el n�mero m�nimo de prefabs sea v�lido.
        if (minimoPrefabs < 0 || minimoPrefabs > puntosDeSpawn.Count)
        {
            Debug.LogWarning($"El valor de minimoPrefabs ({minimoPrefabs}) no es v�lido. Debe estar entre 0 y {puntosDeSpawn.Count}. Ajustando a un valor v�lido.");
            minimoPrefabs = Mathf.Clamp(minimoPrefabs, 0, puntosDeSpawn.Count);
        }

        // Calcular el n�mero de puntos a ocupar aleatoriamente, asegurando al menos el m�nimo especificado.
        // Se elige un n�mero entre minimoPrefabs y el total de puntos disponibles.
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

            // Instanciar el prefab en la posici�n y rotaci�n del punto de spawn, con el contenedor como padre.
            // Si no hay contenedor asignado, se instancia sin padre.
            Instantiate(prefabAleatorio, puntosMezclados[i].position, puntosMezclados[i].rotation, contenedorPrefabs != null ? contenedorPrefabs.transform : null);
        }
    }

    // M�todo auxiliar para mezclar una lista gen�rica (Fisher-Yates shuffle).
    // Esto asegura aleatoriedad en la selecci�n de puntos.
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