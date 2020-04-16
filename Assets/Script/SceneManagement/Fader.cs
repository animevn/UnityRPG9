using System;
using System.Collections;
using UnityEngine;

namespace Script.SceneManagement
{
    public class Fader : MonoBehaviour
    {

        private CanvasGroup canvas;
        
        private void Start()
        {
            canvas = GetComponent<CanvasGroup>();
        }

//        public IEnumerator FadeOutIn(float time)
//        {
//            yield return FadeOut(time);
//            print("Fadeout OK");
//            yield return FadeIn(time);
//            print("Fadein OK");
//        }


        public void FadeOutImmediate()
        {
            canvas.alpha = 1;
        }

        public IEnumerator FadeOut(float time)
        {
            while (Math.Abs(canvas.alpha - 1) > 0)
            {
                canvas.alpha += Time.deltaTime / time;
                yield return null;
            }
        }
        
        public IEnumerator FadeIn(float time)
        {
            while (Math.Abs(canvas.alpha) > 0)
            {
                canvas.alpha -= Time.deltaTime / time;
                yield return null;
            }
        }
    }   
}
