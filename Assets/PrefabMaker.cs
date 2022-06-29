using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabMaker : MonoBehaviour
{
    [SerializeField]
    int prefabCount;

    [SerializeField]
    GameObject prefab;


    private void Awake()
    {
        for(int i = 0; i < prefabCount; ++i)
        {
            var instanced = GameObject.Instantiate(prefab);
            var rand = Random.insideUnitCircle * 10;

            instanced.transform.position = (new Vector3(rand.x, 0, rand.y) + transform.position);
        }
    }

}
