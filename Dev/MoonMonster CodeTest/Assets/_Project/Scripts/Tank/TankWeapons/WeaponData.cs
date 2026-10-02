using UnityEngine;

namespace MoonMonster.Codetest
{
    /// <summary>
    /// Shared weapon data for multiple weapons
    /// </summary>
    [CreateAssetMenu(fileName = "WeaponData", menuName = "Weapons/Weapon")]
    public class WeaponData : ScriptableObject
    {
        public Rigidbody Projectile;
        public AudioClip FireClip;
        public float LaunchForce;
        public float FireDelay;
    }
}
