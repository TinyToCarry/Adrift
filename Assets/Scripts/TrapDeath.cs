using System.Collections;
using UnityEngine;

public class TrapDeath : MonoBehaviour
{
    BoxCollider2D boxcoll2d;
    BodyMover body;
    public GameObject trapLeftTarget;
    public GameObject trapRightTarget;
    public GameObject trapLeftTargetOpen;
    public GameObject trapRightTargetOpen;
    public GameObject trapLeftTargetClose;
    public GameObject trapRightTargetClose;
    public float trapCloseSpeed;
    public AudioSource aud;

    void Start()
    {
        boxcoll2d = GetComponent<BoxCollider2D>();
        aud = gameObject.GetComponent<AudioSource>();
        // Assuming BodyMover is a component of the player
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            body = player.GetComponent<BodyMover>();
        }

        trapLeftTarget.transform.position = trapLeftTargetOpen.transform.position;
        trapRightTarget.transform.position = trapRightTargetOpen.transform.position;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && body != null)
        {
            body.isDead = true;
            aud.pitch = Random.Range(0.9f, 1.2f);
            aud.Play();
            StartCoroutine(trapAnim());
        }
    }

    IEnumerator trapAnim()
    {
        float elapsedTime = 0f;
        Vector3 initialLeftPos = trapLeftTarget.transform.position;
        Vector3 initialRightPos = trapRightTarget.transform.position;
        Vector3 targetLeftPos = trapLeftTargetClose.transform.position;
        Vector3 targetRightPos = trapRightTargetClose.transform.position;

        while (elapsedTime < trapCloseSpeed)
        {
            trapLeftTarget.transform.position = Vector3.Lerp(initialLeftPos, targetLeftPos, elapsedTime / trapCloseSpeed);
            trapRightTarget.transform.position = Vector3.Lerp(initialRightPos, targetRightPos, elapsedTime / trapCloseSpeed);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        trapLeftTarget.transform.position = targetLeftPos;
        trapRightTarget.transform.position = targetRightPos;

        yield return new WaitForSeconds(2);

        elapsedTime = 0f;
        initialLeftPos = trapLeftTarget.transform.position;
        initialRightPos = trapRightTarget.transform.position;
        targetLeftPos = trapLeftTargetOpen.transform.position;
        targetRightPos = trapRightTargetOpen.transform.position;

        while (elapsedTime < trapCloseSpeed / 3)
        {
            trapLeftTarget.transform.position = Vector3.Lerp(initialLeftPos, targetLeftPos, elapsedTime / (trapCloseSpeed * 3));
            trapRightTarget.transform.position = Vector3.Lerp(initialRightPos, targetRightPos, elapsedTime / (trapCloseSpeed * 3));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        trapLeftTarget.transform.position = targetLeftPos;
        trapRightTarget.transform.position = targetRightPos;

        yield return new WaitForSeconds(6);
    }
}

