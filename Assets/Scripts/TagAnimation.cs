using UnityEngine;
using System.Collections;

public class TagAnimation : MonoBehaviour 
{
    [SerializeField]
    Vector3 direction = new Vector3(0, 1, 0);

    // Update is called once per frame
    void Update()
    {
        //offsety = Mathf.Sin(Time.time)*0.5f;
        transform.localRotation *= Quaternion.Euler(direction);
        //transform.localPosition = origin + new Vector3(0, offsety, 0);
    }
}
