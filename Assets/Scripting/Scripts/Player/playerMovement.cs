using System.Collections;
using UnityEngine;
using UnityEngine.Events;

#if UNITY_EDITOR
using UnityEditor;
#endif

[RequireComponent(typeof(Rigidbody)), RequireComponent(typeof(CapsuleCollider))]
public class playerMovement : MonoBehaviour
{
    [Header("Important Information")]
    public playerMovementAction current_playerMovementAction;
    public bool grounded;
    public bool gravityAffected = true;
    public bool canAffectMovement = true;
    public bool canAffectRotation = true;
    public bool dragAffected = true;

    #region Action Cooldowns
    [Space, Header("Cooldowns and If The Player Can Do That Action")]
    public float action_jumpCooldownTime = 0.1f;
    public float action_dashCooldownTime = 0.1f;
    public float action_slideCooldownTime = 0.1f;
    public float action_slamCooldownTime = 0.1f;
    public float action_flipCooldownTime = 0.1f;
    public bool action_CanJump { private set; get; } = true;
    public bool action_CanDash { private set; get; } = true;
    public bool action_CanSlide { private set; get; } = true;
    public bool action_CanSlam { private set; get; } = true;
    public bool action_CanFlip { private set; get; } = true;
    #endregion

    #region Ground Check Calculations
    [Space, Header("Ground Calculations")]
    public float timeInSeconds_GroundedCoyoteTime;
    public float current_CoyoteTime;
    private RaycastHit groundCheck_HitInformation;
    public float groundCheck_Distance;
    public float groundCheck_Radius;
    public LayerMask groundCheck_LayersToHit;
    #endregion

    #region horizontal Movement
    [Space, Header("Horizontal Movement")]
    public Vector3 horizontal_playerVelocity;
    public float acceleration_Ground, acceleration_Air;
    public float terminalVelocity_Ground, terminalVelocity_Air;
    public float speedStopThreshold;
    public float maxSlopeAngle;
    #endregion

    #region Vertical Movement
    [Space, Header("Vertical Movement")]
    public Vector3 vertical_playerVelocity;
    public float jumpHeight;
    public int numberOf_MidairJumps = 1;
    private int current_NumberOfMidairJumps;
    public float gravityAcceleration;
    public float terminalVelocity_Gravity;
    public float terminalVelocity_Jump;
    #endregion

    #region Action Dash
    [Space, Header("Action Dash")]
    public float dashDistance;
    public float dashDuration;
    public float dashVelocityDivider = 0.75f;
    public int numberOf_MaximumDashCharges = 3;
    public float current_NumberOfDashCharges { private set; get; }
    public float timeInSeconds_ToRechargeOneCharge = 1f;
    #endregion

    #region Action Slide
    [Space, Header("Action Slide")]
    public float acceleration_Slide;
    public float acceleration_SlideJumpBoost;
    [HideInInspector] public bool canSlideJump;
    public float timeInSeconds_ExtraJumpCoyoteTime;
    public float terminalVelocity_Slide;
    [HideInInspector] public float colliderHeight_normal;
    public float colliderHeight_Slide;
    [HideInInspector] public Vector3 slideTempDir { private set; get; } = Vector3.zero;
    #endregion

    #region Action Slam
    [Space, Header("Action Slam")]
    public float raiseDistance_Slam;
    public float timeInSeconds_ToRaise;
    public float acceleration_Slam_Downward;
    public float terminalVelocity_Slam_Downward;
    #endregion

    #region Action Flip
    [Space, Header("Action Gravity Flip")]
    public playerRotationState current_PlayerRotationState;
    public float timeInSeconds_ToFlip;
    public float timeInSeconds_CurrentGravityFlipDuration;
    public float timeInSeconds_GravityFlipDuration;
    public float timeInSeconds_ToFullyRechargeGravity;
    public bool overchargedGravityFlip;
    public float overchargeRechargePenalty = .75f;
    public Vector3 directionalVector_NonFlipped = new Vector3(0, -1, 0);
    public Vector3 directionalVector_Flipped = new Vector3(0, 1, 0);
    [HideInInspector] public Vector3 rotationalOffset;
    #endregion

    #region Assignables
    private Rigidbody rb;
    private GameObject directionalOrientation;
    private MovementInputActions current_PlayerInputActions;
    private CapsuleCollider playerCollider;
    private playerHealth playerStats;
    #endregion

    #region control variables
    private Vector3 current_playerDirectionalVector;
    #endregion

