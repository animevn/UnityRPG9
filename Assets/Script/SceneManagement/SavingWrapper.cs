using System;
using System.Collections;
using Script.Saving;
using UnityEngine;

namespace Script.SceneManagement
{
    public class SavingWrapper : MonoBehaviour
    {
        private const string DefaultSave = "save";

        private IEnumerator Start()
        {
            var fader = FindObjectOfType<Fader>();
            fader.FadeOutImmediate();
            yield return GetComponent<SavingSystem>().LoadLastScene(DefaultSave);
            yield return fader.FadeIn(1f);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.L))
            {
                Load();
            }

            if (Input.GetKeyDown(KeyCode.S))
            {
                Save();
            }
        }

        public void Save()
        {
            GetComponent<SavingSystem>().Save(DefaultSave);
        }

        public void Load()
        {
            GetComponent<SavingSystem>().Load(DefaultSave);
        }
    }
}
