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
        
        [Header("Weapon Values")]
        public Transform gunBarrel;

        [Header("Sight Values")]
        public float sightDistance = 20f;
        public float FOV = 85f;
        
        
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
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
    }
}