    #region Action Events
    [HideInInspector] public UnityEvent onAction_Jump_Start;
    [HideInInspector] public UnityEvent onAction_SlideJumpStart;
    [HideInInspector] public UnityEvent onAction_Dash_Start;
    [HideInInspector] public UnityEvent onAction_DashFW_Start;
    [HideInInspector] public UnityEvent onAction_Slide_Start;
    [HideInInspector] public UnityEvent onAction_Slam_Start;
    [HideInInspector] public UnityEvent onAction_Flip_Start;
    [HideInInspector] public UnityEvent onAction_Slide_End;
    [HideInInspector] public UnityEvent onAction_Slam_End;
    [HideInInspector] public UnityEvent onAction_Flip_End;
    [HideInInspector] public UnityEvent onAction_CannotAirJump;
    [HideInInspector] public UnityEvent onAction_CannotDash;
    [HideInInspector] public UnityEvent onAction_CannotFlip;
    [HideInInspector] public UnityEvent onAction_OverchargeFlip;
    #endregion

    #region Debug Variables
    private Vector3 positionLastFrame;
    private float current_DragForce;
    Vector3 tempJumpStartVector;
    #endregion

    #region Editor Options
    [Space, Header("Debug Descisions")]
    public bool debug_ShowGroundedRay;
    public bool debug_ShowJumpHeight;
    public bool debug_ShowDashDistance;
    public bool debug_showPlayerSpeed;

    public GUIStyle textStyle = new GUIStyle();

    public Vector2 playerSpeedTextPos;
    public Vector2 playerVelMagTextPos;
    public Vector2 playerSpeedTextSize;
    #endregion

    private void OnEnable()
    {
        current_PlayerInputActions.Enable();
    }

    private void OnDisable()
    {
        current_PlayerInputActions.Disable();
    }

    private void Awake()
    {
        current_PlayerInputActions = new MovementInputActions();
        rb = GetComponent<Rigidbody>();
        playerCollider = GetComponent<CapsuleCollider>();
        playerStats = GetComponent<playerHealth>();
        colliderHeight_normal = playerCollider.height;
        groundCheck_HitInformation = new RaycastHit();
        colliderHeight_normal = playerCollider.height;
        timeInSeconds_CurrentGravityFlipDuration = timeInSeconds_GravityFlipDuration;
        if (GameObject.Find("Orientation").gameObject) directionalOrientation = GameObject.Find("Orientation").gameObject;
        else Debug.LogWarning("There is no object in the scene named 'Orientation'. \nThe player movement script will not function properly without one. Please create one and try again.");
    }

    private void Update()
    {
        getPlayerInput();
        groundDetection();
        rechargeDashCharges();
        rechargeGravity();
    }

    private void FixedUpdate()
    {
        if (gravityAffected) applyGravity();
        if (canAffectRotation) 
        {
            Vector3 tempRot = new Vector3(directionalOrientation.transform.eulerAngles.x, directionalOrientation.transform.rotation.y, directionalOrientation.transform.Find("CameraHolder").transform.eulerAngles.z);
            transform.rotation = Quaternion.Euler(tempRot);
        }
        if (canAffectMovement)
        {
            applyHorizontalAcceleration();
        }

        rb.velocity = horizontal_playerVelocity + vertical_playerVelocity;
        if (dragAffected) applyDrag();
        positionLastFrame = transform.position;
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        #region Ground check raycast gizmo
        if (debug_ShowGroundedRay)
        {
            if (grounded) Gizmos.color = Color.green;
            else Gizmos.color = Color.red;

            Vector3 tempPositionaloffSet = Vector3.zero;
            if (current_PlayerRotationState == playerRotationState.nonFlipped) tempPositionaloffSet *= directionalVector_Flipped.y;
            else tempPositionaloffSet *= directionalVector_NonFlipped.y;
            Gizmos.DrawRay(transform.position + tempPositionaloffSet, -transform.up * groundCheck_Distance);
            Gizmos.DrawWireSphere((transform.position + tempPositionaloffSet) -transform.up * groundCheck_Distance, groundCheck_Radius);
        }
        #endregion

        #region Dash Distance Gizmo
        if (debug_ShowDashDistance)
        {
            if (current_NumberOfDashCharges > 0) Handles.color = Color.green;
            else Handles.color = Color.red;
            Handles.DrawWireDisc(transform.position, transform.up, dashDistance);
        }
        #endregion

        #region Draw Jump Heigt
        if (debug_ShowJumpHeight && current_PlayerInputActions != null)
        {
            if (current_playerMovementAction != playerMovementAction.jumping && grounded) tempJumpStartVector = transform.position;
            if (current_PlayerInputActions.playerMovment.Jump.WasPressedThisFrame()) tempJumpStartVector = transform.position;
            if (action_CanJump)
            {
                if (grounded || !grounded && current_NumberOfMidairJumps > 0)
                {
                    Gizmos.color = Color.green;
                }
            }
            else Gizmos.color = Color.red;
            for (int i = 0; i < jumpHeight - 1; i++)
            {
                Handles.DrawWireDisc(tempJumpStartVector + (transform.up * (i - 0.75f)), transform.up, playerCollider.radius);
            }
        }
        #endregion
    }

