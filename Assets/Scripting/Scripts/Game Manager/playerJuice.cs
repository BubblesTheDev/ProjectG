using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class playerJuice : MonoBehaviour
{
    public static playerJuice playerJuiceReference;

    [Space, Header("Headbob VFX")]
    [SerializeField] private bool enableHeadbob = true;
    [SerializeField] private Vector2 amplitude = new Vector2(0.03f, 0.015f), frequency = new Vector2(12f, 12f);
    [SerializeField] private float headbobActivateLimit = 3, headbobIntensity = 1;


    [Space, Header("Gun Lag VFX")]
    [SerializeField] private bool enableGunLag = true;
    [SerializeField] private float sideLagDistance, heightLagDifference, frontLagDifference;
    [SerializeField] private float smoothness;
    [SerializeField] private GameObject objThatFollows;
    [ SerializeField] private AnimationCurve intensityCurve;
    [SerializeField] private GameObject objToShake;

    [Space, Header("Speed Lines VFX")]
    [ SerializeField] private ParticleSystem speedLines;
    [SerializeField] private float speedlineVelThreshold;

    [Space, Header("Speed Lines VFX")]
    [SerializeField] private ParticleSystem fallLines;
    [SerializeField] private float fallLineVelThreshold;

    [Space, Header("Dash VFX")]
    [SerializeField] private ParticleSystem[] dashParticles;
    [SerializeField] private float percentForFovChange = .25f;
    [SerializeField] private float fovChange = 10f;
    
    [Space, Header("Slide VFX")]
    [SerializeField] private ParticleSystem[] slideParticles;
    [SerializeField] private GameObject slideParticleObject;
    private Rigidbody rb;
    private cameraControl camControl;
    private playerMovement playerMoveScript;


    private void Awake()
    {
        rb = GameObject.Find("Player").GetComponent<Rigidbody>();
        camControl = GameObject.Find("Player").GetComponent<cameraControl>();
        playerMoveScript = GameObject.Find("Player").GetComponent<playerMovement>();
        playerJuiceReference = this;

#if !UNITY_EDITOR
        getSettings();
#endif
    }


    private void Update()
    {
        speedLineVFX();
        slideVFX();
        playerMoveScript.onAction_Dash_Start.AddListener(startDashVFX);
        playerMoveScript.onAction_DashFW_Start.AddListener(startDashVFXFW);
    }
    private void FixedUpdate()
    {
        if (playerMoveScript.grounded && playerMoveScript.current_playerMovementAction == playerMovementAction.moving) enableHeadbob = true; else enableHeadbob = false;
        smoothFollow();
        headbob();
    }

    public void getSettings()
    {
        if (PlayerPrefs.GetString("weaponBounceEnableSetting") == "true") enableGunLag = true;
        else enableGunLag = false;
        if (PlayerPrefs.GetString("headbobEnableSettings") == "true") enableHeadbob = true;
        else enableHeadbob = false;
        headbobIntensity = PlayerPrefs.GetFloat("headbobIntensitySettings");
    }

    void headbob()
    {
        if (!enableHeadbob || objThatFollows == null) return;

        Vector3 pos = Vector3.zero;
        pos.y -= Mathf.Abs(Mathf.Sin(Time.time * frequency.y * headbobIntensity) * amplitude.y * headbobIntensity);
        pos.x += Mathf.Sin(Time.time * frequency.x * headbobIntensity) * amplitude.x * headbobIntensity;

        if (rb.velocity.magnitude > headbobActivateLimit) objThatFollows.transform.localPosition += pos;
    }

    void smoothFollow()
    {
        if (!enableGunLag || objThatFollows == null || objToShake == null) return;
        Vector3 localRBVelocity = objThatFollows.transform.parent.transform.InverseTransformDirection(rb.velocity);
        Vector3 targetPos = new Vector3(sideLagDistance * (-Mathf.Clamp(localRBVelocity.x, -15, 15) / 100),
            heightLagDifference * (-Mathf.Clamp(localRBVelocity.y, -15, 15) / 100),
            frontLagDifference * (-Mathf.Clamp(localRBVelocity.z, -15, 15) / 100));

        objThatFollows.transform.localPosition = Vector3.Lerp(objThatFollows.transform.localPosition, targetPos, smoothness);
    }

    public void speedLineVFX()
    {
        if(speedLines == null)
        {
            Debug.Log("There is no speedlines particle effect");
            return;
        }
        if (Vector3.Scale(rb.velocity, new Vector3(1,0,1)).magnitude < speedlineVelThreshold)
        {
            speedLines.loop = false;
        }
        else
        {
            speedLines.loop = true;
            speedLines.Play();
        }
    }

    public void fallLineVFX()
    {
        if (fallLines == null)
        {
            Debug.Log("There is no fall lines particle effect");
            return;
        }
        if (Vector3.Scale(rb.velocity, new Vector3(0, 1, 0)).magnitude < fallLineVelThreshold)
        {
            fallLines.loop = false;
        }
        else
        {
            fallLines.Play();
            fallLines.loop = true;
        }
    }

    private void startDashVFXFW()
    {
        StartCoroutine(dashVFX(true));
    }

    private void startDashVFX()
    {
        StartCoroutine(dashVFX(false));
    }
    public IEnumerator dashVFX(bool dashForward)
    {
        if (dashParticles == null) print("There is no dash particle effects assigned");

        foreach (var particle in dashParticles) 
        {
            particle.Play();
        }

        float startingFOV = Camera.main.GetComponent<Camera>().fieldOfView;
        if (dashForward) 
        {
            float timeElapsed = 0;
            while (timeElapsed < playerMoveScript.dashDuration * percentForFovChange)
            {
                Camera.main.GetComponent<Camera>().fieldOfView = Mathf.Lerp(startingFOV, startingFOV + fovChange, timeElapsed / playerMoveScript.dashDuration * percentForFovChange);
                timeElapsed += Time.deltaTime;
                yield return null;
            }

            timeElapsed = 0;
            while (Camera.main.GetComponent<Camera>().fieldOfView != startingFOV)
            {
                Camera.main.GetComponent<Camera>().fieldOfView = Mathf.Lerp(startingFOV + fovChange, startingFOV, timeElapsed);
                timeElapsed += Time.deltaTime;
            }

        }

        yield return null;
    }

    private void slideVFX()
    {
        slideParticleObject.transform.rotation = Quaternion.LookRotation(playerMoveScript.slideTempDir);
        if(playerMoveScript.current_playerMovementAction == playerMovementAction.sliding) 
        {
            foreach (ParticleSystem x in slideParticles)
            {
                if(!x.isPlaying)
                {
                    x.Play();
                }
            }
        } else
        {
            foreach (ParticleSystem x in slideParticles)
            {
                x.Stop();
            }
        }
    }
}
