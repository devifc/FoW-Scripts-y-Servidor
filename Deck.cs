using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;

public class Deck : MonoBehaviour
{
    //public Stack<InstantiableCard> iCards = new Stack<InstantiableCard>();
    public static Stack<Card> cards;
    public GameObject deck;
    public static int cardsInDeck = 40;
    public GameObject prefab;
    private static bool completed = false;
    //private CardDBAccess db = new CardDBAccess();
    
    IEnumerator StartGame()
    {
        for (int i = 0; i < cardsInDeck; i++)
        {
            yield return new WaitForSeconds(0.5f);
            //prefab.GetComponent<Text>().text = card.name;
            Instantiate(prefab, transform.position, transform.rotation);
        }
       /* foreach (var card in cards)
        {
            
        }*/
    }
    // Start is called before the first frame update
    void Start()
    {
        cards = new Stack<Card>();
        

        /* foreach (var card in cards)
         {
             UnityEngine.Debug.Log(card.name);
             InstantiableCard iCard = new InstantiableCard();

             iCard.id = card.id;
             iCard.name = card.name;
             iCard.type = card.type;
             iCard.race = card.race;
             iCard.cost = card.cost;
             iCard.colour = card.colour;
             iCard.ATK = card.ATK;
             iCard.DEF = card.DEF;
             iCard.abilities = card.abilities;
             iCard.divinity = card.divinity;
             iCard.flavour = card.flavour;
             iCard.artist = card.artist;
             iCard.rarity = card.rarity;
             iCard.fowID = card.fowID;

             //iCard.backCard = card.backCard;
             //iCard.will = card.will;
            //iCard.artwork = card.artwork;
            // iCard.extraCost = card.extraCost;

             iCards.Push(iCard);
             //CardDisplay.Instantiate(prefab,transform.position,Quaternion.identity);

             //CardDisplay cd = new(iCard);
         }
         */

        /*foreach (var card in allCards)
        {
            for (int i = 0; i < 40; i++)
            {
                if (card.id.Equals($"ADW-0{UnityEngine.Random.Range(10, 99)}"))
                {
                    cards.Push(card);
                }
            }
            break;
        }*/
        /*cards.Push(allCards[0]);
        foreach (Card card in cards)
        {
            card.backCard.SetActive(true);
            Instantiate(card, transform.position, Quaternion.identity);
        }
        foreach (Card card in cards)
        {
            Console.WriteLine($"Card ID: {card.id}");
            Console.WriteLine($"Name: {card.name}");
            Console.WriteLine($"Type: {string.Join(", ", card.type)}"); // Join list elements for display
            Console.WriteLine($"Race: {string.Join(", ", card.race)}"); // Join list elements for display
            Console.WriteLine($"Cost: {card.cost}");
            Console.WriteLine($"Colour: {string.Join(", ", card.colour)}"); // Join list elements for display
            Console.WriteLine($"ATK: {card.ATK}");
            Console.WriteLine($"DEF: {card.DEF}");
            Console.WriteLine($"Abilities: {string.Join(", ", card.abilities)}"); // Join list elements for display
            Console.WriteLine($"Divinity: {card.divinity}");
            Console.WriteLine($"Flavour: {card.flavour}");
            Console.WriteLine($"Artist: {card.artist}");
            Console.WriteLine($"Rarity: {card.rarity}");
            Console.WriteLine($"FowID: {card.fowID}");
        }
            /*cards = new Stack<Card>(allCards);
            //cards.Peek(); //metodo para ver el top        
            //cards.Take(1); //roba del top
            for (int i = 0; i < 40; i++)
            {
                cards.Push(new Card($"ADW-0{UnityEngine.Random.Range(10,99)}"));
            }
            foreach (Card card in cards)
            {
                card.backCard.SetActive(true);
                Instantiate(card, transform.position, Quaternion.identity);
            }*/
    }

    // Update is called once per frame
    void Update()
    {
        
        //for (int i = 0; i < cardsInDeck; i++)
        //{
        if (CardDBAccess.cards != null && CardDBAccess.cards.Count > 100)
        {
            if (completed == false)
            {
                //Introducing 4 copies
                for (int j = 0; j < 4; j++)
                {                                       //$"ADW-0{UnityEngine.Random.Range(10, 99)}")
                    cards.Push(CardDBAccess.GetCardByName($"Gradius"));//                   x4
                    cards.Push(CardDBAccess.GetCardByName($"Everfrost"));//                 x4
                    cards.Push(CardDBAccess.GetCardByName($"Atomic Bahamut"));//            x4
                    cards.Push(CardDBAccess.GetCardByName($"Charlotte's Protector"));//     x4
                    cards.Push(CardDBAccess.GetCardByName($"Brave Force"));//               x4
                    cards.Push(CardDBAccess.GetCardByName($"Tiny Violet"));//               x4

                }
                //Introducing 3 copies
                for (int j = 0; j < 3; j++)
                {
                    cards.Push(CardDBAccess.GetCardByName($"Improved Healing Robot"));//    x3
                }
                //Introducing 2 copies
                for (int j = 0; j < 2; j++)
                {
                    cards.Push(CardDBAccess.GetCardByName($"Aegis"));//                     x2
                    cards.Push(CardDBAccess.GetCardByName($"Eternal Wind"));//              x2
                    cards.Push(CardDBAccess.GetCardByName($"Dark Prominence"));//           x2
                    cards.Push(CardDBAccess.GetCardByName($"Magical Loveliness"));//        x2
                    cards.Push(CardDBAccess.GetCardByName($"Void"));//                      x2
                    cards.Push(CardDBAccess.GetCardByName($"White Garden"));//              x2
                }
                //Introducing 1 copy
                cards.Push(CardDBAccess.GetCardByName($"Deathscythe, the Life Reaper"));//  x1

                /*for (int i = 0; i < cardsInDeck; i++)
                {
                    Debug.Log(cards.ToArray()[i].name);
                }*/
                //deck.GetComponents<GameObject>()[i].GetComponents<Text>()[1].text = (cards.ToList<Card>()[i].name);
                //UnityEngine.Debug.Log(deck.GetComponents<GameObject>()[i].GetComponents<Text>()[1].text);
                //}
                //   StartCoroutine(StartGame());
                if (cards.Count == 40)
                {
                    completed = true;
                }
            }
        }
        
    }
    public void Shuffle() {
        //Convert to list and shuffle
        List<Card> list = cards.ToList();
        list.Sort();
        //Shuffling algorithm
        for (int i = list.Count - 1; i > 0; i--)
        {
            int randomIndex = UnityEngine.Random.Range(1,i);
            (list[i], list[randomIndex]) = (list[randomIndex], list[i]);
        }
        //Create a new Stack from the modified List
        //Stack<Card> shuffledStack = new Stack<Card>();
        cards.Clear();
        foreach (Card card in list)
        {
            //shuffledStack.Push(card);
            cards.Push((Card)card);
        }
        
    }
    public void Forsee(int numCards)
    {
        if (numCards == 1)
        {
            cards.Peek().backCard.SetActive(false);
        }
        else
        {
            foreach (Card card in cards.Take(numCards))
            {
                card.backCard.SetActive(false);
                Console.WriteLine("Peeked element: {0}", card.name);
            }
        }
    }

    public void PickCards(int numCards)
    {
        for (int i = 0; i < numCards; i++)
        {
            cards.Pop();
        }
        foreach (Card card in cards.Take(numCards))
        {
            card.backCard.SetActive(false);
            Console.WriteLine("Peeked element: {0}", card.name);
        }
    }
}
