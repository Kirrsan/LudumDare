using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.WSA.Persistence;

public enum State { PAUSE, INGAME, WIN, LOOSE }
public class GameManager : MonoBehaviour
{

    public static GameManager instance;

    public State state;
    public System.Action onStateChange;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        ChangeState(State.INGAME);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape) && state != State.PAUSE)
        {
            ChangeState(State.PAUSE);
        }
        else if (Input.GetKeyUp(KeyCode.Escape) && state == State.PAUSE)
        {
            ChangeState(State.INGAME);
        }
    }

    public void ChangeState(State newState)
    {
        state = newState;
        if (onStateChange != null) onStateChange.Invoke();
    }
}
