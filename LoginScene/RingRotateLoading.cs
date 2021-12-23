using UnityEngine;

public class RingRotateLoading : MonoBehaviour {

    public bool isOutside;
    public float speedRotate = 100;

    void Update()
    {
        if(isOutside)
              transform.Rotate(Vector3.forward * Time.deltaTime * speedRotate);
        else
            transform.Rotate(- Vector3.forward * Time.deltaTime * speedRotate / 3);
    }
}
