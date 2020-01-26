//using System;
//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour
{
    Rigidbody rigidBody;
    AudioSource audioSource;

    // game settings
    [SerializeField] float mainThrust = 2000f;
    [SerializeField] float lateralThrust = 200f;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        Thrust();
        Rotate(); 
    }

    void OnCollisionEnter(Collision collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Friendly":
                // nothing happens
                break;

            case "Fuel":
                //TODO implement some fuel or energy gain mechanic
                break;

            case "Finish":
                print("WIN!");    //TODO do something on win
                SceneManager.LoadScene(1);
                break;

            default:
                print("You're dead!");   //TODO implement some sort of life or energy drain mechanic (and remove message!)   
                SceneManager.LoadScene(0);
                break;
        }
    }

    private void Thrust()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            float thrustThisFrame = mainThrust * Time.deltaTime;
            rigidBody.AddRelativeForce(Vector3.up * thrustThisFrame);

            if (!audioSource.isPlaying)
            {
                audioSource.Play();
            }
        }
        else
        {
            audioSource.Stop();
        }
    }

    private void Rotate()
    {
        rigidBody.freezeRotation = true;        // take manual control of rotation

        float rotationThisFrame = lateralThrust * Time.deltaTime;

        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(Vector3.forward * rotationThisFrame);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(-Vector3.forward * rotationThisFrame);
        }

        rigidBody.freezeRotation = false;       // resume rotation by physics
    }
}
