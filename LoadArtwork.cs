using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LoadArtwork : MonoBehaviour
{
    private FileInfo[] _foundFiles;
    private string _imageName;
    private Card _card;
    public LoadArtwork(FileInfo[] foundFiles, string imageName, Card card)
    {
        _foundFiles = foundFiles;
        _imageName = imageName;
        _card = card;
        
    }
    // Start is called before the first frame update
    void Start()
    {
        //LOOP FOUND FILES
        foreach (FileInfo file in _foundFiles)
        {
            //IT COMPARES NAMES
            if (file.Name.Equals(_imageName))
            {
                //FILE FOUND
                Console.WriteLine($"Archivo encontrado: {file.FullName}");

                _card.artwork = Resources.Load<Sprite>(file.Name.Split(".")[0]);

                break; //Break loop if finds the file
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
