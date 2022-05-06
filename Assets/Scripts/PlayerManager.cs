using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;


namespace Tunnel
{
    public class PlayerManager : MonoBehaviour
    {
        private PlayersControls _controls;
        private InputAction _playerMovement;
        [SerializeField]
        private MenuManager _menuManager;
        [SerializeField]
        private Canvas _pause;
        [SerializeField]
        private GameObject _panel;
        private RectTransform _rectTransform;

        private Vector3 motor;

        private bool ThisPlayer1;               

        private void Awake()
        {
            _controls = new PlayersControls();            
            motor = Vector3.zero;
        }

        private void OnEnable()
        {
            if (gameObject.name.Contains("1"))
            {
                _controls.Player1.Enable();
                _playerMovement = _controls.Player1.Movement;
                ThisPlayer1 = true;
            }
            else if (gameObject.name.Contains("2"))
            {
                _controls.Player2.Enable();
                _playerMovement = _controls.Player2.Movement;
                ThisPlayer1 = false;
            }
            else
                Debug.LogError("Игрок не найдет");

            if (ThisPlayer1)
            {
                _controls.Player1.Pause.Enable();
                _controls.Player1.Pause.performed += PauseMenu;
            }
        }

        private void Start()
        {
            if (ThisPlayer1)
                _rectTransform = _panel.GetComponent<RectTransform>();
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

        private void PauseMenu(CallbackContext callbackContext)
        {
            _menuManager.close = true;
            _menuManager.ScaleUI(_rectTransform);            
        }

        private void OnDisable()
        {
            if (gameObject.name.Contains("1"))
                _controls.Player1.Disable();
            if (gameObject.name.Contains("2"))
                _controls.Player2.Disable();
        }
    }
}