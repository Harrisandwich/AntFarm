using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Timers;
using System.IO;
using System.Threading;
namespace AntFarm
{
    public partial class AntFarmForm : Form
    {

        private static int timerInterval = 1000;
        public static Dictionary<int, AntHill> antHills = new Dictionary<int, AntHill>(99);


        private static String[] cityNames = new String[] {"Antakya", "Constantinople", "Antioch", "Antitolia", "Antwerp", "Antarctica", "Antigua", "Istantbul", "Byzantium", "Vantican", "Antopia", "Antopolis"};
        private static String[] regionPrefixes = new String[] { "Ant", "Dur", "Ott", "Green", "Pur", "Ost", "Black", "Nord", "Sud", "West", "Castle" };
        private static String[] regionSuffixes = new String[] { "stan", "holm", "ite", "ues", "awa", "oss", "dun", "grut", "tree", "wood", "land", "ia", "any", "ton", "ana", "watch" };
        private static String[] townshipPrefixes = new String[] { "Meso", "Wood", "Green", "Bowman", "Peters", "Ponty", "Ants", "Towers", "Waites", "Ports", "Ost", "Black", "Nord", "Sud", "West", "Nights", "Forts" };
        private static String[] townshipSuffixes = new String[] { "ton", "ville", "berg", "grad", "port", "hill", "spin", "church", "cross", "borough", "mouth", "heim", "place", "watch" };

        delegate void SetTextCallback(string text);
        //inspectionValues
        private int anthillNum = 0;
        private int antNum = 0;
        private int tickCount = 0;
        private int lastTickCount = 0;

        private bool removeInProgress = false;
        private bool runTimerRunning = false;
        private bool antFarmCreated = false;

        //Food gen
        private bool isGeneratingFood = false;
        private string foodGenRoot = "";
        private string[] folderList;

        private string enviromentFullPath = "";

        private const int MAX_ANTHILLS = 99;
        private string savePath = "";

        private string selectedFolder;

        private List<int> selectedEntities;

        private FormState formState; 

        //Property functions

        public FormState FormStateProperty
        {
            get
            {
                return formState; 
            }
            set
            {
                if(value != formState)
                {
                    OnFormStateChanged(value);
                    formState = value; 
                }
                
            }
        }

        
        //Main Functions
        public AntFarmForm()
        {
            InitializeComponent();
        }

        private void AntFarmForm_Load(object sender, EventArgs e)
        {
            AntHill dummy = new AntHill(0, "", false);
            antHills.Add(0, dummy);
            txtOutput.Text += "Welcome to AntFarm!";
            cbxSpeed.SelectedIndex = 4;
            runTimer.Stop();

            //initial state
            setState(FormState.Initial);
        }

        private string createAntHill(string path)
        {
            string dest;
            string returnString;
            AntHill newAntHill;
            bool antHillCreated = false;
            string errorString = "";
            returnString = "Failed";
            
            //split the path to ensure that the anthill is being placed in the enviroment
            //For now, i dont want to allow placment in any folder. 
            string[] pathSplit = path.Split(Path.DirectorySeparatorChar);
            string[] enviroSplit = enviromentFullPath.Split(Path.DirectorySeparatorChar);
            

            if (pathSplit.Contains(enviroSplit.Last()))
            {
                if (pathSplit.Last() == enviroSplit.Last())
                {
                    path += @"\Entrance";
                }
            }
            else
            {
                return returnString + ": Not in Enviroment";
            }

            
            if (antHills.Count < MAX_ANTHILLS)
            {
                for (int i = 0; i < MAX_ANTHILLS; i++)
                {
                    try
                    {
                        
                        dest = path + @"\Anthill_" + i.ToString("D2");
                        newAntHill = new AntHill(i, dest, true);

                        //Reenable if i ever want more then one anthill
                        /*if (antHills.ContainsKey(i) && antHills[i] == null)
                        {
                            antHills[i] = newAntHill;
                        }
                        else
                        {
                            antHills.Add(i, newAntHill);
                        }*/

                        //single hill code
                        antHills[0] = newAntHill;


                        Directory.CreateDirectory(dest);
                        newAntHill.StartHill(true);
                        returnString = "AntHill_" + i.ToString("D2") + " was created at " + dest;
                        cbxAntHill.Items.Add(i.ToString("D2"));
                        btnBrowseAnthill.Enabled = false;
                        antFarmCreated = true;
                        
                        /*
                         * Update the folder selector
                         * 
                         */

                        //entitySelector.Items[0].BackColor


                        if (i == 0)
                        {
                            cbxAntHill.SelectedIndex = 0;
                        }
                        antHillCreated = true;
                        i = MAX_ANTHILLS;

                        if (!runTimerRunning)
                        {
                            runTimer = new System.Windows.Forms.Timer();
                            runTimer.Interval = timerInterval;
                            runTimer.Tick -= new EventHandler(runTimerTick);
                            runTimer.Tick += new EventHandler(runTimerTick);
                            runTimer.Start();
                            runTimerRunning = true;
                            statusLabel.Text = "Running";
                            statusLabel.BackColor = Color.Green;


                            setState(FormState.Running);
                        }


                       
                        
                        

                    }
                    catch (ArgumentException e)
                    {
                        newAntHill = null;
                        returnString = "Looking for free slot";
                        antHillCreated = false;
                        errorString = e.Message;
                        
                    }
                }

                if (!antHillCreated)
                {
                    returnString = "Anthill creation failed: " + errorString;
                }

            }
            else
            {
                returnString = "Max AntHills reached. Delete an AntHill or wait for one to die.";
            }




            return returnString;
        }
        
        private void btnBrowseAnthill_Click(object sender, EventArgs e)
        {
            folderBrowserDialog.ShowNewFolderButton = false;
            DialogResult result = folderBrowserDialog.ShowDialog();

            if (result == DialogResult.OK)
            {

                txtOutput.Text += Environment.NewLine + createAntHill(folderBrowserDialog.SelectedPath);
            }
            
            
            
        }

