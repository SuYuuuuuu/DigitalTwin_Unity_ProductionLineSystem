using Common;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace LabProductLine.TransisionModule
{

    public class LoadResource : MonoBehaviour
    {
        public event Action CoroutineCallback;
        public Slider slider;
        public float speed = 10f;

        private void Start()
        {
            CoroutineCallback += OnResourceLoaded;
            Coroutine task = StartCoroutine(ResourceManager.GetConfigFile("config.txt", CoroutineCallback));
        }

        private void OnResourceLoaded()
        {
            slider.value = 1;
            StartCoroutine(LoadScene());

        }

        private void Update()
        {
            slider.value += Time.deltaTime * speed;
        }

        private IEnumerator LoadScene()
        {
            yield return new WaitForSeconds(2f);
            SceneManager.LoadSceneAsync(1);
        }

    }
}