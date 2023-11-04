using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCollider : MonoBehaviour
{
    public int Atk
    {
        get;
        set;
    }

    private IEnumerator<object> AutoDisable()
    {
        yield return new WaitForSeconds(0.1f);

        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        StartCoroutine(AutoDisable());
    }
}
