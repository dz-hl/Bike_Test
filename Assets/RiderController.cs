using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Events;
using UltimateReplay;
using UltimateReplay.Storage;
public class RiderController : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] public Button restartButton;
    [SerializeField] public Text finishText;
    [SerializeField] public GameObject car1Prop;
    [SerializeField] public GameObject cat1Prop;
    [SerializeField] public GameObject car2Prop;
    public GameObject replayManager;

    [SerializeField] public Camera playerCamera;
    [SerializeField] public Camera replayCamera;

    public float speed = 6.0f;
    public float rotSpeed = 15.0f;
    ReplayStorageTarget storageTarget;
    ReplayScene storageScene;
    ReplayHandle replayHandle;

    enum State { Alive, Trancsending, Hurt}
    State state = State.Alive;

    Rigidbody _rigidbody;

    private void Start()
    {
        Cursor.visible = false;
        _rigidbody = GetComponent<Rigidbody>();
        finishText.gameObject.SetActive(false);
        restartButton.gameObject.SetActive(false);
        restartButton.onClick.AddListener(Restart);
        storageTarget = ReplayMemoryTarget.CreateUnlimited();
        storageScene = new ReplayScene(new ExamplePreparer());
        storageScene.AddReplayObject(this.GetComponent<ReplayObject>());
        storageScene.AddReplayObject(car1Prop.GetComponent<ReplayObject>());
        storageScene.AddReplayObject(car2Prop.GetComponent<ReplayObject>());
        storageScene.AddReplayObject(cat1Prop.GetComponent<ReplayObject>());
        replayHandle = ReplayManager.BeginRecording(storageTarget, storageScene);
        replayManager.gameObject.SetActive(false);
        playerCamera.enabled = true;
        replayCamera.enabled = false;
    }

    void Update()
    {
        Vector3 movement = Vector3.zero;
        if (state == State.Alive)
        {
            movement = playerMovement(movement);
        }
    }

 

    private Vector3 playerMovement(Vector3 movement)
    {
        //Moving forward and backward
        float deltaX = Input.GetAxis("Horizontal") * speed;
        float deltaZ = Input.GetAxis("Vertical") * speed;
        transform.Translate(0, 0, -deltaZ * Time.deltaTime);
        //rotation around Y coordinate
        if (deltaX != 0)
        {
            movement.x = deltaX;
            movement.z = deltaZ;

            Quaternion tmp = target.rotation;
            target.eulerAngles = new Vector3(0, target.eulerAngles.y, 0);
            movement = target.TransformDirection(movement);
            target.rotation = tmp;
            Quaternion direction = Quaternion.LookRotation(movement);
            transform.rotation = Quaternion.Lerp(transform.rotation, direction, rotSpeed * Time.deltaTime);
        }

        return movement;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (state != State.Alive)
        {
            return;
        }
        switch (collision.gameObject.tag)
        {
            case "Enemy":
                state = State.Hurt;
                Restart();
                break;
            case "Finish":
                state = State.Trancsending;
                LevelFinish();
                if (Input.GetKey(KeyCode.R))
                {
                    Restart();
                }
                break;
            default:
                break;
        }
    }

    void LevelFinish()
    {
        ReplayManager.StopRecording(ref replayHandle);
        replayManager.gameObject.SetActive(true);
        playerCamera.enabled = false;
        replayCamera.enabled = true;
        ReplayManager.BeginPlayback(storageTarget, storageScene);
        finishText.gameObject.SetActive(true);
        restartButton.gameObject.SetActive(true);
        Cursor.visible = true;
    }
    void  Restart()
    {
        playerCamera.enabled = true;
        replayCamera.enabled = false;
        finishText.gameObject.SetActive(false);
        restartButton.gameObject.SetActive(false);
        SceneManager.LoadScene(0);
    }
}

