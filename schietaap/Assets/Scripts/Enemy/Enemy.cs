using UnityEngine;
using UnityEngine.AI;

namespace Enemy
{
    public class Enemy : MonoBehaviour
    {
        private StateMachine stateMachine;
        private NavMeshAgent agent;

        public NavMeshAgent Agent => agent;

        [SerializeField] private string currentState;

        public PathScript path;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            stateMachine = GetComponent<StateMachine>();
            agent = GetComponent<NavMeshAgent>();
            stateMachine.Init();
        }

        // Update is called once per frame
        void Update()
        {
        }
    }
}