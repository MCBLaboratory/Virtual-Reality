using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System.IO;
using LSL;

public class MainItem : MonoBehaviour
{
    // Main file that contains the main logic of the game, storage of the variables of interest and other miscellaneous functions
    [Header("Update stagenumbers")]
    public int stageNumber = 0;
    public int stageNumberText = 1;
    
    [Header("Stage related settings")]
    public int currentStage1;
    public TextMeshProUGUI stageNum;
    public TextMeshProUGUI timerNum;

    [Header("Objects")]
    public GameObject leftObject;
    public GameObject rightObject;
    public GrabAdvance item5;
    public GameObject startPreference;

    [Header("Start button")]
    public StartMainGame startButton; 

    [Header("Variables grabbing items")]
    public bool grabItemLeft = false;
    public bool grabItemRight = false;
    public string itemTypeTempSide = "";
    public string itemTypeTemp = "";

    [Header("Logging grabbing event")]
    public float timer = 6f;
    public bool reportActive = false;
    public bool grabbedInTime = false;
    public bool grabbedInTimeOnce = false;
    public bool updateItemGrab = false;
    public bool finished = false;
    private bool finishedWriting = false;
    bool firstReport = false;

    [Header("Random seed")]
    public int randomSeed;

    [Header("Block trials")]
    public int[] blocksTrials = new int[10];
    public string currentBlock = "";
    public int currentBlockNum = 0;
    int tempIndexBT = 0;

    [Header("LSL")]
    string StreamName = "MainFile LSL";
    string StreamType = "Markers";
    public StreamOutlet outlet;
    private string[] sample = {""};
    private bool isRunning = false;

    public int[] distributionProducts = new int[101];

    public bool stageTriggered = false;
    private string csvFilePath;
    public AudioSource audio2;

    void Start()
    {
        randomSeed = System.DateTime.Now.Millisecond;
        UnityEngine.Random.InitState(randomSeed);

        GenerateRandomBlockTrials();
        GenerateRandomList();

        currentBlock = blocksTrials[currentBlockNum] == 0 ? "C1" : "C2";

        string timestamp = System.DateTime.Now.ToString("MM-dd_HH-mm");

        // Define the path for the .csv file
        csvFilePath = Application.dataPath + "/Data/" + timestamp + "_Rseed" + randomSeed + " LoggingData.csv";

        if(!isRunning)
        {
            var hash = new Hash128();
            hash.Append(StreamName);
            hash.Append(StreamType);
            StreamInfo streamInfo = new StreamInfo(StreamName, StreamType, 1, LSL.LSL.IRREGULAR_RATE,
                channel_format_t.cf_string, hash.ToString());
            outlet = new StreamOutlet(streamInfo);
            isRunning = true;
            Debug.Log("Opening LSL");
        //
        }
    }

    public void PushSampleToOutlet(string[] sample)
    {
        outlet.push_sample(sample);
    }

    void GenerateRandomBlockTrials()
    {  
        blocksTrials = new int[10] {0,0,0,0,0,1,1,1,1,1};
        Shuffle(blocksTrials);
    }

    void GenerateRandomList()
    {
        int n = 0;

        while (n < 10) {
            int[] tempArrayTrials;
            var tempBT = blocksTrials[tempIndexBT];
            if(tempBT == 0)
            {
                var diverseTemp = new List<int>{};
                for (int i = 0; i < 8; i++)
                {
                    diverseTemp.Add(1);
                }
                int[] a = diverseTemp.ToArray();
                
                var targetTemp = new List<int>{};
                for (int i = 0; i < 2; i++)
                {
                    targetTemp.Add(0);
                }
                int[] b = targetTemp.ToArray();

                tempArrayTrials = new int[a.Length + b.Length];
                a.CopyTo(tempArrayTrials, 0);
                b.CopyTo(tempArrayTrials, a.Length);
                Shuffle(tempArrayTrials);
            }

            else
            {
                var diverseTemp = new List<int>{};
                for (int i = 0; i < 2; i++)
                {
                    diverseTemp.Add(1);
                }
                int[] a = diverseTemp.ToArray();
                
                var targetTemp = new List<int>{};
                for (int i = 0; i < 8; i++)
                {
                    targetTemp.Add(0);
                }
                int[] b = targetTemp.ToArray();

                tempArrayTrials = new int[a.Length + b.Length];
                a.CopyTo(tempArrayTrials, 0);
                b.CopyTo(tempArrayTrials, a.Length);
                Shuffle(tempArrayTrials);
            }

            int startTempIndexList = n * 10;

            System.Array.Copy(tempArrayTrials, 0, distributionProducts, startTempIndexList, 9);

            tempIndexBT++;
            n++;
            
        }
    }

    public int[] Shuffle(int[] array)
    {
        int n = array.Length;

        while (n > 1)
        {
            int i = UnityEngine.Random.Range(0,n);
            n--;
            (array[i],array[n]) = (array[n], array[i]);
        }

        return array;
    }

