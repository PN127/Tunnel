using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Tunnel
{
    public class BallManager : MonoBehaviour
    {
        private Vector3 _direction;

        [SerializeField, Range(1, 15)]
        private float _ballSpeed = 2;

        private bool _release = false;

        void Start()
        {
            _direction = transform.forward;
        }

        void Update()
        {
            if (Keyboard.current[Key.Space].isPressed)
                _release = true;
            if (_release)
                BallMove();
        }

        void BallMove()
        {
            transform.parent = null;

            transform.position += _direction * Time.deltaTime * _ballSpeed;
        }
        
        //КОД РАБОТАЕТ НЕ ТАК! НУЖНА ДОРОБОТКА
        private void OnCollisionEnter(Collision collision)
        {
            var _normal = collision.contacts[0].normal;
            _direction = transform.position.normalized;
            _direction = Vector3.Reflect(_direction, _normal);

            Debug.Log("COLLISION" + collision.gameObject.name);
        }

        //private void OnTriggerEnter(Collider other)
        //{
        //    var target = other.gameObject;
        //    if(target.name.Contains("Gates"))
        //        UnityEditor.EditorApplication.isPaused = true;

            
        //    var normal = target.transform.position;
        //    //var reflection = Vector3.Reflect(transform.position, other.con);
        //    //_direction = reflection.normalized;
        //    Debug.Log("Ola" + target.name + " " + _direction);
        //}

    }
}