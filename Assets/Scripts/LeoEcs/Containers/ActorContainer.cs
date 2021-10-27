using Client;
using Leopotam.Ecs;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEngine;

public class ActorContainer : SerializedMonoBehaviour
{
    public EcsEntity Entity { get; set; }
    [SerializeField] private IComponent[] component = new IComponent[0];

    private bool isInitialized = false;

    private void OnEnable()
    {
        if (isInitialized) return;
        Initialize();
    }

    public void Initialize()
    {
        Entity = EcsStartup.NewEntity(name);
        foreach (var item in component)
        {
            item?.SetOwner(Entity);
            if (item is IHaveEntity haveEntity)
                haveEntity.Entity = Entity;
        }

        var haveEntityMonoComponents = GetComponentsInChildren<IHaveEntity>(true);
        if (haveEntityMonoComponents == null) return;
        foreach (var components in haveEntityMonoComponents)
            components.Entity = Entity;
        isInitialized = true;
    }

    public T GetEntityComponent<T>() where T : struct
    {
        if (Entity.Has<T>())
            return Entity.Get<T>();

        throw new System.Exception();
    }

    private void OnDestroy()
    {
        if (Entity.IsAlive())
        {
            Entity.Destroy();
        }
    }
}
