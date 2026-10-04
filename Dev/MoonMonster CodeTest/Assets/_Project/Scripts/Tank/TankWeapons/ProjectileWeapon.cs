using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace MoonMonster.Codetest
{
    public class ProjectileWeapon : WeaponBase
    {
        public ProjectileWeapon(WeaponData weaponData) : base(weaponData)
        {
        }

        public override void Fire(Vector3 position, Quaternion rotation)
        {
            if (_hasFired)
                return;
        }
    }
}
