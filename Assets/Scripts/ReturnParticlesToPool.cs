using UnityEngine;

namespace Spawners
{
    /// <summary>
    /// 
    /// 
    /// Author: William Min
    /// </summary>
    public class ReturnParticlesToPool : MonoBehaviour
    {
        private void OnParticleSystemStopped()
        {
            ObjectPoolManager.ReturnObjectToPool(gameObject);
        }
    }
}
