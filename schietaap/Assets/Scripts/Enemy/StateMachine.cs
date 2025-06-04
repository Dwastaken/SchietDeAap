using Enemy.States;
using UnityEngine;

namespace Enemy
{
    public class StateMachine : MonoBehaviour
    {
        public BaseState activeState;
        

        public void Init()
        {
            ChangeState(new PatrolState());
        }
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
            if  (activeState != null)
                activeState.Perform(); // Voert de state uit
        }

        public void ChangeState(BaseState newState)
        {   
            // Check activestate niet gelijk aan null
            if  (activeState != null)
                activeState.Exit(); // Zorgt ervoor dat de state stopt
            
        
            // Verander naar newState
            activeState = newState;

            //Check of newstate niet null is
            if (activeState != null)
            {
                // Setup nieuwe state
                activeState.stateMachine = this;
                
                activeState.enemy = GetComponent<Enemy>();
                // Zet state
                activeState.Enter();
            }
        }
    }
}
