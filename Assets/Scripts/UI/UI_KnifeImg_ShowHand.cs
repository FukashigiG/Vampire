using UnityEngine;
using UnityEngine.UI;

public class UI_KnifeImg_ShowHand : MonoBehaviour
{
    [SerializeField] Image _image;

    Animator _animator;

    public void Initialize(Sprite sprite)
    {
        _image.sprite = sprite;

        _animator = GetComponent<Animator>();

        _animator.SetTrigger("Appear");
    }

    public void DisappearAnim()
    {
        _animator.SetTrigger("Disappear");
    }

    // Called by Animator
    public void DestroyThis()
    {
        Destroy(gameObject);
    }
}
