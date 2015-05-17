using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;


namespace AntFarm
{
    public abstract class Ant
    {
        #region Fields
        //fields
        protected List<String> pheromoneLocations = new List<string>();
        protected String localPheromonePath = "";

        private int health; // The base health of the ant, can be effected by hunger and other external threats. When it reaches 0 the file is deleted 
        private int hunger; //The hunger level of the ant. If this drops to 0, health starts to decrease
        private int carryAmount; //the weight of the object(s) being carried
        private int ID; // a unique id to seperate each ant from its sister
        private int colonyID;
        private int age;
        private int stepCount;
        private bool inNest;
        private bool newThroneRoom;
        private bool beingCarried;
        private AntHill myAnthill; // this is a pointer to the hill the ant belongs too, allowing the ant to access important anthill info
        


        private String location; // the folder path the ant
        private String fullPath; // the file path the ant
        private String destination; //the path of the folder the ant plans to move to
        private String previousLocation; // the path that the ant previously existed at. 
        private String carriedObjectLocation; // the path of the object currently being carried 
        private String carriedObjectFullPath; // the  full path of the object currently being carried 
        private Ant carriedAnt; //Pointer to ant being carried. Not sure if this will work the way I want, but i damn well hope so;

        //enums
        private State currentState; // the current state of the ant, displayed in the file name
        private PheromoneEnum currentPheromone; // the pheremone the ant is set to lay when 'layPheremone' is called
        private Carrying currentlyCarrying; // the type of object the ant is carrying. 
        private AntType type;

        #endregion

        #region constants
        protected const int MATURE_AGE = 80;
        protected const int DEATH_AGE = 80;
        #endregion 

        #region Accessor Methods

        //Basic 
        
        public int HealthProperty
        {
            get
            {
                return health;
            }
            set
            {
                health = value;
            }
        }

        public int HungerProperty
        {
            get
            {
                return hunger;
            }
            set
            {
                hunger = value;
            }
        }

        public int CarryAmountProperty
        {
            get
            {
                return carryAmount;
            }
            set
            {
                carryAmount = value;
            }
        }

        public int IDProperty
        {
            get
            {
                return ID;
            }
            set
            {
                ID = value;
            }
        }

        public int ColonyIDProperty
        {
            get
            {
                return colonyID;
            }
            set
            {
                colonyID = value;
            }
        }

        public int AgeProperty
        {
            get
            {
                return age;
            }
            set
            {
                age = value;
            }
        }

        public bool InNestProperty
        {
            get
            {

                return inNest;
            }

            set
            {
                
                stepCount = 0;
                
                inNest = value;
            }
        }

        public String LocationProperty
        {
            get
            {
                return location;
            }

            set
            {
                location = value;
            }
        }
        public String PreviousLocationProperty
        {
            get
            {
                return previousLocation;
            }

            set
            {
                previousLocation = value;
            }
        }


        public String FullPathProperty
        {
            get
            {
                return fullPath;
            }

            set
            {
                fullPath = value;
            }
        }

        public String DestinationProperty
        {
            get
            {
                return destination;
            }
            set
            {
                destination = value;
            }
        }

        public String CarriedObjectLocationProperty
        {
            get
            {
                return carriedObjectLocation;
            }
            set
            {
                carriedObjectLocation = value;
            }
        }
        public String CarriedObjectFullPathProperty
        {
            get
            {
                return carriedObjectFullPath;
            }
            set
            {
                carriedObjectFullPath = value;
            }
        }
        public State StateProperty
        {
            get
            {
                return currentState;
            }

            set
            {
                currentState = value;

            }
        }

        public PheromoneEnum PheromoneProperty
        {
            get
            {
                return currentPheromone;
            }

            set
            {
                currentPheromone = value;
            }
        }

        public Carrying CarryingProperty
        {
            get
            {
                return currentlyCarrying;
            }

            set
            {
                currentlyCarrying = value;
            }
        }


        public AntType TypeProperty
        {
            get
            {

                return type;
            }

            set
            {
                type = value;
            }
        }

        public int StepCountProperty
        {
            get
            {

                return stepCount;
            }

            set
            {
                stepCount = value;
            }


        }

        public Ant CarriedAntProperty
        {
            get
            {

                return carriedAnt;
            }

            set
            {
                carriedAnt = value;
            }
        }

