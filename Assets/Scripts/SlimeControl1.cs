using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeControl1 : MonoBehaviour
{
    public float speed;
    public float travelDistance;
    public float offset;
    public float y;


    // Update is called once per frame
    void Update()
    {
        Vector2 pos = new Vector2(offset + Mathf.PingPong(speed * Time.time, travelDistance), y);
        transform.position = pos;
    }
}
