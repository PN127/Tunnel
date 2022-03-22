using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Tunnel
{
    public class BallManager : MonoBehaviour
    {
        public delegate void EventVoid();
        public event EventVoid EventGoal;

        private GameManager _gm;

        private Vector3 _normal;
        private Vector3 _ballBase;

        [SerializeField, Range(1, 15)]
        private float _ballSpeed;
        [SerializeField]
        private float _ballSpeedStart = 2;
        

        private bool _release = false;        

        private void Start()
        {
            _ballBase = transform.localPosition;
            _gm = GameObject.Find("Walls").GetComponent<GameManager>();
            _ballSpeed = _ballSpeedStart;
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
            
            
            transform.position += transform.forward * Time.deltaTime * _ballSpeed;
        }

        private void OnCollisionEnter(Collision collision)
        {
            _normal = collision.contacts[0].normal;
            transform.forward = Vector3.Reflect(transform.forward, _normal);

            var obj = collision.gameObject;
            if (obj.tag == "Impediment")
            {
                _gm.ImpedimentList("remove", collision.transform);
                Destroy(obj);

                _ballSpeed += 0.5f;

                if (_gm.ImpedimentList("count") <= 0)
                {
                    _gm.CreateLevel(true);
                    _ballSpeedStart += 0.5f;
                    _ballSpeed = _ballSpeedStart;
                }
            }
            if (obj.tag == "Finish")
                EventGoal?.Invoke();
        }

        public void BallRestart()
        {
            _release = false;
            transform.parent = GameObject.Find("Player1").transform;
            transform.localPosition = _ballBase;
            transform.rotation = new Quaternion(0,0,0,0);
            _ballSpeed = _ballSpeedStart;
        }

    }
}