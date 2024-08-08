using UnityEngine;

public class Rotater : MonoBehaviour
{
    [SerializeField] private float _speed = 2f;
    
    private void Update()
    {
        transform.Rotate(Vector3.one * (_speed * Time.deltaTime));
    }
}
