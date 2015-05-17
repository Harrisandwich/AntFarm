using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;

/* 
 * A worker ant is responsible for collecting food, creating tunnels, creating chambers, tending to the young, and tending to the queen.
 */ 

namespace AntFarm
{
    public class Worker : Ant
    {
        
        #region Fields

        //Constants
        private const int WORKER_START_HEALTH = 1000;
        private const int WORKER_START_HUNGER = 1000;
        private const int FOOD_CONVERSION_RATIO = 10;
        private const int CARRYING_CAPACITY = 100;

        //digging variables 
        private int digPhase;
        private String digTask;
        private int stepsRequired;

        private Chamber currentChamber;

        //Tending to queen variables
        private int queenTendingPhase = 0;

        //Tending to offspring variables
        private int offspringTendingPhase = 0; 

        string[] dirSplit;
        string chamber; 


        #endregion

        #region Constructor
        public Worker(int id, int colonyID, String location, bool loaded = false)
            : base(WORKER_START_HEALTH, WORKER_START_HUNGER, id, colonyID, 0, location, AntType.Worker)
        {
           

            //Basic Properties
            this.LocationProperty = location;
            this.FullPathProperty = LocationProperty + @"\" + TypeProperty + "_" + ColonyIDProperty.ToString("D2") + "_" + IDProperty.ToString("D4") + "_" + StateProperty + ".ant";
            this.StateProperty = State.Scouting;

            //delete old pupae from colony array
            if (!File.Exists(FullPathProperty) && loaded == false)
            {
                // Create a file to write to. 
                using (StreamWriter sw = File.CreateText(FullPathProperty))
                {
                    sw.WriteLine("Type:" + this.TypeProperty);
                    sw.WriteLine("Health:" + this.HealthProperty);
                    sw.Close();
                }
            }

            //Search dictionary for null space
            for (int a = 0; a < AntFarmForm.antHills[this.ColonyIDProperty].ants.Count; a++)
            {
                if (MyAnthillProperty.ants[a] != null && MyAnthillProperty.ants[a].IDProperty == id && MyAnthillProperty.ants[a].TypeProperty == AntType.Pupae)
                {
                    MyAnthillProperty.ants[a] = this;

                }
            }
        }

        #endregion

        #region Methods
        override public void Act()// Here the ant decides how to act, and "per-tick" actions take place (ex. hunger drops etc)
        {
            Pheromone placedPheromone = null;
            string directoryName = Path.GetFileName(Path.GetDirectoryName(FullPathProperty));
            pheromoneLocations.Clear();
            DestinationProperty = "";

            

            Update();

            /*

             * Split logic into two major chains
             * InNest logic and !InNest logic.
             *
             * The reason behind this split is due to the fact that different things should happen in the nest compared to outside
             * for example:
             *      if the ant has food and is OUTSIDE the nest, she must find her way back by following "ToNest" pheromones
             *      
             *      else if the ant is INSIDE the nest, the ant must find her way to the food stashes and back to the entrance
             *      following the "ToFoodStash" and "ToEntrance" pheromones.
             *      
             *      also, ants that have no task inside the nest must be in a "Scouting" State. Outside the nest they would look for food
             *      but INSIDE the nest they would have to find the entrance to the nest in order to leave.
             *      
             * thus i have split the logic into two function calls to InNest and !InNest logic.
             * The key to these seperate logic groups is that i can also easily identify when the ant has entered or left the nest
             * allowing me to track its InNest state. 
            */
            if(!InNestProperty)
            {

                OutOfNestLogic(placedPheromone, directoryName);
            }
            else
            {
                dirSplit = Regex.Split(directoryName, "_");
                chamber = dirSplit[0];
                if (chamber != "Anthill")
                {
                    try
                    {
                        currentChamber = MyAnthillProperty.chambers[int.Parse(dirSplit[1])];
                    }
                    catch (KeyNotFoundException err)
                    { }


                }
                InNestLogic(placedPheromone, directoryName);
            }

            

        }

        #region Decision Making

        /**
         * 
         * Below is the "In Nest" Logic. This handles all decision making by ants that are inside the nest. 
         * As opposed to out of the nest, different conditions apply for similer circumstances.
         */

