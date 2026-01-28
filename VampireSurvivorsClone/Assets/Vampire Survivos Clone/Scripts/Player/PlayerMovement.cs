using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 5f;

    void FixedUpdate()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");
        Vector2 input = new Vector2(x, y);

        if (input.sqrMagnitude > 1f)
        {
            input.Normalize();
        }

        Vector3 delta = new Vector3(input.x, input.y, 0f) * _moveSpeed * Time.deltaTime;
        transform.position += delta;
    }
}
