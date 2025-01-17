using System;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public static Action OnEndReached;
    
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private Waypoint waypoint;
    
    private int _currentWaypointIndex;
    private EnemyHealth _enemyHealth;

    private Vector3 CurrentPointPosition => waypoint.GetWaypointPosition(_currentWaypointIndex);

    private void Start()
    {
        _currentWaypointIndex = 0;
        _enemyHealth = GetComponent<EnemyHealth>();
        waypoint = FindObjectOfType<Waypoint>();
    }

    private void Update()
    {
        Move();

        if (CurrentPointPositionReached())
        {
            UpdateCurrentPointIndex();
        }
    }

    private void Move()
    {
        transform.position = Vector3.MoveTowards(transform.position, CurrentPointPosition, moveSpeed * Time.deltaTime);
    }
    
    private bool CurrentPointPositionReached()
    {
        float distanceToNextPointPosition = (transform.position - CurrentPointPosition).magnitude;

        if (distanceToNextPointPosition < 0.1f)
        {
            return true;
        }
        return false;
    }

    private void UpdateCurrentPointIndex()
    {
        int lastWaypointIndex = waypoint.Points.Length - 1;
        if (_currentWaypointIndex < lastWaypointIndex)
        {
            _currentWaypointIndex++;
        }
        else
        {
            ReturnEnemyToPool();
        }
    }

    private void ReturnEnemyToPool()
    {
        OnEndReached?.Invoke();
        ObjectPooler.ReturnToPool(gameObject);
    }
}
