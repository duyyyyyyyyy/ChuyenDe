﻿using Fusion;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public NavMeshAgent agent;
    public GameObject[] targets;
    public Animator animator;

    public int maxHealth = 100;
    private int currentHealth;
    private bool isDead = false;

    private bool isAttacking = false;

    public NetworkRunner networkRunner;

    void Start()
    {
        currentHealth = maxHealth;
    }

    void Update()
    {
        if (isDead) return;

        targets = GameObject.FindGameObjectsWithTag("Player");
        if (targets.Length == 0) return;

        GameObject target = null;
        float minDistance = Mathf.Infinity;
        foreach (var t in targets)
        {
            var distance = Vector3.Distance(t.transform.position, transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                target = t;
            }
        }

        if (target != null)
        {
            agent.SetDestination(target.transform.position);

            animator.SetBool("isChasing", true);
            animator.SetBool("isIdle", false);

            if (minDistance < 2f && !isAttacking)
            {
                isAttacking = true;
                animator.SetBool("isAttacking", true);
            }
            else if (minDistance >= 2f && isAttacking)
            {
                isAttacking = false;
                animator.SetBool("isAttacking", false);
            }
        }
        else
        {
            animator.SetBool("isChasing", false);
            animator.SetBool("isIdle", true);
        }
    }

    public void TakeDamage(int amount)
    {
        if (isDead) return;

        currentHealth -= amount;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        isDead = true;
        animator.SetTrigger("Die");
        agent.isStopped = true;

        // Tuỳ chọn: huỷ đối tượng sau 3 giây
        Destroy(gameObject, 3f);
    }
}
