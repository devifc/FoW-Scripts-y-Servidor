using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantiableCard: ScriptableObject
{
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

    /*public GameObject backCard;
    public Sprite will;//image that shows the will
    public Sprite artwork;
    public int extraCost;//It shows the extra cost behind the will
    */
}
