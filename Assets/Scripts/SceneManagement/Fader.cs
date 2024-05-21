using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace RPG.SceneManagement
{
    public class Fader : MonoBehaviour
    {
        CanvasGroup _canvasGroup;
        void Awake()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
            _canvasGroup.alpha = 0f;
        }

        public void FadeOutImmediate()
        {
            _canvasGroup.alpha = 1f;
        }


        public IEnumerator FadeOut(float time)
        {
            while (_canvasGroup.alpha < 1) 
            {
                _canvasGroup.alpha += Time.deltaTime / time;
                yield return null;
            }
        } 
        public IEnumerator FadeIn(float time)
        {
            while (_canvasGroup.alpha > 0)  
            {
                _canvasGroup.alpha -= Time.deltaTime / time;
                yield return null;
            }
        } 
    }
}