        //Make ajustments to allow for "ToEntrance" Pheromones to be detected and created. 
        private void InNestLogic(Pheromone placedPheromone, String directoryName)
        {
            if (StateProperty == State.Standby)
            {
                //all ant task checks are taken care of by the anthill class for this ant. 
                //so i guess if an ant is on standby it will be given a task, but ill leave this if open just in case. 
                
            }
            else if (StateProperty == State.Scouting && CarryingProperty == Carrying.Nothing)
            {
                queenTendingPhase = 0;
                offspringTendingPhase = 0;
                //if they are not currently at the entrance to the anthill
                //Check to make sure no other tasks are needed.

                if (directoryName != "Anthill_" + ColonyIDProperty.ToString("D2"))
                {
                    //go up 
                    string parentPath = new FileInfo(LocationProperty).Directory.FullName;

                    DestinationProperty = parentPath;
                    Move();

                }
                else
                {
                    //else they are
                    //leave the nest
                    string parentPath = new FileInfo(LocationProperty).Directory.FullName;

                    DestinationProperty = parentPath;
                    Move();
                    InNestProperty = false;



                }
               
                
            }
            else if (StateProperty == State.Scouting && CarryingProperty == Carrying.Food)
            {
                queenTendingPhase = 0;
                offspringTendingPhase = 0;
                //This is just for the sake of a possible bug
                //Some scouting ants may still think they are carrying food. 
                //if they do think they are carrying food, they double check the amount and change the state accordingly 
                if (CarryAmountProperty > 0)
                {
                    //The scout is still carrying food
                    //It reaturns to a gathering state
                    StateProperty = State.Gathering;

                }
                else
                {
                    //no food being carried
                    //corrects the carrying property 
                    CarryingProperty = Carrying.Nothing;
                }

            }
            else if (StateProperty == State.Gathering && CarryingProperty == Carrying.Food)
            {
                Console.WriteLine("Ant ID:" + this.IDProperty);
                queenTendingPhase = 0;
                offspringTendingPhase = 0;
                //A group of strings used for parsing location data
                string[] folders = Directory.GetDirectories(LocationProperty);
                //

                if (currentChamber.TypeProperty == ChamberType.FoodStash)
                {
                    if (currentChamber.CapacityReachedProperty)
                    {
                        PreviousLocationProperty = "";
                        SearchForPheromones("ToFoodStash");
                        Move();
                    }
                    else
                    {
                        CarryingProperty = Carrying.Nothing;
                        PreviousLocationProperty = "";
                        DepositFood(CarryAmountProperty);
                    }
                }
                else
                {
                    if (folders.Length == 0)
                    {
                        if (chamber == "Anthill")
                        {
                            FlagConstructionJob("FoodStash");
                            CarryingProperty = Carrying.Nothing;
                            PreviousLocationProperty = "";
                            DepositFood(CarryAmountProperty);
                        }
                        else
                        {
                            PreviousLocationProperty = "";
                            Move();
                        }
                    }
                    else
                    {
                        SearchForPheromones("ToFoodStash");
                        Move();
                    }


                }
                



                //if (folders.Length == 0 && directoryName == "Anthill_" + ColonyIDProperty.ToString("D2"))
                //{
                //    //there are no tunnels and the ant is just within the main entrance of the nest
                //    //(typical case in an early nest, though may become incredibly rare once there are other workers to dig tunnels
                //    //The ant disposites the food into a makeshift food stash

                //    //the ant should flag a digging job
                //    FlagConstructionJob("FoodStash");
                //    CarryingProperty = Carrying.Nothing;
                //    PreviousLocationProperty = "";

                //    DepositFood(CarryAmountProperty);
                //    StepCountProperty = 0;
                //}
                //else if (folders.Length == 0  && chamber != "FoodStash")
                //{
                //    //there are no tunnels and the ant is not at the entrance. This means the ant has reached a dead end
                //    //and should backtrack
                //    PreviousLocationProperty = "";
                //    Move();
                //    StepCountProperty += 1;
                //}
                //else if(chamber == "FoodStash")
                //{
                //    //the ant is in the food stash, drops off food
                //    //NOTE: Should adjust to have the ant flag for a new stash if this one is almost "full"
                //    if (currentChamber.TypeProperty == ChamberType.FoodStash && !currentChamber.CapacityReachedProperty)
                //    {
                //        CarryingProperty = Carrying.Nothing;
                //        PreviousLocationProperty = "";
                //        DepositFood(CarryAmountProperty);
                //        StepCountProperty = 0;
                //    }
                //    else
                //    {
                //        StepCountProperty += 1;
                //        SearchForPheromones("ToFoodStash");
                //        Move();
                //    }
                //}
                //if (StepCountProperty > MyAnthillProperty.chambers.Count && chamber != "FoodStash")
                //{
                //    FlagConstructionJob("FoodStash");
                //    SearchForPheromones("ToFoodStash");
                //    Move();
                //    StepCountProperty = 0;
                //}
                //else
                //{
                //    //Keep looking for a foodstash
                //    StepCountProperty += 1;
                //    SearchForPheromones("ToFoodStash");
                //    Move();
                //}


                //**NOTE: Maybe foodstash objects should automatically project a food smell a couple folders out to guide ants who have no pheromones to follow.**
            }
            else if (StateProperty == State.Gathering && CarryingProperty == Carrying.Nothing)
            {
                queenTendingPhase = 0;
                offspringTendingPhase = 0;
                //search for entrance to the nest, leaving a foodstash pheromone behind them to guide others.
                LayPheromone("ToFoodStash", directoryName, placedPheromone);

                Console.WriteLine("Ant ID:" + this.IDProperty);
                //if they are not at the entrance to the anthill
                if (directoryName != "Anthill_" + ColonyIDProperty.ToString("D2"))
                {
                    //look for pheromones to follow
                    string parentPath = new FileInfo(LocationProperty).Directory.FullName;

                    DestinationProperty = parentPath;
                    Move();

                }
                else
                {
                    //else they are
                    //leave the nest
                    string parentPath = new FileInfo(LocationProperty).Directory.FullName;

                    DestinationProperty = parentPath;
                    Move();
                    InNestProperty = false;
                    StateProperty = State.Scouting;

                }
                
               
                



            }
            else if (StateProperty == State.TendingToQueen && CarryingProperty == Carrying.Nothing)
            {
                offspringTendingPhase = 0;
                /*
                 * this ant has a few responsabilities. Mostly the same to the ants taking care of the offspring.
                 * 
                 * The ant needs to check if the queen is hungry by seeing if she laid a pheromone to alert that.
                 * then the ant needs to feed the queen. 
                 * 
                 * If the size of the colony has reached certain thresholds a new, deeper throne room should be made
                 * The queen is in charge of flagging that job. And then the ant must carry the queen to a new throne room.
                 * When the ant is moving the Queen, it should change the folder name to a Nursery. This will insure that
                 * other ants tending to the queen leave in search of the new throne room. 
                 * 
                 * Otherwise, this ant does not move from the throne room except if food stores in the throne room are low.
                 * 
                 * 
                 * 
                 */ 

                //This ant should have a few different "task phases". 

                //0 Basic tending: The ant checks the queen, the food stash, and the name of the folder.
                if (queenTendingPhase == 0)
                {
                   


                //if the name of the folder changes, set task to "Find new throne room" 
                    if (chamber != "ThroneRoom")
                    {
                        queenTendingPhase = 2;
                    }
                        
                    //Check for food stash
                        //if the food stash exists
                            //if low, set to 1
                        //else
                            //set to 1
                    else if (File.Exists(LocationProperty + @"\FoodStash_" + ColonyIDProperty.ToString("D2")))
                    {
                        //If the queen is hungry, feed her
                        if (MyAnthillProperty.ants[0].HungerProperty < 800)
                        {
                            Feed(MyAnthillProperty.ants[0], 200);
                            
                            int foodAmount = 0;
                            if (File.Exists(LocationProperty + @"\FoodStash_" + ColonyIDProperty.ToString("D2")))
                            {
                                using (StreamReader sr = new StreamReader(LocationProperty + @"\FoodStash_" + ColonyIDProperty.ToString("D2")))
                                {
                                    String line = sr.ReadToEnd();

                                    int.TryParse(line, out foodAmount);

                                    if (foodAmount < 200)
                                    {
                                        queenTendingPhase = 1;
                                    }
                                    sr.Close();
                                }
                            }
                            else
                            {
                                queenTendingPhase = 1;
                            }

                            
                        }
                    }
                    else if (!File.Exists(LocationProperty + @"\FoodStash_" + ColonyIDProperty.ToString("D2")))
                    {
                        queenTendingPhase = 1;
                    }
                    else
                    {
                        queenTendingPhase = 0;
                    }

                }
                //1 Retreive food:
                //search for food store, pick up food. The carrying property will change so the rest will be handled elsewhere
                else if (queenTendingPhase == 1)
                {
                    if (chamber != "FoodStash")
                    {
                        //Search for a food stash, leaving a trail behind you.
                        LayPheromone("ToThroneRoom", directoryName, placedPheromone);
                        SearchForPheromones("ToFoodStash");
                        Move();
                    }
                    else
                    {
                        if (File.Exists(LocationProperty + @"\FoodStash_" + ColonyIDProperty.ToString("D2")))
                        {
                            GatherFromFoodStash();
                        }
                        else
                        {
                            //I have to prevent the ant from leaving the nest to search 
                            PreviousLocationProperty = "";
                            Move();
                        }
                    }
                }
                //2 Find new throne room phase 1: The ant notices that there is a new throne room. They leave to search for it. When found, goto phase 2
                else if (queenTendingPhase == 2)
                {
                    if (chamber != "ThroneRoom")
                    {
                        //Search for a food stash, leaving a trail behind.
                        //I have to prevent the ant from leaving the nest to search 
                        LayPheromone("ToQueen", directoryName, placedPheromone);
                        SearchForPheromones("ToThroneRoom");
                        Move();
                    }
                    else
                    {
                        if (MyAnthillProperty.ants[0].LocationProperty == this.LocationProperty)
                        {
                           //The ant has found the new throne room, but the queen has already been moved by another ant. The ant resumes regular duties. 
                            queenTendingPhase = 0;
                        }
                        else
                        {
                            //The ant found an empty throne room. Time to return to the queen 
                            queenTendingPhase = 3;
                            
                        }
                    }
                }
                //3 Find new throne room phase 2: Return to the queen, leaving a pheromone trail to the new throne room. Once the ant returns, change carrying state to "Queen"
                else if (queenTendingPhase == 3)
                {
                    if (MyAnthillProperty.ants[0].LocationProperty == this.LocationProperty && MyAnthillProperty.ants[0].BeingCarriedProperty == false && chamber != "ThroneRoom")
                    {
                        PreviousLocationProperty = "";
                        PickUp(MyAnthillProperty.ants[0]);
                        CarryingProperty = Carrying.Queen;
                        SearchForPheromones("ToThroneRoom");
                        Move();
                    }
                    else if (MyAnthillProperty.ants[0].LocationProperty == this.LocationProperty && MyAnthillProperty.ants[0].BeingCarriedProperty == false && chamber == "ThroneRoom")
                    {
                        queenTendingPhase = 0;
                    }
                    else
                    {
                        LayPheromone("ToThroneRoom", directoryName, placedPheromone);
                        SearchForPheromones("ToQueen");
                        Move();
                    }
                }


            }
            else if (StateProperty == State.TendingToQueen && CarryingProperty == Carrying.Food)
            {
                offspringTendingPhase = 0;
                //the ant is going to take food back to the throne room.
                if (chamber != "ThroneRoom")
                {

                    LayPheromone("ToFoodStash", directoryName, placedPheromone);
                    SearchForPheromones("ToThroneRoom");
                    Move();
                }
                else
                {
                    DepositFood(CarryAmountProperty);
                    CarryingProperty = Carrying.Nothing;
                    queenTendingPhase = 0;
                }
            }
            else if (StateProperty == State.TendingToQueen && CarryingProperty == Carrying.Queen)
            {
                offspringTendingPhase = 0;
                /*
                 * This ant is responisble with moving the queen to the new throne room. 
                 * The ant will wander around semi randomly(maybe aided somehow to prevent it from getting lost)
                 * carrying the queen until it reaches the new throne room and drops off the queen
                 */
                if (chamber != "ThroneRoom")
                {
                    SearchForPheromones("ToThroneRoom");
                    Move();
                }
                else
                {
                    PutDown();
                    CarryingProperty = Carrying.Nothing;
                    queenTendingPhase = 0;
                }
            }
            else if (StateProperty == State.TendingToEggs && CarryingProperty == Carrying.Nothing)
            {
                queenTendingPhase = 0;
                /* the ant is just simply tending to the eggs. Its behaviour should be:
                 * 
                 * it should move around the nest from nursery to nursery (if there are multiple) 
                 * checking that all the larvae and pupae are well fed(possibly by smelling pheromones), and if need be, move offspring to new locations
                 * if a nursery is overcrowded.
                 * 
                 * They also need to habitually check the throne room for new eggs to move to the nurseries. 
                 * 
                 * if a nusery is overcrowded and there are no more to take the offspring too, flag a digging job. 
                 * NOTE: What i might do for the flagging idea, is to have the ant discover that a nursery is overcrowded. 
                 * Once that happens, set a counter to tick down the number of steps it takes to find a new nursery(based off the size of the colony).
                 * If the ant passes the threshold for the number of steps it should take to find a new nursery, then flag for a new one. 
                 * This method could also be used for ants looking to drop off food. 
                 * 
                 * This ensures that the ant isnt flagging for a nursery every single time they enter a crowded nursery
                 *
                 * 
                 */

                /**
                 * This will work in a similar way to the queen, with phases that dictate what disicions are to be made.
                 * 
                 * 0 Patrol: This is the basic functionallity. The ant explores the nest, maybe at random, looking for nurserys or the throne room. 
                 * 
                 * 1 check:check When it arrives at one, it does a check of all of the hunger of the offspring, and the number of offspring present.
                 * If the offspring are hungery, feed them. if the nursery is crowded, start moving offspring elsewhere. 
                 * 
                 * 2 Retreive food: if the ant cant find a foodstash in the nursery, they will go find one. 
                 * 
                 * 3 Find a new nursery phase 1: similar to the same phase in queen tending, the ant goes to find a new nursery that has space
                 * 
                 * 4 Find a new nursery phase 1: the ant then returns to the nursery it left to retreive an offspring. The rest is taken care of in another area
                 */
                //patrol
                if (offspringTendingPhase == 0)
                {
                    if (chamber != "Nursery")
                    {
                        LayPheromone("ToNursery", directoryName, placedPheromone);
                        SearchForPheromones("ToNursery");
                        Move();
                    }
                    else
                    {
                        //nursery entered. check state
                        offspringTendingPhase = 1;
                    }

                }
                else if (offspringTendingPhase == 1)
                {

                    
                    //If the nursery is overcrowded
                    if (currentChamber.CapacityReachedProperty)
                    {
                        //move first
                        LayPheromone("ToNursery", directoryName, placedPheromone);
                        SearchForPheromones("ToNursery");
                        Move();
                        //then set phase to 2
                        offspringTendingPhase = 2;

                    }
                    //Check for food stash
 
                    else if (File.Exists(LocationProperty + @"\FoodStash_" + ColonyIDProperty.ToString("D2")))
                    {
                        //need to check the hunger of ants in this nursery
                        Ant target;
                        
                        target = CheckForLocalHungryOffspring(LocationProperty);

                        ////If the queen is hungry, feed her
                        if (target != null)
                        {
                            Feed(target, 200);

                            int foodAmount = 0;
                            if (File.Exists(LocationProperty + @"\FoodStash_" + ColonyIDProperty.ToString("D2")))
                            {
                                using (StreamReader sr = new StreamReader(LocationProperty + @"\FoodStash_" + ColonyIDProperty.ToString("D2")))
                                {
                                    String line = sr.ReadToEnd();

                                    int.TryParse(line, out foodAmount);

                                    if (foodAmount < 400)
                                    {
                                        offspringTendingPhase = 0;
                                        LayPheromone("ToNursery", directoryName, placedPheromone);
                                        SearchForPheromones("ToNursery");
                                        Move();
                                    }
                                    sr.Close();
                                }
                            }


                        }
                        else
                        {
                            offspringTendingPhase = 0;
                            LayPheromone("ToNursery", directoryName, placedPheromone);
                            SearchForPheromones("ToNursery");
                            Move();
                        }
                        
                       
                           
                       
                    }
                    else if (!File.Exists(LocationProperty + @"\FoodStash_" + ColonyIDProperty.ToString("D2")))
                    {
                        offspringTendingPhase = 2;
                    }
                    else
                    {
                        offspringTendingPhase = 0;
                        LayPheromone("ToNursery", directoryName, placedPheromone);
                        SearchForPheromones("ToNursery");
                        Move();
                    }

                }
                //2 Retreive food:
                //search for food store, pick up food. The carrying property will change so the rest will be handled elsewhere
                else if (offspringTendingPhase == 2)
                {
                    if (chamber != "FoodStash")
                    {
                        //Search for a food stash, leaving a trail behind you.
                        LayPheromone("ToNursery", directoryName, placedPheromone);
                        SearchForPheromones("ToFoodStash");
                        Move();
                    }
                    else
                    {
                        if (File.Exists(LocationProperty + @"\FoodStash_" + ColonyIDProperty.ToString("D2")))
                        {
                            GatherFromFoodStash();
                        }
                        else
                        {
                            //I have to prevent the ant from leaving the nest to search 
                            PreviousLocationProperty = "";
                            Move();
                        }
                    }
                }
                //3 Find new nursery phase 1
                else if (offspringTendingPhase == 3)
                {

                    if (chamber != "Nursery" && StepCountProperty > MyAnthillProperty.chambers.Count)
                    {

                        FlagConstructionJob("Nursery");
                        LayPheromone("ToOffspring", directoryName, placedPheromone);
                        SearchForPheromones("ToNursery");
                        Move();

                    }
                    else if (chamber != "Nursery" && StepCountProperty < MyAnthillProperty.chambers.Count)
                    {
                        LayPheromone("ToOffspring", directoryName, placedPheromone);
                        SearchForPheromones("ToNursery");
                        Move();

                    }
                    else
                    {

                        if (currentChamber.CapacityReachedProperty && StepCountProperty > MyAnthillProperty.chambers.Count)
                        {
                            FlagConstructionJob("Nursery");
                            LayPheromone("ToOffspring", directoryName, placedPheromone);
                            SearchForPheromones("ToNursery");
                            Move();

                        }
                        //if this new nursery is unsuitable
                        else if (currentChamber.CapacityReachedProperty)
                        {

                            //move first
                            LayPheromone("ToOffspring", directoryName, placedPheromone);
                            SearchForPheromones("ToNursery");
                            Move();


                        }
                        else
                        {
                            //The ant found a suitable nursery. Time to return to the offspring
                            offspringTendingPhase = 4;

                        }
                    }
                }
                //4 Find new nursery phase 2
                else if (offspringTendingPhase == 4)
                {
                    //if the original nursery or a nursery that is overcrowded is reached
                    if (chamber == "Nursery" && currentChamber.CapacityReachedProperty)
                    {
                        Ant target = null;
                        target = CheckForLocalAntType(AntType.Pupae, LocationProperty);

                        //you might ask why this isnt an 'else if'
                        if (target == null)
                        {
                            //if there are no pupae, then go down the line and check for other types
                            target = CheckForLocalAntType(AntType.Larvae, LocationProperty);
                            if (target == null)
                            {
                                target = CheckForLocalAntType(AntType.Egg, LocationProperty);
                                if (target == null)
                                {
                                    //no types found so move
                                    LayPheromone("ToOffspring", directoryName, placedPheromone);
                                    SearchForPheromones("ToNursery");
                                    Move();
                                }
                            }
                        }

                        //this will not occur if the sweep fails
                        if (target != null)
                        {
                            if (target.BeingCarriedProperty == false)
                            {
                                //pick up an offspring if not already getting carried.
                                PreviousLocationProperty = "";

                                PickUp(target);
                                CarryingProperty = Carrying.Queen;
                                SearchForPheromones("ToOffspring");
                                Move();
                            }
                        }
                    }
                    else
                    {
                        LayPheromone("ToOffspring", directoryName, placedPheromone);
                        SearchForPheromones("ToNursery");
                        Move();
                    }
                }

            }
            else if (StateProperty == State.TendingToEggs && (CarryingProperty == Carrying.Egg ||CarryingProperty == Carrying.Larvae ||CarryingProperty == Carrying.Pupae))
            {
                queenTendingPhase = 0;
                /*the ant is tending to the brood and has decided to move an offspring for whatever reason 
                 * The only reason to move an offspring is to avoid overcrowded nurseries or to get them away from the entrance or throne room. 
                 * so the ant should search for another nursery that is not at max capacity to place the egg.
                 * 
                 * 
                 */

                if (chamber != "Nursery" && StepCountProperty > MyAnthillProperty.chambers.Count)
                {
                    FlagConstructionJob("Nursery");
                    SearchForPheromones("ToNursery");
                    Move();

                }
                else if (chamber != "Nursery" && StepCountProperty < MyAnthillProperty.chambers.Count)
                {
                    SearchForPheromones("ToNursery");
                    Move();

                }
                else if(chamber == "Nursery")
                {
                    //if the ant has arrived at a nursery that has space
                    if (!currentChamber.CapacityReachedProperty)
                    {

                        PutDown();
                        CarryingProperty = Carrying.Nothing;
                        offspringTendingPhase = 0;
                        SearchForPheromones("ToNursery");
                        Move();

                    }
                    else
                    {
                        if (StepCountProperty > MyAnthillProperty.chambers.Count)
                        {

                            FlagConstructionJob("Nursery");


                        }
                        SearchForPheromones("ToNursery");
                        Move();
                    }
                }
            }
            else if (StateProperty == State.TendingToEggs && CarryingProperty == Carrying.Food)
            {
                queenTendingPhase = 0;
                //the ant is going to take food back to the Nursery.
                if (chamber != "Nursery")
                {

                    LayPheromone("ToFoodStash", directoryName, placedPheromone);
                    SearchForPheromones("ToNursery");
                    Move();
                }
                else
                {
                    //if the nursery it has arrived at required food
                    if (!File.Exists(LocationProperty + @"\FoodStash_" + ColonyIDProperty.ToString("D2")))
                    {
                        
                        DepositFood(CarryAmountProperty);
                        CarryingProperty = Carrying.Nothing;
                        offspringTendingPhase = 0;
                         
                    }
                    //else keep looking, as this is the wrong nursery or at least one that doesnt need food.
                    /**
                     * NOTE: This may cause ants to return to the nursery they were watching and then leave it once they realize that 
                     * another ant has already got food for it. This isnt a huge deal, because i can make the ant eat the food it's carrying if
                     * its about to start getting hungery, which would result in it reseting its carrying state and returning to normal tending
                     * activities.
                     */
                }
            }
            else if (StateProperty == State.MovingEggs && CarryingProperty == Carrying.Nothing)
            {
                queenTendingPhase = 0;
                offspringTendingPhase = 0;
                //the ant is not in the throne room and is not carrying an egg. find the throne room
                if (chamber != "ThroneRoom")
                {
                    SearchForPheromones("ToThroneRoom");
                    Move();

                }
                else
                {
                    Ant foundAnt = null;
                    /**
                     * 
                     * check if there are any eggs that arn't being moved
                     */

                    
                    //This function returns the first ant of this type, maybe save it to a pointer? 
                     foundAnt = CheckForLocalAntTypeToCarry(AntType.Egg, LocationProperty);

                    
                     /**
                     * if the function returns null 
                      */

                     if (foundAnt != null)
                     {
                         

                         /**
                          * if not null, check if being carried, grab it and carry it to a nursery 
                          * 
                          */
                         if (foundAnt.BeingCarriedProperty == false)
                         {
                             PickUp(foundAnt);
                             LayPheromone("ToThroneRoom", directoryName, placedPheromone);
                             SearchForPheromones("ToNursery");
                             Move();
                         }
                     }
                     else
                     {

                         foundAnt = CheckForLocalAntTypeToCarry(AntType.Larvae, LocationProperty);

                         if (foundAnt != null)
                         {
                             if (foundAnt.BeingCarriedProperty == false)
                             {
                                 PickUp(foundAnt);
                                 LayPheromone("ToThroneRoom", directoryName, placedPheromone);
                                 SearchForPheromones("ToNursery");
                                 Move();
                             }
                             else
                             {

                                 foundAnt = CheckForLocalAntTypeToCarry(AntType.Pupae, LocationProperty);

                             }
                         }

                     }
                    
                }
            }
            else if (StateProperty == State.MovingEggs && CarryingProperty != Carrying.Nothing)
            {
                queenTendingPhase = 0;
                offspringTendingPhase = 0;
                /*This ant is moving an egg to a nursery 
                 * 
                 * 
                 */

                if (chamber != "Nursery" && StepCountProperty < MyAnthillProperty.chambers.Count)
                {
                    SearchForPheromones("ToNursery");
                    Move();
                }
                else if (chamber != "Nursery" && StepCountProperty > MyAnthillProperty.chambers.Count)
                {
                    FlagConstructionJob("Nursery");
                    SearchForPheromones("ToNursery");
                    Move();
                }
                else if (chamber == "Nursery")
                {
                   
                   
                    //if the ant has arrived at a nursery that has space
                    if (!currentChamber.CapacityReachedProperty)
                    {

                        PutDown();
                        CarryingProperty = Carrying.Nothing;

                    }
                    else
                    {
                        if (StepCountProperty > MyAnthillProperty.chambers.Count)
                        {

                            FlagConstructionJob("Nursery");

                        }
                        SearchForPheromones("ToNursery");
                        Move();
                    }
                    
                }
            }
            else if (StateProperty == State.Digging)
            {

                queenTendingPhase = 0;
                offspringTendingPhase = 0;
                //Check flags. if none, revert to scouting 
                if (digPhase == 0)
                {
                    if (MyAnthillProperty.jobFlags.Count() > 0)
                    {

                        //if flags exist, take the first one, Change the flag to "chamberType + underconstruction", proceed with building task.
                        foreach (String job in MyAnthillProperty.jobFlags)
                        {
                            
                            if (job != "FoodStash_UnderConstruction" && job != "Nursery_UnderConstruction" && job != "ThroneRoom_UnderConstruction")
                            {

                                digTask = job;
                                stepsRequired = (int)(MyAnthillProperty.AntsTotalProperty * 0.2); //the steps are retreived based on how many ants there are in this colony
                                digPhase++;
                                break;
                            }

                        }

                        if (digTask != "")
                        {
                            MyAnthillProperty.jobFlags[0] = MyAnthillProperty.jobFlags[0] + "_UnderConstruction";
                        }
                        else
                        {
                            StateProperty = State.Scouting;//there are no jobs
                            digPhase = 0;//Dig is reset

                        }
                        /*
                            * The trick with this is that it the ant should try to avoid building a bunch of folders close to the surfice,
                            * so it should check if there are places to go from the entrance, and if there are, travel a certain amount of steps
                            * depending on colony size or until it reaches a dead end and then build.
                            * 
                            * 
                            *
                            */
                        //check dig phase
                        /* The building works in a few phases:
                            * The ant finds a good location for building. This might take multiple turns as the ant will basically just move until it has stepped enough times
                            * The ant builds a tunnel folder(1 turn)
                            * the ant builds another folder(1 turn)
                            * the ant builds a chamber there. ( 1 turn)
                         * 
                         * 
                         * 
                            */
                    }
                    else
                    {
                        StateProperty = State.Scouting;//there are no jobs
                        digPhase = 0;//Dig is reset

                    }
                }
                else if(digPhase == 1)//the first phase starts 
                {
                    if (stepsRequired != 0) // if more steps are to be taken
                    {
                        //Check if there are folders available
                        string[] folders = Directory.GetDirectories(LocationProperty);
                        if (folders.Length > 0)
                        {
                            //if so move(using the move function could result in the ant moving up the tree, may need to find a way to fix)
                            Move();
                            stepsRequired--; // reduce the required number of steps
                        }
                        else
                        {
                            //no folders were found here, there is no where to go. 
                            //The ant will just build from here.
                            folders = null;
                            stepsRequired = 0;
                            //start the digging part of the task
                            digPhase++;
                        }
                           
                    }
                    else
                    {
                        //start the digging part of the task
                        digPhase++;
                    }
                }
                else if (digPhase == 2)
                {
                    //dig first tunnel
                    DigTunnel(); // the DigTunnel function also causes the ant to enter the folder it creates. 
                    digPhase++;
                        
                }
                else if (digPhase == 3)
                {
                    //Dig second tunnel
                    DigTunnel();
                    digPhase++;
                }
                else if (digPhase == 4)
                {
                    //here the chamber is created. 
                    //and the ants state is changed
                    CreateChamber();
                    digPhase = 0;
                    digTask = "";
                    MyAnthillProperty.jobFlags.RemoveAt(0);

                }
                else
                {
                    digPhase = 0;
                    digTask = "";
                    stepsRequired = 0;

                }
            
                
            }

        }

