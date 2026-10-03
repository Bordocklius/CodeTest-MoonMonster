using UnityEngine;

namespace MoonMonster.Codetest
{
    [CreateAssetMenu(fileName = "WeaponData", menuName = "Weapons/Weapon")]
    public class WeaponData : ScriptableObject
    {
        public GameObject Projectile;
        public AudioClip FireClip;
        public AudioClip SelectClip;
        public float LaunchForce;
        public float FireDelay;
    }
}
