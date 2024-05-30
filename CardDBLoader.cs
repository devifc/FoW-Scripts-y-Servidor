using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Unity.Collections.LowLevel.Unsafe;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class CardDBLoader: MonoBehaviour//sin esto se cala
{
    private void Start()
    {
        // Llama a LoadCards() desde Start() en lugar de llamarlo directamente.
        //StartCoroutine(LoadCardsCoroutine());
    }

    private IEnumerator LoadCardsCoroutine()
    {
        // Realiza la carga de cartas.
        List<Card> cards = LoadCards();

        // Espera a que la carga de cartas se complete antes de continuar.
        yield return null;
    }
    public List<Card> LoadCards()
    {
        //READ JSON FROM FILE
        string jsonText = File.ReadAllText("Assets/DB/cards.json");

        //DESERIALIZE JSON DATA
        FOWData data = JsonConvert.DeserializeObject<FOWData>(jsonText);

        //PROCESS DATA TO CREATE CARD OBJECTS
        List<Card> cards = new List<Card>();
        foreach (var cluster in data.fow.clusters)
        {
            foreach (var set in cluster.sets)
            {
                foreach (var cardData in set.cards)
                {
                    cards.Add(cardData);
                }
            }
        }
        //WE DIVIDE THE LIST TO MORE PERFORMANCE
        List<List<Card>> parts = PartitionList(cards, Environment.ProcessorCount);

        //PROCESS EACH PART AT THE SAME TIME
        List<Card> result = new List<Card>();
        Parallel.ForEach(parts, sublist =>
        {
            List<Card> processedSublist = ProcessSublist(sublist);
            lock (result)
            {
                result.AddRange(processedSublist);
            }
        });

        //JOIN RESULTS
        result = result.Distinct().ToList();

        foreach (var card in result)
        {
            Debug.Log(card.name + "" +
                      card.will + "" +
                      card.artwork);
        }

        return result;
    }
    private List<Card> ProcessSublist(List<Card> sublist)
    {
        //ADDING IMAGES AND TEMPLATES TO EACH CARD
        foreach (var card in sublist)
        {
            ////TYPE
            //for (int i = 0; i < card.type.Count; i++)
            //{
            //    string capsType = card.type[i].ToUpper();

            //    if (capsType.Contains("RESONATOR"))
            //    {
            //        //ATK and DEF template
            //        card.chantBackground.SetActive(false);
            //        card.stats.SetActive(true);
            //    }
            //    else
            //    {
            //        //template without ATK and DEF
            //        card.chantBackground.SetActive(true);
            //        card.stats.SetActive(false);
            //    }
            //}
            //ARTWORK
            string resources = "Assets/Resources"; //To Resources
            string imageName = $"{card.name.ToLower()}.png"; //Name of the file. Format: [atomic bahamut.png]

            //RESOURCES DIRECTORY INFO
            DirectoryInfo dirInfo = new DirectoryInfo(resources);

            //LOOKING FOR FILES IN RESOURCES AND IT'S INTERNAL FOLDERS
            FileInfo[] foundFiles = dirInfo.GetFiles("*", SearchOption.AllDirectories);

            //StartCoroutine(LoadArtwork(foundFiles,imageName,card));
            //LOOP FOUND FILES
            /*foreach (FileInfo file in foundFiles)
            {
                //IT COMPARES NAMES
                if (file.Name.Equals(imageName))
                {
                    //FILE FOUND
                    Console.WriteLine($"Archivo encontrado: {file.FullName}");

                    card.artwork = Resources.Load<Sprite>(file.Name.Split(".")[0]);

                    break; //Break loop if finds the file
                }
            }*/
            //LoadArtwork la = new LoadArtwork(foundFiles, imageName, card);


            int R = 0, B = 0, W = 0, U = 0, G = 0;
            string num = "";

            /*for (int i = 0; i < colour.Count; i++)
             {
                 colourText.text += " "+colour[i];
             }*/
            /*COSTS
            "0"
            {R} Red
            {B} Black/Purple
            {W} White/Yellow
            {U} Blue
            {G} Green
            {1},{2},{3} ... additional cost
             */

            /*COLOURS
            "R" Red
            "B" Black
            "W" White
            "U" Blue
            "G" Green
             */
            //ORDER: W>R>U>G>B
            for (int i = 0; i < card.colour.Count; i++)
            {
                if (card.cost.Contains<char>('W'))
                {
                    W++;
                }
                if (card.cost.Contains<char>('R'))
                {
                    R++;
                }
                if (card.cost.Contains<char>('U'))
                {
                    U++;
                }
                if (card.cost.Contains<char>('G'))
                {
                    G++;
                }
                if (card.cost.Contains<char>('B'))
                {
                    B++;
                }
            }

            string p = @"\d+";
            bool conNum = Regex.IsMatch(card.cost, p);

            if (card.cost != null && card.cost.Length >= 3 && conNum == true)
            {
                /*if (card.cost.Length > 3)
                {
                    num = card.cost.Substring(card.cost.Length - 4, 3);
                }
                else
                {*/
                num = card.cost.Substring(card.cost.Length - 3, 2);
                //}

            }
            else
            {
                num = "0";
            }

            SetWillImage(R, B, W, U, G, num, card);
        }
        return sublist;
    }
    private List<List<Card>> PartitionList(List<Card> cards, int partitions)
    {
        int count = cards.Count / partitions;
        List<List<Card>> result = new List<List<Card>>();
        for (int i = 0; i < partitions; i++)
        {
            List<Card> part = cards.Skip(i * count).Take(count).ToList();
            result.Add(part);
        }
        return result;
    }

    private void SetWillImage(int R, int B, int W, int U, int G, string num, Card card)
    {
        string resources = "Assets/Resources"; 
        string imageName = ""; //File to search

        //HERE IT'S IMPORTANT THE ORDER W>R>U>G>B
        if (W != 0)
        {
            for (int i = 0; i < W; i++)
            {
                imageName += "W";
            }
        }
        if (R != 0)
        {
            for (int i = 0; i < R; i++)
            {
                imageName += "R";
            }
        }
        if (U != 0)
        {
            for (int i = 0; i < U; i++)
            {
                imageName += "U";
            }
        }
        if (G != 0)
        {
            for (int i = 0; i < G; i++)
            {
                imageName += "G";
            }
        }
        if (B != 0)
        {
            for (int i = 0; i < B; i++)
            {
                imageName += "B";
            }
        }

        string pattern = @"\d+";
        bool containsNumbers = Regex.IsMatch(num, pattern);

        if (containsNumbers == true && !num.Equals("0"))
        {
            //EXTRA COST
            Debug.Log(card.cost +" "+ card.name+" "+ num);
            string splittedCost = num.Split("{")[0].Split("}")[0];
            Debug.Log(splittedCost);
            if (splittedCost.Equals("0"))
            {
                card.extraCost = "0";
            }
            else
            { 
                card.extraCost = "" + splittedCost;
            }
                
            // 0  1  2
            // {  10  }
        }
        else
        {
            card.extraCost = "0";
        }
        imageName += ".png"; //example: RRR.png if it repeats 3 times the red colour

        DirectoryInfo dirInfo = new DirectoryInfo(resources);
        FileInfo[] foundFiles = dirInfo.GetFiles("*", SearchOption.AllDirectories);

        //StartCoroutine(LoadImage(imageName, card));
        //StartCoroutine(LoadWill(foundFiles,imageName,card));
        //LoadWill(foundFiles, imageName, card);
        /*foreach (FileInfo file in foundFiles)
        {
            if (file.Name.Equals(imageName))
            {
                //MUST REMOVE EXTENSION TO LOAD THE SPRITE PROPERLY
                card.will = Resources.Load<Sprite>($"{file.Name.Split(".")[0]}");

                break;
            }
        }*/
    }
    private IEnumerator LoadImage(string imageName, Card card)
    {
        string resourcesPath = "Assets/Resources";
        string fullPath = Path.Combine(resourcesPath, imageName);

        // Espera a que la carga se complete antes de continuar
        yield return null;

        // Carga la imagen desde el hilo principal
        card.artwork = Resources.Load<Sprite>(fullPath);
    }
    IEnumerator LoadWill(FileInfo[] foundFiles, string imageName, Card card)
    {
        Debug.Log("LOAD WILL: " + foundFiles +" "+imageName+ " "+card.name);
        foreach (FileInfo file in foundFiles)
        {
            yield return new WaitForSeconds(0.5f);
            if (file.Name.Equals(imageName))
            {
                //MUST REMOVE EXTENSION TO LOAD THE SPRITE PROPERLY
                card.will = Resources.Load<Sprite>($"{file.Name.Split(".")[0]}");
                
                break;
            }
        }
    }
    IEnumerator LoadArtwork(FileInfo[] foundFiles, string imageName, Card card)
    {
        Debug.Log("LOAD ART: "+foundFiles + " " + imageName + " " + card.name);
        foreach (FileInfo file in foundFiles)
        {
            yield return new WaitForSeconds(0.5f);
            //IT COMPARES NAMES
            if (file.Name.Equals(imageName))
            {
                //FILE FOUND
                Console.WriteLine($"Archivo encontrado: {file.FullName}");

                card.artwork = Resources.Load<Sprite>(file.Name.Split(".")[0]);

                break; //Break loop if finds the file
            }
        }
    }

}
