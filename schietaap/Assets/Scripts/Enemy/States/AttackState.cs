using Enemy.States;
using UnityEngine;

public class AttackState : BaseState
{
    private float moveTimer;
    private float losePlayerTimer;
    private float shotTimer;
    private int shotCounter;

    private bool isReloading;
    private float reloadTimer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    public override void Enter()
    {
    }

    public override void Perform()
    {
        if (enemy.CanSeePlayer())
        {
            losePlayerTimer = 0;
            moveTimer += Time.deltaTime;
            shotTimer += Time.deltaTime;

            enemy.transform.LookAt(enemy.Player.transform);
            if (shotCounter < enemy.GunData.maxAmmo && shotTimer > enemy.GunData.fireRate)
            {
                Shoot();
            }

            if (shotCounter >= enemy.GunData.maxAmmo)
            {
                enemy.Animator.SetBool("IsReloading", true);
                reloadTimer += Time.deltaTime;
            }

            if (reloadTimer >= enemy.GunData.reloadTime)
            {
                enemy.Animator.SetBool("IsReloading", false);
                reloadTimer = 0;
                shotCounter = 0;
            }

            if (moveTimer > Random.Range(3, 7))
            {
                enemy.Animator.SetBool("PatrolState", true);
                enemy.Agent.SetDestination(enemy.transform.position + (Random.insideUnitSphere * 5));
                moveTimer = 0;
            }
        }
        else
        {
            losePlayerTimer += Time.deltaTime;
            if (losePlayerTimer > 8)
            {
                //Change Search state
                stateMachine.ChangeState(new PatrolState());
            }
        }
    }

    public override void Exit()
    {
    }

    private void Shoot()
    {
        enemy.Animator.SetBool("AttackState", true);
        Transform gunBarrel = enemy.gunBarrel;
        GameObject bullet = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/Bullet") as GameObject,
            gunBarrel.position, enemy.transform.rotation);
        Vector3 shootDir = (enemy.Player.transform.position - gunBarrel.transform.position).normalized;
        bullet.GetComponent<Rigidbody>().linearVelocity =
            Quaternion.AngleAxis(Random.Range(-3f, 3f), Vector3.up) * shootDir * 40;
        shotTimer = 0;
        shotCounter++;
        Debug.Log(shotCounter);
    }
}