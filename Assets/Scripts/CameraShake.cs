using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    [SerializeField] private float timer = 0.0f;

    public IEnumerator Shake(float duration, float strenght) {
        Vector3 originalPosition = transform.localPosition;

        

        while (timer < duration)
        {
            float x = Random.Range(-1.2f, 1.2f) * strenght;
            float y = Random.Range(-0.7f, 0.7f) * strenght;
            transform.localPosition = new Vector3(x, originalPosition.y, originalPosition.z);
            transform.localRotation = Quaternion.Euler(originalPosition.x, y, originalPosition.z);

            timer += Time.deltaTime;

            yield return null;  //to run alongside the update method. Before next iteration of loop the update needs to run one time
        }
        Debug.Log("original position!");
        transform.localPosition = originalPosition;
        transform.localRotation = Quaternion.Euler(originalPosition);
    }
}
