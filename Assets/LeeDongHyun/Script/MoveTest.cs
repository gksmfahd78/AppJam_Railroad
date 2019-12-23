using UnityEngine;
using System.Collections;

public class MoveTest : MonoBehaviour
{
    public Rigidbody2D Player;
    public Vector2 PlayerVelocity;
    private int Jump_Count;
    private int dash_Count;
    private int face;

    // Use this for initialization
    void Start()
    {
        face = 0;
        Jump_Count = 0;
        dash_Count = 0;
    }

    // Update is called once per frame
    void Update()
    {
        PlayerVelocity = Player.velocity;

        if(Input.GetKey(KeyCode.LeftArrow)) {
            PlayerVelocity.x = -10f;
            face = -1;
        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            PlayerVelocity.x = 10f;
            face = 1;
        }

        if (Input.GetKeyDown(KeyCode.UpArrow) && Jump_Count == 0)
        {
            PlayerVelocity.y = 6f;
            Jump_Count += 1;

            if(Input.GetKey(KeyCode.Space) && dash_Count == 0 && Jump_Count == 1) 
            {
                Player.AddForce(new Vector2(10f * face, 5));
                dash_Count += 1;
            }
                
        }

        Player.velocity = PlayerVelocity;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Jump_Count = 0;
        dash_Count = 0;
    }
}
