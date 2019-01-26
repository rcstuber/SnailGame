using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blitz : MonoBehaviour
{

    public GameObject blitz;
    public GameObject announcePoint;

    public float waitMinTime = 0.1f, WaitMaxTime = 1f;
    public float announceTime;
    public float blitzTime;

    void Start() {
        blitz.SetActive(false);
        announcePoint.SetActive(false);
    }

    void OnEnable() {
        StartCoroutine(WaitForBlitz());
    }


    IEnumerator WaitForBlitz() {
        float waitSec = Random.Range(waitMinTime, WaitMaxTime);
        yield return new WaitForSeconds(waitSec);

        StartCoroutine(AnnounceBlitz());
    }

    IEnumerator AnnounceBlitz() {

        announcePoint.SetActive(true);
        yield return new WaitForSeconds(announceTime);
        announcePoint.SetActive(false);

        StartCoroutine(Blitzen());
    }

    IEnumerator Blitzen() {
        blitz.SetActive(true);
        yield return new WaitForSeconds(blitzTime);
        blitz.SetActive(false);

        this.gameObject.SetActive(false);
    }
}
