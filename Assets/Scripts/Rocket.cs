//using System;
//using System.Collections;
//using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour
{
    Rigidbody rigidBody;
    AudioSource audioSource;

    // game states
    enum State { Alive, Dying, Transcending};
    State state = State.Alive;

    bool isPlayerImmortal = false;

    // game design settings
    [SerializeField] float mainThrust = 2000f;
    [SerializeField] float lateralThrust = 200f;
    [SerializeField] float levelLoadDelay = 2f;

    [SerializeField] AudioClip mainEngine;              // To set the audio file at design time and play it with PlayOneShot
    [SerializeField] AudioClip lose;
    [SerializeField] AudioClip win;

    [SerializeField] ParticleSystem mainEngineParticles;
    [SerializeField] ParticleSystem loseParticles;
    [SerializeField] ParticleSystem winParticles;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (state == State.Alive)
        {
            RespondToThrustInput();
            RespondToRotateInput();
        }

        if (Debug.isDebugBuild)     // only when dev(debug) build
        {
            RespondToDebugKeys();
        }
        
    }

    void OnCollisionEnter(Collision collision)
    {
        if (state != State.Alive) { return; }

        switch (collision.gameObject.tag)
        {
            case "Friendly":
                // nothing happens
                break;

            case "Fuel":
                //TODO implement some fuel or energy gain mechanic
                break;

            case "Finish":
                print("here");
                StartSuccessSequence();
                break;

            default:
                //TODO implement some sort of life or energy drain mechanic
                if (!isPlayerImmortal)
                {
                    StartDeathSequence();
                }
                break;
        }
    }

    private void StartSuccessSequence()
    {
        state = State.Transcending;
        audioSource.Stop();
        audioSource.PlayOneShot(win);
        winParticles.Play();
        Invoke("LoadNextLevel", levelLoadDelay);
    }

    private void StartDeathSequence()
    {
        state = State.Dying;
        audioSource.Stop();
        audioSource.PlayOneShot(lose);
        mainEngineParticles.Stop();
        loseParticles.Play();
        Invoke("LoadFirstLevel", levelLoadDelay);
    }

    private void RespondToThrustInput()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            ApplyThrust();
        }
        else
        {
            audioSource.Stop();
            mainEngineParticles.Stop();
        }
    }

    private void ApplyThrust()
    {
        rigidBody.AddRelativeForce(Vector3.up * mainThrust * Time.deltaTime);

        if (!audioSource.isPlaying)
        {
            //audioSource.Play();
            audioSource.PlayOneShot(mainEngine);
        }

        mainEngineParticles.Play();
    }

    private void RespondToRotateInput()
    {
        rigidBody.freezeRotation = true;        // take manual control of rotation

        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(Vector3.forward * lateralThrust * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(-Vector3.forward * lateralThrust * Time.deltaTime);
        }

        rigidBody.freezeRotation = false;       // resume rotation by physics
    }

    private void LoadNextLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;

        if (nextSceneIndex >= SceneManager.sceneCountInBuildSettings)
        {
            //TODO add some winning scene or message or something
            nextSceneIndex = 0;         // go back to the starting level
        }

        SceneManager.LoadScene(nextSceneIndex);
    }

    private void LoadFirstLevel()
    {
        // always loads firs scene when dead
        SceneManager.LoadScene(0);
    }

    private void RespondToDebugKeys()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            //StartSuccessSequence();
            LoadNextLevel();
        }

        if (Input.GetKeyDown(KeyCode.I))
        {
            isPlayerImmortal = !isPlayerImmortal;
        }
    }
}
