using UnityEngine;

public class Effect : MonoBehaviour
{
    [SerializeField] private ParticleSystem flash;
    [SerializeField] private ParticleSystem hit;

    public void FlashFxPlay()
    {
        flash.Play();
    }

    public void HitFxPlay()
    {
        hit.Play();
    }
}
