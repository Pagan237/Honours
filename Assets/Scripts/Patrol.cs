using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patrol : MonoBehaviour
{
    private float waitTime;
    public float startWaitTime;
//positions AI can move to
    public List<Transform> moveSpots;

    private Transform t;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < 10; i++)
        {
            t = GameObject.FindGameObjectWithTag("MoveSpot").GetComponent<Transform>();
            float y = Random.Range(Camera.main.ScreenToWorldPoint(new Vector2(0, 0)).y+1, Camera.main.ScreenToWorldPoint(new Vector2(0, Screen.height)).y - 1);
            float x = Random.Range(Camera.main.ScreenToWorldPoint(new Vector2(0, 0)).x+1, Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, 0)).x - 1);
            t.position = new Vector2(x, y);
            moveSpots.Add(t);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
