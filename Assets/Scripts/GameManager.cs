using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private GameObject Impediment;
    [SerializeField, Range(10, 50)]
    private int volume;
    [SerializeField]
    public List<Material> materials = new List<Material>(4);

    private GameObject ligthCentery;
    private bool up = true;

    void Start()
    {
        ligthCentery = GameObject.Find("PointLightCenter");
        GenerationImpediments(Impediment);
    }

    void Update()
    {
        if (up)
            ligthCentery.transform.position += Vector3.up * Time.deltaTime * 4;
        else
            ligthCentery.transform.position += Vector3.down * Time.deltaTime * 4;

        if (7.5 < ligthCentery.transform.position.y && ligthCentery.transform.position.y < 8)
            up = false;
        if (-8 < ligthCentery.transform.position.y && ligthCentery.transform.position.y < -7.5)
            up = true;

    }

    void GenerationImpediments(GameObject imped)
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
            obj.GetComponent<MeshRenderer>().material = materials[Random.Range(0, 4)];

            i++;
        }
    }
}
