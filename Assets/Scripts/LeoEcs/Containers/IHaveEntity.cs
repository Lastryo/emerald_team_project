using Leopotam.Ecs;
/// <summary>
/// ��������� �������� ��� ��� Mono �����������, ��� � ��� ����������� ������ ActorContainer
/// </summary>
public interface IHaveEntity
{
    EcsEntity Entity { get; set; }
}