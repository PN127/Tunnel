using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Tunnel
{
    public class BallManager : MonoBehaviour
    {
        private Vector3 _direction;
        private Vector3 _v1;
        private Vector3 _v2;
        private Vector3 _normal;
        private Vector3 _pos;
        private Vector3 _target_pos;
        private Vector3 _contactPoint;

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

            transform.position += transform.forward * Time.deltaTime * _ballSpeed;
        }

        private void OnDrawGizmos()
        {
            var camera = SceneView.currentDrawingSceneView.camera;

            Draw();
        }

        void Draw()
        {
            Gizmos.color = Color.red;
            if (_target_pos != null && _normal != null)
                Gizmos.DrawLine(_target_pos, _pos+_v2);
            Gizmos.color = Color.yellow;
            if (_target_pos != null && _normal != null)
                Gizmos.DrawLine(_target_pos,_target_pos + _normal);
            Gizmos.color = Color.blue;
            if (_target_pos != null && _normal != null)
                Gizmos.DrawLine(_target_pos,  _direction);
            Gizmos.color = Color.green;
            if (_target_pos != null && _normal != null)
                Gizmos.DrawLine(_target_pos, _contactPoint);
        }

        private void OnCollisionEnter(Collision collision)
        {
            _target_pos = collision.transform.position;
            _normal = collision.contacts[0].normal;
            _contactPoint = collision.contacts[0].point;
            _v2 = _contactPoint - _target_pos;
            _pos = transform.position - _v2;
            _v1 = _target_pos - _pos;

            _direction = Vector3.Reflect(_v1, _normal);
            transform.LookAt(_direction + transform.position);
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