        private void OutOfNestLogic(Pheromone placedPheromone, String directoryName)
        {
            /**
            * 
            * Eventually hunger needs to be handled and the ant needs to act accordingly 
            */
             if (StateProperty == State.Standby)
            {
                //check numbers for ants taking care of certain tasks(queen, brood) 
                //if essentials are filled(based on the current size of the colony) start scouting.
                StateProperty = State.Scouting;
            }
            else if (StateProperty == State.Scouting && CarryingProperty == Carrying.Nothing)
            {
                
                if (directoryName != "Anthill_" + ColonyIDProperty.ToString("D2"))
                {


                    if (!SearchForLocalFood())
                    {
                        LayPheromone("ToNest", directoryName, placedPheromone);

                        SearchForPheromones("Food");

                        Move();
                    }
                    else
                    {

                        Gather();
                    }
                }
                else
                {
                    InNestProperty = true;
                }
            }
             else if (StateProperty == State.Scouting && CarryingProperty == Carrying.Food)
             {
                 if (CarryAmountProperty > 0)
                 {
                     StateProperty = State.Gathering;

                 }
                 else
                 {
                     CarryingProperty = Carrying.Nothing;
                 }

             }
            //if gathering, follow a 'to nest' pheromone back strengthening 'food' pheromone. 
            else if (StateProperty == State.Gathering && CarryingProperty == Carrying.Food)
            {
                //look for the nest
                if (!SearchForLocalNest())
                {
                    //the nest was not found, so the ant searches for "ToNest" pheromones to follow back
                    //leaving "food" pheromones for other ants to follow. 
                    LayPheromone("Food", directoryName, placedPheromone);
                    SearchForPheromones("ToNest");
                    
                }
                else
                {
                    //if found
                    //Enters the nest
                    DestinationProperty = LocationProperty + @"\Anthill_"+ColonyIDProperty.ToString("D2");
                    InNestProperty = true;
                        
                }
                Move();

            }
             else if (StateProperty == State.TendingToQueen || StateProperty == State.TendingToEggs || StateProperty == State.Digging || StateProperty == State.MovingEggs)
            {
                 /**
                  * IMPORTANT: 
                  * Theres an issue where the ants outside the nest that should be in the nest get stuck in a loop
                  * or get lost. So we need some way for them to find their way back to the nest. 
                  * 
                  * THIS ISSUE MAY GO DEEPER THEN THIS
                  * 
                  * This problem may be something i want to fix with any movment in general. but for now it seems like a seperate issue. 
                  * Maybe pheromones need less strength and the ants who place them need to be controlled a little differently. 
                  * 
                  * NOTE 5/19/2014: I may have fixed this with the step counting mechanic. Ants can only go so far from the nest
                  * depending on how many ants reside in the nest
                  * 
                  * 
                  */ 
                if (!SearchForLocalNest())
                {
                    //the nest was not found, so the ant searches for "ToNest" pheromones to follow back
                    //leaving "food" pheromones for other ants to follow. 
                    SearchForPheromones("ToNest");
                    Move();

                }
                else
                {
                    //if found
                    //Enters the nest
                    DestinationProperty = LocationProperty + @"\Anthill_" + ColonyIDProperty.ToString("D2");
                    InNestProperty = true;
                    Move();

                }


            }
            

        }
        #endregion

