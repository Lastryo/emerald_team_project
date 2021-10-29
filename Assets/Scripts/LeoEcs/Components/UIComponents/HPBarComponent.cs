using Leopotam.Ecs;
using Sirenix.OdinInspector;
using UnityEngine.UI;

namespace Client
{
    struct HPBarComponent : IComponent
    {
        public Slider slider;

        public void SetHP(int value, int maxValue)
        {
            slider.value = (float)value / maxValue;
        }
#if UNITY_EDITOR
        [ShowInInspector]
        public string Doc => "Компонент который отвечает за HPBar";
#endif
        public void SetOwner(ref EcsEntity entity, out IComponent component)
        {
            component = entity.Get<HPBarComponent>() = this;
        }
    }
}