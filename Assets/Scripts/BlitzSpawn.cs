using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlitzSpawn : MonoBehaviour
{
    public float atSameTime = 2;
    public float radiusOfEarth = 0.85f;
    public Vector2 spawnAnge = new Vector2 (-20, -180);

    [Header("Until Next Blitz")]
    [Space]
    public float waitMinTime = 2; 
    public float WaitMaxTime = 3;

    [Header("Pooling")]
    [Space]
    public GameObject pooledObject;
    public int pooledAmount = 20;
    List<GameObject> pooledObjects;

    void Start(){
        pooledObjects = new List<GameObject>();
        for (int i = 0; i < pooledAmount; i++) {
            GameObject obj = (GameObject)Instantiate(pooledObject);
            obj.transform.parent = transform;
            obj.SetActive(false);
            pooledObjects.Add(obj);
        }

        StartCoroutine(ActivateBlitz());
    }

    IEnumerator ActivateBlitz() {
        float waitSec = Random.Range(waitMinTime, WaitMaxTime);
        yield return new WaitForSeconds(waitSec);

        for (int a = 0; a < atSameTime; a++) {

            for (int i = 0; i < pooledObjects.Count; i++) {
                if (!pooledObjects[i].activeInHierarchy) {
                    pooledObjects[i].SetActive(true);
                    pooledObjects[i].transform.position = transform.position;

                    // hier muss eine random Winkeldrehung rein
                    pooledObjects[i].transform.transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, Random.Range(spawnAnge.x, spawnAnge.y));
                    pooledObjects[i].transform.Translate(new Vector3(0, radiusOfEarth, 0));
                    break;
                }
            }
        }

        StartCoroutine(ActivateBlitz());
    }
}
