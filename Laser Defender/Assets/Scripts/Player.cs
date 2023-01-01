using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField] int moveSpeed;

    Vector2 rawInput;

    private void Update()
    {
        PlayerMovement();
    }

    private void PlayerMovement()
    {
        Vector3 delta = rawInput;
        transform.position += delta * moveSpeed * Time.deltaTime;
    }

    private void OnMove(InputValue value)
    {
        rawInput = value.Get<Vector2>();
        Debug.Log(rawInput);
    }
}
