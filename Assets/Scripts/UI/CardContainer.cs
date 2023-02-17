using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using ElRaccoone.Tweens;
using PlayerManagement;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Serialization;

namespace UI {
    public class CardContainer : MonoBehaviour {
        public PlayableCharacterType? currentTurn = PlayableCharacterType.Kuro;
        public PlayableCharacterType hostCharacter = PlayableCharacterType.Kuro;
        public bool isHostsTurn => currentTurn == hostCharacter;
        private List<GameObject> sprites = new();
        public IEnumerable<CardData> cardsInHand => sprites.Select(s => s.GetComponent<Card>().cardData);
        private Lazy<Camera> _mainCamera = new(() => Camera.main);
        private Camera mainCamera => _mainCamera.Value;

        private int GetCurrentEnergy {
            get {
                var ch = hostCharacter switch {
                    PlayableCharacterType.Kuro => kuro,
                    _ => shiro
                };
                return int.Parse(ch.GetComponentInChildren<TMP_Text>().text);
            }
        }
        [SerializeField] private Texture2D texture;

        [FormerlySerializedAs("RotationPerCard")] [SerializeField] private float rotationPerCard = 10f;
        [SerializeField] private float cardScaleFactor = 1.6f;
        [SerializeField] private Card cardPrefab;
        [SerializeField] private GameObject shiro;
        [SerializeField] private GameObject kuro;
        //[SerializeField] private List<CardData> sampleCards;

        public delegate void CardEvent(Card data, GameObject target, PlayerController source);

        public event CardEvent onCardPlayed;
        
        private readonly DragManager dragManager = new();
        private readonly RaycastHit2D[] raycastResults = new RaycastHit2D[16];
        private ContactFilter2D contactFilter;
        private Vector2 cardVelocity = Vector2.zero;
        private GameObject lastHoveredCard;
        private GameObject secondaryHoveredCard;

        public CardContainer() {
            contactFilter.useLayerMask = true;
            dragManager.onDragPerformed += (draggedCard, position, stage) => {
                var newPos = Vector2.SmoothDamp(draggedCard.transform.position, position, ref cardVelocity, 0.1f);
                draggedCard.transform.position = new Vector3(newPos.x, newPos.y, 1);
                if (stage == DragManager.DragStage.Done) {
                    var target = FindObjectUnderMouse(LayerMask.GetMask("Enemy", "Player", "Card"), o => o != draggedCard);
                    if (target != null && target.layer == LayerMask.NameToLayer("Card")) {
                        var newIndex = sprites.IndexOf(target);
                        sprites.Remove(draggedCard);
                        sprites.Insert(newIndex, draggedCard);
                    } else if (target != null) {
                        lastHoveredCard = null;
                        var targetType = LayerMask.LayerToName(target.layer) switch
                        {
                            "Enemy" => CardTarget.Enemy,
                            "Player" => CardTarget.TeamMate,
                            _ => CardTarget.None
                        };
                        if (target.name == "KuroIcon" && hostCharacter == PlayableCharacterType.Kuro ||
                            target.name == "ShiroIcon" && hostCharacter == PlayableCharacterType.Shiro) {
                            targetType = CardTarget.Self;
                        }

                        var tempDraggedCard = draggedCard.GetComponent<Card>();
                        
                        Debug.Log($"Trying to use card at target {targetType}; usable at {tempDraggedCard.cardData.targets}");
                        
                        var usable = isHostsTurn && (tempDraggedCard.cardData.targets & targetType) != 0 && tempDraggedCard.cardData.useCosts <= GetCurrentEnergy;
                        if (usable) {
                            var ch = currentTurn switch
                            {
                                PlayableCharacterType.Kuro => kuro,
                                _ => shiro
                            };
                            onCardPlayed?.Invoke(tempDraggedCard, target, ch.GetComponent<PlayerController>());
                            RemoveCard(draggedCard);
                        }
                    }

                    AlignCards();
                }

                if (stage == DragManager.DragStage.Begin) {
                    draggedCard.GetComponent<SortingGroup>().sortingOrder = 100;
                    draggedCard.TweenRotation(Vector3.zero, 0.3f);
                } 
            };
        }

        private void Start()
        {
            if (SessionManager.Instance.player0Controller.userCharacterType==PlayableCharacterType.Kuro)
            {
                shiro = SessionManager.Instance.player1Controller.gameObject;
                kuro = SessionManager.Instance.player0Controller.gameObject;
            }
            else
            {
                shiro = SessionManager.Instance.player0Controller.gameObject;
                kuro = SessionManager.Instance.player1Controller.gameObject;
            }
        }
        private void RemoveCard(GameObject draggedObject) {
            draggedObject.TweenLocalScale(Vector3.zero, 0.3f);
            draggedObject.transform.parent = null;
            sprites.Remove(draggedObject);
            Destroy(draggedObject, 1);
        }

        public void AddCard(CardData data) {
            AddNewCard(data);
            AlignCards(true);
        }

        public void RemoveCard(int index) {
            RemoveCard(sprites[index]);
            AlignCards();
        }

        private GameObject FindObjectUnderMouse(int layerMask, Func<GameObject, bool> filter = null) {
            contactFilter.layerMask = layerMask;
            var size = Physics2D.Raycast(GetMousePosition(), Vector2.left, contactFilter, raycastResults, 4f);
            if (size > 0) {
                return raycastResults
                    .Take(size)
                    .Select(a => a.collider.gameObject)
                    .Where(filter ?? (_ => true))
                    .OrderByDescending(c => c.transform.position.z)
                    .FirstOrDefault();
            }

            return null;
        }

        private Vector2 GetMousePosition() => mainCamera.ScreenToWorldPoint(Input.mousePosition);