    private void OnGUI()
    {
        if (debug_showPlayerSpeed)
        {
            GUI.Label(new Rect(playerSpeedTextPos.x, playerSpeedTextPos.y, playerSpeedTextSize.x, playerSpeedTextSize.y), "SPD: " + Mathf.Round((Vector3.Distance(transform.position, positionLastFrame) / Time.deltaTime)).ToString("#.000") + "m/s", textStyle);
            GUI.Label(new Rect(playerVelMagTextPos.x, playerVelMagTextPos.y, playerSpeedTextSize.x, playerSpeedTextSize.y), "VEL.MAG: " + directionalOrientation.transform.InverseTransformDirection(rb.velocity).magnitude.ToString("#.000") + "m/s", textStyle);
        }
    }
#endif

    private void groundDetection()
    {
        if (current_playerMovementAction == playerMovementAction.jumping || current_playerMovementAction == playerMovementAction.flipping) return;

        Vector3 tempPositionaloffSet = Vector3.zero;
        if (current_PlayerRotationState == playerRotationState.nonFlipped) tempPositionaloffSet *= directionalVector_Flipped.y;
        else tempPositionaloffSet *= directionalVector_NonFlipped.y;
       

        if (Physics.SphereCast(transform.position + tempPositionaloffSet, groundCheck_Radius, -transform.up, out groundCheck_HitInformation, groundCheck_Distance, groundCheck_LayersToHit) 
            && Vector3.Angle(transform.up, groundCheck_HitInformation.normal) <= maxSlopeAngle)
        {
            grounded = true;
            current_NumberOfMidairJumps = numberOf_MidairJumps;
            current_CoyoteTime = timeInSeconds_GroundedCoyoteTime;
        }
        else if (current_CoyoteTime > 0)
        {
            grounded = true;
            current_CoyoteTime -= Time.deltaTime;
        }
        else
        {
            grounded = false;
        }

        
    }

    private void getPlayerInput()
    {
        if(current_PlayerRotationState == playerRotationState.nonFlipped)
        {
            current_playerDirectionalVector = directionalOrientation.transform.forward *
            current_PlayerInputActions.playerMovment.HorizontalMovement.ReadValue<Vector2>().y +
            directionalOrientation.transform.right *
            current_PlayerInputActions.playerMovment.HorizontalMovement.ReadValue<Vector2>().x;
        } else
        {
            current_playerDirectionalVector = directionalOrientation.transform.forward *
            current_PlayerInputActions.playerMovment.HorizontalMovement.ReadValue<Vector2>().y -
            directionalOrientation.transform.right *
            current_PlayerInputActions.playerMovment.HorizontalMovement.ReadValue<Vector2>().x;
        }

        if (current_PlayerInputActions.playerMovment.Dash.WasPressedThisFrame()) StartCoroutine(action_Dash());
        if (current_PlayerInputActions.playerMovment.Slam.WasPressedThisFrame()) StartCoroutine(action_Slam());
        if (current_PlayerInputActions.playerMovment.Slide.IsPressed() 
            && !current_PlayerInputActions.playerMovment.Jump.IsPressed() 
            && !current_PlayerInputActions.playerMovment.FlipGravity.IsPressed()) StartCoroutine(action_Slide());
        if (current_PlayerInputActions.playerMovment.Jump.WasPressedThisFrame() && !grounded) StartCoroutine(action_Jump());
        else if(current_PlayerInputActions.playerMovment.Jump.IsPressed() && grounded) StartCoroutine(action_Jump());
        if (current_PlayerInputActions.playerMovment.FlipGravity.WasPressedThisFrame()) StartCoroutine(action_Flip());
    }

