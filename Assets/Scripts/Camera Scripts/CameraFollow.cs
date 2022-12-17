using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

    private Transform target;

    private bool followPlayer;

    public float min_Y_Treshold = -2.6f;

    public GameObject _starting_banana;
    public GameObject bG_Spawner;
    public GameObject collector;
    public GameObject platform_Spawner;
    public GameObject player_Bounds;
    public GameObject player_Fall_Down;

    private GameObject _bg , _collector , _platform_Spawner , _player_Bounds , _player_Fall_Down;


    private void OnEnable()
    {
   //     PlayerScript.player_fall += restart_messages;
    }
    private void OnDisable()
    {
   //     PlayerScript.player_fall -= restart_messages;
    }

    void Awake() {
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Start()
    {
     //   Instant_All_Platform_Obj();      
    }


    private void restart_messages(string action)
    {
        if (action == "start" )
        {
            Instant_All_Platform_Obj();
        }
        if (action == "fall_Down")
        {
            Kill_All_Platform_Obj();
            Instant_All_Platform_Obj();
        }
    }
    private void Instant_All_Platform_Obj()
    {
        _bg = Instantiate(bG_Spawner, bG_Spawner.transform.position, transform.rotation);
        _collector = Instantiate(collector, collector.transform.position, transform.rotation);
        _platform_Spawner = Instantiate(platform_Spawner, platform_Spawner.transform.position, transform.rotation);
        _player_Bounds = Instantiate(player_Bounds, player_Bounds.transform.position, transform.rotation);
        _player_Fall_Down = Instantiate(player_Fall_Down, player_Fall_Down.transform.position, transform.rotation);
        _bg.transform.parent = gameObject.transform;
        _collector.transform.parent = gameObject.transform;
        _platform_Spawner.transform.parent = gameObject.transform;
        _player_Bounds.transform.parent = gameObject.transform;
        _player_Fall_Down.transform.parent = gameObject.transform;
        _starting_banana.SetActive(true);
    }

    private void Kill_All_Platform_Obj()
    {
        Destroy(_bg);
        Destroy(_collector);
        Destroy(_platform_Spawner);
        Destroy(_player_Bounds);
        Destroy(_player_Fall_Down);
    }



    // Update is called once per frame
    void Update() {
        Follow();
    }

    void Follow() {

        if(target.position.y < (transform.position.y - min_Y_Treshold)) {
            followPlayer = false;
        }

        if(target.position.y > transform.position.y) {
            followPlayer = true;
        }

        if(followPlayer) {
            Vector3 temp = transform.position;
            temp.y = target.position.y;
            transform.position = temp;
        }

    }

} // class















































