using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class CardSpawner : MonoBehaviour
{
    public GameObject cardOriginal;
    //public GameObject selectedFilters;
    //public GameObject filter1, filter2, filter3, filter4, filter5, filter6, filter7;
    //public TextField tf;
    
    // Start is called before the first frame update
    void Start()
    {
        CreateCards(3);
    }
    public void CreateCards(int cardNum)
    {
        for (int i = 0; i < cardNum; i++)
        {
            GameObject cardPrefab = Instantiate(cardOriginal, new Vector3(i, cardOriginal.transform.position.y, i), cardOriginal.transform.rotation);
        }
    }
    // Update is called once per frame
 /*   void Update()
    {
        string[] attArray= {"Darkness","Light","Fire","Wind","Water"};
        string[] raceArray = { "Machine", "Demonic World", "Human", "Story", "Atom" };
        string[] tcArray = { "0","1", "2", "3", "4", "5","6","7","8","9","10","11","12","13","14","15" };
        string[] setArray = { "(ADW) Assault into the Demonic World", "(NWE) A New World Emerges", "(GOG) Game of Gods" };
        string[] rarityArray = { "C", "U", "R", "SR", "XR", "T", "MR", "RR", "JR", "JR*", "N", "AR", "JAR" };
        string[] typeArray = { "Resonator", "Chant", "Addition", "Ruler", "Regalia", "Magic Stone" };
        string[] kwordArray = { "Swiftness", "Bane", "Barrier", "Beligerance", "Drain","Eternal","First Strike","Quickcast","Judgment","Remnant" };
        string attribute,race,totalCost,set,rarity,type,keyword;
        //recorre los filtros
        for (int i = 0; i < selectedFilters.GetComponents<GameObject>().Count(); i++)
        {
            //si el filtro está activado, lo va a comparar
            if (selectedFilters.GetComponent<GameObject>().activeInHierarchy == true)
            {
                #region for-if-filters
                //compara cada elemento de la lista con el contenido del filtro
                //ATTRIBUTE
                for (int j = 0; j < attArray.Length; j++)
                {
                    //cuando el contenido del filtro coincide, guarda el valor y analiza los siguientes filtros
                    if (selectedFilters.GetComponent<GameObject>().GetComponent<Label>().Equals(attArray[j]))
                    {
                        attribute = attArray[j];
                    }
                }
                //RACE
                for (int j = 0; j < raceArray.Length; j++)
                {
                    //cuando el contenido del filtro coincide, guarda el valor y analiza los siguientes filtros
                    if (selectedFilters.GetComponent<GameObject>().GetComponent<Label>().Equals(raceArray[j]))
                    {
                        race = raceArray[j];
                    }
                }
                //TOTALCOST
                for (int j = 0; j < tcArray.Length; j++)
                {
                    //cuando el contenido del filtro coincide, guarda el valor y analiza los siguientes filtros
                    if (selectedFilters.GetComponent<GameObject>().GetComponent<Label>().Equals(tcArray[j]))
                    {
                        totalCost = tcArray[j];
                    }
                }
                //SET
                for (int j = 0; j < setArray.Length; j++)
                {
                    //cuando el contenido del filtro coincide, guarda el valor y analiza los siguientes filtros
                    if (selectedFilters.GetComponent<GameObject>().GetComponent<Label>().Equals(setArray[j]))
                    {
                        set = setArray[j];
                    }
                }
                //RARITY
                for (int j = 0; j < rarityArray.Length; j++)
                {
                    //cuando el contenido del filtro coincide, guarda el valor y analiza los siguientes filtros
                    if (selectedFilters.GetComponent<GameObject>().GetComponent<Label>().Equals(rarityArray[j]))
                    {
                        rarity = rarityArray[j];
                    }
                }
                //TYPE
                for (int j = 0; j < typeArray.Length; j++)
                {
                    //cuando el contenido del filtro coincide, guarda el valor y analiza los siguientes filtros
                    if (selectedFilters.GetComponent<GameObject>().GetComponent<Label>().Equals(typeArray[j]))
                    {
                        type = typeArray[j];
                    }
                }
                //KEYWORD
                for (int j = 0; j < kwordArray.Length; j++)
                {
                    //cuando el contenido del filtro coincide, guarda el valor y analiza los siguientes filtros
                    if (selectedFilters.GetComponent<GameObject>().GetComponent<Label>().Equals(kwordArray[j]))
                    {
                        keyword = kwordArray[j];
                    }
                }
                #endregion
            }
        }
        //sumar a los filtros la búsqueda del search
        //hacer una query en base a todo
        //instanciar las cartas que cumplan los requisitos
        //Instantiate(cardPrefab(attribute, race, totalCost, set, rarity, type, keyword), transform.position, Quaternion.identity);
    }
*/
}