        private void DepositFood(int food)
        {
            if (File.Exists(LocationProperty + @"\FoodStash_" + ColonyIDProperty.ToString("D2")))
            {
               
                int currentFood = 0;
                if (localPheromonePath.Length > 0)
                {
                    using (StreamReader sr = new StreamReader(LocationProperty + @"\FoodStash_" + ColonyIDProperty.ToString("D2")))
                    {
                        String line = sr.ReadToEnd();

                        int.TryParse(line, out currentFood);

                        currentFood += food;
                        currentChamber.AmountOfFoodProperty += food;
                        sr.Close();

                    }

                    File.WriteAllText(LocationProperty + @"\FoodStash_" + ColonyIDProperty.ToString("D2"), String.Empty);
                    using (StreamWriter sw = File.CreateText(LocationProperty + @"\FoodStash_" + ColonyIDProperty.ToString("D2")))
                    {
                        sw.WriteLine(currentFood);
                        sw.Close();
                    }
                }
                
            }
            else
            {
                using (StreamWriter sw = File.CreateText(LocationProperty + @"\FoodStash_" + ColonyIDProperty.ToString("D2")))
                {
                    sw.WriteLine(food);
                    sw.Close();
                }
            }
            CarryAmountProperty -= food;
            if (CarryAmountProperty == 0)
            {
                CarryingProperty = Carrying.Nothing;
            }

            


        }



