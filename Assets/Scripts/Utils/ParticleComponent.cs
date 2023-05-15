using UnityEngine;

namespace Mandragora.Utils
{
    public class ParticleComponent : MonoBehaviour
    {
        [SerializeField] private ParticleSystem _particle;
        
        public void PlayParticleEffect()
        {
            _particle.Play();
        }
    }
}