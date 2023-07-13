using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayNightCycleScript : MonoBehaviour
{
    [SerializeField]
    private Light _sunLight;
    [SerializeField]
    private Light _moonLight;
    [SerializeField]
    private float _sunSet = 160f;
    [SerializeField]
    private float _sunRise = 20f;

    [SerializeField]
    private float _dayCycleDuration = 50f;

    private float _angleStepPerSecond;

    public float viewRadiusDay;
    public float surroundRadiusDay;
    [Range(0, 360)]
    public float viewAngleDay;

    public float viewRadiusNight;
    public float surroundRadiusNight;
    [Range(0, 360)]
    public float viewAngleNight;

    public FieldOfView fieldOfView;

    void Start()
    {
        _angleStepPerSecond = 360f / _dayCycleDuration;
        //Get field of view component from player
        fieldOfView.viewRadius = viewRadiusDay;
        fieldOfView.surroundRadius = surroundRadiusDay;
        fieldOfView.viewAngle = viewAngleDay;
    }

    void Update()
    {
        float angle = (Time.time * _angleStepPerSecond) % 360f;

        _sunLight.transform.rotation = Quaternion.Euler(new Vector3(angle, 270f, 0f));

        //Moon moves at half the speed of the sun
        _moonLight.transform.rotation = Quaternion.Euler(new Vector3(angle + 180f, 270f, 0f));

        if (angle > _sunSet)
        {
            //Calculate percentage transition
            float perc = Mathf.Abs(Mathf.Clamp(angle, _sunSet, 180f) - _sunSet) / (180f - _sunSet);
            _moonLight.intensity = 0.06f * perc;
            _sunLight.intensity = 1f * (1 - perc);

            //Transition from day field of view to night field of view
            fieldOfView.viewRadius = Mathf.Lerp(viewRadiusNight, viewRadiusDay, 1 - perc);
            fieldOfView.surroundRadius = Mathf.Lerp(surroundRadiusNight, surroundRadiusDay, 1 - perc);
            fieldOfView.viewAngle = Mathf.Lerp(viewAngleNight, viewAngleDay, 1 - perc);
        }
        else if (angle >= 0f && angle < _sunRise)
        {
            float perc = Mathf.Abs(Mathf.Clamp(angle, 0f, _sunRise) - _sunRise) / (_sunRise - 0f);
            _moonLight.intensity = 0.06f * perc;
            _sunLight.intensity = 1f * (1 - perc);

            //Transition from night field of view to day field of view
            fieldOfView.viewRadius = Mathf.Lerp(viewRadiusNight, viewRadiusDay, 1 - perc);
            fieldOfView.surroundRadius = Mathf.Lerp(surroundRadiusNight, surroundRadiusDay, 1 - perc);
            fieldOfView.viewAngle = Mathf.Lerp(viewAngleNight, viewAngleDay, 1 - perc);
        }

    }
}