    private void applyDrag()
    {
        #region horizontal drag
        float idealHDragForce = 0;
        if (grounded)
        {
            switch (current_playerMovementAction)
            {
                case playerMovementAction.moving:
                    idealHDragForce = acceleration_Ground / terminalVelocity_Ground;
                    break;
                case playerMovementAction.sliding:
                    idealHDragForce = acceleration_Slide / terminalVelocity_Slide;
                    break;
            }
        }
        else if(!grounded)
        {
            switch(current_playerMovementAction)
            {
                case playerMovementAction.moving:
                    idealHDragForce = acceleration_Air / terminalVelocity_Air;
                    break;
                case playerMovementAction.sliding:
                    idealHDragForce = acceleration_Slide / terminalVelocity_Slide;
                    break;
            }
        }

        current_DragForce = idealHDragForce;
        horizontal_playerVelocity *= 1 - Time.deltaTime * current_DragForce;
        if (horizontal_playerVelocity.magnitude < speedStopThreshold) horizontal_playerVelocity *= 0;
        #endregion

        float idealVDragForce = 0;
        if (!grounded)
        {
            switch (current_playerMovementAction)
            {
                case playerMovementAction.moving:
                    idealVDragForce = gravityAcceleration / terminalVelocity_Gravity;
                    break;
                case playerMovementAction.jumping:
                    idealVDragForce = gravityAcceleration / terminalVelocity_Jump;
                    break;
                case playerMovementAction.slamming:
                    idealVDragForce = gravityAcceleration / terminalVelocity_Slam_Downward;
                    break;
            }
        }
        vertical_playerVelocity *= 1 - Time.deltaTime * idealVDragForce;
    }

    private void applyGravity()
    {
        if (!grounded)
        {
            switch (current_PlayerRotationState)
            {
                case playerRotationState.nonFlipped:
                    vertical_playerVelocity += (directionalVector_NonFlipped * gravityAcceleration) * Mathf.Exp(2) * Time.deltaTime;
                    break;
                case playerRotationState.flipped:
                    vertical_playerVelocity += (directionalVector_Flipped * gravityAcceleration) * Mathf.Exp(2) * Time.deltaTime;
                    break;
            }
        }
        else if (grounded && current_playerMovementAction != playerMovementAction.jumping) vertical_playerVelocity = -transform.up * 2f;
    }

    private void applyHorizontalAcceleration()
    {
        if (current_PlayerInputActions.playerMovment.HorizontalMovement.ReadValue<Vector2>().magnitude == 0) return;
        if (grounded) horizontal_playerVelocity += Vector3.ProjectOnPlane(current_playerDirectionalVector.normalized, groundCheck_HitInformation.normal) * acceleration_Ground * Time.deltaTime;
        else horizontal_playerVelocity += current_playerDirectionalVector.normalized * acceleration_Air * Time.deltaTime;
    }

    private void rechargeDashCharges()
    {
        if (current_NumberOfDashCharges < numberOf_MaximumDashCharges && action_CanDash)
        {
            current_NumberOfDashCharges += timeInSeconds_ToRechargeOneCharge * Time.deltaTime;
        }
        else if (current_NumberOfDashCharges > numberOf_MaximumDashCharges) current_NumberOfDashCharges = numberOf_MaximumDashCharges;
    }

    private void rechargeGravity()
    {
        if (current_PlayerRotationState == playerRotationState.nonFlipped)
        {
            if (timeInSeconds_CurrentGravityFlipDuration < timeInSeconds_GravityFlipDuration && !overchargedGravityFlip)
            {
                timeInSeconds_CurrentGravityFlipDuration += (timeInSeconds_ToFullyRechargeGravity / timeInSeconds_GravityFlipDuration) * Time.deltaTime;
            }
            else if (timeInSeconds_CurrentGravityFlipDuration < timeInSeconds_GravityFlipDuration && overchargedGravityFlip)
            {
                timeInSeconds_CurrentGravityFlipDuration += (timeInSeconds_ToFullyRechargeGravity / timeInSeconds_GravityFlipDuration) * Time.deltaTime * overchargeRechargePenalty;
            }
        }
        else if (current_PlayerRotationState == playerRotationState.flipped)
        {
            timeInSeconds_CurrentGravityFlipDuration -= Time.deltaTime;
            if (timeInSeconds_CurrentGravityFlipDuration <= 0)
            {
                StartCoroutine(action_Flip());
                overchargedGravityFlip = true;
            }
        }

        if (timeInSeconds_CurrentGravityFlipDuration > timeInSeconds_GravityFlipDuration)
        {
            timeInSeconds_CurrentGravityFlipDuration = timeInSeconds_GravityFlipDuration;
            overchargedGravityFlip = false;
        }
    }

