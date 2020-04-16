using UnityEngine;

namespace Script.Core
{
    public class PersistenceObject : MonoBehaviour
    {
        
        [SerializeField] private GameObject persistenceObjectPrefab;
        // ReSharper disable once InconsistentNaming
        private static bool hasSpawn;

        private void Awake()
        {
            if (hasSpawn) return;
            SpawnPersistenceObject();
            hasSpawn = true;
        }

        private void SpawnPersistenceObject()
        {
            var persistenceObject = Instantiate(persistenceObjectPrefab);
            DontDestroyOnLoad(persistenceObject);
        }
    }
}
