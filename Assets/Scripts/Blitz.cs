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

    public bool startLerp;
    Vector3 startScale;

    void Start() {
        blitz.SetActive(false);
        announcePoint.SetActive(false);

        startScale = announcePoint.transform.localScale;
    }

    private void Update() {
        if (startLerp) {
            announcePoint.transform.localScale = Vector3.Lerp(announcePoint.transform.localScale, new Vector3(0.01f, 0.2f, announcePoint.transform.localScale.z), 1.8f * Time.deltaTime);
        }
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
        startLerp = true;
        yield return new WaitForSeconds(announceTime);
        announcePoint.SetActive(false);
        announcePoint.transform.localScale = startScale;
        startLerp = false;

        StartCoroutine(Blitzen());
    }

    IEnumerator Blitzen() {

        SoundManager.instance.PlaySound(SoundManager.instance.soundElectrocute, 0.2f, Random.Range(1, 1.3f));

        blitz.SetActive(true);
        yield return new WaitForSeconds(blitzTime);
        blitz.SetActive(false);

        this.gameObject.SetActive(false);
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if(other.tag == "Player" && blitz.active)
        {
            other.gameObject.SendMessage("OnHitByLightning");
        }
    }
}
