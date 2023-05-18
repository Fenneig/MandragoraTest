using System.Collections;
using UnityEngine;

namespace Mandragora.Utils
{
    public class Coroutines : MonoBehaviour
    {
        private static Coroutines Instance
        {
            get
            {
                if (_instance != null) return _instance;
                
                var go = new GameObject("COROUTINE MANAGER");
                _instance = go.AddComponent<Coroutines>();
                DontDestroyOnLoad(go);
                return _instance;
            }
        }

        private static Coroutines _instance;

        public static Coroutine StartRoutine(IEnumerator enumerator)
        {
            return Instance.StartCoroutine(enumerator);
        }

        public static void StopRoutine(Coroutine routine)
        {
            Instance.StopCoroutine(routine);
        }
    }
}