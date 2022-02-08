using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

namespace Tunnel
{
    public class PlayerMovement : MonoBehaviour
    {
        private PlayersControls controls;
        private InputAction _playerMovement;
        private Vector3 motor;

        private void Awake()
        {
            controls = new PlayersControls();
            motor = Vector3.zero;
        }

        private void OnEnable()
        {
            if (gameObject.name.Contains("1"))
            {
                controls.Player1.Enable();
                _playerMovement = controls.Player1.Movement;
            }
            else if (gameObject.name.Contains("2"))
            {
                controls.Player2.Enable();
                _playerMovement = controls.Player2.Movement;
            }
            else
                Debug.LogError("Игрок не найдет");
        }

        void Update()
        {            
            var value = _playerMovement.ReadValue<Vector2>();

            motor += new Vector3(value.x * 0.3f, 0, value.y * 0.3f);
            transform.position += motor * Time.deltaTime * 0.4f;
            transform.position = new Vector3(Mathf.Clamp(transform.position.x, -4, 4), transform.position.y, Mathf.Clamp(transform.position.z, -4, 4));

            if (transform.position.x == 4 || transform.position.x == -4)
                motor.x = 0;
            
            if (transform.position.z == 4 || transform.position.z == -4)
                motor.z = 0;

            if (motor.x > 0.0f)
                motor.x -= 0.1f;
            if (motor.x < 0.0f)
                motor.x += 0.1f;
            if (-0.1 < motor.x && motor.x < 0.1)
                motor.x = 0;
                        
            if (motor.z > 0.0f)
                motor.z -= 0.1f;
            if (motor.z < 0.0f)
                motor.z += 0.1f;
            if (-0.1 < motor.z && motor.z < 0.1)
                motor.z = 0;
        }        

        private void OnDisable()
        {

            if (gameObject.name.Contains("1"))
                controls.Player1.Disable();
            if (gameObject.name.Contains("2"))
                controls.Player2.Disable();
        }
    }
}