        private void OnMouseDown() {
            var card = FindObjectUnderMouse(LayerMask.GetMask("Card"));
            if (card != null) {
                dragManager.BeginDrag(card, GetMousePosition());
            }
        }

        private void OnMouseUp() {
            dragManager.EndDrag(GetMousePosition());
        }

        private void OnMouseOver() {
            if (!dragManager.isDragging) {
                var card = FindObjectUnderMouse(LayerMask.GetMask("Card"));
                // 動いてるからもう必要ない
                //Debug.Log($"{(card == null ? "カードなんてないよ" : card.gameObject.GetInstanceID())}　見つけた uwu");
                if (lastHoveredCard != null && lastHoveredCard != card) {
                    lastHoveredCard.TweenLocalScale(Vector3.one, 0.2f);
                }
                if (card != null) {
                    card.TweenLocalScale(Vector3.one * cardScaleFactor, 0.2f);
                }

                lastHoveredCard = card;
            } else {
                var card = FindObjectUnderMouse(LayerMask.GetMask("Card"), c => c != dragManager.draggedObject);
                if (secondaryHoveredCard != null && secondaryHoveredCard != card) {
                    secondaryHoveredCard.TweenLocalScale(Vector3.one, 0.2f);
                }

                if (card != null) {
                    card.TweenLocalScale(Vector3.one * 1.15f, 0.2f);
                }

                secondaryHoveredCard = card;
            }
        }

        private void OnMouseExit() {
            Debug.Log("出口");
            if (lastHoveredCard != null && !dragManager.isDragging) {
                lastHoveredCard.TweenLocalScale(Vector3.one, 0.2f);
                lastHoveredCard = null;
            }

            if (secondaryHoveredCard != null) {
                secondaryHoveredCard.TweenLocalScale(Vector3.one, 0.2f);
                secondaryHoveredCard = null;
            }
        }

        private void Update() {
            dragManager.MoveMouse(GetMousePosition());
        }

        private void AddNewCard(CardData data) {
            var go = Instantiate(cardPrefab, transform);
            go.Initialise(data);
            
            //var go = new GameObject("Card");
            var sr = go.GetComponent<SortingGroup>();
            // sr.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f), 1080);
            sr.sortingOrder = sprites.Count;
            go.gameObject.layer = LayerMask.NameToLayer("Card");
            sprites.Add(go.gameObject);
        }

        // ReSharper disable Unity.PerformanceAnalysis
        private void AlignCards(bool newCard = false) {
            var size = (sprites.Count - 1);
            for (int i = 0; i < sprites.Count; ++i) {
                var sprite = sprites[i];
                
                if (sprite.gameObject == dragManager.draggedObject) continue;
                
                var sprTransform = sprite.transform;
                var sprCollider = sprite.GetComponent<BoxCollider2D>();
                var pivot = (sprites.Count - 1.0f) / 2.0f;
                var distance = i - pivot;
                var rotation = new Vector3(0, 0, -rotationPerCard * distance);
                var currentCardPosition = sprTransform.position;
                var offset = (Vector3) sprCollider.size / 2;
                var highPoint = rotation.z switch {
                    < 0 => currentCardPosition + offset,
                    > 0 => currentCardPosition + Vector3.Scale(offset, new Vector3(-1, 1, 1)),
                    _ => currentCardPosition + new Vector3(0, offset.y)
                };
                var rotatedHighPoint = RotatePointAroundPivot(highPoint, currentCardPosition, rotation);
                var yOffset = Math.Abs(currentCardPosition.y + offset.y - rotatedHighPoint.y);
                // LEAVE IT AS IS FOR NOW, I SPENT 2 WEEKS TRYING TO ADJUST IT AND IM CRYING
                var position = new Vector3((0.9f * i) - size / 2.0f, -yOffset * 2 /*+ (float) (-Math.Abs(rotation.z) * 0.015)*/, i + 1);
                if (i == sprites.Count - 1 && newCard) {
                    sprTransform.eulerAngles = rotation;
                    sprTransform.localPosition = position;
                    sprTransform.localScale = Vector3.zero;
                    sprTransform.TweenLocalScale(Vector3.one, 0.3f);
                } else {
                    sprite.GetComponent<SortingGroup>().sortingOrder = i;
                    sprTransform.TweenRotation(rotation, 0.3f);
                    sprTransform.TweenLocalPosition(position, 0.3f);
                }
            }
        }

        private void OnDrawGizmos() {
            Gizmos.color = Color.blue;
            var pivot = (sprites.Count - 1.0f) / 2.0f;
            for(int i = 0; i < sprites.Count; ++i) {
                var sprite = sprites[i];
                var sprCollider = sprite.GetComponent<BoxCollider2D>();
                var distance = i - pivot;
                var rotation = new Vector3(0, 0, -rotationPerCard * distance);
                var currentCardPosition = sprite.transform.position;
                var offset = (Vector3) sprCollider.size / 2;
                var highPoint = rotation.z switch {
                    < 0 => currentCardPosition + offset,
                    > 0 => currentCardPosition + Vector3.Scale(offset, new Vector3(-1, 1, 1)),
                    _ => currentCardPosition + new Vector3(0, offset.y)
                };
                var rotatedHighPoint = RotatePointAroundPivot(highPoint, currentCardPosition, rotation);
                Gizmos.color = Color.blue;
                Gizmos.DrawSphere(rotatedHighPoint, 0.1f);
                Gizmos.color = Color.green;
                Gizmos.DrawSphere(highPoint, 0.1f);
            }
        }

        private Vector3 RotatePointAroundPivot(Vector3 point, Vector3 pivot, Vector3 angles) {
            var dir = point - pivot;
            dir = Quaternion.Euler(angles) * dir;
            return dir + pivot;
        }
    }
}
