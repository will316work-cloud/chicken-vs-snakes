using UnityEngine;

using ChickenSnakes.Entities;

namespace Spawners
{
    /// <summary>
    /// Holds general projectile data for firing.
    /// 
    /// Author: William Min
    /// Date: 12/13/25
    /// </summary>
    [CreateAssetMenu(fileName = "Fire Projectile Template", menuName = "Chicken VS Snakes Scriptables/Fire Projectile")]
    public class FireProjectile : ScriptableObject
    {
        [SerializeField] private GameObject _projectilePrefab;

        public GameObject SummonProjectile(Vector3 position, Quaternion rotation, Vector3 scale, GameObject owner = null, int teamIndex = 0)
        {
            GameObject projectile = ObjectPoolManager.SpawnObject(_projectilePrefab, position, rotation);
            projectile.transform.localScale = scale;

            Entity projectileEntity = projectile.GetComponentInChildren<Entity>();

            if (projectileEntity != null)
            {
                projectileEntity.Owner = owner;
                projectileEntity.TeamIndex = teamIndex;
            }

            return projectile;
        }

        public GameObject SummonProjectile(Vector3 position, Vector3 eulerAngles, Vector3 scale, GameObject owner = null, int teamIndex = 0)
        {
            return SummonProjectile(position, Quaternion.Euler(eulerAngles), scale, owner, teamIndex);
        }

        public GameObject SummonProjectile(Transform origin, GameObject owner = null, int teamIndex = 0)
        {
            return SummonProjectile(origin.position, origin.rotation, _projectilePrefab.transform.localScale, owner, teamIndex);
        }
    }
}
