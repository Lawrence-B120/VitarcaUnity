using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] float runSpeedMult = 0.01f;

    void Update()
    {
        gameObject.transform.position += new Vector3(Input.GetAxis("Horizontal") * runSpeedMult, 0, 0);
    }
}
