using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.VFX;

public class implosionBullet : MonoBehaviour
{
    [Header("Bullet Variables")]
    public float velocity = 250f;
    [SerializeField] private LayerMask layersToHit;
    [SerializeField] private float collisionForwardOffset;
    [SerializeField] private float collisionDistance;
    private bool isDead = false;

    [Space, Header("Implosion Variables")]
    public float implosionRange;
    public float damage;
    public float implosionForce;

    [Space, Header("Player Pull Settings")]
    [SerializeField] private float pullForce;
    [SerializeField] private float maxDistance;
    [SerializeField] private float stoppingDistance;
    [SerializeField] private float overFlowMaxTime;
    [SerializeField] private bool isPulling;
    [HideInInspector] public bool canPull = true;


    [Space, Header("VFX Variables")]
    public VisualEffect bulletEffect;
    [SerializeField] private LineRenderer gravityChain;
    [SerializeField] private GameObject explosion;


    InteractionInputActions inputActions;
    Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        inputActions = new InteractionInputActions();
    }

    private void OnEnable()
    {
        inputActions.Enable();
    }

    private void OnDisable()
    {
        inputActions.Disable();
    }

    private void FixedUpdate()
    {
        if (!isDead) rb.velocity = transform.forward * velocity * Time.deltaTime;
        else rb.velocity = Vector3.zero;
    }

    private void Update()
    {
        if (inputActions.Combat.Fire2.WasPressedThisFrame() && !isDead && Vector3.Distance(transform.position, GameObject.Find("Player").transform.position) < maxDistance && canPull)
        {
            StartCoroutine(pullPlayer());
            implosion();
        }
    }

    private void LateUpdate()
    {
        if (isDead && !isPulling) Destroy(gameObject, 5f);
    }

    void implosion()
    {
        AudioManager.instance.PlaySFX(FMODEvents.instance.chargePistolExplode, this.transform.position);
        Instantiate(explosion, transform.position, Quaternion.identity, GameObject.Find("VFX Holder").transform);

        isDead = true;

        List<Collider> enemiesHitEdge = Physics.OverlapSphere(transform.position, implosionRange, layersToHit).ToList();
        List<Collider> enemiesHitCenter = Physics.OverlapSphere(transform.position, implosionRange / 4, layersToHit).ToList();

        for (int i = 0; i < enemiesHitEdge.Count; i++)
        {
            RaycastHit hit;
            Vector3 direction = enemiesHitEdge[i].transform.position - transform.position;
            if (enemiesHitEdge[i].CompareTag("Enemy") && Physics.Raycast(transform.position, direction.normalized, out hit) && hit.transform.CompareTag("Enemy"))
            {
                enemiesHitEdge[i].GetComponent<enemyStats>().takeDamage(damage/2);
            }


        }

        for (int i = 0; i < enemiesHitCenter.Count; i++)
        {
            RaycastHit hit;
            Vector3 direction = enemiesHitCenter[i].transform.position - transform.position;

            if (enemiesHitCenter[i].CompareTag("Enemy") && Physics.Raycast(transform.position, direction.normalized, out hit) && hit.transform.CompareTag("Enemy"))
            {
                enemiesHitCenter[i].GetComponent<enemyStats>().takeDamage(damage / 2);
            }


        }


    }

    IEnumerator pullPlayer()
    {
        GameObject player = GameObject.Find("Player");
        GameObject orientation = GameObject.Find("Orientation");
        playerMovement tempReference = player.GetComponent<playerMovement>();

        isPulling = true;
        canPull = false;

        float overFlowTime = 0;
        while (Vector3.Distance(player.transform.position, transform.position) > stoppingDistance || overFlowTime < overFlowMaxTime)
        {
            Vector3 tempDirVector = (transform.position - player.transform.position).normalized;
            
            tempReference.horizontal_playerVelocity += new Vector3(tempDirVector.x, 0, tempDirVector.z) * pullForce * Time.deltaTime;
            tempReference.vertical_playerVelocity += new Vector3(0, tempDirVector.y, 0) * pullForce * Time.deltaTime;
            tempReference.canAffectMovement = false;
            tempReference.gravityAffected = false;
            tempReference.dragAffected = false;
            tempReference.current_playerMovementAction = playerMovementAction.jumping;

            gravityChain.positionCount = 2;
            gravityChain.SetPosition(0, transform.position);
            gravityChain.SetPosition(1, orientation.transform.position + (-Vector3.up * 0.5f));

            overFlowTime += Time.deltaTime;
            if(overFlowTime > overFlowMaxTime || Vector3.Distance(player.transform.position, transform.position) <= stoppingDistance) break;
            yield return null;
        }

        tempReference.canAffectMovement = true;
        tempReference.gravityAffected = true;
        tempReference.dragAffected = true;
        tempReference.current_playerMovementAction = playerMovementAction.moving;
        gravityChain.positionCount = 1;

        yield return null;
        isPulling = false;
        isDead = true;
    }


    private void OnCollisionEnter(Collision collision)
    {
        if ((layersToHit & (1 << collision.gameObject.layer)) != 0 && !isDead) implosion();

        else Physics.IgnoreCollision(GetComponent<Collider>(), collision.gameObject.GetComponent<Collider>());
        
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawLine(transform.position + transform.forward * collisionForwardOffset, transform.position + -transform.forward * collisionDistance);
        Gizmos.DrawWireSphere(transform.position, implosionRange);

    }
}
