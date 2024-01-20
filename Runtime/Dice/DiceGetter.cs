using UnityEngine;
using VInspector;
using UnityEngine.UI;
using Meangpu.Audio;
using System.Collections;
using UnityEngine.Events;
using System;

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
        [SerializeField] Vector2 _timeBetweenRandom = new(0.03f, 0.05f);
        [SerializeField] Vector2Int _randomShuffleCount = new(5, 10);
        [SerializeField] SOSound _randomShuffleAudio;

        public static Action<SpriteWithValue> OnFinishDice;

        [Button]
        public SpriteWithValue GetRandomDice()
        {
            _onStartEvent?.Invoke();
            SpriteWithValue finalDiceObj = _pool.GetRandomDice();

            if (_doPlayAnimation)
            {
                StartCoroutine(ShuffleDiceAnimation(finalDiceObj));
                return finalDiceObj;
            }

            _diceImgDisplay.sprite = finalDiceObj.Sprite;
            FinishInvoke(finalDiceObj);

            return finalDiceObj;
        }

        private void FinishInvoke(SpriteWithValue finalDiceObj)
        {
            _finishSound?.PlayOneShot();
            _onFinishEvent?.Invoke();
            OnFinishDice?.Invoke(finalDiceObj);
        }

        IEnumerator ShuffleDiceAnimation(SpriteWithValue finalFaceTarget)
        {
            int numRandomCount = UnityEngine.Random.Range(_randomShuffleCount.x, _randomShuffleCount.y);
            for (var i = 0; i < numRandomCount; i++)
            {
                float waitTime = UnityEngine.Random.Range(_timeBetweenRandom.x, _timeBetweenRandom.y);
                yield return new WaitForSeconds(waitTime);
                _onShuffleEvent?.Invoke();
                _randomShuffleAudio?.PlayOneShot();
                SpriteWithValue diceObj = _pool.GetRandomDice();
                _diceImgDisplay.sprite = diceObj.Sprite;
            }
            _diceImgDisplay.sprite = finalFaceTarget.Sprite;
            FinishInvoke(finalFaceTarget);
        }

        [Button]
        public void DoTestShuffleNoReturn() => GetRandomDice();
    }
}