        public bool NewThroneRoomProperty
        {
            get
            {
                return newThroneRoom;
            }
            set
            {
                newThroneRoom = value;
            }
        }
        public bool BeingCarriedProperty
        {
            get
            {
                return beingCarried;
            }
            set
            {
                beingCarried = value;
            }
        }

        public AntHill MyAnthillProperty
        {
            get
            {
                return myAnthill;
            }
            set
            {
                myAnthill = value;
            }
        }



        #endregion 

        #region Methods
        //Basic ant functionality
        //Every ant class that inherits from here needs these functions.
        public abstract void Act(); // Here the ant decides how to act, and "per-tick" actions take place (ex. hunger drops etc)
        public abstract void Move(); // The ant finds a suitable folder to enter, sets it to 'destination' then moves itself there. 
        protected abstract void Gather(); //food gathering takes up a turn. if the ant has moved 
        protected abstract void Eat(); //eating takes up a turn. It replenishes hunger to full
        protected abstract void Feed(Ant ant, int amount); //eating takes up a turn. It replenishes hunger to full
        protected abstract void PickUp(Ant ant); //the ant picks up and object. Picking up an object means that the object moves when this ant does
        protected abstract void PutDown(); // the link between this ant and the object it was carrying is severed
        protected abstract void Chirp(); //the ant emits a chirp, alerting closeby ants to its location
        protected abstract void LayPheromone(String pheromoneType, String directoryName, Pheromone placedPheromone);// the ant strengthens a pheremone or, if its a queen, lays a pheremone to direct other ants. it finds a pheremone file and sends the pheremone type to the file.


        protected void Update()
        {
            if (TypeProperty != AntType.Egg)
            {
                //heal the ant if properly fed
                if (HungerProperty > 800)
                {
                    HealthProperty++;
                }
                else if (HungerProperty == 0)
                {
                    //hurt the ant if not
                    HealthProperty--;
                }

                HungerProperty--;
            }
            AgeProperty++;

            if (File.Exists(FullPathProperty))
            {

                using (StreamWriter sw = File.CreateText(FullPathProperty))
                {
                    sw.WriteLine("Type:" + this.TypeProperty);
                    sw.WriteLine("Health:" + this.HealthProperty);
                    sw.WriteLine("Hunger:" + this.HungerProperty);
                    sw.WriteLine("Age:" + this.AgeProperty);
                    sw.Close();
                }
            }
            else
            {
                MyAnthillProperty.ants[this.IDProperty] = null;
            }
        }

        protected void UpdateFileName()
        {
            String newName = @"\" + TypeProperty + "_" + ColonyIDProperty.ToString("D2") + "_" + IDProperty.ToString("D4") + "_" + StateProperty + ".ant";
            System.IO.File.Move(FullPathProperty, LocationProperty + newName);
            FullPathProperty = (LocationProperty + newName);
        }

