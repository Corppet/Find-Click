using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MemoryCannon : MonoBehaviour
{
    public float launchForce = 10f;
    public float maxLaunchInterval = 3f;
    [SerializeField] private GameObject[] memoryPrefabs;

    private IEnumerator StartLaunch()
    {
        // initial launch is consistent
        yield return new WaitForSeconds(.5f);

        // exit early if no longer in play
        if (!CursorController.instance.isInPlay || !enabled)
            yield break;

        GameObject memory = Instantiate(memoryPrefabs[0], transform.position, Quaternion.identity);

        // apply force to the memory
        Rigidbody2D memoryRigidbody = memory.GetComponent<Rigidbody2D>();
        float launchAngle = (transform.eulerAngles.z + 90f) * Mathf.Deg2Rad;
        memoryRigidbody.AddForce(new Vector2(Mathf.Cos(launchAngle), Mathf.Sin(launchAngle)) * launchForce,
            ForceMode2D.Impulse);

        // launch another memory
        StartCoroutine(LaunchProjectile());
    }

    private IEnumerator LaunchProjectile()
    {
        // wait before launching another object
        yield return new WaitForSeconds(UnityEngine.Random.Range(1f, maxLaunchInterval + 1f));

        // exit early if no longer in play
        if (!CursorController.instance.isInPlay || !enabled)
            yield break;

        // launch a random object
        GameObject memory = Instantiate(memoryPrefabs[UnityEngine.Random.Range(0, memoryPrefabs.Length)],
            transform.position, Quaternion.identity);

        // apply force to the object
        Rigidbody2D memoryRigidbody = memory.GetComponent<Rigidbody2D>();
        float launchAngle = (transform.eulerAngles.z + 90f) * Mathf.Deg2Rad;
        memoryRigidbody.AddForce(new Vector2(Mathf.Cos(launchAngle), Mathf.Sin(launchAngle)) * launchForce,
            ForceMode2D.Impulse);

        // launch another object
        StartCoroutine(LaunchProjectile());
    }

    private void Start()
    {
        StartCoroutine(LaunchProjectile());
    }
}
