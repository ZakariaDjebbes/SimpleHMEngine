using Core.Entity;

namespace Core.Collider;

public static class CollisionManager
{
    private static readonly Dictionary<(ColliderBase, ColliderBase), bool> CollisionStates = new();

    public static void UpdateCollision()
    {
        var colliders = Scene.Current.GetComponents<ColliderBase>().ToList();
        // colliders.EnumeratedForEach(c => c.Parent.OnFixedUpdate());

        for (var i = 0; i < colliders.Count; i++)
        {
            for (var j = i + 1; j < colliders.Count; j++)
            {
                var colliderA = colliders.ElementAt(i);
                var colliderB = colliders.ElementAt(j);

                var isColliding = colliderA.CheckCollision(colliderB);
                var key = (colliderA, colliderB);

                var wasColliding = CollisionStates.GetValueOrDefault(key, false);

                switch (isColliding)
                {
                    case true when !wasColliding:
                        colliderA.RaiseCollisionEnter(colliderB);
                        colliderB.RaiseCollisionEnter(colliderA);
                        break;
                    case false when wasColliding:
                        colliderA.RaiseCollisionExit(colliderB);
                        colliderB.RaiseCollisionExit(colliderA);
                        break;
                    case true:
                        colliderA.RaiseCollisionStay(colliderB);
                        colliderB.RaiseCollisionStay(colliderA);
                        break;
                }

                CollisionStates[key] = isColliding;
            }
        }
    }
}