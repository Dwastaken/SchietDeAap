using Enemy.States;
using UnityEngine;

public class PatrolState : BaseState
{
    //Houd bij welke waypoint je naar toe gaat
    public int WaypointIndex = 0;
    public float waitTime;


    public override void Enter()
    {
    }

    public override void Perform()
    {
        PatrolCycle();
        if (enemy.CanSeePlayer())
        {
            stateMachine.ChangeState(new AttackState());
        }
    }

    public override void Exit()
    {
    }

    public void PatrolCycle()
    {
        if (enemy.Agent.remainingDistance < 0.2f)
        {
            waitTime += Time.deltaTime;
            enemy.Animator.SetBool("PatrolState", false);
            enemy.Animator.SetBool("AttackState", true);
            if (waitTime > 3f)
            {
                if (WaypointIndex < enemy.path.waypoints.Count - 1)
                    WaypointIndex++;
                else
                    WaypointIndex = 0;

                enemy.Agent.SetDestination(enemy.path.waypoints[WaypointIndex].position);
                enemy.Animator.SetBool("PatrolState", true);
                enemy.Animator.SetBool("AttackState", false);
                waitTime = 0f;
            }
        }
    }
}