        private void btnBrowseFoodSource_Click(object sender, EventArgs e)
        {
            //saveFileDialog.FileName = "FoodSource.txt";
            //saveFileDialog.ShowDialog();
            int size = 500;

            folderBrowserDialog.ShowNewFolderButton = false;
            DialogResult result = folderBrowserDialog.ShowDialog();

            if (result == DialogResult.OK)
            {
                if (!File.Exists(folderBrowserDialog.SelectedPath))
                {
                    if (int.TryParse(txtFoodSourceSize.Text, out size))
                    {
                        using (StreamWriter sw = File.CreateText(folderBrowserDialog.SelectedPath + @"\FoodSource.txt"))
                        {
                            sw.WriteLine(size);
                            sw.Close();
                        }
                    }
                    else
                    {
                        using (StreamWriter sw = File.CreateText(folderBrowserDialog.SelectedPath + @"\FoodSource.txt"))
                        {
                            size = 500;
                            sw.WriteLine(size);
                            sw.Close();
                        }

                    }
                }

                //File.WriteAllText(saveFileDialog.FileName, size.ToString());
                txtOutput.Text += Environment.NewLine + "Food Source created at " + saveFileDialog.FileName + " with a value of " + size;
            }
           
           
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            runTimer.Stop();
            runTimer = new System.Windows.Forms.Timer();
            runTimer.Interval = timerInterval;
            runTimer.Tick -= new EventHandler(runTimerTick);
            runTimer.Tick += new EventHandler(runTimerTick);
            runTimer.Start();
            runTimerRunning = true;
            btnStart.Enabled = false;
            btnPause.Enabled = true;
            txtOutput.Text += Environment.NewLine + "Simulation running at " + timerInterval + " miliseconds-per-tick";
            statusLabel.Text = "Running";
            statusLabel.BackColor = Color.Green;
        }

        private void btnPause_Click(object sender, EventArgs e)
        {
            runTimerRunning = false;
            runTimer.Stop();
            btnStart.Enabled = true;
            btnPause.Enabled = false;
            runTimer = new System.Windows.Forms.Timer();
            txtOutput.Text += Environment.NewLine + "Simulation stopped. All ants are frozen";
            statusLabel.Text = "Stopped";
            statusLabel.BackColor = Color.Red;
        }

        private void cbxSpeed_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (runTimerRunning)
            {
                txtOutput.Text += Environment.NewLine + "Simulation stopped. All ants are frozen";
            }
            runTimerRunning = false;
            runTimer.Stop();
            statusLabel.Text = "Stopped";
            statusLabel.BackColor = Color.Red;
            int.TryParse(cbxSpeed.Text,out timerInterval);
            runTimer = new System.Windows.Forms.Timer();
            runTimer.Interval = timerInterval;
            runTimer.Tick -= new EventHandler(runTimerTick);
            runTimer.Tick += new EventHandler(runTimerTick);
            txtOutput.Text += Environment.NewLine + "Run timer speed set to " + timerInterval  + " miliseconds-per-tick";
        }

        private void cbxAnthill_SelectedIndexChanged(object sender, EventArgs e)
        {
            
            if (cbxAntHill.Text != "")
            {
                int.TryParse(cbxAntHill.Text, out anthillNum);
                lblAnthillInfo.Text = "";
                lblAnthillInfo.Text += "AntHill_" + anthillNum.ToString("D2") + " info:";
                lblAnthillInfo.Text += Environment.NewLine + "Total Ants: " + antHills[anthillNum].AntsTotalProperty;
                lblAnthillInfo.Text += Environment.NewLine + "Location: " + antHills[anthillNum].LocationProperty;

                cbxAnt.Items.Clear();
                for(int i = 0; i< antHills[anthillNum].AntsTotalProperty; i++)
                {
                    cbxAnt.Items.Add(i.ToString("D4"));
                    cbxAnt.Enabled = true;
                }

            }


        }

        private void cbxAnt_SelectedIndexChanged(object sender, EventArgs e)
        {
            
            if (cbxAnt.Text != "")
            {
                int.TryParse(cbxAnt.Text, out antNum);
                lblAntInfo.Text = "";
                lblAntInfo.Text += "Ant Number " + antNum.ToString("D4") + " info:";
                lblAntInfo.Text += Environment.NewLine + "Type: " + antHills[anthillNum].ants[antNum].TypeProperty;
                lblAntInfo.Text += Environment.NewLine + "Health: " + antHills[anthillNum].ants[antNum].HealthProperty;
                lblAntInfo.Text += Environment.NewLine + "Hunger: " + antHills[anthillNum].ants[antNum].HungerProperty;
                lblAntInfo.Text += Environment.NewLine + "Age: " + antHills[anthillNum].ants[antNum].AgeProperty;
                lblAntInfo.Text += Environment.NewLine + "State: " + antHills[anthillNum].ants[antNum].StateProperty;
                lblAntInfo.Text += Environment.NewLine + "Carrying: " + antHills[anthillNum].ants[antNum].CarryingProperty;
                lblAntInfo.Text += Environment.NewLine + "In Nest: " + antHills[anthillNum].ants[antNum].InNestProperty;
                lblAntInfo.Text += Environment.NewLine + "Location: " + antHills[anthillNum].ants[antNum].LocationProperty;
               
            }
        }

        private void Anthills_Update(object sender, EventArgs e)
        {
            cbxAntHill.Items.Clear();
            try
            {
                for (int i = 0; i <= (antHills.Count - 1); i++)
                {
                    
                    if (antHills[i] != null)
                    {
                        cbxAntHill.Items.Add(i.ToString("D2"));
                    }

                }
            }
            catch (KeyNotFoundException err) { }


        }

        private void Ants_Update(object sender, EventArgs e)
        {
            cbxAnt.Items.Clear();
            try
            {
                if (antHills[anthillNum].AntsTotalProperty > 0)
                {
                    for (int i = 0; i < antHills[anthillNum].AntsTotalProperty; i++)
                    {
                        if (antHills[anthillNum].ants[i] != null)
                        {
                            cbxAnt.Items.Add(i.ToString("D4"));
                        }
                    }
                }
            }
            catch (KeyNotFoundException err)
            { }
        }

