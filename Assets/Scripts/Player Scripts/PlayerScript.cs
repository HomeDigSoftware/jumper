using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class PlayerScript : MonoBehaviour {

    public static Action<string , int> player_Score;
    public static Action<string> player_fall;
    private Rigidbody2D myBody;

    public float move_Speed = 2f;

    public float normal_Push = 10f;
    public float extra_Push = 14f;
    public TMP_Text points_Text;
    private bool initial_Push;
   // private int best_Score;
    private int push_Count;

    private bool player_Died;
    [SerializeField]
    private int points = 0;

    void Awake() {
        myBody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate() {
        Move();
    }

    void Move() {

        if (player_Died)
            return;

        if(Input.GetAxisRaw("Horizontal") > 0) {

            myBody.velocity = new Vector2(move_Speed, myBody.velocity.y);

        } else if(Input.GetAxisRaw("Horizontal") < 0) {

            myBody.velocity = new Vector2(-move_Speed, myBody.velocity.y);

        }

    } // player movement

    void OnTriggerEnter2D(Collider2D target) {

        if (player_Died)                    
            return;

        if (target.tag == "ExtraPush") { 

            if(!initial_Push) {

                initial_Push = true;

                myBody.velocity = new Vector2(myBody.velocity.x, 18f);

                target.gameObject.SetActive(false);

                SoundManager.instance.JumpSoundFX();

                // exit from the on trigger enter because of initial push
                return;
            } // initial push
            points += 30;
            points_Text.text = points.ToString();
            Debug.Log(" got pointss  ++  " + points);
            // outside of the initial push

        } // because of the initial push

        if(target.tag == "NormalPush") {

            myBody.velocity = new Vector2(myBody.velocity.x, normal_Push);

            target.gameObject.SetActive(false);

            push_Count++;

            SoundManager.instance.JumpSoundFX();
            points += 10;
            points_Text.text = points.ToString();
            Debug.Log(" got pointss  ++  " + points);
            player_Score?.Invoke("new_score", points);
        }

        if (target.tag == "ExtraPush") {

            myBody.velocity = new Vector2(myBody.velocity.x, extra_Push);

            target.gameObject.SetActive(false);

            push_Count++;

            SoundManager.instance.JumpSoundFX();

        } 

        if(push_Count == 2) {

            push_Count = 0;
            PlatformSpawner.instance.SpawnPlatforms();

        }

        if(target.tag == "FallDown" || target.tag == "Bird") {

            player_Died = true;

            SoundManager.instance.GameOverSoundFX();

            GameManager.instance.RestartGame();
          //  player_fall?.Invoke("fall_Down");
        }

    } // on trigger enter


} // class











































