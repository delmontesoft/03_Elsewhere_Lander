using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]     // Won let add this component more than once to a object
public class Oscillator : MonoBehaviour
{
    // Oscillator settings
    [SerializeField] Vector3 movementVector = new Vector3(10f, 10f, 10f);   // amount of movement in units per axis
    [SerializeField] float movementTime = 2f;   // period in seconds (period from sin function)
    float movementFactor;   // add [Range(0,1)] to variable to force between 0 and 1 (percentage)

    Vector3 startingPosition;

    // Start is called before the first frame update
    void Start()
    {
        startingPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        // protect agains movementTime = 0 (Mathf.Epsilon is the smallest float number)
        if  (movementTime <= Mathf.Epsilon) { return;  }

        float cycles = Time.time / movementTime;       // Time.time is framerate independant, so no need to do Time.deltaTime
        const float tau = Mathf.PI * 2;     //tau is about 6.28
        float rawSinWave = Mathf.Sin(cycles * tau);     // goes from -1 to +1

        movementFactor = rawSinWave / 2f + 0.5f;        // to force it to go between 0 and 1 (from -1 to +1)
        Vector3 movementOffset = movementVector * movementFactor;
        transform.position = startingPosition + movementOffset;
    }
}
