using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

namespace Tunnel
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField]
        private GameObject _impediment;
        private GameObject _ligthCentery;
        private GameObject _ball;

        [SerializeField, Range(10, 50)]
        private int _volume;
        [SerializeField]
        private int _health;
        private int _lvl;

        private bool _up = true;

        [SerializeField]
        private List<Material> _materials = new List<Material>(4);
        private List<Transform> _impedimentsList = new List<Transform>();

        private BallManager _bm;

        void Start()
        {
            _ball = GameObject.Find("Ball");
            _ligthCentery = GameObject.Find("PointLightCenter");            
            _bm = _ball.GetComponent<BallManager>();
            _bm.EventGoal += () => Goal();
            _health = 3;
            _lvl = 1;
            CreateLevel();
        }

        void Update()
        {
            LCMovement();
        }

        void GenerationImpediments(GameObject imped, int volume)
        {
            var pos = new Vector3();
            var rot = new Quaternion();
            int i = 0;

            while (i < volume)
            {
                pos.y = Random.Range(-10, 10);
                pos.x = Random.Range(-2, 2);
                pos.z = Random.Range(-2, 2);

                rot.x = Random.Range(0, 180);
                rot.y = Random.Range(0, 180);
                rot.z = Random.Range(0, 180);

                var obj = Instantiate(imped, pos, rot, gameObject.transform);
                obj.GetComponent<MeshRenderer>().material = _materials[Random.Range(0, 4)];
                _impedimentsList.Add(obj.transform);
                i++;
            }
        }

        //ligth cemtery movement
        void LCMovement()
        {
            if (_up)
                _ligthCentery.transform.position += Vector3.up * Time.deltaTime * 4;
            else
                _ligthCentery.transform.position += Vector3.down * Time.deltaTime * 4;

            if (7.5 < _ligthCentery.transform.position.y && _ligthCentery.transform.position.y < 8)
                _up = false;
            if (-8 < _ligthCentery.transform.position.y && _ligthCentery.transform.position.y < -7.5)
                _up = true;
        }

        void Goal()
        {
            if (_health > 1)
            {
                _health--;
                Debug.Log($"Осталось жизней: {_health} ");
                _bm.BallRestart();
            }
            else
                Restart();
        }

        public void CreateLevel(bool up = false)
        {
            if(up)
                _lvl++;
            switch (_lvl)
            {
                case 1:
                    _volume = 1;
                    break;
                case 2:
                    _volume = 10;
                    break;
                case 3:
                    _volume = 15;
                    break;
                case 4:
                    _volume = 20;
                    break;
                case 5:
                    _volume = 25;
                    break;
                
            }
            GenerationImpediments(_impediment, _volume);
            if (_lvl == 1) return; 
            _bm.BallRestart();
        }

        void Restart()
        {
            _bm.BallRestart();
            Debug.Log("---GAME OVER---");
            UnityEditor.EditorApplication.isPaused = true;
        }

        public int ImpedimentList(string text, Transform obj = null)
        {
            int count = 0;
            switch (text)
            {
                case "remove":
                    _impedimentsList.Remove(obj);
                    break;
                case "count":
                    count = _impedimentsList.Count;
                    break;
            }
            return count;
        }
    }
}
