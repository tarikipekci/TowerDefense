using UnityEngine;

namespace NPC
{
    public class NPC : MonoBehaviour
    {
        [SerializeField] private float moveSpeed = 3f;
        public Vector2 startPos, endPos;

        private void Update()
        {
            transform.position = Vector2.MoveTowards(transform.position, endPos, moveSpeed * Time.deltaTime);

            if (Mathf.Abs(Vector2.Distance(transform.position, endPos)) <= 0.1f)
            {
                transform.position = startPos;
            }
        }
    }
}
