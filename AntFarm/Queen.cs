using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;

/* 
 * The queen ant is the life of the nest. She lays eggs, never moves unless carried. Will use pheremones to alert other ants to her needs. 
 */ 

namespace AntFarm
{
    public class Queen :Ant
    {
        #region Fields
        private int tickCount = 0;


        private const int QUEEN_START_HEALTH = 1000;
        private const int QUEEN_START_HUNGER = 1000;
        private const int BROOD_SIZE = 10;
        private const int COLONY_SIZE_THRSHLD = 100;
        private const int FOOD_CONVERSION_RATIO = 20;


        private bool queuedToLayEggs = false;


        string[] dirSplit;
        string chamber;


        #endregion

        #region Constructor
        public Queen(int id, int colonyID, String location, bool loaded = false)
            : base(QUEEN_START_HEALTH, QUEEN_START_HUNGER, id, colonyID, 0, location, AntType.Queen)
        {
            TypeProperty = AntType.Queen;
            CarryingProperty = Carrying.Food;
            CarryAmountProperty = 10000;
            LocationProperty = location;
            FullPathProperty = LocationProperty +@"\" + TypeProperty + "_" + ColonyIDProperty.ToString("D2") + "_" + IDProperty.ToString("D4") + "_" + StateProperty + ".ant";
            StateProperty = State.Standby;
            if (!File.Exists(FullPathProperty) && loaded == false)
            {
                // Create a file to write to. 
                using (StreamWriter sw = File.CreateText(FullPathProperty))
                {
                    sw.WriteLine("Type:" + this.TypeProperty);
                    sw.WriteLine("Health:"+ this.HealthProperty);
                    sw.WriteLine("Hunger:"+ this.HungerProperty);
                    sw.Close();
                }
            }
        }
        #endregion

        #region Methods
        override public void Act()// Here the ant decides how to act, and "per-tick" actions take place (ex. hunger drops etc)
        {
            StateProperty = State.Standby;
            string directoryName = Path.GetFileName(Path.GetDirectoryName(FullPathProperty));
            int parseOut;
            string parseIn = "" + (MyAnthillProperty.AntsTotalProperty / COLONY_SIZE_THRSHLD) + "";
            //UpdateFileName();
            Update();
            tickCount++;

            dirSplit = null;
            dirSplit = Regex.Split(directoryName, "_");
            chamber = dirSplit[0];

            //if the number of ants in the colony can be divided by the COLONY_SIZE_THRSHLD into a whole number 
            //then flag for a new throne room. 
            
            if (NewThroneRoomProperty == true)
            {
                if (chamber == "ThroneRoom")
                {
                    String dirParent = Directory.GetParent(LocationProperty).ToString();
                    //Rename the folder to "Nursery". This signifies to the other ants that there is a new throne room.
                    Directory.Move(LocationProperty, dirParent + "Nursery_" + dirSplit[1]);
                    
                }
                NewThroneRoomProperty = false;

            }
            else
            {
               
                if (MyAnthillProperty.AntsTotalProperty < 2)
                {
                    LayEggs();
                    tickCount = 0;
                    tickCount = 0;
                }
                //if the tick count reaches the threshold and the queens hunger is high enough, lay eggs.
                if (HungerProperty > 800 && queuedToLayEggs == false && tickCount > 200)
                {

                    queuedToLayEggs = true;
                }


                if (queuedToLayEggs)
                {
                    if(chamber == "ThroneRoom")
                    {
                        LayEggs();
                        tickCount = 0;
                        queuedToLayEggs = false;
                        tickCount = 0;
                        //if (int.TryParse(parseIn, out parseOut) && MyAnthillProperty.AntsTotalProperty > 21)
                        //{
                        //    FlagConstructionJob("ThroneRoom");
                        //}
                    }
                }





                
            }
            

            //search the colony for larvae or pupae, if their location is the same as the queen. attempt to feed.
            


            //The Queen will almost NEVER move/gather

        }
        public void LayEggs()
        {
            int eggCount = 20;
            StateProperty = State.LayingEggs;
            //UpdateFileName();
            //create egg objects and assign them ids 
            for(int i = 0; i < eggCount; i++)
            {
                Egg newEgg = null;


                if (MyAnthillProperty.ants[i] == null)
                {
                    newEgg = new Egg(i, this.ColonyIDProperty, this.LocationProperty);
                    MyAnthillProperty.ants[i] = newEgg;

                }
                else
                {
                    eggCount++;
                }

               
                
            }
        }


        override protected void LayPheromone(String pheromoneType, String directoryName, Pheromone placedPheromone)// the ant strengthens a pheremone or, if its a queen, lays a pheremone to direct other ants. it finds a pheremone file and sends the pheremone type to the file.
        {
            
            //the queens version of this function creats a pheremone unique to the queen that signals her needs.
        }
       
        override protected void Eat()//eating takes up a turn. It replenishes hunger to full
        {
            StateProperty = State.Eating;
            //if foodsource is present, eat from it. If no food source, signal for food and be fed by another ant
        }
        override protected void Feed(Ant ant, int amount)//eating takes up a turn. It replenishes hunger to full
        {
            StateProperty = State.Feeding;
            //take food from carrying amount and add it to the hunger of the ant being fed
        }

        override public void Move()// The ant finds a suitable folder to enter, sets it to 'destination' then moves itself there. 
        {
            //not used by queen unless being carried by another ant.

            if (DestinationProperty != "")
            {

                //Set current location to previouslocation property.

                try
                {

                    PreviousLocationProperty = LocationProperty;

                    File.Move(FullPathProperty, DestinationProperty + @"\" + TypeProperty + "_" + ColonyIDProperty.ToString("D2") + "_" + IDProperty.ToString("D4") + "_" + StateProperty + ".ant");
                    LocationProperty = DestinationProperty;
                    FullPathProperty = DestinationProperty + @"\" + TypeProperty + "_" + ColonyIDProperty.ToString("D2") + "_" + IDProperty.ToString("D4") + "_" + StateProperty + ".ant";

                }
                catch (UnauthorizedAccessException err)
                {

                }
            }
        }
        override protected void Gather()//food gathering takes up a turn. if the ant has moved 
        {
            StateProperty = State.Gathering;
            //not used by queen
        }
        override protected void PickUp(Ant ant)//the ant picks up and object. Picking up an object means that the object moves when this ant does
        {
            //not used by queen
        }
        override protected void PutDown() // the link between this ant and the object it was carrying is severed
        {
            //not used by queen
        }
        override protected void Chirp() //the ant emits a chirp, alerting closeby ants to its location
        {
            //not used by queen
        }

        #endregion


    }
}
