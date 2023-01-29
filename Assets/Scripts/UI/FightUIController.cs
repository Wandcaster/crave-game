using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using PlayerManagement;
using TMPro;
using UnityEngine;

namespace UI {
    public class FightUIController : MonoBehaviour {
        [SerializeField] private CardContainer cardsInHandContainer;
        [SerializeField] private EnemyCupboard enemyContainer;
        [SerializeField] private CardSelectionContainer selectionContainer;
        [SerializeField] private GameObject kuro;
        [SerializeField] private GameObject shiro;
        [SerializeField] private PlayedCardFx playedCardFx;
        public static FightUIController instance;

        private void Start()
        {
            instance = this;
            kuro=SessionManager.Instance.player0Controller.gameObject;
            shiro=SessionManager.Instance.player1Controller.gameObject;
        }
        /**
         * This emits the OnEndTurn event, this function is called by the End Turn button (only if it is the current
         * host's turn
         */
        public void EndTurn() {
                OnEndTurn?.Invoke();
        }
        
        public event CardContainer.CardEvent OnCardPlayed {
            add => cardsInHandContainer.onCardPlayed += value;
            remove => cardsInHandContainer.onCardPlayed -= value;
        }

        public event Action OnEndTurn;
        
        /**
         * Display a selection of **THREE** cards and pick one, returns the selected card
         * Behaviour is undefined if `cards` does not contain exactly three elements
         */
        public async UniTask<CardData> DrawCards(ICollection<CardData> cards) {
            return await selectionContainer.SelectCard(cards);
        }

        public IEnumerable<CardData> cardsInHand => cardsInHandContainer.cardsInHand;

        /**
         * Adds card to hand
         */
        public async UniTask AddToHand(CardData card) {
            // HACK: move this elsewhere if possible
            foreach (var effectData in card.effect) {
                effectData.act = (Effect) Activator.CreateInstance(Type.GetType(effectData.effectType.ToString()) ??
                                                                      throw new InvalidOperationException(
                                                                          "Null effect type"));
            }
            cardsInHandContainer.AddCard(card);
        }

        
        /**
         * Forces a card to be removed from the hand. It's not necessary to call it from OnCardPlayed
         * as cards validate if they can be used beforehand and auto-remove to improve UI smoothness
         * This can be used to remove a card due to external factors, such as mill cards
         */
        public async UniTask RemoveCard(int index) {
            cardsInHandContainer.RemoveCard(index);
        }

        /**
         * Set the current character's turn, this isn't the host's turn but the in game character's
         * Set null for no player turn (aka enemy turn)
         * This is responsible for disabling card interactions 
         */
        //public void SetTurn(PlayableCharacterType? playableCharacter) {
        //    cardsInHandContainer.currentTurn = playableCharacter;
        //}

        /**
         * Set the character the player on this computer is using 
         */
        public void SetPlayerOnThisHost(PlayableCharacterType playableCharacter) {
            cardsInHandContainer.hostCharacter = playableCharacter;
        }

        /**
         * Creates a new EnemyController and initialises it with `data`
         */
        public void AddEnemy(EnemyData data) {
            // This is a stub
            enemyContainer.AddEnemy(data);
        }

        
        /**
         * Remove an enemy by its network object id (EnemyController.NetworkObjectId)
         */
        public void RemoveEnemy(ulong id) {
            enemyContainer.RemoveEnemy(id);
        }

        public void SetHp(PlayableCharacterType character, float currentHp, float maxHp) {
            var obj = character switch
            {
                PlayableCharacterType.Kuro => kuro,
                PlayableCharacterType.Shiro => shiro,
                _ => null
            } ;
            Debug.Log(character + "" + obj);
            if (obj == null) return;
            var hpBar = obj.transform.GetComponentInChildren<HealthBar>();
            hpBar.SetHp(currentHp, maxHp);
        }

        public void SetEnergy(PlayableCharacterType character, int energy) {
            var obj = character switch {
                PlayableCharacterType.Kuro => kuro,
                _ => shiro
            };
            var txt = obj.transform.GetComponentInChildren<TMP_Text>();
            txt.SetText(energy.ToString());
        }

        /**
         * Plays card effects, for example when the other player used a card
         * to show this host its effects
         */
        public async UniTask PlayCardEffects(CardData data) {
            await playedCardFx.PlayCardEffect(data);
        }
    }
}
