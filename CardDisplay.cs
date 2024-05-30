using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Text.RegularExpressions;
using Unity.Collections.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.AdaptivePerformance.Provider;
using UnityEngine.UI;
using UnityEngine.Windows;
using UnityEngine.XR;

public class CardDisplay : MonoBehaviour//InstantiableCard
{
    //public Card card; //The card info
    public GameObject prefab; //Prefab 
    public float scaleFactor = 3.5f; //Big scale
    public int rotationFactor = 270;
    public bool expanded = false;
    public Vector3 _previousPosition;
    public Vector3 _previousScale;
    public Quaternion _previousRotation;
    public int _previousOrder;
    public static bool alreadyOneExpansion = false;



    public GameObject playerDeck;
    public GameObject stats; //Image that shows the ATK and DEF if the card have it
    public GameObject chantBackground; //If type is "Chant" it will show this Image
    public Text nameText; //name
    public Text raceText; //If card has a race, it will show it
    public Text cardTypeText; //Type. Resonator, chant, ruler...
    public Text abilitiesText; //description of the card
    //public Sprite will;
    //public Sprite artwork;
    public Image artWork; //artwork
    public Image willCost; //Image displayed depending on the cost of the card
    public Text extraCost; //Extra cost of the card (cost that is a number)
    public Text attackText; //ATK
    public Text defenseText; //DEF
    //public Text colourText; //primary colors of the card
    public bool cardBack;
    public static bool staticCardBack;
    public int numberOfCardsInDeck;
    public Card displayCard;
    public List<Card> cardsToDisplay = new List<Card>();
    public string displayName;
    public string cardExtraCost;//It shows the extra cost behind the will

    public string id;
    public string name;
    public List<string> type;
    public List<string> race;
    public string cost;
    public List<string> colour;
    public string ATK;
    public string DEF;
    public List<string> abilities;
    public string divinity;
    public string flavour;
    public string artist;
    public string rarity;
    public string fowID;
    public GameObject backCard;
    public Sprite will;//image that shows the will
    public Sprite artwork;

    //public GameObject prefab;
    public enum CardStatus { InDeck, InHand, OnTable, Destroyed };
    public CardStatus cardStatus = CardStatus.InDeck;

    //public static int remainingCardInDeck = Deck.cardsInDeck;
    void Start()
    {
        //cards = Deck.cards.ToList();
        //displayName = prefab.GetComponent<Text>().text;
        //Debug.Log("displayName en START() : "+displayName);
        //displayName = displayCard.name;
        //CardDBAccess db = new CardDBAccess();
        //db.GetCardByName(nameText.text)



        //Card card = new Card();
    }

