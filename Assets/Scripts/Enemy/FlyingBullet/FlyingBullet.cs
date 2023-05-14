using GbitProjectControl;
using UnityEngine;
using EnemyController;

public class FlyingBullet : Enemy
{
    void Awake()
    {
        enemyState = gameObject.AddComponent<EnemyState>();
        enemyEnergy = 1;
        attackScale = 10;
        maxInterval = 20;
        currentInterval = maxInterval * 0.5f;
        player = GameObject.Find("Player");
        animator = GetComponent<Animator>();
    }

	void Update()
	{
        StateJudge();
        StateUpdate();
	}

	void StateJudge()
	{
        if (enemyEnergy < 1 && enemyState.CurrentState != EnemyState.State.Dead)
        {
            enemyState.CurrentState = EnemyState.State.Dead;
        }
        else if (Vector2.Distance(transform.position, player.transform.position) < attackScale)
        {
            enemyState.CurrentState = EnemyState.State.Attack;
        }
        else if(Vector2.Distance(transform.position, player.transform.position) > attackScale)
		{
            enemyState.CurrentState = EnemyState.State.Idle;
		}
    }

    public void StateUpdate()
    {
        switch (enemyState.CurrentState)
        {
            case EnemyState.State.Idle:
                Idle();
                break;
            case EnemyState.State.Attack:
                Attack();
                break;
            case EnemyState.State.Dead:
                Dead();
                break;
            default:
                Debug.Log("Error");
                break;  
        }
    }

    void Idle()
	{
        animator.SetBool("Attack", false);
	}

	void Attack()
	{
        animator.SetBool("Attack", true); 
        if(currentInterval < maxInterval)
		{
            currentInterval += Time.deltaTime * 10;
		}
        else
		{
            Shoot();
            currentInterval = 0;
		}
	}

    void Dead()
	{
        //TODO:anim
        Destroy(gameObject);
	}

    void Shoot()
    {
        Bullet bullet = BulletPool.Instance.GetFromPool();
        if (bullet != null)
        {
            bullet.gameObject.SetActive(true);
            bullet.transform.SetParent(transform);
            bullet.Init(1.7f, 0.6f);
        }
    }


}