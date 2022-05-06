using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;
using UnityEngine.UI;


namespace Tunnel
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField]
        private GameObject _impediment;
        private GameObject _ligthCentery;
        private GameObject _ball;

        [SerializeField]
        private int _health;
        private int _volume;
        private int _lvl;

        private bool _up = true;

        [SerializeField]
        private List<Material> _materials = new List<Material>(4);
        private List<Transform> _impedimentsList = new List<Transform>();        
        [SerializeField]
        private List<int> _volumeOnLvl = new List<int>(5);

        [SerializeField]
        private Text _showCountHealthPlayer1;
        [SerializeField]
        private Text _showCountHealthPlayer2;

        private BallManager _bm;

        void Start()
        {
            _ball = GameObject.Find("Ball");
            _ligthCentery = GameObject.Find("PointLightCenter");            
            _bm = _ball.GetComponent<BallManager>();
            _bm.EventGoal += () => Goal();
            _lvl = 1;
            CreateLevel();
            _showCountHealthPlayer1.text = $"<color=#E00101><b>{_health}</b></color>";
            _showCountHealthPlayer2.text = $"<color=#E00101><b>{_health}</b></color>";
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
                _showCountHealthPlayer1.text = $"<color=#E00101><b>{_health}</b></color>";
                _showCountHealthPlayer2.text = $"<color=#E00101><b>{_health}</b></color>";
                _bm.BallRestart();
            }
            else
            {
                _health--;
                _showCountHealthPlayer1.text = $"<color=#E00101><b>{_health}</b></color>";
                _showCountHealthPlayer2.text = $"<color=#E00101><b>{_health}</b></color>";
                GameOver();
            }
        }

        public void CreateLevel(bool up = false)
        {
            if (up)
            {
                _lvl++;
            }

            switch (_lvl)
            {
                case 1:
                    if(_volumeOnLvl[0] <= 25)
                        _volume = _volumeOnLvl[0];
                    else
                        _volume = 25;
                    break;
                case 2:
                    if (_volumeOnLvl[1] <= 25)
                        _volume = _volumeOnLvl[1];
                    else
                        _volume = 25;
                    break;
                case 3:
                    if (_volumeOnLvl[2] <= 25)
                        _volume = _volumeOnLvl[2];
                    else
                        _volume = 25;
                    break;
                case 4:
                    if (_volumeOnLvl[3] <= 25)
                        _volume = _volumeOnLvl[3];
                    else
                        _volume = 25;
                    break;
                case 5:
                    if (_volumeOnLvl[4] <= 25)
                        _volume = _volumeOnLvl[4];
                    else
                        _volume = 25;
                    break;

            }
            GenerationImpediments(_impediment, _volume);
            if (_lvl == 1) return; 
            _bm.BallRestart();
        }

        void GameOver()
        {
            _bm.BallRestart();
            Debug.Log("---GAME OVER---");
            UnityEditor.EditorApplication.isPaused = true;
        }

        public void Restart()
        {
            _bm.BallRestart();
            ImpedimentList("restart");
            _lvl = 1;
            CreateLevel();
        }

        public int ImpedimentList(string text, Transform obj = null)
        {
            int count = 0;
            switch (text)
            {
                case "remove":
                    _impedimentsList.Remove(obj);
                    break;

                case "restart":
                    int i = 0;
                    int iC = _impedimentsList.Count - 1;
                    while (i < iC)
                    {
                        Destroy(_impedimentsList[0].gameObject);
                        _impedimentsList.Remove(_impedimentsList[0]);                        
                        i++;
                    }
                    break;

                case "count":
                    count = _impedimentsList.Count;
                    break;
            }
            return count;
        }
    }
}
