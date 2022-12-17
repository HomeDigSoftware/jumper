using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public static GameManager instance;
    public GameObject _camera;
    public GameObject _player;
    private Vector3 cam_Pos;
    private Vector3 player_Pos;

    void Awake() {
        if (instance == null)
            instance = this;

    }
    private void Start()
    {
        player_Pos = _player.transform.position;
        cam_Pos = _camera.transform.position;
    }
    public void RestartGame() {
        Invoke("LoadGameplay", 2f);
    }

    void LoadGameplay() {
          UnityEngine.SceneManagement.SceneManager.LoadScene("Gameplay");
       //  new restart for the game 
      //  _player.transform.position = player_Pos;
      //  _camera.transform.position = cam_Pos;

    }



} // class





























