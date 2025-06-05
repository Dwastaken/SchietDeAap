using UnityEngine;
using UnityEngine.AI;

namespace Enemy
{
    public class Enemy : MonoBehaviour
    {
        [Header("Gun Data")]
        [SerializeField] private GunData gunData;

        private StateMachine stateMachine;
        private NavMeshAgent agent;
        [SerializeField] private GameObject player;
        [SerializeField] private Animator animator;

        [Header("References")]
        public PathScript path;
        private Ragdoll ragdoll;

        public GunData GunData => gunData;
        public GameObject Player => player;
        public NavMeshAgent Agent => agent;
        public Animator Animator => animator;

        [Header("Debug")]
        [SerializeField] private string currentState;
        public bool setRagdoll = false;
        public bool showDebugMessages = true;

        [Header("Weapon Values")]
        public Transform gunBarrel;

        [Header("Sight Values")]
        public float sightDistance = 20f;
        public float FOV = 85f;

        [Header("Health")]
        public float health = 50f;
        public float maxHealth = 50f; // Om health te kunnen resetten

        [Header("Ammo Drop")]
        public GameObject ammoPickupPrefab;
        [Range(0f, 1f)]
        public float dropChance = 0.25f;
        public Vector3 dropOffset = Vector3.up;


        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            maxHealth = health; // Zet max health op start waarde
            stateMachine = GetComponent<StateMachine>();
            agent = GetComponent<NavMeshAgent>();
            stateMachine.Init();
            player = GameObject.FindGameObjectWithTag("Player");
            ragdoll = GetComponent<Ragdoll>();

            animator.SetBool("AttackState", true);
        }

        // Update is called once per frame
        void Update()
        {
            if (animator == null)
                Debug.Log("Animator is null");

            SetRagdoll();
            CanSeePlayer();
            currentState = stateMachine.activeState.ToString();
        }

        public bool CanSeePlayer()
        {
            if (player != null)
            {
                if (Vector3.Distance(transform.position, player.transform.position) < sightDistance)
                {
                    Vector3 targetDir = player.transform.position - transform.position;
                    float angleToPlayer =
                        Vector3.Angle(targetDir, transform.forward);

                    if (angleToPlayer >= -FOV && angleToPlayer <= FOV)
                    {
                        Ray ray = new Ray(transform.position, targetDir);
                        RaycastHit hitInfo = new RaycastHit();
                        if (Physics.Raycast(ray, out hitInfo, sightDistance))
                        {
                            if (hitInfo.transform.gameObject == player)
                            {
                                Debug.DrawRay(ray.origin, ray.direction * sightDistance, Color.yellow);
                                return true;
                            }
                        }
                    }
                }
            }
            return false;
        }

        private void SetRagdoll()
        {
            if (setRagdoll)
                ragdoll.IsRagdoll(setRagdoll);
        }


        public void takedDamage(float amount)
        {
            health -= amount;

            if (showDebugMessages)
            {
                Debug.Log($"{gameObject.name} kreeg {amount} schade. Health: {health}/{maxHealth}");
            }

            if (health <= 0f)
            {
                Die();
            }
        }

        void Die()
        {
            ragdoll.IsRagdoll(true);

            if (showDebugMessages)
            {
                Debug.Log($"{gameObject.name} is gestorven!");
            }

            // Spawn ammo drop met kans
            if (ammoPickupPrefab != null && Random.Range(0f, 1f) <= dropChance)
            {
                SpawnAmmoDrop();
            }
        }

        void SpawnAmmoDrop()
        {
            Vector3 dropPosition = transform.position + dropOffset;
            GameObject ammoDropObject = Instantiate(ammoPickupPrefab, dropPosition, Quaternion.identity);

            if (showDebugMessages)
            {
                Debug.Log("Ammo drop gespawnd!");
            }

            // Zorg ervoor dat de ammo drop een trigger collider heeft
            Collider collider = ammoDropObject.GetComponent<Collider>();
            if (collider != null)
            {
                collider.isTrigger = true;
            }
            else
            {
                SphereCollider sphereCollider = ammoDropObject.AddComponent<SphereCollider>();
                sphereCollider.isTrigger = true;
                sphereCollider.radius = 1f;
            }
        }
    }
}