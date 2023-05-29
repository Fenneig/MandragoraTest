using System.Collections.Generic;
using Mandragora.Commands;
using Mandragora.Services;
using Mandragora.Utils;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Mandragora.Systems
{
    public class GameManager : MonoBehaviour
    {
        public static ServiceLocator<IService> ServiceLocator { get; private set; }
        
        private List<AsyncOperation> _loadOperations;

        private void Start()
        {
            _loadOperations = new List<AsyncOperation>();
            
            DontDestroyOnLoad(this);
            
            InitServices();

            LoadLevel(Idents.Scenes.GameScene);
            LoadLevel(Idents.Scenes.HudScene);
        }

        private void InitServices()
        {
            ServiceLocator = new ServiceLocator<IService>();
            
            AlertService alertService = ServiceLocator.Register(new AlertService());
            ServiceLocator.Register(new CommandService(alertService));
        }

        public void LoadLevel(string levelName)
        {
            AsyncOperation loadOperation = SceneManager.LoadSceneAsync(levelName, LoadSceneMode.Additive);
            if (loadOperation == null)
            {
                Debug.LogError($"[GameManager] Unable to load level {levelName}");
                return;
            }
        
            loadOperation.completed += OnLoadOperationComplete;
            _loadOperations.Add(loadOperation);
        }
        
        private void OnLoadOperationComplete(AsyncOperation loadOperation)
        {
            if (_loadOperations.Contains(loadOperation))
            {
                _loadOperations.Remove(loadOperation);
            }
        }
    }
}