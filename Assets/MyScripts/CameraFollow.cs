using UnityEngine;

public class CameraSegue : MonoBehaviour
{
    public Transform alvo; // o carro

    void LateUpdate()
    {
       transform.position = new Vector3(alvo.position.x, transform.position.y, transform.position.z);
    }
}