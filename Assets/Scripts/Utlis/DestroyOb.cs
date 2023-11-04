using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOb : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(DestroyAfterDelay(gameObject, 1f));
    }

    IEnumerator DestroyAfterDelay(GameObject target, float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(target);
    }
}