    private IEnumerator action_Jump()
    {
        if (!action_CanJump || current_playerMovementAction != playerMovementAction.moving && current_playerMovementAction != playerMovementAction.sliding) yield break;
        else if (!grounded && current_NumberOfMidairJumps <= 0)
        {
            onAction_CannotAirJump.Invoke();
            yield break;
        }
        else if (!grounded && current_NumberOfMidairJumps > 0) current_NumberOfMidairJumps--;
        if(current_playerMovementAction == playerMovementAction.sliding) onAction_Slide_End.Invoke();


        current_playerMovementAction = playerMovementAction.jumping;
        action_CanJump = false;
        current_CoyoteTime = 0;
        grounded = false;
        vertical_playerVelocity = Vector3.zero;
        float jumpVelocity = Mathf.Sqrt(-2 * -(gravityAcceleration * Mathf.Exp(2)) * jumpHeight);
        vertical_playerVelocity += transform.up * jumpVelocity;
        if (canSlideJump)
        {
            onAction_SlideJumpStart.Invoke();
            horizontal_playerVelocity += directionalOrientation.transform.forward.normalized * acceleration_SlideJumpBoost;
        }
        else onAction_Jump_Start.Invoke();


        yield return new WaitForSeconds(0.015f);
        current_playerMovementAction = playerMovementAction.moving;
        yield return new WaitForSeconds(action_jumpCooldownTime);
        action_CanJump = true;
    }
    private IEnumerator action_Dash()
    {
        if (!action_CanDash || current_playerMovementAction != playerMovementAction.moving || current_NumberOfDashCharges < 1)
        {
            onAction_CannotDash.Invoke();
            yield break;
        }

        onAction_Dash_Start.Invoke();

        current_playerMovementAction = playerMovementAction.dashing;
        playerStats.canTakeDamage = false;
        action_CanDash = false;
        dragAffected = false;
        canAffectMovement = false;
        gravityAffected = false;
        current_NumberOfDashCharges--;

        Vector3 tempDashDirectionalVector = Vector3.zero;
        if (current_PlayerInputActions.playerMovment.HorizontalMovement.ReadValue<Vector2>().magnitude == 0) tempDashDirectionalVector = Vector3.Scale(directionalOrientation.transform.forward, new Vector3(1, 0, 1)).normalized;
        else tempDashDirectionalVector = current_playerDirectionalVector.normalized;
        Vector3 dashStartPos = transform.position;

        vertical_playerVelocity *= .75f;
        float dashVelocity = dashDistance / dashDuration;
        horizontal_playerVelocity += tempDashDirectionalVector * dashVelocity;


        float dashOverride = 0f;
        while (Vector3.Distance(dashStartPos, transform.position) < dashDistance && dashOverride < dashDuration)
        {
            dragAffected = false;
            dashOverride += Time.deltaTime;
            yield return null;
        }
        horizontal_playerVelocity -= tempDashDirectionalVector * dashVelocity;
        playerStats.canTakeDamage = true;

        dragAffected = true;
        gravityAffected = true;
        canAffectMovement = true;
        current_playerMovementAction = playerMovementAction.moving;
        yield return new WaitForSeconds(action_dashCooldownTime);
        action_CanDash = true;

    }
    private IEnumerator action_Slide()
    {

        if (!action_CanSlide || !grounded || current_playerMovementAction != playerMovementAction.moving) yield break;

        onAction_Slide_Start.Invoke();
        current_playerMovementAction = playerMovementAction.sliding;
        action_CanSlide = false;
        playerCollider.height = colliderHeight_Slide;
        slideTempDir = Vector3.zero;
        if (current_PlayerInputActions.playerMovment.HorizontalMovement.ReadValue<Vector2>().magnitude == 0) slideTempDir = directionalOrientation.transform.forward.normalized;
        else slideTempDir = current_playerDirectionalVector.normalized;
        transform.position -= Vector3.up * colliderHeight_Slide;
        canAffectMovement = false;
        canSlideJump = true;

        while (current_PlayerInputActions.playerMovment.Slide.IsPressed() 
            && current_playerMovementAction != playerMovementAction.jumping 
            && current_playerMovementAction != playerMovementAction.flipping)
        {
            horizontal_playerVelocity += slideTempDir * acceleration_Slide * Time.deltaTime;
            yield return null;
        }
        onAction_Slide_End.Invoke();
        playerCollider.height = colliderHeight_normal;
        canAffectMovement = true;
        current_playerMovementAction = playerMovementAction.moving;


        float timeToWaitForCooldown = 0;
        while (canSlideJump || !action_CanSlide)
        {
            if (timeToWaitForCooldown >= action_dashCooldownTime) action_CanSlide = true;
            if (timeToWaitForCooldown >= timeInSeconds_ExtraJumpCoyoteTime) canSlideJump = false;
            timeToWaitForCooldown += Time.deltaTime;
            yield return null;
        }
    }
    private IEnumerator action_Slam()
    {
        if (!action_CanSlam || grounded || current_playerMovementAction != playerMovementAction.moving) yield break;
        onAction_Slam_Start.Invoke();

        current_playerMovementAction = playerMovementAction.slamming;
        action_CanSlam = false;
        gravityAffected = false;
        canAffectMovement = false;
        float tempTimer = 0;
        vertical_playerVelocity = Vector3.zero;

        while (tempTimer < timeInSeconds_ToRaise)
        {
            vertical_playerVelocity = transform.up * (raiseDistance_Slam / timeInSeconds_ToRaise);

            tempTimer += Time.deltaTime;
            yield return null;
        }
        vertical_playerVelocity = Vector3.zero;
        while (!grounded)
        {
            vertical_playerVelocity += -transform.up * acceleration_Slam_Downward * Time.deltaTime;
            yield return null;

        }

        onAction_Slam_End.Invoke();

        gravityAffected = true;
        canAffectMovement = true;
        vertical_playerVelocity = Vector3.zero;
        current_playerMovementAction = playerMovementAction.moving;
        yield return new WaitForSeconds(action_slamCooldownTime);
        action_CanSlam = true;
    }
    private IEnumerator action_Flip()
    {
        if (!action_CanFlip 
            || current_playerMovementAction == playerMovementAction.jumping
            || current_playerMovementAction == playerMovementAction.slamming
            || current_playerMovementAction == playerMovementAction.dashing)
        {
            onAction_CannotFlip.Invoke();
            yield break;
        }
        if (current_PlayerRotationState == playerRotationState.nonFlipped && overchargedGravityFlip
            || current_PlayerRotationState == playerRotationState.nonFlipped && timeInSeconds_CurrentGravityFlipDuration < 1f)
        {
            onAction_CannotFlip.Invoke();
            yield break;
        }

        if (!overchargedGravityFlip)
        {
            if (current_PlayerRotationState == playerRotationState.nonFlipped) onAction_Flip_Start.Invoke();
            else onAction_Flip_End.Invoke();
        }
        else
        {
            onAction_OverchargeFlip.Invoke();
        }
        current_playerMovementAction = playerMovementAction.flipping;
        action_CanFlip = false;
        gravityAffected = false;
        canAffectMovement = false;
        canAffectRotation = false;
        current_CoyoteTime = 0;
        grounded = false;
        vertical_playerVelocity = Vector3.zero;



        float tempTimer = 0;
        while (tempTimer < timeInSeconds_ToFlip)
        {
            action_CanSlide = false;
            current_playerMovementAction = playerMovementAction.flipping;
            directionalOrientation.transform.Find("CameraHolder").transform.eulerAngles += new Vector3(0, 0, (180 / timeInSeconds_ToFlip) * Time.deltaTime);
            tempTimer += Time.deltaTime;
            yield return null;
        }
        transform.localEulerAngles += new Vector3(0, 0, 180);
        current_CoyoteTime = 0;
        grounded = false;

        if (current_PlayerRotationState == playerRotationState.nonFlipped) current_PlayerRotationState = playerRotationState.flipped;
        else current_PlayerRotationState = playerRotationState.nonFlipped;
        action_CanSlide = true;
        gravityAffected = true;
        canAffectRotation = true;
        canAffectMovement = true;
        current_playerMovementAction = playerMovementAction.moving;

        yield return new WaitForSeconds(action_flipCooldownTime);
        action_CanFlip = true;
    }

}


[System.Serializable]
public enum playerMovementAction
{
    moving = default,
    jumping,
    dashing,
    sliding,
    slamming,
    flipping
}

[System.Serializable]
public enum playerRotationState
{
    nonFlipped = default,
    flipped
}