    public void Update()
    {
        if(currentBlockNum == 10) {}
        else {currentBlock = blocksTrials[currentBlockNum] == 0 ? "C1" : "C2";}

        if(stageNumber < 100)
        {
            currentStage1 = distributionProducts[stageNumber];
            stageNum.text = "Stagenumber: " + stageNumberText.ToString();
            timerNum.text = timer.ToString($"F{1}");
            
            if(stageNumber == 0 && !firstReport && reportActive)
                {
                    string itemType = "None";
                    string itemDiverseTarget = "None";
                    string eventType = "Begin";

                    RecordEvent(eventType, itemType, itemDiverseTarget);
                    firstReport = true;
                }

            if(reportActive && stageNumber <= 99 && !finished)
            {
                timer -= Time.deltaTime;
                updateItemGrab = true;
                if(timer < 0f && !finished)
                {
                    if(!grabbedInTime && !grabbedInTimeOnce)
                    {
                        string itemType = "None";
                        string itemDiverseTarget = "None";
                        string eventType = "No grab";

                        RecordEvent(eventType, itemType, itemDiverseTarget);
                        grabbedInTimeOnce = true;

                    }

                    timer = 6f;
                    grabItemLeft = false;
                    grabItemRight = false;
                    stageNumber += 1;
                    stageNumberText = stageNumber + 1;
                    item5.UpdateMaterial();
                    grabbedInTimeOnce = false;
                    updateItemGrab = false;

                    if(stageNumber % 10 == 0 && stageNumber != 0)
                    {
                        string itemType = "BREAK";
                        string itemDiverseTarget = "BREAK";
                        string eventType = "BREAK";

                        RecordEvent(eventType, itemType, itemDiverseTarget);
                        reportActive = false;

                        leftObject.SetActive(true);
                        rightObject.SetActive(true);

                        startButton.Start();
                        currentBlockNum++;
                        stageTriggered = true;


                    }
                }
            }
        }
        else
        {
            finished = true;
            if(stageNumber == 100 && !finishedWriting) {FinalRound();}
        }
        
    }

    void FinalRound()
    {
        string itemType = "Finished";
        string itemDiverseTarget = "Finished";
        string eventType = "Finished";


        RecordEvent(eventType, itemType, itemDiverseTarget);

        leftObject.SetActive(false);
        rightObject.SetActive(false);
        stageNum.text = "Almost finished!";
        timerNum.text = "";
        timer = 0f;
        reportActive = false;
        startButton.Quit();
        finishedWriting = true;
        audio2.Play();
        startPreference.SetActive(true);
    }

    public void RecordEvent(string eventType, string itemType, string itemDiverseTarget)
    {
        itemTypeTempSide = itemType;
        itemTypeTemp = itemDiverseTarget;

        // Get current timestamp
        //string timestamp = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff");
        string date = System.DateTime.Now.ToString("yyyy-MM-dd");
        string timestamp = System.DateTime.Now.ToString("HH:mm:ss:fff");

        // Write data to .csv file
        string temp1 = "";

        var temp = distributionProducts[stageNumber];
        if(temp == 1)
        {
            temp1 = "DD";
        }
        else
        {
           temp1 = "STP-D"; 
        }
        
        if(stageNumber == 0)
        {
            string arrayString = $"\"[{string.Join(",", distributionProducts)}]\"";
            string arrayString1 = $"\"[{string.Join(",", blocksTrials)}]\"";

            string materialLeft = item5.objectRenderer.material.ToString();
            string materialRight = item5.otherObjectRenderer.material.ToString();
            materialLeft = materialLeft.Replace(" (Instance) (UnityEngine.Material)", "");
            materialRight = materialRight.Replace(" (Instance) (UnityEngine.Material)", "");

            int tempStageNumber = stageNumber + 1;
            string data0 = $"{date},{eventType},{currentBlock},{temp1},{tempStageNumber},{materialLeft},{materialRight},{itemType},{itemDiverseTarget},{timestamp},{randomSeed},{arrayString1},{arrayString}";
            WriteToCSV(data0);
        }
        else
        {
            string materialLeft = item5.objectRenderer.material.ToString();
            string materialRight = item5.otherObjectRenderer.material.ToString();
            materialLeft = materialLeft.Replace(" (Instance) (UnityEngine.Material)", "");
            materialRight = materialRight.Replace(" (Instance) (UnityEngine.Material)", "");

            int tempStageNumber = stageNumber + 1;
            string data = $"{date},{eventType},{currentBlock},{temp1},{tempStageNumber},{materialLeft},{materialRight},{itemType},{itemDiverseTarget},{timestamp},{randomSeed}";
            WriteToCSV(data);
        }
    
    }

    private void WriteToCSV(string data)
    {
        // Check if .csv file exists, if not, create it and write header
        if (!File.Exists(csvFilePath))
        {
            // Write header to .csv file
            File.WriteAllText(csvFilePath, "date,eventType,currentBlock,currentStage,StageNumber,materialLeft,materialRight,itemSide,itemChosen,Timestamp(H:M:S:MS),randomSeed,blocksTrials,distributionProducts\n");
        }

        // Check if the file is empty
        if (new FileInfo(csvFilePath).Length == 0)
        {
            // Write header to .csv file
            File.WriteAllText(csvFilePath, "date,eventType,currentBlock,currentStage,StageNumber,materialLeft,materialRight,itemSide,itemChosen,Timestamp(H:M:S:MS),randomSeed,blocksTrials,distributionProducts\n");
    }

        // Append data to .csv file
        File.AppendAllText(csvFilePath, data + "\n");
    }
}