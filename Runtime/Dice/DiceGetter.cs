using UnityEngine;
using EasyButtons;
using UnityEngine.UI;
using Meangpu.Audio;
using System.Collections;
using UnityEngine.Events;

namespace Meangpu.Dice
{
    public class DiceGetter : MonoBehaviour
    {
        [Expandable]
        [SerializeField] protected SODicePool _pool;
        [SerializeField] protected Image _diceImgDisplay;
        [SerializeField] UnityEvent _onStartEvent;
        [SerializeField] UnityEvent _onShuffleEvent;
        [SerializeField] UnityEvent _onFinishEvent;
        [SerializeField] SOSound _finishSound;
        [Header("Setting")]
        [SerializeField] bool _doPlayAnimation;
        [SerializeField] float _timeBetweenRandom = 0.05f;
        [SerializeField] float _randomShuffleCount = 10;
        [SerializeField] SOSound _randomShuffleAudio;

        [Button]
        public SpriteWithValue GetRandomDice()
        {
            _onStartEvent?.Invoke();

            SpriteWithValue finalDiceObj = _pool.GetRandomDice();

            if (_doPlayAnimation)
            {
                DoShuffleDiceAnimation(finalDiceObj);
                return finalDiceObj;
            }

            _diceImgDisplay.sprite = finalDiceObj.Sprite;

            _finishSound?.PlayOneShot();
            _onFinishEvent?.Invoke();

            return finalDiceObj;
        }

        [Button]
        public void DoShuffleDiceAnimation(SpriteWithValue finalFaceTarget) => StartCoroutine(ShuffleDiceAnimation(finalFaceTarget));

        IEnumerator ShuffleDiceAnimation(SpriteWithValue finalFaceTarget)
        {
            for (var i = 0; i < _randomShuffleCount; i++)
            {
                yield return new WaitForSeconds(_timeBetweenRandom);
                _onShuffleEvent?.Invoke();
                _randomShuffleAudio?.PlayOneShot();
                SpriteWithValue diceObj = _pool.GetRandomDice();
                _diceImgDisplay.sprite = diceObj.Sprite;
            }

            _diceImgDisplay.sprite = finalFaceTarget.Sprite;

            _finishSound?.PlayOneShot();
            _onFinishEvent?.Invoke();
        }
    }
}