using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Tunnel
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField]
        private GameObject _impediment;
        //[SerializeField]
        //private GameObject _ball;
        [SerializeField, Range(10, 50)]
        private int _volume;
        [SerializeField]
        private List<Material> _materials = new List<Material>(4);

        private GameObject _ligthCentery;

        private bool _up = true;

        void Start()
        {
            _ligthCentery = GameObject.Find("PointLightCenter");
            GenerationImpediments(_impediment);
        }

        void Update()
        {
            LCMovement();

        }

        void GenerationImpediments(GameObject imped)
        {
            var pos = new Vector3();
            var rot = new Quaternion();
            int i = 0;

            while (i < _volume)
            {
                pos.y = Random.Range(-10, 10);
                pos.x = Random.Range(-2, 2);
                pos.z = Random.Range(-2, 2);

                rot.x = Random.Range(0, 180);
                rot.y = Random.Range(0, 180);
                rot.z = Random.Range(0, 180);

                var obj = Instantiate(imped, pos, rot, gameObject.transform);
                obj.GetComponent<MeshRenderer>().material = _materials[Random.Range(0, 4)];

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

        
    }
}
