using Leopotam.Ecs;
using UnityEngine;

namespace Client
{
    public class AnimationEventProvider : MonoBehaviour
    {
        private void Shoot()
        {
            EcsStartup.World.NewEntity().Get<ShootEvent>();
        }
    }
}