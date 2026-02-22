using UnityEngine;
using UnityEngine.UI;

public class CharaAbilityCutIn : SingletonMono<CharaAbilityCutIn>
{
    [SerializeField] Animator _animator;

    [SerializeField] Image charaIllust;

    public void CutInAnimation(Sprite _sprite)
    {
        charaIllust.sprite = _sprite;

        _animator.SetTrigger("Anim");
    }
}