        private void CleanUp(object sender, FormClosedEventArgs e)
        {
            /*
            try
            {
                runTimer.Stop();
                removeInProgress = true;
                CheckRemove();
            }
            catch (Exception err) { };
            */
            if (antFarmCreated == false && enviromentFullPath != "")
            {
                System.IO.DirectoryInfo enviroPath = new DirectoryInfo(enviromentFullPath);
                if (File.Exists(enviromentFullPath + @"\DoNotEnter.phe")) 
                {
                    antHills[0].pheromonesPlaced.Clear();

      
                }
                foreach (FileInfo file in enviroPath.GetFiles())
                {
                    file.Delete();
                }
                foreach (DirectoryInfo dir in enviroPath.GetDirectories())
                {
                    dir.Delete(true);
                }
                Directory.Delete(enviromentFullPath);
                enviromentFullPath = "";
            }
        }

       
        private bool CheckRemove()
        {
            //remove all 
            runTimer.Stop();
          
            if(removeInProgress)
            {
                try
                {

                    for (int i = 0; i <= (antHills.Count - 1); i++)
                    {
                        for (int b = 0; b <= (antHills[i].AntsTotalProperty - 1); b++)
                        {
                            File.Delete(antHills[i].ants[b].FullPathProperty);
                            antHills[i].ants.Remove(b);
                            txtOutput.Text += Environment.NewLine + "Ant " + b + " removed";



                        }

                        for (int p = 0; p <= (antHills[i].pheromonesPlaced.Count - 1); p++)
                        {
                            File.Delete(antHills[i].pheromonesPlaced[p].FullPathProperty);
                            txtOutput.Text += Environment.NewLine + antHills[i].pheromonesPlaced[p].TypeProperty + " pheromone removed from " + Environment.NewLine + antHills[i].pheromonesPlaced[p].LocationProperty;

                        }
                        antHills[i].pheromonesPlaced.Clear();
                        antHills[i].AntsTotalProperty = 0;
                        Directory.Delete(antHills[i].LocationProperty,true);

                        txtOutput.Text += Environment.NewLine + "Anthill " + i + " removed";
                    }
                    antHills.Clear();
                    lblAntInfo.Text = "";
                    lblAnthillInfo.Text = "";
                    cbxAnt.Items.Clear();
                    cbxAntHill.Items.Clear();
                    cbxAnt.Text = "";
                    cbxAntHill.Text = "";
                    cbxAnt.Enabled = false;
                    txtOutput.Text += Environment.NewLine + "Clean up Complete!";
                    removeInProgress = false;

                }
                catch (NullReferenceException err)
                {

                }

                return true;
            }
            else
            {
                // runTimer = new System.Timers.Timer(timerInterval);
                //runTimer.Elapsed += new ElapsedEventHandler(OnTimerElapsed);
                //runTimer.Start();
                return false;
            }

                

        }

        private void runTimerTick(object sender, EventArgs e)
        {
            runTimer.Stop();
            tickCount++;
            
            
            timeElapsedLabel.Text = "Time Elapsed: " + tickCount;
            //run each ant
           
                for (int i = 0; i <= (antHills.Count - 1); i++)
                {
                    try
                    {
                        if (antHills[i] != null)
                        {
                            antHills[i].CheckStatus();
                            for (int a = 0; a < antHills[i].AntsTotalProperty; a++)
                            {
                                try
                                {
                                    antHills[i].ants[a].Act();
                                }
                                catch (NullReferenceException err)
                                {
                                    //Certain ants that DO exist are throwing null reference exceptions on the act method 
                                    Console.WriteLine(err.Message);

                                }
                            }
                            if (cbxAnt.Text != "" && cbxAnt.Enabled == true)
                            {

                                lblAntInfo.Text = "";
                                lblAntInfo.Text +=
                                "Ant Number " + antNum.ToString("D4") + " info:"
                                    + Environment.NewLine + "Type: " + antHills[anthillNum].ants[antNum].TypeProperty
                                    + Environment.NewLine + "Health: " + antHills[anthillNum].ants[antNum].HealthProperty
                                    + Environment.NewLine + "Hunger: " + antHills[anthillNum].ants[antNum].HungerProperty
                                    + Environment.NewLine + "Age: " + antHills[anthillNum].ants[antNum].AgeProperty
                                    + Environment.NewLine + "State: " + antHills[anthillNum].ants[antNum].StateProperty
                                    + Environment.NewLine + "Carrying: " + antHills[anthillNum].ants[antNum].CarryingProperty
                                    + Environment.NewLine + "In Nest: " + antHills[anthillNum].ants[antNum].InNestProperty
                                    + Environment.NewLine + "Location: " + antHills[anthillNum].ants[antNum].LocationProperty;
                            }
                        }
                    }
                    catch (NullReferenceException err)
                    {
                        //Certain ants that DO exist are throwing null reference exceptions on the act method 
                        Console.WriteLine(err.Message);

                    }

                }
                if (cbxAntHill.Text != "")
                {
                    lblAnthillInfo.Text = "";
                    lblAnthillInfo.Text += "AntHill_" + anthillNum.ToString("D2") + " info:"
                         + Environment.NewLine + "Total Ants: " + antHills[anthillNum].AntsTotalProperty
                        + Environment.NewLine + "Location: " + antHills[anthillNum].LocationProperty;
                }
            

               

               
           

           //if the food generator is running
                if (isGeneratingFood && tickCount > (lastTickCount + 100))
                {
                    txtOutput.Text += Environment.NewLine + "Food was Generated at " + GenerateFood(folderList);
                    
                    lastTickCount = tickCount; 

                }

                if (ckbxAutosave.Checked == true && tickCount > (lastTickCount + 50))
                {
                    UpdateSaveFile(savePath);
                    lastTickCount = tickCount;
                }



            if (!CheckRemove())
            {
                runTimer = new System.Windows.Forms.Timer();
                runTimer.Interval = timerInterval;
                runTimer.Tick -= new EventHandler(runTimerTick);
                runTimer.Tick += new EventHandler(runTimerTick);
                runTimer.Start();
            }
           
           

        }

        private void cleanUpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string message = "This will remove every Anthill and Ant on your computer. Do you wish to continue?";
            string caption = "Warning";
            MessageBoxButtons buttons = MessageBoxButtons.YesNo;
            DialogResult result;

            // Displays the MessageBox.


            result = MessageBox.Show(message, caption, buttons);

