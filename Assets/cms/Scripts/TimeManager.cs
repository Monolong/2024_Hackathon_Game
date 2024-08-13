using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    // Time management variables
    public int Hours { get; private set; } // Current hour (0~23)
    public int Minutes { get; private set; } // Current minutes (0~59)

    [SerializeField] private int startHour = 0; // Starting hour
    [SerializeField] private int startMinute = 0; // Starting minute

    private float elapsedTime = 0.0f; // Variable to store elapsed time
    private float timeScale = 60.0f;  // Set to advance 1 minute per second

    void Awake()
    {
        // Initialize time
        Hours = startHour;
        Minutes = startMinute;
    }

    void Update()
    {
        // Accumulate elapsed time
        elapsedTime += Time.deltaTime * timeScale;

        // Check if 1 minute has passed
        if (elapsedTime >= 60.0f)
        {
            elapsedTime -= 60.0f;
            Minutes++;

            if (Minutes >= 60)
            {
                Minutes = 0;
                Hours++;

                if (Hours >= 24)
                {
                    Hours = 0; // Reset to 0 hours after a full day
                }

                CheckForEvents(); // Check for time-based events
            }
        }
    }

    // Check for work start/end time events
    private void CheckForEvents()
    {
        if (Hours >= 8 && Hours < 9)
        {
            WorkStart();
        }
        else if (Hours >= 18 && Hours < 19)
        {
            WorkEnd();
        }
    }

    // Work start event
    private void WorkStart()
    {
        // Add logic for work start event
    }

    // Work end event
    private void WorkEnd()
    {
        // Add logic for work end event
    }
}