        protected void SearchForPheromones(String pheromoneType)
        {
            pheromoneLocations = new List<string>();
            //get string array of directory paths.
            string[] folders;
            string directoryName = Path.GetFileName(Path.GetDirectoryName(FullPathProperty));
            String parentPath = "";
            parentPath = new FileInfo(LocationProperty).Directory.FullName;
            folders = Directory.GetDirectories(LocationProperty);

            //get directory path of previous folder. If the folder being checked is that folder, dont include in the list. 

            

            if (folders.Length != 0)
            {
                for (int i = 0; i < folders.Length; i++)
                {
                    if (folders[i] != PreviousLocationProperty)
                    {
                        if (!File.Exists(folders[i] + @"\DoNotEnter.phe")) //the the user placed "DoNotEnter" Pheromone file exists in a folder, the ant will refuse to enter it.
                        {
                            if (i <= folders.Length && File.Exists(folders[i] + @"\" + pheromoneType + "_" + ColonyIDProperty.ToString("D2") + ".phe"))//File.Exists(folders[i] + pheromoneType + ".phe")
                            {
                                //add location to list

                                pheromoneLocations.Add(folders[i] + @"\" + pheromoneType + "_" + ColonyIDProperty.ToString("D2") + ".phe");


                            }
                        }
                    }

                }
            }

            if (!File.Exists(parentPath + @"\DoNotEnter.phe")) //if the user placed "DoNotEnter" Pheromone file exists in a folder, the ant will refuse to enter it.
            {
                if (File.Exists(parentPath + @"\" + pheromoneType + "_" + ColonyIDProperty.ToString("D2") + ".phe") && parentPath != PreviousLocationProperty)//File.Exists(folders[i] + pheromoneType + ".phe")
                {
                    //This is a fail safe to prevent ants from leaving the nest in search of inNest tasks without explicitly being told. 
                    if (!InNestProperty)
                    {
                        //add location to list
                        pheromoneLocations.Add(parentPath + @"\" + pheromoneType + "_" + ColonyIDProperty.ToString("D2") + ".phe");
                    }
                    else
                    {
                        if (directoryName != "Anthill_" + ColonyIDProperty.ToString("D2"))
                        {
                            //add location to list
                            pheromoneLocations.Add(parentPath + @"\" + pheromoneType + "_" + ColonyIDProperty.ToString("D2") + ".phe");
                        }
                    }
                        
                

                }
            }
           
             
            
        }

        protected void SearchForLocalPheromones(String pheromoneType)
        {
            localPheromonePath = "";
            if (File.Exists(LocationProperty + @"\" + pheromoneType + "_" + ColonyIDProperty.ToString("D2") + ".phe"))//File.Exists(folders[i] + pheromoneType + ".phe")
            {
                //add location to list

                localPheromonePath = LocationProperty + @"\" + pheromoneType + "_" + ColonyIDProperty.ToString("D2") + ".phe";

            }
         


        }

        protected bool SearchForLocalFood()
        {

            if (File.Exists(LocationProperty + @"\FoodSource.txt"))
            {
             

                return true;

            }
            else
            {
                return false;
            }



        }
        protected bool SearchForLocalNest()
        {

            if (Directory.Exists(LocationProperty + @"\Anthill_" + ColonyIDProperty.ToString("D2")))
            {
                

                return true;

            }
            else
            {
                return false;
            }



        }

        protected bool SearchForLocalFoodStash()
        {

            if (File.Exists(LocationProperty + @"\FoodStash_" + ColonyIDProperty.ToString("D2") + ".txt"))
            {
                return true;
            }
            else
            {
                return false;
            }



        }
        protected void IncreaseLocalPheromoneStrength(String location)
        {
            int currentStrength = 0;
            if (localPheromonePath.Length > 0)
            {
                using (StreamReader sr = new StreamReader(location))
                {
                    String line = sr.ReadToEnd();

                    int.TryParse(line, out currentStrength);

                    if (currentStrength < Pheromone.MAX)
                    {
                        currentStrength += Pheromone.MAX;
                    }
                    sr.Close();
                    
                }

                File.WriteAllText(location, String.Empty);
                using (StreamWriter sw = File.CreateText(location))
                {
                    sw.WriteLine(currentStrength);
                    sw.Close();
                }
            }
        }

        protected String FindStrongestPheromone(List<string> pheromoneList)
        {
            String strongestPheromonePath = "";
            int strength = 0;
            int strongest = 0;

            //BUG: I need something to help decide if there are pheromones that are the same strength 
            for (int i = 1; i < pheromoneList.Count(); i++)
            {
                using (StreamReader sr = new StreamReader(pheromoneList[i]))
                {
                    String line = sr.ReadToEnd();

                    int.TryParse(line, out strength);

                    if (strength > strongest)
                    {
                        strongest = strength;
                        string lastFolderName = new FileInfo(pheromoneList[i]).Directory.FullName;
                        
                        strongestPheromonePath = lastFolderName;
                    }
                    
                    
                    sr.Close();

                }
            }

            return strongestPheromonePath;
        }

        protected String GetRandomPath()
        {
            //for some reason this is returning blank
            String path = "";
            Random rnd = new Random();
            int index = 0;
            List<string> folderList = new List<string>();
            string[] folders;
            string directoryName = Path.GetFileName(Path.GetDirectoryName(FullPathProperty));
            String parentPath = "";
            parentPath = new FileInfo(LocationProperty).Directory.FullName;
            folders = Directory.GetDirectories(LocationProperty);

            //If there are folders at the current directory, add them to the list of possible Destinations 
            if (folders.Length != 0)
            {
                for (int i = 0; i < folders.Length; i++)
                {
                    //if the folder is not the previous location of the ant 
                    if (folders[i] != PreviousLocationProperty)
                    {
                        //if the do not enter phermone does not exist at the potential destination
                        if (!File.Exists(folders[i] + @"\DoNotEnter.phe")) //the the user placed "DoNotEnter" Pheromone file exists in a folder, the ant will refuse to enter it.
                        {
                            //if the ant is not in the nest, it has to check if one of the folders is its anthill
                            if (InNestProperty == false)
                            {
                                
                                if (folders[i] != (LocationProperty + "Anthill_" + ColonyIDProperty.ToString("D2")))
                                {
                                    //if the folder in question is not the anthill
                                    folderList.Add(folders[i]);

                                }
                            }
                            else
                            {
                                //the ant is in the nest.
                                folderList.Add(folders[i]);
                            }
                        }
                    }
                }
            }

            //the goal of this logic to to determine why the parent path cannot be added to the list
            //if the ant is within the nest 
            if (InNestProperty)
            {
                //make sure it is not leaving the nest
                if (directoryName != "Anthill_" + ColonyIDProperty.ToString("D2"))
                {
                    if (parentPath != PreviousLocationProperty)
                    {
                        //the parent path is not the previous location
                        if (!File.Exists(parentPath + @"\DoNotEnter.phe")) //check if the the user placed "DoNotEnter" Pheromone file exists in a folder
                        {
                            //add
                            folderList.Add(parentPath);
                        }
                    }
                    else
                    {
                        //if the ant just came from the parent, check to see if there are other folders to enter
                        if (folderList.Count == 0)
                        {
                            //if not, then the ant is at a dead end and should add the parent path
                            //check if there is not DONOTENTER
                            if (!File.Exists(parentPath + @"\DoNotEnter.phe")) //check if the the user placed "DoNotEnter" Pheromone file exists in a folder
                            {
                                //add
                                folderList.Add(parentPath);
                            }
                        }
                    }
                }
            }
            else
            {
                if (parentPath != PreviousLocationProperty)
                {
                    //the parent path is not the previous location
                    if (!File.Exists(parentPath + @"\DoNotEnter.phe")) //check if the the user placed "DoNotEnter" Pheromone file exists in a folder
                    {
                        //add
                        folderList.Add(parentPath);
                    }
                }
                else
                {
                    //if the ant just came from the parent, check to see if there are other folders to enter
                    if (folderList.Count == 0)
                    {
                        //if not, then the ant is at a dead end and should add the parent path
                        //check if there is not DONOTENTER
                        if (!File.Exists(parentPath + @"\DoNotEnter.phe")) //check if the the user placed "DoNotEnter" Pheromone file exists in a folder
                        {
                            //add
                            folderList.Add(parentPath);
                        }
                    }
                }
            }
           
            //Attempt to choose a random path from the list 
            try
            {
                index = rnd.Next(folderList.Count);

                path = folderList[index];
            }
            catch (ArgumentException err)
            {
                //if it fails, then somehow no paths were properly added. I NEED TO FIX THIS 
                //this string will be read by the move function
                path = "nopath";
            }
            return path;
        }


        protected void FlagConstructionJob(String chamberType)
        {
            //this little if prevents new flags from being listed if the job is already being done, or if the job exists and has yet to be picked up
            //this prevents multiple ants flagging for the same job and piling up flags for chambers already being built or already in demand.
            //if the chamber is still in demand after construction is done, then the flag can be resubmitted. 
            if (!MyAnthillProperty.jobFlags.Contains(chamberType) && !MyAnthillProperty.jobFlags.Contains(chamberType + "_UnderConstruction"))
            {
                MyAnthillProperty.jobFlags.Add(chamberType);
            }

        }
        #endregion

        #region Constructor

        protected Ant(int health, int hunger, int id, int colonyID, int age, String location, AntType type)
        {
            HealthProperty = health;
            HungerProperty = hunger;
            IDProperty = id;
            ColonyIDProperty = colonyID;
            AgeProperty = age;
            LocationProperty = location;
            CarryingProperty = Carrying.Nothing;
            StateProperty = State.Standby;
            PheromoneProperty = PheromoneEnum.ToNest;
            InNestProperty = true;
            TypeProperty = type;
            BeingCarriedProperty = false;
            MyAnthillProperty = AntFarmForm.antHills[this.ColonyIDProperty];


            if (TypeProperty == AntType.Egg || TypeProperty == AntType.Queen)
            {
                MyAnthillProperty.AntsTotalProperty += 1;
            }
        }
        #endregion 

        #region Events

        
        #endregion

    }
}
