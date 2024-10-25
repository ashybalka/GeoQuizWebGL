using UnityEngine;

public class PointText : MonoBehaviour
{
    private double lifetime = 0;
    void Update()
    {
        gameObject.transform.Translate(Vector3.up * Time.deltaTime* 30);
        lifetime += Time.deltaTime;
        if (lifetime > 2)
        { 
            Destroy(gameObject); 
        }
    }
}
