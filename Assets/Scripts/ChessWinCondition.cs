using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ChessWinCondition : MonoBehaviour
{
    [System.Serializable]
    public class TargetCondition
    {
        public string id;
        public Transform targetField;
        public float maxDistance = 0.03f;
        public Transform piece;
        public ChessPuzzleSpawner spawner;
        public string spawnerPieceId;
        public UnityEvent onSatisfied;
        public UnityEvent onUnsatisfied;

        private bool wasSatisfied;

        public bool Evaluate()
        {
            var resolvedPiece = ResolvePiece();
            if (resolvedPiece == null || targetField == null)
            {
                return false;
            }

            var distance = Vector3.Distance(resolvedPiece.position, targetField.position);
            return distance <= maxDistance;
        }

        private Transform ResolvePiece()
        {
            if (piece != null)
            {
                return piece;
            }

            if (spawner == null || string.IsNullOrEmpty(spawnerPieceId))
            {
                return null;
            }

            return spawner.GetSpawnedById(spawnerPieceId);
        }

        public void UpdateState(bool isSatisfied)
        {
            if (isSatisfied == wasSatisfied)
            {
                return;
            }

            wasSatisfied = isSatisfied;
            if (isSatisfied)
            {
                onSatisfied?.Invoke();
            }
            else
            {
                onUnsatisfied?.Invoke();
            }
        }
    }

    [Header("Conditions")]
    [SerializeField] private List<TargetCondition> conditions = new List<TargetCondition>();

    [Header("Options")]
    [SerializeField] private bool checkOnStart = true;
    [SerializeField] private bool continuousCheck = true;
    [SerializeField] private float checkIntervalSeconds = 0.2f;

    [Header("Events")]
    [SerializeField] private UnityEvent onAllSatisfied;
    [SerializeField] private UnityEvent onNotAllSatisfied;

    private bool allSatisfied;
    private float nextCheckTime;

    private void Start()
    {
        if (checkOnStart)
        {
            EvaluateAll();
        }
    }

    private void Update()
    {
        if (!continuousCheck)
        {
            return;
        }

        if (Time.time < nextCheckTime)
        {
            return;
        }

        nextCheckTime = Time.time + checkIntervalSeconds;
        EvaluateAll();
    }

    public void EvaluateAll()
    {
        var satisfied = true;
        for (var i = 0; i < conditions.Count; i++)
        {
            var condition = conditions[i];
            if (condition == null)
            {
                continue;
            }

            var isSatisfied = condition.Evaluate();
            condition.UpdateState(isSatisfied);
            if (!isSatisfied)
            {
                satisfied = false;
            }
        }

        if (satisfied == allSatisfied)
        {
            return;
        }

        allSatisfied = satisfied;
        if (allSatisfied)
        {
            onAllSatisfied?.Invoke();
        }
        else
        {
            onNotAllSatisfied?.Invoke();
        }
    }
}
