using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class explodeBirdy : MonoBehaviour
{
    
    float length = 1f;

    void Start()
    {
        StartCoroutine(destroyBirdy());
    }

    IEnumerator destroyBirdy()
    {
        // wait
        yield return new WaitForSeconds(length);
        Destroy(this.gameObject);
    }
}
