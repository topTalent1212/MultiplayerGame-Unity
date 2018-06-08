﻿using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class MapGeneratorPanel : MonoBehaviour {

    public GameObject warningMessageS;
    public GameObject warningMessageM;

    public Text TextseedS;
    public Slider SlidermapSizeS;
    public Slider SliderResourcesS;
    public Slider SliderjitterProbabilityS;
    public Slider SliderchunkSizeMinS;
    public Slider SliderchunkSizeMaxS;
    public Slider SliderlandPercentageS;
    public Slider SliderelevationMaximumS;
    public Slider SliderregionBorderS;
    public Slider SliderregionCountS;
    public Slider SlidererosionPercentageS;

    public Text TextseedM;
    public Slider SlidermapSizeM;
    public Slider SliderResourcesM;
    public Slider SliderjitterProbabilityM;
    public Slider SliderchunkSizeMinM;
    public Slider SliderchunkSizeMaxM;
    public Slider SliderlandPercentageM;
    public Slider SliderelevationMaximumM;
    public Slider SliderregionBorderM;
    public Slider SliderregionCountM;
    public Slider SlidererosionPercentageM;

    ////////////
    public Text map1;
    public Text map2;
    public Text map3;
    public Text map4;
    public Text map5;
    public Text map6;
    public Text map7;
    public Text map8;
    public Text map9;
    public Text map10;

    public GameObject map1G;
    public GameObject map2G;
    public GameObject map3G;
    public GameObject map4G;
    public GameObject map5G;
    public GameObject map6G;
    public GameObject map7G;
    public GameObject map8G;
    public GameObject map9G;
    public GameObject map10G;

    public GameObject[] mapsG;
    public Text[] maps;

    public GameObject randomPanel;
    public GameObject loadPanel;
    public Text button;

    bool modeRandom = true;

    private void Update()
    {
        warningMessageS.SetActive(SlidermapSizeS.value >= 3);
        warningMessageM.SetActive(SlidermapSizeM.value >= 3);
    }

    public void ChangeGenMode()
    {
        loadPanel.SetActive(modeRandom);
        modeRandom = !modeRandom;
        randomPanel.SetActive(modeRandom);

        if(modeRandom)
        {
            button.text = "LOAD";
            GameManager.Instance.path = "";
        }
        else
        {
            mapsG = new GameObject[] { map1G, map2G, map3G, map4G, map5G, map6G, map7G, map8G, map9G, map10G};
            maps = new Text[] { map1, map2, map3, map4, map5, map6, map7, map8, map9, map10 };

            foreach(GameObject g in mapsG)
                g.SetActive(false);

            button.text = "RANDOM";

            string[] paths = Directory.GetFiles(Application.persistentDataPath, "*.map");
            Array.Sort(paths);
            for(int i = 0; i < paths.Length; ++i)
                if(i < 11)
                {
                    maps[i].text = Path.GetFileNameWithoutExtension(paths[i]);
                    mapsG[i].SetActive(true);
                }
        }
    }

    public void SelectMap(int i)
    {
        GameManager.Instance.path = Path.Combine(Application.persistentDataPath, maps[i].text + ".map");
    }
}
