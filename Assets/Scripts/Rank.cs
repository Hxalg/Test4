using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class Rank : MonoBehaviour
{

    public GameObject ItemPrefab;
    public Image ItemParent;
    
    List<Score> scoreList = new List<Score>();

    // Use this for initialization
    void Start()
    {
        StreamReader sr = new StreamReader(Application.dataPath + "/Resources/RankingList.txt");
        string nextLine;
        while ((nextLine = sr.ReadLine()) != null)
        {
            scoreList.Add(JsonUtility.FromJson<Score>(nextLine));
        }
        sr.Close();

        for (int i = 0; i < scoreList.Count; i++)
        {
            GameObject item = Instantiate(ItemPrefab, ItemParent.transform.position, Quaternion.identity) as GameObject;
            item.transform.parent = ItemParent.transform;
            Text[] Children = item.GetComponentsInChildren<Text>();
            //item.transform.Find("Number").GetComponent<Text>().text = (i + 1).ToString();
            //item.transform.Find("Name").GetComponent<Text>().text = scoreList[i].name;
            //item.transform.Find("Score").GetComponent<Text>().text = scoreList[i].score.ToString();
            
            Children[0].text = (i + 1).ToString();
            Children[1].text = scoreList[i].name;
            Children[2].text = scoreList[i].score.ToString();
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
