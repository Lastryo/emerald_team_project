using UnityEngine;

public class Effect : MonoBehaviour
{
    [SerializeField] private ParticleSystem flash;

    public void FlashFxPlay()
    {
        flash.Play();
    }
}
