using System.Collections;
using Script.Controller;
using Script.Core;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AI;

namespace Script.SceneManagement
{
    public class Portal : MonoBehaviour
    {
        private enum DestinationIdentifier
        {
            A, B, C
        }
        
        [SerializeField] private int sceneToLoad = -1;
        [SerializeField] private Transform spawnPawn;
        [SerializeField] private DestinationIdentifier destination;
        [SerializeField] private float fadeWaitTime = 2f;
        [SerializeField] private float timeFadeOut = 1f;
        [SerializeField] private float timeFadeIn = 2f;

        private static bool IsPlayerCollider(Component player)
        {
            return player.CompareTag("Player");
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if (IsPlayerCollider(other))
            {
                StartCoroutine(Transition());
            }
        }

        private IEnumerator Transition()
        {
            if (sceneToLoad < 0)    
            {
                Debug.LogError("No scene to load");
                yield break;
            }
            
            //destroy the other portal when player come straight back to portal
            foreach (var portal in FindObjectsOfType<Portal>())
            {
                if (portal != this) Destroy(portal.gameObject);
            }
            
            var player = GameObject.FindWithTag("Player");
            var saveWrapper = FindObjectOfType<SavingWrapper>();
            player.GetComponent<ActionScheduler>().CancelAction();
            player.GetComponent<PlayerController>().enabled = false;
            var fader = FindObjectOfType<Fader>();
            DontDestroyOnLoad(gameObject);
            print(gameObject.name);
            yield return fader.FadeOut(timeFadeOut);

            saveWrapper.Save();
            
            yield return SceneManager.LoadSceneAsync(sceneToLoad);
            
            saveWrapper.Load();
            
            UpdatePlayer(GetOtherPortal());
            
            saveWrapper.Save();
            
            yield return new WaitForSeconds(fadeWaitTime);
            yield return fader.FadeIn(timeFadeIn);
            Destroy(gameObject);
        }

        private void UpdatePlayer(Portal otherPortal)
        {
            var player = GameObject.FindWithTag("Player");
            player.GetComponent<NavMeshAgent>().enabled = false;
            player.transform.position = otherPortal.spawnPawn.position;
            player.transform.rotation = otherPortal.spawnPawn.rotation;
            player.GetComponent<NavMeshAgent>().enabled = true;
        }

        private Portal GetOtherPortal()
        {
            foreach (var portal in FindObjectsOfType<Portal>())
            {
                if (portal == this) continue;
                if (portal.destination != destination) continue;
                return portal;
            }

            return null;
        }
    }
}
