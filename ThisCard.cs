using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public class ThisCard : MonoBehaviour
{
    public List<Card> thisCard = new List<Card>();
    public int thisId;

    public string id;
    public string cardName;
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
    //public string cardExtraCost;//It shows the extra cost behind the will

    //public GameObject playerDeck;
    //public GameObject prefab;
    public GameObject stats; //Image that shows the ATK and DEF if the card have it
    public GameObject chantBackground; //If type is "Chant" it will show this Image
    public Text nameText; //name
    public Text raceText; //If card has a race, it will show it
    public Text cardTypeText; //Type. Resonator, chant, ruler...
    public Text abilitiesText; //description of the card
    public Image artWork; //artwork
    public Image willCost; //Image displayed depending on the cost of the card
    public Text extraCost; //Extra cost of the card (cost that is a number)
    public Text attackText; //ATK
    public Text defenseText; //DEF
    public Text colourText; //primary colors of the card
    public bool cardBack;

    // Start is called before the first frame update
    void Start()
    {
        //CardDBAccess db = new CardDBAccess(); solo sirve esto cuando el campo no es estático
        //thisCard[0] = CardDBAccess.cards[thisId];
    }

    // Update is called once per frame
    void Update()
    {
        id = thisCard[0].id;
        name = thisCard[0].name;
        race = thisCard[0].race;
        cost = thisCard[0].cost;
        colour = thisCard[0].colour;
        type = thisCard[0].type;
        ATK = thisCard[0].ATK;
        DEF = thisCard[0].DEF;
        abilities = thisCard[0].abilities;
        divinity = thisCard[0].divinity;
        flavour = thisCard[0].flavour;
        artist = thisCard[0].artist;
        rarity = thisCard[0].rarity;
        fowID = thisCard[0].fowID;
        artwork = thisCard[0].artwork;
        backCard = thisCard[0].backCard;
        will = thisCard[0].will;
        //cardExtraCost = thisCard[0].extraCost;

        nameText.text = "";
        raceText.text = "";
        cardTypeText.text = "";
        abilitiesText.text = "";
        attackText.text = "";
        defenseText.text = "";
        //cardExtraCost = "";

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

        string resources = "Assets/Resources"; // Ruta a la carpeta de recursos
        string imageName = $"{name.ToLower()}.png"; // Nombre del archivo a buscar formato: [atomic bahamut.png]

        // Obtiene información sobre la carpeta de recursos
        DirectoryInfo dirInfo = new DirectoryInfo(resources);

        // Busca archivos en la carpeta y sus subcarpetas
        FileInfo[] foundFiles = dirInfo.GetFiles("*", SearchOption.AllDirectories);

        // Recorre los archivos encontrados
        foreach (FileInfo file in foundFiles)
        {
            // Compara el nombre del archivo actual con el nombre buscado
            if (file.Name.Equals(imageName))
            {
                // Se encontró el archivo
                Console.WriteLine($"Archivo encontrado: {file.FullName}");

                artWork.sprite = Resources.Load<Sprite>(file.Name.Split(".")[0]);           //pendiente de revision

                break; // Salir del bucle si se encuentra el archivo
            }
        }
        int R = 0, B = 0, W = 0, U = 0, G = 0;
        string num = "";

        /*for (int i = 0; i < colour.Count; i++)
         {
             colourText.text += " "+colour[i];
         }*/
        for (int i = 0; i < colour.Count; i++)
        {
            if (cost.Contains<char>('R'))
            {
                R++;
            }
            if (cost.Contains<char>('B'))
            {
                B++;
            }
            if (cost.Contains<char>('W'))
            {
                W++;
            }
            if (cost.Contains<char>('U'))
            {
                U++;
            }
            if (cost.Contains<char>('G'))
            {
                G++;
            }

        }

        if (cost != null && cost.Length >= 3)
        {
            num = cost.Substring(cost.Length - 3, 2);
        }
        else
        {
            num = "0";
            cost = "0";//cards like J-Rulers without cost
        }

        SetWillImage(R, B, W, U, G, num);

    }

    public void SetWillImage(int R, int B, int W, int U, int G, string num)
    {
        string resources = "Assets/Resources"; // Ruta a la carpeta de recursos
        string imageName = ""; // Nombre del archivo a buscar

        if (R != 0)
        {
            for (int i = 0; i < R; i++)
            {
                imageName += "R";
            }
        }
        if (B != 0)
        {
            for (int i = 0; i < B; i++)
            {
                imageName += "B";
            }
        }
        if (W != 0)
        {
            for (int i = 0; i < W; i++)
            {
                imageName += "W";
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
        string pattern = @"\d+";
        bool containsNumbers = Regex.IsMatch(num, pattern);

        if (containsNumbers == true && !num.Equals("0"))
        {
            //EXTRA COST
            Debug.Log(name);
            Debug.Log(cost);
            extraCost.text = "" + num.Split("}")[0].Split("{")[1];
            // 0  1  2
            // {  10  }
        }
        else
        {
            extraCost.text = "0";
        }
        imageName += ".png";// RRR.png if it repeats 3 times
        /* Debug.Log(name);
         Debug.Log(cost);
         Debug.Log(num);
         Debug.Log(imageName);*/


        // Obtiene información sobre la carpeta de recursos
        DirectoryInfo dirInfo = new DirectoryInfo(resources);

        // Busca archivos en la carpeta y sus subcarpetas
        FileInfo[] foundFiles = dirInfo.GetFiles("*", SearchOption.AllDirectories);


        // Recorre los archivos encontrados
        foreach (FileInfo file in foundFiles)
        {
            // Compara el nombre del archivo actual con el nombre buscado
            if (file.Name.Equals(imageName))
            {
                // Se encontró el archivo
                Console.WriteLine($"Archivo encontrado: {file.FullName}");
                //must remove extension
                willCost.sprite = Resources.Load<Sprite>($"{file.Name.Split(".")[0]}");

                break; // Salir del bucle si se encuentra el archivo
            }
        }
    }
}
