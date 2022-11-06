using System;
using Cysharp.Threading.Tasks;
using ElRaccoone.Tweens;
using UnityEngine;
using UnityEngine.UI;

namespace PlayerManagement {
    public class PlayerSelector : MonoBehaviour {

        public PlayableCharacterType? getCharacterForPlayer(PlayerType player) {
            return CharacterRef(player);
        }

        public void ChangeCharacter(PlayerType player, PlayableCharacterType character) {
            var oldCharacter = getCharacterForPlayer(player);
            if (character == oldCharacter) character = PlayableCharacterType.None;
            SetCharacter(player, character);
            var otherPlayer = player switch {
                PlayerType.P1 => PlayerType.P2,
                PlayerType.P2 => PlayerType.P1,
                _ => throw new ArgumentOutOfRangeException(nameof(player), player, null)
            };
            float offs = 0;
            if (character == CharacterRef(otherPlayer)) {
                offs = player switch {
                    PlayerType.P1 => -offset,
                    PlayerType.P2 => +offset,
                    _ => throw new ArgumentOutOfRangeException(nameof(player), player, null)
                };
                CalculateStacks(otherPlayer, true);
            } else if (oldCharacter == CharacterRef(otherPlayer)) {
                CalculateStacks(otherPlayer, false);
            }

            if (oldCharacter != PlayableCharacterType.None && character != PlayableCharacterType.None) {
                BounceBall(player, character, offs).Forget();
            } else if (character == PlayableCharacterType.None) {
                FadeOutBall(player, character, offs).Forget();
            } else if (oldCharacter == PlayableCharacterType.None) {
                FadeInBall(player, character, offs).Forget();
            }
        }

        private void Awake() {
            restPoint = P1Ball.transform.position;
            P1Ball.transform.localPosition += Vector3.up * offset;
            P2Ball.transform.localPosition += Vector3.down * offset;
        }

        private async UniTaskVoid FadeInBall(PlayerType player, PlayableCharacterType character, float offset) {
            var ball = PlayerBall(player);
            ball.TweenMaterialAlpha(0, fadeDuration);
            await ball.TweenLocalScale(Vector3.zero, fadeDuration).Await();
            var destination = ScreenImageForChar(character);
            ball.transform.position = destination.transform.position + new Vector3(offset, -100, 0);
            ball.TweenGraphicAlpha(1, fadeDuration);
            await ball.TweenLocalScale(Vector3.one, fadeDuration).Await();
        }

        private async UniTaskVoid FadeOutBall(PlayerType player, PlayableCharacterType character, float offset) {
            var ball = PlayerBall(player);
            ball.TweenGraphicAlpha(0, fadeDuration);
            await ball.TweenLocalScale(Vector3.zero, fadeDuration).Await();
            cancelJump = true;
            ball.transform.position = restPoint + Vector3.down * offset;
            ball.TweenGraphicAlpha(1, fadeDuration);
            await ball.TweenLocalScale(Vector3.one, fadeDuration).Await();
        }

        private async UniTaskVoid BounceBall(PlayerType player, PlayableCharacterType character, float offset) {
            cancelJump = false;
            var ball = PlayerBall(player);
            var maxDuration = jumpDuration;
            var leftoverDuration = maxDuration;
            var ballPosition = ball.transform.position;
            if (character is not PlayableCharacterType.None) {
                var toCharacter = ScreenImageForChar(character);
                var destY = toCharacter.transform.position.y - 100;
                while (leftoverDuration > 0) {
                    await UniTask.Yield();
                    if (cancelJump) return;
                    leftoverDuration = Mathf.Max(0, leftoverDuration - Time.deltaTime);
                    var progress = 1 - leftoverDuration / maxDuration;
                    var ypos = 100 * Mathf.Sin(2 * progress * Mathf.PI) + 100 * Mathf.Sin(1 * progress * Mathf.PI) + destY;
                    var xpos = Mathf.SmoothStep(ballPosition.x, toCharacter.transform.position.x + offset, progress);
                    ball.transform.position = new Vector3(xpos, ypos, ballPosition.z);
                }
            }
        }

        private ref PlayableCharacterType CharacterRef(PlayerType player) {
            if (player == PlayerType.P1) return ref p1Character;
            return ref p2Character;
        }

        private void SetCharacter(PlayerType player, PlayableCharacterType character) {
            ref var ch = ref CharacterRef(player);
            ch = character;
        }

        private Image ScreenImageForChar(PlayableCharacterType character) => character switch {
            PlayableCharacterType.Kuro => KuroScreenImage,
            PlayableCharacterType.Shiro => ShiroScreenImage,
            _ => null
        };

        private Image PlayerBall(PlayerType p) => p switch {
            PlayerType.P1 => P1Ball,
            PlayerType.P2 => P2Ball,
            _ => throw new ArgumentOutOfRangeException(nameof(p), p, null)
        };

        private void CalculateStacks(PlayerType otherPlayer, bool popout) {
            var ch = CharacterRef(otherPlayer);
            var offs = otherPlayer switch {
                PlayerType.P1 => +offset,
                PlayerType.P2 => -offset,
                _ => throw new ArgumentOutOfRangeException(nameof(otherPlayer), otherPlayer, null)
            };
            var offsetVector = ch switch {
                PlayableCharacterType.None => new Vector2(0, -offs),
                _ => new Vector2(offs, 0)
            };
            if (popout) offsetVector *= -1;
            Debug.Log($"{offsetVector}");
            var relevantBall = otherPlayer == PlayerType.P1 ? P1Ball : P2Ball;
            var localPos = relevantBall.transform.localPosition;
            relevantBall.transform.TweenLocalPosition(localPos + (Vector3) offsetVector, 0.3f);

            /*if (p1c == p2c) {
                if (p1c == p1Character) P1Ball.TweenLocalPositionX(P1Ball.transform.localPosition.x - (ballWidth / 2 + margin), 0.5f);
                if (p2c == p2Character) P2Ball.TweenLocalPositionX(P2Ball.transform.localPosition.x + (ballWidth / 2 + margin), 0.5f);
            }

            if (p1Character == p2Character) {
                if (p1c == p1Character)
                    P1Ball.TweenLocalPositionX(P1Ball.transform.localPosition.x + (ballWidth / 2 + margin), 0.5f);
                if (p2c == p2Character)
                    P2Ball.TweenLocalPositionX(P2Ball.transform.localPosition.x - (ballWidth / 2 + margin), 0.5f);
            }*/
        }

        [SerializeField] private Image P1Ball;
        [SerializeField] private Image P2Ball;
        [SerializeField] private Image KuroScreenImage;
        [SerializeField] private Image ShiroScreenImage;
        private Vector3 restPoint;
        public PlayableCharacterType p1Character = PlayableCharacterType.None;
        public PlayableCharacterType p2Character = PlayableCharacterType.None;
        private bool cancelJump = false;
        private float ballWidth => P1Ball.rectTransform.rect.width;
        private const float margin = 10f;
        private float offset => ballWidth / 2 + margin / 2;

        private const float fadeDuration = 0.3f;
        private const float jumpDuration = 0.8f;
    }
}
