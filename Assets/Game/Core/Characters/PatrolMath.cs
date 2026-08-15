using UnityEngine;

namespace Core.Characters
{
    public static class PatrolMath
    {
        /// <summary>
        /// Avança na direção do ponto atual (A ou B, conforme movingToB) e alterna
        /// o destino quando chega perto o suficiente. reachedTarget indica que o
        /// destino usado nesse passo (ainda o antigo) foi alcançado.
        /// </summary>
        public static Vector3 Step(Vector3 current, Vector3 pointA, Vector3 pointB, float speed, ref bool movingToB, out bool reachedTarget)
        {
            var target = movingToB ? pointB : pointA;

            reachedTarget = Vector2.Distance(current, target) < 0.1f;
            if (reachedTarget)
            {
                movingToB = !movingToB;
            }

            return Vector3.MoveTowards(current, target, speed * Time.deltaTime);
        }
    }
}