            if (result == System.Windows.Forms.DialogResult.Yes && runTimerRunning == true)
            {

                removeInProgress = true;

            }
            else if (result == System.Windows.Forms.DialogResult.Yes && !runTimerRunning)
            {
                removeInProgress = true;
                CheckRemove();
            }

        }

        private void OutputChanged(object sender, EventArgs e)
        {
            txtOutput.SelectionStart = txtOutput.TextLength;
            txtOutput.ScrollToCaret();
        }

        private void btnBrowseEviro_Click(object sender, EventArgs e)
        {
            folderBrowserDialog.ShowNewFolderButton = false;
            DialogResult result = folderBrowserDialog.ShowDialog();

            if (result == DialogResult.OK)
            {

                CreateEnviroment(folderBrowserDialog.SelectedPath);
            }

        }

        private string GenerateFood(string[] folderList)
        {
            //This function will run on ticks and generate food in a specified folder tree.

            //Search directories, pick random folder, generate food source. 
            //Im not sure how to make this process quick, as the time it takes to pick a folder
            //will increase with the size of the tree. I also need to unsure that food is not being generated in an anthill

            /**
             *
             * 
             * The folder list will be generated when the root gets changed.
             * 
             * 
             * get a random number between 0 and the max size of the list
             * generate food at that location
             * 
             * 
             */

            Random rnd = new Random();
            int index = 0;
            string path = "";

            try
            {
                index = rnd.Next(folderList.Length);
                path = folderList[index];
                
            }
            catch (ArgumentException err)
            {
                //if it fails, then somehow no paths were properly added. I NEED TO FIX THIS 
                //this string will be read by the move function
                path = "nopath";
            }

            if (path != "nopath")
            {
                if (!File.Exists(path + @"\DoNotEnter.phe"))
                {
                    
                    if (!File.Exists(path + @"\FoodSource.txt"))
                    {
                        using (StreamWriter sw = File.CreateText(path + @"\FoodSource.txt"))
                        {
                            sw.WriteLine("1000");
                            sw.Close();

                        }
                        

                    }
                }
               //close this file stream
            }

            return path;


        }

        private string[] GetFoodGenFolderList(String rootPath)
        {
            string[] folderArray; 


            //here all the folders will be obtained from a root

            /**
             * 
             * search each folder in a folder array for directories
             */
            folderArray = Directory.GetDirectories(rootPath, "*", SearchOption.AllDirectories);


            return folderArray;

        }

        private void CreateEnviroment(String rootPath)
        {
            Random rnd = new Random();
            int index = 0;
            int cities = 0; //per enviro
            int regions; //per city
            int townships; //per region

            int enviroNum = 0;

            String currentCity;
            String currentRegion;
            String currentTownship;

            if (cbxEnviroSize.SelectedIndex >= 0)
            {
                cities = cbxEnviroSize.SelectedIndex + 1;
            }
            else
            {
                cities = 2;
                cbxEnviroSize.SelectedIndex = 1;

            }

            regions = cities * 2;
            townships = regions * 2;
            //create root folder at dialog path
            while (Directory.Exists(rootPath + @"\AntFarm_Enviroment"+enviroNum.ToString("D2")))
            {
                enviroNum++;
            }
            Directory.CreateDirectory(rootPath + @"\AntFarm_Enviroment" + enviroNum.ToString("D2"));
            using (StreamWriter sw = File.CreateText(rootPath + @"\AntFarm_Enviroment" + enviroNum.ToString("D2") + @"\DoNotEnter.phe"))
            {
                
                sw.Close();
            }
            using (StreamWriter sw = File.CreateText(rootPath + @"\AntFarm_Enviroment" + enviroNum.ToString("D2") + @"\saveData.txt"))
            {

                sw.Close();
            }

   
            
            

            Directory.CreateDirectory(rootPath + @"\AntFarm_Enviroment" + enviroNum.ToString("D2") + @"\Entrance");

            //The size decides on how many citys there will be, how many regions per city, and how many townships per region

            for (int c = 0; c < cities; c++)
            {

                //pick random city name and send it to "current city"
                if (cities > cityNames.Length)
                {
                    index = rnd.Next(cityNames.Length);
                }
                else
                {
                    index = rnd.Next(cities);
                }
                currentCity = cityNames[index];
                //create directory at root

                Directory.CreateDirectory(rootPath + @"\AntFarm_Enviroment" + enviroNum.ToString("D2") + @"\Entrance\" + currentCity);
                for (int r = 0; r < regions; r++)
                {
                    //pick random prefix, then sufix and save to "current region"
                    if (regions > regionPrefixes.Length)
                    {
                        index = rnd.Next(regionPrefixes.Length);
                    }
                    else
                    {
                        index = rnd.Next(regions);
                    }
                    currentRegion = regionPrefixes[index];

                    if (regions > regionSuffixes.Length)
                    {
                        index = rnd.Next(regionSuffixes.Length);
                    }
                    else
                    {
                        index = rnd.Next(regions);
                    }

                    currentRegion += regionSuffixes[index];
                    //create directory at root

                    Directory.CreateDirectory(rootPath + @"\AntFarm_Enviroment" + enviroNum.ToString("D2") + @"\Entrance\" + currentCity + @"\" + currentRegion);


                    for (int t = 0; t < townships; t++)
                    {
                        //pick random prefix, then sufix and save to "current township"  
                        //create directory in region
                        //pick random prefix, then sufix and save to "current region"
                        if (townships > townshipPrefixes.Length)
                        {
                            index = rnd.Next(townshipPrefixes.Length);
                        }
                        else
                        {
                            index = rnd.Next(townships);
                        }
                        currentTownship = townshipPrefixes[index];

                        if (townships > townshipSuffixes.Length)
                        {
                            index = rnd.Next(townshipSuffixes.Length);
                        }
                        else
                        {
                            index = rnd.Next(townships);
                        }

                        currentTownship += townshipSuffixes[index];
                        //create directory at root


                        Directory.CreateDirectory(rootPath + @"\AntFarm_Enviroment" + enviroNum.ToString("D2") + @"\Entrance\" + currentCity + @"\" + currentRegion + @"\" + currentTownship);
                        index = rnd.Next(10);
                        

                        if (ckbxWithFood.Checked && index < 3)
                        {
                            using (StreamWriter sw = File.CreateText(rootPath + @"\AntFarm_Enviroment" + enviroNum.ToString("D2") + @"\Entrance\" + currentCity + @"\" + currentRegion + @"\" + currentTownship + @"\FoodSource.txt"))
                            {

                                sw.WriteLine("1000");
                                sw.Close();
                            }
                        }

                    }

                }


            }
            txtOutput.Text += Environment.NewLine + cbxEnviroSize.Text +" AntFarm enviroment created in " + rootPath;
            this.enviromentFullPath = rootPath + @"\AntFarm_Enviroment" + enviroNum.ToString("D2");
            btnBrowseEviro.Enabled = false;
            cbxEnviroSize.Enabled = false;
            ckbxWithFood.Enabled = false;
            btnBrowseAnthill.Enabled = true;
            savePath = enviromentFullPath + @"\saveData.txt";


            

        }

        private void btnFoodGeneratorBrowse_Click(object sender, EventArgs e)
        {
            folderBrowserDialog.ShowNewFolderButton = false;
            DialogResult result = folderBrowserDialog.ShowDialog();

            if (result == DialogResult.OK)
            {

                foodGenRoot = folderBrowserDialog.SelectedPath;
                lblFoodGenRoot.Text = foodGenRoot;
                folderList = GetFoodGenFolderList(foodGenRoot);
                btnFoodGenStart.Enabled = true;
                btnFoodGenPause.Enabled = false;
            }
        }

        private void btnFoodGenStart_Click(object sender, EventArgs e)
        {
            btnFoodGenStart.Enabled = false;
            btnFoodGenPause.Enabled = true;

            isGeneratingFood = true; 
        }

        private void btnFoodGenPause_Click(object sender, EventArgs e)
        {
            btnFoodGenStart.Enabled = true;
            btnFoodGenPause.Enabled = false;

            isGeneratingFood = false; 
        }


        private void UpdateSaveFile(string filePath)
        {

            //This function saves the all of the stats for the ants and the anthill every X ticks
            //All antfarm related files in the enviroment will remain when the program closes 
            //The purpose is so the scenario can be loaded easily without having to replace every file.
            //as long as the stats are collected, the program will know where to find the files it needs. 

            using (StreamWriter sw = new StreamWriter(filePath))
            {
                sw.WriteLine(this.enviromentFullPath);
                sw.WriteLine("[Enviroment Stats End]");
                sw.WriteLine(antHills[0].LocationProperty);
                sw.WriteLine(antHills[0].IDProperty);
                sw.WriteLine(antHills[0].AntsDiggingProperty);
                sw.WriteLine(antHills[0].AntsMoveingEggsProperty);
                sw.WriteLine(antHills[0].AntsScoutingProperty);
                sw.WriteLine(antHills[0].AntsTendingToBroodProperty);
                sw.WriteLine(antHills[0].AntsTendingToQueenProperty);
                sw.WriteLine("[Anthill Stats End]");

                //loop for chambers
                
                foreach(KeyValuePair<int,Chamber> chamber in antHills[0].chambers)
                {
                    sw.WriteLine("[Anthill Chamber Start]");
                    sw.WriteLine(chamber.Value.IDProperty);
                    sw.WriteLine(chamber.Value.TypeProperty);
                    sw.WriteLine(chamber.Value.LocationProperty);
                    sw.WriteLine(chamber.Value.AmountOfFoodProperty);
                    sw.WriteLine(chamber.Value.AmountOfOffspringProperty);
                    sw.WriteLine(chamber.Value.CapacityReachedProperty);
                    sw.WriteLine("[Anthill Chamber End]");
                }
                sw.WriteLine("[Anthill Chamber Stats End]");


                //loop for pheremones 
                foreach(Pheromone phe in antHills[0].pheromonesPlaced)
                {
                    sw.WriteLine(phe.ColonyIDProperty);
                    sw.WriteLine(phe.FullPathProperty);
                    sw.WriteLine(phe.LocationProperty);
                    sw.WriteLine(phe.StrengthProperty);
                    sw.WriteLine(phe.TypeProperty);
                    sw.WriteLine("[Pheremone End]");
                }
                sw.WriteLine("[Pheremone Stats End]");

                for (int i = 0; i < antHills[0].AntsTotalProperty; i++)
                {
                    sw.WriteLine(antHills[0].ants[i].TypeProperty);
                    sw.WriteLine(antHills[0].ants[i].IDProperty);
                    sw.WriteLine(antHills[0].ants[i].FullPathProperty);
                    sw.WriteLine(antHills[0].ants[i].LocationProperty);
                    sw.WriteLine(antHills[0].ants[i].HungerProperty);
                    sw.WriteLine(antHills[0].ants[i].HealthProperty);
                    sw.WriteLine(antHills[0].ants[i].AgeProperty);
                    sw.WriteLine(antHills[0].ants[i].BeingCarriedProperty);

                    //null value possible
                    if (antHills[0].ants[i].CarriedAntProperty == null)
                    {
                        sw.WriteLine("nullValue");
                        //null value possible
                        sw.WriteLine("nullValue");
                        //null value possible
                        sw.WriteLine("nullValue");

                    }
                    else
                    {
                        sw.WriteLine(antHills[0].ants[i].CarriedAntProperty.IDProperty);
                        //null value possible
                        sw.WriteLine(antHills[0].ants[i].CarriedObjectFullPathProperty);
                        //null value possible
                        sw.WriteLine(antHills[0].ants[i].CarriedObjectLocationProperty);
                    }

                    sw.WriteLine(antHills[0].ants[i].CarryAmountProperty);
                    sw.WriteLine(antHills[0].ants[i].CarryingProperty);
                    sw.WriteLine(antHills[0].ants[i].ColonyIDProperty);
                    sw.WriteLine(antHills[0].ants[i].DestinationProperty);
                    sw.WriteLine(antHills[0].ants[i].InNestProperty);
                    sw.WriteLine(antHills[0].ants[i].NewThroneRoomProperty);
                    sw.WriteLine(antHills[0].ants[i].PheromoneProperty);
                    sw.WriteLine(antHills[0].ants[i].PreviousLocationProperty);
                    sw.WriteLine(antHills[0].ants[i].StateProperty);
                    sw.WriteLine(antHills[0].ants[i].StepCountProperty);

                    sw.WriteLine("[Ant End]"); 
                }
                sw.WriteLine("[Ant Stats End]");
                txtOutput.Text += Environment.NewLine + "SAVE COMPLETE";
                sw.Close();

            }

        }
        

        //passed the enviroment root 
        private void LoadScenario(string saveFilePath)
        {

            //take root folder as selected by user
            //init ant hill
            //store anthill stats. 
            //store pointer in dictionary
            //init ant
            //store stats 
            //store pointer in dictionary
            if (new FileInfo(saveFilePath).Length != 0)
            {

                using (StreamReader sr = new StreamReader(saveFilePath))
                {
                    string line = "";
                    int counter = 0;

                    //A temp value for parsing ints 
                    int temp;
                    bool tempBool;

                    //temp objects
                    AntHill tempAnthill;
                    Chamber tempChamber = new Chamber();
                    Pheromone tempPhe = new Pheromone(0, "", PheromoneEnum.Danger, true);


                    Worker tempWorker = new Worker(0, 0, "", true);
                    Queen tempQueen = new Queen(0, 0, "", true);
                    Egg tempEgg = new Egg(0, 0, "", true);
                    Larvae tempLarvae = new Larvae(0, 0, "", true);
                    Pupae tempPupae = new Pupae(0, 0, "", true);
                    Ant antPointer = tempWorker;

                    while ((line = sr.ReadLine()) != "[Enviroment Stats End]")
                    {
                        counter++;

                        if (counter == 1)
                        {
                            this.enviromentFullPath = line;
                        }
                    }


                    tempAnthill = new AntHill(0, "", false);
                    counter = 0;
                    while ((line = sr.ReadLine()) != "[Anthill Stats End]")
                    {

                        counter++;

                        /*
                         1   sw.WriteLine(antHills[0].LocationProperty);
                         2   sw.WriteLine(antHills[0].IDProperty);
                         3   sw.WriteLine(antHills[0].AntsDiggingProperty);
                         4   sw.WriteLine(antHills[0].AntsMoveingEggsProperty);
                         5   sw.WriteLine(antHills[0].AntsScoutingProperty);
                         6   sw.WriteLine(antHills[0].AntsTendingToBroodProperty);
                         7   sw.WriteLine(antHills[0].AntsTendingToQueenProperty);
                         8   sw.WriteLine(antHills[0].AntsTotalProperty);
                         */

                        if (counter == 1)
                        {
                            tempAnthill.LocationProperty = line;

                        }
                        else if (counter == 2)
                        {

                            int.TryParse(line, out temp);
                            tempAnthill.IDProperty = temp;


                        }
                        else if (counter == 3)
                        {
                            int.TryParse(line, out temp);
                            tempAnthill.AntsDiggingProperty = temp;

                        }
                        else if (counter == 4)
                        {
                            int.TryParse(line, out temp);
                            tempAnthill.AntsMoveingEggsProperty = temp;

                        }
                        else if (counter == 5)
                        {
                            int.TryParse(line, out temp);
                            tempAnthill.AntsScoutingProperty = temp;

                        }
                        else if (counter == 6)
                        {
                            int.TryParse(line, out temp);
                            tempAnthill.AntsTendingToBroodProperty = temp;

                        }
                        else if (counter == 7)
                        {
                            int.TryParse(line, out temp);
                            tempAnthill.AntsTendingToQueenProperty = temp;
                            antHills[0] = tempAnthill;
                            txtOutput.Text += Environment.NewLine + "Anthill" + tempAnthill.IDProperty + " Loaded";

                        }


                    }

                    counter = 0;
                    txtOutput.Text += Environment.NewLine + "Chambers Start: ";
                    //For the rest of the categories, I will have to set up a couple of loops.
                    //One for the category as a whole, and one for each record in that category. 
                    while ((line = sr.ReadLine()) != "[Anthill Chamber Stats End]")
                    {
                        if (line == "[Anthill Chamber End]")
                        {
                            tempChamber = new Chamber();
                            counter = 0;
                        }
                        else
                        {
                            counter++;
                        }

                        /*                   
                        1 sw.WriteLine(antHills[0].chambers[i].IDProperty);
                        2 sw.WriteLine(antHills[0].chambers[i].TypeProperty);
                        3 sw.WriteLine(antHills[0].chambers[i].LocationProperty);
                        4 sw.WriteLine(antHills[0].chambers[i].AmountOfFoodProperty);
                        5 sw.WriteLine(antHills[0].chambers[i].AmountOfOffspringProperty);
                        6 sw.WriteLine(antHills[0].chambers[i].CapacityReachedProperty);
                        */


                        if (counter == 1)
                        {
                            int.TryParse(line, out temp);
                            tempChamber.IDProperty = temp;

                        }
                        else if (counter == 2)
                        {
                            tempChamber.TypeProperty = (ChamberType)Enum.Parse(typeof(ChamberType), line);

                        }
                        else if (counter == 3)
                        {
                            tempChamber.LocationProperty = line;

                        }
                        else if (counter == 4)
                        {
                            int.TryParse(line, out temp);
                            tempChamber.AmountOfFoodProperty = temp;

                        }
                        else if (counter == 5)
                        {
                            int.TryParse(line, out temp);
                            tempChamber.AmountOfOffspringProperty = temp;

                        }
                        else if (counter == 6)
                        {
                            bool.TryParse(line, out tempBool);
                            tempChamber.CapacityReachedProperty = tempBool;
                            txtOutput.Text += Environment.NewLine + "Anthill Chamber" + tempChamber.IDProperty + " Loaded";

                        }

                        //Add to list

                    }

                    while ((line = sr.ReadLine()) != "[Pheremone Stats End]")
                    {
                        if (line == "[Anthill Chamber End]")
                        {
                            tempPhe = new Pheromone(0, "", PheromoneEnum.Danger, true);
                            counter = 0;
                        }
                        else
                        {
                            counter++;
                        }

                        /*
                        1 sw.WriteLine(antHills[0].pheromonesPlaced[i].ColonyIDProperty);
                        2 sw.WriteLine(antHills[0].pheromonesPlaced[i].FullPathProperty);
                        3 sw.WriteLine(antHills[0].pheromonesPlaced[i].LocationProperty);
                        4 sw.WriteLine(antHills[0].pheromonesPlaced[i].StrengthProperty);
                        5 sw.WriteLine(antHills[0].pheromonesPlaced[i].TypeProperty);
                         */

                        if (counter == 1)
                        {
                            int.TryParse(line, out temp);
                            tempPhe.ColonyIDProperty = temp;
                        }
                        else if (counter == 2)
                        {
                            tempPhe.FullPathProperty = line;
                        }
                        else if (counter == 3)
                        {
                            tempPhe.LocationProperty = line;
                        }
                        else if (counter == 4)
                        {
                            int.TryParse(line, out temp);
                            tempPhe.StrengthProperty = temp;
                        }
                        else if (counter == 5)
                        {
                            tempPhe.TypeProperty = (PheromoneEnum)Enum.Parse(typeof(PheromoneEnum), line);
                            txtOutput.Text += Environment.NewLine + "Pheremone" + tempPhe.TypeProperty.ToString() + " Loaded";
                        }

                    }

                    while ((line = sr.ReadLine()) != "[Ant Stats End]")
                    {

                        if (line == "[Ant End]")
                        {
                            counter = 0;
                        }
                        else
                        {
                            counter++;
                        }

                        /*
                        1 sw.WriteLine(antHills[0].ants[i].TypeProperty);
                        2 sw.WriteLine(antHills[0].ants[i].IDProperty);
                        3 sw.WriteLine(antHills[0].ants[i].FullPathProperty);
                        4 sw.WriteLine(antHills[0].ants[i].LocationProperty);
                        5 sw.WriteLine(antHills[0].ants[i].HungerProperty);
                        6 sw.WriteLine(antHills[0].ants[i].HealthProperty);
                        7 sw.WriteLine(antHills[0].ants[i].AgeProperty);
                        8 sw.WriteLine(antHills[0].ants[i].BeingCarriedProperty);
                        9 sw.WriteLine(antHills[0].ants[i].CarriedAntProperty.IDProperty);
                        10 sw.WriteLine(antHills[0].ants[i].CarriedObjectFullPathProperty);
                        11 sw.WriteLine(antHills[0].ants[i].CarriedObjectLocationProperty);
                        12 sw.WriteLine(antHills[0].ants[i].CarryAmountProperty);
                        13 sw.WriteLine(antHills[0].ants[i].CarryingProperty);
                        14 sw.WriteLine(antHills[0].ants[i].ColonyIDProperty);
                        15 sw.WriteLine(antHills[0].ants[i].DestinationProperty);
                        16 sw.WriteLine(antHills[0].ants[i].InNestProperty);
                        17 sw.WriteLine(antHills[0].ants[i].NewThroneRoomProperty);
                        18 sw.WriteLine(antHills[0].ants[i].PheromoneProperty);
                        19 sw.WriteLine(antHills[0].ants[i].PreviousLocationProperty);
                        20 sw.WriteLine(antHills[0].ants[i].StateProperty);
                        21 sw.WriteLine(antHills[0].ants[i].StepCountProperty);

                        */

                        if (counter == 1)
                        {
                            if (line == "Queen")
                            {
                                tempQueen = new Queen(0, 0, "", true);
                                antPointer = tempQueen;
                            }
                            else if (line == "Egg")
                            {
                                tempEgg = new Egg(0, 0, "", true);
                                antPointer = tempEgg;
                            }
                            else if (line == "Larvae")
                            {
                                tempLarvae = new Larvae(0, 0, "", true);
                                antPointer = tempLarvae;
                            }
                            else if (line == "Pupae")
                            {
                                tempPupae = new Pupae(0, 0, "", true);
                                antPointer = tempPupae;
                            }
                            else if (line == "Worker")
                            {
                                tempWorker = new Worker(0, 0, "", true);
                                antPointer = tempWorker;
                            }

                        }
                        else if (counter == 2)
                        {
                            int.TryParse(line, out temp);
                            antPointer.IDProperty = temp;
                        }
                        else if (counter == 3)
                        {
                            antPointer.FullPathProperty = line;
                        }
                        else if (counter == 4)
                        {
                            antPointer.LocationProperty = line;
                        }
                        else if (counter == 5)
                        {
                            int.TryParse(line, out temp);
                            antPointer.HungerProperty = temp;
                        }
                        else if (counter == 6)
                        {
                            int.TryParse(line, out temp);
                            antPointer.HealthProperty = temp;
                        }
                        else if (counter == 7)
                        {
                            int.TryParse(line, out temp);
                            antPointer.AgeProperty = temp;
                        }
                        else if (counter == 8)
                        {
                            bool.TryParse(line, out tempBool);
                            antPointer.BeingCarriedProperty = tempBool;
                        }
                        else if (counter == 9)
                        {
                            //This is an object, so i will have to perhaps store the id of the ant instead, then get that ant.
                            //Or ignore this all together, but thats not good.
                            //I might have to change this property to store the ID anyway, then have the ant look for the 
                            //cooresponding ant by ID.
                            //antPointer.CarriedAntProperty 

                            //what i can do is use the ant dictionary and just point to the ID regardless of if it's a null value.
                            //that ant will get placed in the dictionary anyway at the end of the load. 
                            if (line == "nullValue")
                            {
                                antPointer.CarriedAntProperty = null;
                            }
                            else
                            {
                                int.TryParse(line, out temp);
                                antPointer.CarriedAntProperty = antHills[0].ants[temp];
                            }


                        }
                        else if (counter == 10)
                        {
                            //10 sw.WriteLine(antHills[0].ants[i].CarriedObjectFullPathProperty);
                            if (line != "nullValue")
                            {
                                antPointer.CarriedObjectFullPathProperty = line;
                            }
                        }
                        else if (counter == 11)
                        {
                            //11 sw.WriteLine(antHills[0].ants[i].CarriedObjectLocationProperty);
                            if (line != "nullValue")
                            {
                                antPointer.CarriedObjectLocationProperty = line;
                            }
                        }
                        else if (counter == 12)
                        {
                            //12 sw.WriteLine(antHills[0].ants[i].CarryAmountProperty);
                            int.TryParse(line, out temp);
                            antPointer.CarryAmountProperty = temp;

                        }
                        else if (counter == 13)
                        {
                            //13 sw.WriteLine(antHills[0].ants[i].CarryingProperty);

                            antPointer.CarryingProperty = (Carrying)Enum.Parse(typeof(Carrying), line);

                        }
                        else if (counter == 14)
                        {
                            //14 sw.WriteLine(antHills[0].ants[i].ColonyIDProperty);
                            int.TryParse(line, out temp);
                            antPointer.ColonyIDProperty = temp;

                        }
                        else if (counter == 15)
                        {
                            //15 sw.WriteLine(antHills[0].ants[i].DestinationProperty);
                            antPointer.DestinationProperty = line;

                        }
                        else if (counter == 16)
                        {
                            //16 sw.WriteLine(antHills[0].ants[i].InNestProperty);
                            bool.TryParse(line, out tempBool);
                            antPointer.InNestProperty = tempBool;


                        }
                        else if (counter == 17)
                        {
                            // 17 sw.WriteLine(antHills[0].ants[i].NewThroneRoomProperty);
                            bool.TryParse(line, out tempBool);
                            antPointer.NewThroneRoomProperty = tempBool;


                        }
                        else if (counter == 18)
                        {
                            //18 sw.WriteLine(antHills[0].ants[i].PheromoneProperty);

                            antPointer.PheromoneProperty = (PheromoneEnum)Enum.Parse(typeof(PheromoneEnum), line);


                        }
                        else if (counter == 19)
                        {
                            //19 sw.WriteLine(antHills[0].ants[i].PreviousLocationProperty);
                            antPointer.PreviousLocationProperty = line;

                        }
                        else if (counter == 20)
                        {
                            //20 sw.WriteLine(antHills[0].ants[i].StateProperty);
                            antPointer.StateProperty = (State)Enum.Parse(typeof(State), line);

                        }
                        else if (counter == 21)
                        {
                            //21 sw.WriteLine(antHills[0].ants[i].StepCountProperty);
                            int.TryParse(line, out temp);
                            antPointer.StepCountProperty = temp;

                            antPointer.MyAnthillProperty = antHills[0];
                            antHills[0].ants[antPointer.IDProperty] = antPointer;

                            txtOutput.Text += Environment.NewLine + "Ant" + antPointer.ColonyIDProperty.ToString("D2") + "_" + antPointer.IDProperty.ToString("D4") + " Loaded";
                        }

                        //add ant to dictionary. 
                        //set anthill pointer to anthill[0]
                        //


                    }


                    txtOutput.Text += Environment.NewLine + "LOAD COMPLETE";
                    sr.Close();
                }
       


            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {

            //Create file. Pass file to save function
            if(File.Exists(savePath))
            {
                UpdateSaveFile(savePath);
            }
            
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string saveDataPath = "";
            folderBrowserDialog.ShowNewFolderButton = false;
            DialogResult result = folderBrowserDialog.ShowDialog();

            if (result == DialogResult.OK)
            {

                saveDataPath = folderBrowserDialog.SelectedPath + @"\saveData.txt";
                LoadScenario(saveDataPath);
                setState(FormState.Running);

                btnStart.Enabled = true;
                btnPause.Enabled = false;
            }
        }

        private void tsiNewAntfarm_Click(object sender, EventArgs e)
        {
            setState(FormState.NewScenario);
        }

        private void scenarioToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (antFarmCreated == false && enviromentFullPath != "")
            {
                System.IO.DirectoryInfo enviroPath = new DirectoryInfo(enviromentFullPath);
                if (File.Exists(enviromentFullPath + @"\DoNotEnter.phe"))
                {
                    antHills[0].pheromonesPlaced.Clear();
                }
                foreach (FileInfo file in enviroPath.GetFiles())
                {
                    file.Delete();
                }
                foreach (DirectoryInfo dir in enviroPath.GetDirectories())
                {
                    dir.Delete(true);
                }
                Directory.Delete(enviromentFullPath);
                enviromentFullPath = "";
            }
            else if (antFarmCreated == true)
            {

                UpdateSaveFile(savePath);
                
            }
            setState(FormState.Initial);
        }


        private void setState(FormState state)
        {
            if (state == FormState.Initial)
            {
                FormStateProperty = FormState.Initial;
            }
            else if (state == FormState.NewScenario)
            {
                FormStateProperty = FormState.NewScenario; 
            }
            else if (state == FormState.Running)
            {
                FormStateProperty = FormState.Running;
            }
        }

        private void OnFormStateChanged(FormState state)
        {
            if (state == FormState.Initial)
            {
                btnPause.Enabled = false;
                btnBrowseAnthill.Enabled = false;
                btnBrowseEviro.Enabled = false;
                btnBrowseFoodSource.Enabled = false;
                btnFoodGeneratorBrowse.Enabled = false;
                btnFoodGenPause.Enabled = false;
                btnFoodGenStart.Enabled = false;
                btnPause.Enabled = false;
                btnPrint.Enabled = false;
                btnStart.Enabled = false;

                cbxAnt.Enabled = false;
                cbxAntHill.Enabled = false;
                cbxEnviroSize.Enabled = false;
                cbxSpeed.Enabled = false;

                ckbxAutosave.Enabled = false;
                ckbxWithFood.Enabled = false;

                txtFoodSourceSize.Enabled = false;
                txtOutput.Enabled = false;

                tsiNewAntfarm.Enabled = true;
                openToolStripMenuItem.Enabled = true;
                saveToolStripMenuItem.Enabled = false;
                cleanUpToolStripMenuItem.Enabled = false;
                scenarioToolStripMenuItem.Enabled = false;

                runTimer.Stop();
                statusLabel.Text = "Stopped";
                statusLabel.BackColor = Color.Red;

                lblAntInfo.Text = "";
                lblAnthillInfo.Text = "";
                lblFoodGenLabel.Text = "";
                enviromentFullPath = "";
                antFarmCreated = false;
                txtOutput.Text = "Welcome to AntFarm!";



            }
            else if (state == FormState.NewScenario)
            {
                btnPause.Enabled = false;
                btnBrowseAnthill.Enabled = false;
                btnBrowseEviro.Enabled = true;
                btnBrowseFoodSource.Enabled = false;
                btnFoodGeneratorBrowse.Enabled = false;
                btnFoodGenPause.Enabled = false;
                btnFoodGenStart.Enabled = false;
                btnPause.Enabled = false;
                btnPrint.Enabled = false;
                btnStart.Enabled = false;

                cbxAnt.Enabled = false;
                cbxAntHill.Enabled = false;
                cbxEnviroSize.Enabled = true;
                cbxSpeed.Enabled = false;

                ckbxAutosave.Enabled = false;
                ckbxWithFood.Enabled = true;

                txtFoodSourceSize.Enabled = false;
                txtOutput.Enabled = true;

                tsiNewAntfarm.Enabled = false;
                openToolStripMenuItem.Enabled = false;
                saveToolStripMenuItem.Enabled = true;
                cleanUpToolStripMenuItem.Enabled = true;
                scenarioToolStripMenuItem.Enabled = true;
                enviromentFullPath = "";


            }
            else if (state == FormState.Running)
            {
                btnPause.Enabled = false;
                btnBrowseAnthill.Enabled = false;
                btnBrowseEviro.Enabled = true;
                btnBrowseFoodSource.Enabled = false;
                btnFoodGeneratorBrowse.Enabled = true;
                btnFoodGenPause.Enabled = false;
                btnFoodGenStart.Enabled = false;
                btnPause.Enabled = true;
                btnPrint.Enabled = false;
                btnStart.Enabled = false;

                cbxAnt.Enabled = true;
                cbxAntHill.Enabled = true;
                cbxEnviroSize.Enabled = false;
                cbxSpeed.Enabled = true;

                ckbxAutosave.Enabled = true;
                ckbxWithFood.Enabled = false;

                txtFoodSourceSize.Enabled = true;
                txtOutput.Enabled = true;

                tsiNewAntfarm.Enabled = false;
                openToolStripMenuItem.Enabled = false;
                saveToolStripMenuItem.Enabled = true;
                cleanUpToolStripMenuItem.Enabled = true;
                scenarioToolStripMenuItem.Enabled = true;

            }

        }

        private void updateLocationSelector()
        {
            /*
             * every tick, update the folder tree.
             * 
             */

        }

        private void updateEntitySelector(string folderPath)
        {
            /*
             * When a node is selected, fill list with ants
             * 
             */
        }

        private void CloseAntfarmToolStripItem_Click(object sender, EventArgs e)
        {
            if (FormStateProperty == FormState.Initial)
            {
                Close();
            }
            else if (FormStateProperty == FormState.NewScenario)
            {
                Close();
            }
            else if (FormStateProperty == FormState.Running)
            {
                DialogResult result = MessageBox.Show("A scenario is currently open. Are you sure you want to quit? (progress will be saved)", "Exit Confirmation", MessageBoxButtons.YesNo);

                if (result == System.Windows.Forms.DialogResult.Yes)
                {
                    UpdateSaveFile(savePath);
                    setState(FormState.Initial);
                    Close();

                }
                
            }
            
        }



        





      

       

    
      

      
    }
}
