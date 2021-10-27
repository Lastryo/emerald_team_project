using Leopotam.Ecs;
/// <summary>
/// Интерфейс работает как для Mono компонентов, так и для компонентов внутри ActorContainer
/// </summary>
public interface IHaveEntity
{
    EcsEntity Entity { get; set; }
}