        override public void Move()// The ant finds a suitable folder to enter, sets it to 'destination' then moves itself there. 
        {
            Console.WriteLine("Ant ID:" + this.IDProperty);//for some reason some ants DONT HAVE FUCKING IDS. IM SO ANNOYED 
            if (DestinationProperty == "")
            {
                if (!InNestProperty)
                {
                    if (StepCountProperty < MyAnthillProperty.stepLimit)
                    {
                        if (pheromoneLocations.Count() > 1)
                        {
                            DestinationProperty = FindStrongestPheromone(pheromoneLocations);
                        }
                        else if (pheromoneLocations.Count() == 1)
                        {
                            DestinationProperty = new FileInfo(pheromoneLocations[0]).Directory.FullName;
                        }
                        else
                        {
                            DestinationProperty = GetRandomPath();
                        }
                    }
                    else
                    {
                        DestinationProperty = PreviousLocationProperty;
                    }
                }
                else
                {
                    DestinationProperty = GetRandomPath();
                }
            }
            
            
            //Set current location to previouslocation property.

            if (DestinationProperty != "nopath")
            {
                try
                {
                    PreviousLocationProperty = LocationProperty;

                    File.Move(FullPathProperty, DestinationProperty + @"\" + TypeProperty + "_" + ColonyIDProperty.ToString("D2") + "_" + IDProperty.ToString("D4") + "_" + StateProperty + ".ant");
                    LocationProperty = DestinationProperty;
                    FullPathProperty = DestinationProperty + @"\" + TypeProperty + "_" + ColonyIDProperty.ToString("D2") + "_" + IDProperty.ToString("D4") + "_" + StateProperty + ".ant";

                    if (CarriedAntProperty != null)
                    {
                        CarriedAntProperty.DestinationProperty = this.DestinationProperty;
                        CarriedAntProperty.Move();
                    }
                    



                }
                catch (UnauthorizedAccessException err)
                {

                }
            }
            
            

        }
        override protected void Gather()//food gathering takes up a turn. if the ant has moved 
        {

            //Set previous location property to be null string. This is because it will need to be able to backtrack in order to return to the nest. 
            PreviousLocationProperty = "";

            //Gather Food

            int foodAmount = 0;
           
            using (StreamReader sr = new StreamReader(LocationProperty + @"\FoodSource.txt"))
            {
                String line = sr.ReadToEnd();

                int.TryParse(line, out foodAmount);

                if (foodAmount > CARRYING_CAPACITY)
                {
                    foodAmount -= CARRYING_CAPACITY;
                    this.CarryAmountProperty += CARRYING_CAPACITY;
                }
                else
                {
                    this.CarryAmountProperty += foodAmount;
                    foodAmount = 0;
                }
                sr.Close();

            }
            File.WriteAllText(LocationProperty + @"\FoodSource.txt", String.Empty);
            using (StreamWriter sw = File.CreateText(LocationProperty + @"\FoodSource.txt"))
            {
                sw.WriteLine(foodAmount);
                sw.Close();
            }

            if (foodAmount <= 0)
            {
                File.Delete(LocationProperty + @"\FoodSource.txt");
            }
            CarryingProperty = Carrying.Food;


            
            //if food source reached and it is depleated. change state. If ant empties the foodsource, delete it. 
        }
        override protected void Eat()//eating takes up a turn. It replenishes hunger to full
        {
            StateProperty = State.Eating;
        }
        override protected void PickUp(Ant ant)//the ant picks up an object. Picking up an object means that the object moves when this ant does
        {
            string strcarryType = ant.TypeProperty.ToString();
            CarriedAntProperty = ant;
            CarriedObjectFullPathProperty = ant.FullPathProperty;
            CarriedObjectLocationProperty = ant.LocationProperty;
            CarryingProperty = (Carrying) Enum.Parse(typeof (Carrying), strcarryType);
            ant.BeingCarriedProperty = true;
        }
        override protected void PutDown() // the link between this ant and the object it was carrying is severed
        {
            CarriedAntProperty.BeingCarriedProperty = false;
            CarriedAntProperty = null;
            CarriedObjectFullPathProperty = "";
            CarriedObjectLocationProperty = "";
            CarryingProperty = Carrying.Nothing;
        }
        override protected void Chirp() //the ant emits a chirp, alerting closeby ants to its location
        {

        }
        override protected void LayPheromone(String pheromoneType, String directoryName, Pheromone placedPheromone)// the ant strengthens a pheremone or, if its a queen, lays a pheremone to direct other ants. it finds a pheremone file and sends the pheremone type to the file.
        {
            //totally forgot about this function. I should probably use it seeing as how all ants lay pheromones the same way.
            SearchForLocalPheromones(pheromoneType);
            PheromoneEnum pheromone = (PheromoneEnum)Enum.Parse(typeof(PheromoneEnum), pheromoneType);
            if (localPheromonePath != "")
            {
                IncreaseLocalPheromoneStrength(localPheromonePath);
            }
            else
            {
                if (directoryName != "Anthill_" + this.ColonyIDProperty.ToString("D2"))
                {
                    placedPheromone = new Pheromone(this.ColonyIDProperty, this.LocationProperty, pheromone);
                    MyAnthillProperty.pheromonesPlaced.Add(placedPheromone);
                }
            }

        }
        override protected void Feed(Ant ant, int amount)
        {
            /**
             * 
             * The ant takes food from the closest food source, and feeds a specified ant in its current location
             */
            if (File.Exists(LocationProperty + @"\FoodStash_" + ColonyIDProperty.ToString("D2")))
            {
                int foodAmount = 0;

                using (StreamReader sr = new StreamReader(LocationProperty + @"\FoodStash_" + ColonyIDProperty.ToString("D2")))
                {
                    String line = sr.ReadToEnd();

                    int.TryParse(line, out foodAmount);

                    if (foodAmount > amount)
                    {
                        foodAmount -= amount;
                        this.CarryAmountProperty += amount;
                    }
                    else
                    {
                        this.CarryAmountProperty += foodAmount;
                        foodAmount = 0;
                    }
                    sr.Close();

                }
                File.WriteAllText(LocationProperty + @"\FoodStash_" + ColonyIDProperty.ToString("D2"), String.Empty);
                using (StreamWriter sw = File.CreateText(LocationProperty + @"\FoodStash_" + ColonyIDProperty.ToString("D2")))
                {
                    sw.WriteLine(foodAmount);
                    sw.Close();
                }

                if (foodAmount <= 0)
                {
                    File.Delete(LocationProperty + @"\FoodStash_" + ColonyIDProperty.ToString("D2"));
                }

                ant.HungerProperty += this.CarryAmountProperty;
                this.CarryingProperty = 0;
            }
        }
        private void DigTunnel()
        {
            int randomTunnelID = 0; 
            // This is a random id for the tunnel. The tunnel is not tracked, but we dont want tunnels overwriting eachother.
            
            Random rnd = new Random();


            /*
             * I think my plan here will be to generate a random 6 digit number 
             * check the folder to see if a tunnel with that id already exists
             * if yes, re-generate the ID and try again. (do -while loop for random generation?)
             * if no, create the tunnel and enter it. 
             */
            do
            {
                randomTunnelID = rnd.Next(999999);//generate ID
            }
            while (Directory.Exists(LocationProperty + @"\Tunnel_" + randomTunnelID.ToString("D6"))); // check if exists.


            Directory.CreateDirectory(LocationProperty + @"\Tunnel_" + randomTunnelID.ToString("D6"));
            File.Move(FullPathProperty, LocationProperty + @"\Tunnel_" + randomTunnelID.ToString("D6") + @"\" + TypeProperty + "_" + ColonyIDProperty.ToString("D2") + "_" + IDProperty.ToString("D4") + "_" + StateProperty + ".ant");
            FullPathProperty = LocationProperty + @"\Tunnel_" + randomTunnelID.ToString("D6") + @"\" + TypeProperty + "_" + ColonyIDProperty.ToString("D2") + "_" + IDProperty.ToString("D4") + "_" + StateProperty + ".ant";
            LocationProperty = LocationProperty + @"\Tunnel_" + randomTunnelID.ToString("D6");




 

        }

        // creates a chamber folder that can contain certain objects and has certain functions.
        private void CreateChamber()
        {
            int randomChamberID = 0; 
            Chamber chamber;
            // This is a random id for the tunnel. The tunnel is not tracked, but we dont want tunnels overwriting eachother.
            
            Random rnd = new Random();


            /*
             * I think my plan here will be to generate a random 6 digit number 
             * check the folder to see if a tunnel with that id already exists
             * if yes, re-generate the ID and try again. (do -while loop for random generation?)
             * if no, create the tunnel and enter it. 
             */
            do
            {
                randomChamberID = rnd.Next(999999);//generate ID
            }
            while (Directory.Exists(LocationProperty + @"\"+ digTask + "_" + randomChamberID.ToString("D6"))); // check if exists.


            Directory.CreateDirectory(LocationProperty + @"\" + digTask +"_" + randomChamberID.ToString("D6"));

            if (digTask == "ThroneRoom")
            {
                MyAnthillProperty.ants[0].NewThroneRoomProperty = true;
            }
            chamber = new Chamber();
            chamber.TypeProperty = (ChamberType) Enum.Parse(typeof(ChamberType), digTask);
            chamber.IDProperty = randomChamberID;
            MyAnthillProperty.chambers.Add(randomChamberID, chamber);

            


        }


        private void GatherFromFoodStash()//food gathering takes up a turn. if the ant has moved 
        {

            //Set previous location property to be null string. This is because it will need to be able to backtrack in order to return to a previous location. 
            PreviousLocationProperty = "";

            //Gather Food

            int foodAmount = 0;

            using (StreamReader sr = new StreamReader(LocationProperty + @"\FoodStash_" + ColonyIDProperty.ToString("D2")))
            {
                String line = sr.ReadToEnd();

                int.TryParse(line, out foodAmount);

                if (foodAmount > CARRYING_CAPACITY)
                {
                    foodAmount -= CARRYING_CAPACITY;
                    this.CarryAmountProperty += CARRYING_CAPACITY;
                    currentChamber.AmountOfFoodProperty -= CARRYING_CAPACITY;
                }
                else
                {
                    this.CarryAmountProperty += foodAmount;
                    foodAmount = 0;
                }
                sr.Close();

            }
            File.WriteAllText(LocationProperty + @"\FoodStash_" + ColonyIDProperty.ToString("D2"), String.Empty);
            using (StreamWriter sw = File.CreateText(LocationProperty + @"\FoodStash_" + ColonyIDProperty.ToString("D2")))
            {
                sw.WriteLine(foodAmount);
                sw.Close();
            }

            if (foodAmount <= 0)
            {
                File.Delete(LocationProperty + @"\FoodStash_" + ColonyIDProperty.ToString("D2"));
            }
            CarryingProperty = Carrying.Food;



            //if food source reached and it is depleated. change state. If ant empties the foodsource, delete it. 
        }


        private Ant CheckForLocalAntType(AntType type, String location)
        {
            Ant foundAnt = null;
            try
            {
                for (int i = 0; i < (MyAnthillProperty.ants.Count - 1); i++)
                {
                    if (MyAnthillProperty.ants[i].TypeProperty == type && MyAnthillProperty.ants[i].LocationProperty == location)
                    {
                        foundAnt = MyAnthillProperty.ants[i];
                        i = (MyAnthillProperty.ants.Count - 1);
                    }
                }
            }
            catch (NullReferenceException ex)
            {
                foundAnt = null;
            }
            return foundAnt;
        }

        private Ant CheckForLocalAntTypeToCarry(AntType type, String location)
        {
            Ant foundAnt = null;
            try
            {
                for (int i = 0; i < (MyAnthillProperty.ants.Count - 1); i++)
                {
                    if (MyAnthillProperty.ants[i].TypeProperty == type && MyAnthillProperty.ants[i].LocationProperty == location && MyAnthillProperty.ants[i].BeingCarriedProperty == false)
                    {
                        foundAnt = MyAnthillProperty.ants[i];
                        i = (MyAnthillProperty.ants.Count - 1);
                    }
                }
            }
            catch (NullReferenceException ex)
            {
                foundAnt = null;
            }
            return foundAnt;
        }

        private Ant CheckForLocalHungryOffspring(String location)
        {
            Ant foundAnt = null;
            try
            {


                for (int i = 0; i < (MyAnthillProperty.ants.Count - 1); i++)
                {
                    if ((MyAnthillProperty.ants[i].TypeProperty == AntType.Larvae || MyAnthillProperty.ants[i].TypeProperty == AntType.Pupae) && MyAnthillProperty.ants[i].LocationProperty == location && MyAnthillProperty.ants[i].HungerProperty < 600)
                    {
                        foundAnt = MyAnthillProperty.ants[i];
                        i = (MyAnthillProperty.ants.Count - 1);
                    }
                }
            }
            catch (NullReferenceException ex)
            {
                foundAnt = null;
            }
            return foundAnt;
        }
        #endregion

    }
}
