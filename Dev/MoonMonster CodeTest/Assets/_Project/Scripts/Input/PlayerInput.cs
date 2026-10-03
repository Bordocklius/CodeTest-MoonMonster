using NaughtyAttributes;
using UnityEngine;

namespace MoonMonster.Codetest
{
    public class PlayerInput : MonoBehaviour
    {
        [SerializeField, Required] private TankMovement _movement;
        [SerializeField, Required] private TankShooting _shooting;

        public int PlayerNumber { get; set; }

        private string _movementAxisName;
        private string _turnAxisName;
        private string _fireButtonName;
        
        void Start()
        {            
            _movementAxisName = "Vertical" + PlayerNumber;
            _turnAxisName = "Horizontal" + PlayerNumber;
            _fireButtonName = "Fire" + PlayerNumber;
        }

        void Update()
        {
            _movement.SetMoveInput(Input.GetAxis (_movementAxisName),Input.GetAxis (_turnAxisName));

            if (Input.GetButton(_fireButtonName))
            {
                _shooting.Fire();
            }

            CheckWeaponSwitch();
        }

        private void CheckWeaponSwitch()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
                _shooting.ChangeWeapon(0);
            else if (Input.GetKeyDown(KeyCode.Alpha2))
                _shooting.ChangeWeapon(1);
            else if (Input.GetKeyDown(KeyCode.Alpha3))
                _shooting.ChangeWeapon(2);
            else if (Input.mouseScrollDelta.y > 0)
                _shooting.ChangeWeapon(true);
            else if (Input.mouseScrollDelta.y < 0)
                _shooting.ChangeWeapon(false);
        }
    }
}