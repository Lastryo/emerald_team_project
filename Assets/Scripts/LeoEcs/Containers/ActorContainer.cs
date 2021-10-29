using Client;
using Leopotam.Ecs;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEngine;

public class ActorContainer : SerializedMonoBehaviour
{
    public EcsEntity Entity;
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
        for (int i = 0; i < component.Length; i++)
        {
            component[i]?.SetOwner(ref Entity, out component[i]);
            if (component[i] is IHaveEntity haveEntity)
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
