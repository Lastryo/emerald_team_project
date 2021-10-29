using Leopotam.Ecs;
using Sirenix.OdinInspector;
using Sirenix.Serialization;

namespace Client
{
    struct ChangeHPComponent : IComponent
    {
        public int Value;
        public ChangeHealthType Type;
#if UNITY_EDITOR
        [ShowInInspector]
        public string Doc => "Компонент который отвечает за нанесение урона или лечение";
#endif
        public void SetOwner(ref EcsEntity entity, out IComponent component)
        {
            component = entity.Get<ChangeHPComponent>() = this;
        }

        public enum ChangeHealthType
        {
            Damage,
            Heal
        }
    }
}