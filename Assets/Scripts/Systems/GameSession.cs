using Mandragora.UnitBased;
using UnityEngine;

namespace Mandragora.Systems
{
    public class GameSession : MonoBehaviour
    {
        [SerializeField] private UnitActionSystem _unitActionSystem;

        public UnitActionSystem UnitActionSystem => _unitActionSystem;

        public static GameSession Instance;
        private void Awake()
        {
            Instance ??= this;
        }
    }
}