    void Update()
    {
        //cardsToDisplay = Deck.cards.ToList();

        /*remainingCardInDeck--;
        if (!(remainingCardInDeck <= 0))
        {*/
        // Using TryPop (safer approach)
        if (Deck.cards.Count > 0)
        {
            Card valueCard;
            bool success = Deck.cards.TryPop(out valueCard);

            if (success)
            {
                //Debug.Log($"Removed value: {valueCard.name}");

                displayCard = valueCard;//Deck.cards.Pop();

                /*CardDBAccess db = new CardDBAccess();
                displayCard = db.GetCardByName(displayName);
                Debug.Log("displayName : "+displayName);
                Debug.Log("displayCard.name : "+displayCard.name);*/

                id = displayCard.id;
                name = displayCard.name;
                race = displayCard.race;
                cost = displayCard.cost;
                colour = displayCard.colour;
                type = displayCard.type;
                ATK = displayCard.ATK;
                DEF = displayCard.DEF;
                abilities = displayCard.abilities;
                divinity = displayCard.divinity;
                flavour = displayCard.flavour;
                artist = displayCard.artist;
                rarity = displayCard.rarity;
                fowID = displayCard.fowID;
                artwork = displayCard.artwork;
                backCard = displayCard.backCard;
                will = displayCard.will;
                cardExtraCost = displayCard.extraCost;

                nameText.text = "";
                raceText.text = "";
                cardTypeText.text = "";
                abilitiesText.text = "";
                attackText.text = "";
                defenseText.text = "";
                //NAME
                nameText.text = " " + name;
                //ABILITIES
                for (int i = 0; i < abilities.Count; i++)
                {
                    abilitiesText.text += " " + abilities[i];
                }
                //RACE
                for (int i = 0; i < race.Count; i++)
                {
                    raceText.text += " " + race[i];
                }
                //TYPE
                for (int i = 0; i < type.Count; i++)
                {
                    string capsType = type[i].ToUpper();
                    cardTypeText.text += "" + type[i];

                    if (capsType.Contains("RESONATOR"))
                    {
                        //ATK and DEF template
                        //ATK & DEF
                        chantBackground.SetActive(false);
                        stats.SetActive(true);
                        attackText.text = "" + ATK;
                        defenseText.text = "" + DEF;
                    }
                    else
                    {
                        //template without ATK and DEF
                        chantBackground.SetActive(true);
                        stats.SetActive(false);
                    }
                }

                //igual tengo que usar variables intermedias
                artWork.sprite = displayCard.artwork;
                extraCost.text = displayCard.extraCost;
                willCost.sprite = displayCard.will;
                //Instantiate(card, transform.position, Quaternion.identity);
                /*   playerDeck = GameObject.Find("PlayerDeck");
                   if (this.transform.parent == playerDeck.transform.parent)
                   {
                       cardBack = false;
                   }
                   staticCardBack = cardBack;

                   if (this.tag == "Clone")
                   {
                       displayCard = Deck.cards.Pop();
                       numberOfCardsInDeck -= 1;
                       Deck.cardsInDeck -= 1;
                       cardBack = false;
                       this.tag = "Untagged";
                   }*/
                //}
            }//END_ IF deck has cards
            else
            {
                //Debug.Log("Stack is empty");
            }
        }
    }

    public void ExpandCard()
    {

        //IF CARD IS NOT EXPANDED
        if (expanded == false)
        {
            _previousPosition = prefab.transform.position;
            _previousScale = prefab.transform.localScale;
            _previousRotation = prefab.transform.rotation;
            //_previousOrder = prefab.transform.GetSiblingIndex();


            //// Crear un nuevo Canvas para el objeto
            //GameObject canvasGameObject = new GameObject("TopCanvas");
            //Canvas canvas = canvasGameObject.AddComponent<Canvas>();
            //canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            //canvas.sortingOrder = 20; // Un valor alto para asegurar que esté al frente



            Vector3 screenCenter = new Vector3(Screen.width / 2, Screen.height / 2, Camera.main.nearClipPlane);
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(screenCenter);
            Quaternion rotation = new Quaternion(0, 0, rotationFactor, rotationFactor);

            // Para objetos 2D
            //SpriteRenderer spriteRenderer = obj.GetComponent<SpriteRenderer>();
            //if (spriteRenderer != null)
            //{
            //    spriteRenderer.sortingOrder = 0; // Ajusta según sea necesario
            //}
            //prefab.transform.SetParent(canvasGameObject.transform, true);
            prefab.transform.position = new Vector3(worldPosition.x, worldPosition.y, 200);
            prefab.transform.rotation = new Quaternion(0, 0, 0, rotation.w);
            prefab.transform.localScale = new Vector3(scaleFactor, scaleFactor, scaleFactor);
            //prefab.transform.SetAsLastSibling();
            expanded = true;
        }
        else
        {
            prefab.transform.position = _previousPosition;
            prefab.transform.rotation = _previousRotation;
            //obj.GetComponent<SpriteRenderer>().sortingOrder = _previousOrder;
            prefab.transform.localScale = _previousScale;
            //prefab.transform.SetSiblingIndex(_previousOrder);

        }


        if (prefab.transform.position != _previousPosition || prefab.transform.localScale != _previousScale || prefab.transform.rotation != _previousRotation)
        {
            expanded = true;
        }
        else
        {
            expanded = false;
        }

    }
}
