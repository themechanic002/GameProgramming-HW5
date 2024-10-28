using UnityEngine;

public class Autodestroy : MonoBehaviour
{
    public float delay;
    public GameObject gameObject;

    void Start()
    {
        Destroy(gameObject, delay);
    }
}
