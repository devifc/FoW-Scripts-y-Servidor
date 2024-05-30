using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Networking;

public class CardDBAccess : MonoBehaviour
{
    //private static CardDBLoader loader = new CardDBLoader();
    //public List<Card> allCards; //= loader.LoadCards();
    public static List<Card> cards= new List<Card>();
    public static int cardsCounter = 0;
    //public static Deck deck;
    //public static Deck oppDeck;
    //IEnumerator LoadJSON()
    //{

    //    //deck = new Deck();
    //    //oppDeck = new Deck(); 
    //}
    void Awake()
    {
        StartCoroutine(CopyAndLoadJSON());
    }

    IEnumerator CopyAndLoadJSON()
    {
        string fileName = "DB/cards.json";
        string sourcePath = Path.Combine(Application.streamingAssetsPath, fileName);
        string destinationPath = Path.Combine(Application.persistentDataPath, fileName);

        // Crear el directorio si no existe
        string destinationDir = Path.GetDirectoryName(destinationPath);
        if (!Directory.Exists(destinationDir))
        {
            Directory.CreateDirectory(destinationDir);
        }

        // Copiar el archivo desde StreamingAssets a persistentDataPath
        if (sourcePath.Contains("://") || sourcePath.Contains(":///"))
        {
            using (WWW www = new WWW(sourcePath))
            {
                yield return www;
                if (string.IsNullOrEmpty(www.error))
                {
                    File.WriteAllBytes(destinationPath, www.bytes);
                }
                else
                {
                    Debug.LogError("Error al copiar el archivo: " + www.error);
                    yield break;
                }
            }
        }
        else
        {
            File.Copy(sourcePath, destinationPath, true);
        }

        // Leer el archivo desde persistentDataPath
        string jsonContent = File.ReadAllText(destinationPath);
        

        FOWData data = JsonConvert.DeserializeObject<FOWData>(jsonContent);

        //PROCESS DATA TO CREATE CARD OBJECTS
        //List<Card> cards = new List<Card>();

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
        //ADDING IMAGES AND TEMPLATES TO EACH CARD
        foreach (var card in cards)
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
            //string resources = "Assets/Resources"; //To Resources
            string imageName = $"{card.name.ToLower()}.png"; //Name of the file. Format: [atomic bahamut.png]

            //RESOURCES DIRECTORY INFO
            /*DirectoryInfo dirInfo = new DirectoryInfo(resources);

            //LOOKING FOR FILES IN RESOURCES AND ITS INTERNAL FOLDERS
            FileInfo[] foundFiles = dirInfo.GetFiles("*", SearchOption.AllDirectories);
            */
            TextAsset fileNamesTextAsset = Resources.Load<TextAsset>("filelist");
            if (fileNamesTextAsset == null)
            {
                Debug.LogError("Failed to load file list.");
                yield break;
            }

            string[] fileNames = fileNamesTextAsset.text.Split(new[] { '\r', '\n' }, System.StringSplitOptions.RemoveEmptyEntries);



            //StartCoroutine(LoadArtwork(foundFiles,imageName,card));
            //LOOP FOUND FILES
            /*foreach (FileInfo file in fileNames)
            {
                //IT COMPARES NAMES
                if (file.Name.Equals(imageName))
                {
                    //FILE FOUND
                    Console.WriteLine($"Archivo encontrado: {file.FullName}");

                    card.artwork = Resources.Load<Sprite>(Path.GetFileNameWithoutExtension(file.Name));

                    break; //Break loop if finds the file
                }
            }*/
            foreach (string file in fileNames)
            {
                /*if (cards.Count < 2)
                {
                    if (imageName.Contains(","))
                    {
                        Debug.Log(imageName.Split(",")[0]);
                        Debug.Log(imageName.Split(",")[1]);
                        imageName = imageName.Split(",")[0] + imageName.Split(",")[1];
                        Debug.Log(imageName);
                    }
                }*/

                //if (cards.Count > 670)
                //{
                //    Debug.Log($"{card.name.ToLower()}.png");
                //}
                string image = Path.GetFileNameWithoutExtension(imageName);
                Sprite sprite = Resources.Load<Sprite>($"{image}");

                if (file.Equals(imageName))
                {
                    if (sprite == null)
                    {
                        Debug.LogError($"Failed to load image: {image}");
                    }
                    else
                    {
                        card.artwork = Resources.Load<Sprite>(Path.GetFileNameWithoutExtension(imageName));
                       
                        Debug.Log($"Successfully loaded image: {image}");
                        break; //Break loop if finds the file
                    }
                }
                
            }
            //LoadArtwork la = new LoadArtwork(foundFiles, imageName, card);

            imageName = ""; //Reset to use the variable with another image
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
            for (int i = 0; i < card.cost.Length; i++)
            {
                if (card.cost[i] == ('W'))
                {
                    W++;
                }
                if (card.cost[i] == ('R'))
                {
                    R++;
                }
                if (card.cost[i] == ('U'))
                {
                    U++;
                }
                if (card.cost[i] == ('G'))
                {
                    G++;
                }
                if (card.cost[i] == ('B'))
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

            //SET WILL


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
                string splittedCost = "";
                //Debug.Log(card.cost + " " + card.name + " " + num);
                if (num.Contains("{") || num.Contains("}"))
                {
                    splittedCost = num.Split("{")[1].Split("}")[0];
                }
                //Debug.Log(splittedCost);
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
                                 //Debug.Log(imageName);
                                 //Debug.Log(card.name);
            /*foreach (FileInfo file in foundFiles)
            {
                if (file.Name.Equals(imageName))
                {
                    
                    //MUST REMOVE EXTENSION TO LOAD THE SPRITE PROPERLY
                    card.will = Resources.Load<Sprite>($"{Path.GetFileNameWithoutExtension(file.Name)}");
                    //Debug.Log($"{file.Name.Split(".")[0]}");
                    break;
                }
                else
                {
                    card.will = Resources.Load<Sprite>($"0");
                    break;
                }
            }*/
            fileNames = fileNamesTextAsset.text.Split(new[] { '\r', '\n' }, System.StringSplitOptions.RemoveEmptyEntries);
            foreach (string file in fileNames)
            {   
                string image = Path.GetFileNameWithoutExtension(imageName);
                Sprite sprite = Resources.Load<Sprite>($"{image}");
                
                if (file.Equals(imageName))
                {
                    if (sprite == null)
                    {
                        Debug.LogError($"Failed to load image: {image}");
                    }
                    else
                    {
                        card.will = Resources.Load<Sprite>($"{Path.GetFileNameWithoutExtension(imageName)}");
                        Debug.Log($"Successfully loaded image: {image}");
                        break;
                    }
                }
                else
                {
                    sprite = Resources.Load<Sprite>($"0");
                    if (sprite != null) 
                    {
                        card.will = sprite;
                    }
                    
                }
            }
        }

    
}
    
    public static Card GetCardById(string id) 
    {
        foreach (var card in cards)
        {
            
            if (card.id.Equals(id))
            {
                return card;
            }
        }
        return null;
    }
    public static Card GetCardByName(string name)
    {
        foreach (var card in cards)
        {
            
            if (card.name.ToLower().Equals(name.ToLower()))
            {
                //Debug.Log(card.name.ToLower()+" COMPARAMOS "+ name.ToLower());
                return card;
            }
        }
        return null;
    }
}
