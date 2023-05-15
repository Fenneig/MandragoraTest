using UnityEngine;
using UnityEngine.SceneManagement;

namespace Mandragora.Utils
{
    public class SceneSystem : MonoBehaviour
    {
        private void Start()
        {
            SceneManager.LoadScene(Idents.Scenes.HudScene, LoadSceneMode.Additive);
        }
    }
}
