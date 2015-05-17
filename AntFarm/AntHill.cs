using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace AntFarm
{
    public class AntHill
    {
        #region Fields
        //dictionary for each ant
        public Dictionary<int, Ant> ants = new Dictionary<int, Ant>(9999);
        public List<Pheromone> pheromonesPlaced = new List<Pheromone>();

        //Chamber dictionary
        public Dictionary<int, Chamber> chambers = new Dictionary<int, Chamber>();

        //Ant job lists
        public List<Ant> antsDigging = new List<Ant>();
        public List<Ant> antsTendingToQueen = new List<Ant>();
        public List<Ant> antsScouting = new List<Ant>();
        public List<Ant> antsTendingToBrood = new List<Ant>();
        public List<Ant> antsMovingEggs = new List<Ant>();

        //Flag list 
        //IMPORTANT: Only allow one flag of each type. no duplicates. This is so only one chamber type gets built at a time
        // and a fuckload of ants arnt all flagging for the same chamber and accidentally asking for a bunch. 
        public List<String> jobFlags = new List<string>();

        public int stepLimit = 0;


        //one queen for each hill
        Queen queen = null;
        //location of the hill
        private String location;
        //hills unique id
        private int ID = 0;
        private int tickCount = -1;

        //Counts for ants tending to certain things
        private int numberAntsDigging = 0;
        private int numberAntsTendingToQueen = 0;
        private int numberAntsTendingToBrood = 0;
        private int numberAntsMovingEggs = 0;
        private int numberAntsScouting = 0;
        private int antsTotal = 0;

        #endregion

        #region Constructor
        public AntHill(int id, string location, bool createdByConsole)
        {
             
            
            this.ID = id;
            this.location = location;

            for (int i = 0; i < 9999; i++)
            {
                ants.Add(i, null);
            }

            //The anthill, when created, should automatically have flags for one food stor and one nursery, so the minute an ant is old enough
            //to work, they get to it immediatly. 
            jobFlags.Add("FoodStash");
            jobFlags.Add("Nursery");
            jobFlags.Add("ThroneRoom");
            
            
        }
        #endregion

        #region Methods
        //The hill is started. 
        public void StartHill(bool createdByConsole)
        {
            if (createdByConsole)//if the hill was created by the user then certain conditions apply. 
            {
                try
                {

                    queen = new Queen(0, this.ID, this.location);
                    ants[0] = queen;
                    Console.WriteLine(queen.FullPathProperty);


                }
                catch (ArgumentException)
                {

                    if (queen != null && queen.FullPathProperty != null)
                    {

                        File.Delete(queen.FullPathProperty);
                        queen = null;
                    }
                }
            }
        }

        public void CheckStatus()
        {

            tickCount++;
            if (!Directory.Exists(AntFarmForm.antHills[this.IDProperty].LocationProperty))
            {
                AntFarmForm.antHills[this.IDProperty] = null;
            }

            foreach (Pheromone phe in pheromonesPlaced)
            {
                phe.Update();

            }

            //Split the task based on the number of ants. (percentages) 
            //60% will be scouting and gathering. The remaining 40% will be split into a few main groups
            //10% will be taking care of the queen
            //10% will be digging tunnels 
            //10% will be tending to the brood
            //10% will be hanging out by the queen, moving eggs to nurseries 

            //any remaining ants will be tasked to scouting.
            try
            {
                if (tickCount == 0 || tickCount == 11)
                {
                    tickCount = 1;
                    if (AntFarmForm.antHills[this.IDProperty] != null)
                    {
                        int loopMax = 0;
                        int passoverCount = 0;
                        //assign brood attendants 
                        AntsMoveingEggsProperty = (int)(0.1 * AntsTotalProperty);
                        //assign brood attendants 
                        AntsTendingToBroodProperty = (int)(0.1 * AntsTotalProperty);
                        //assign diggers
                        AntsDiggingProperty = (int)(0.1 * AntsTotalProperty);
                        //assign royal caretakers
                        AntsTendingToQueenProperty = (int)(0.1 * AntsTotalProperty);
                        //those who dont get assigned scout by default 

                        AntsScoutingProperty = ( AntsTotalProperty - (AntsTendingToBroodProperty + AntsTendingToQueenProperty + AntsDiggingProperty));

                        //for AntsTotalProperty
                        //if i == AntsTendingToBroodProperty 
                        //ants[i].state = tending to brood; 
                        loopMax = AntsTendingToQueenProperty+1;
                        for (int i = 1; i <= loopMax; i++)
                        {
                            //loop until all royal attendants are assigned. 

                            //set i to the number of royal attendants
                            //set loopmax to the number of diggers + number of queen attendants
                            //loop until all diggers are assigned

                            //set i to the number of diggers + attendants
                            //set loopmax to the number of diggers + number of queen attendants + number of brood attendants
                            //loop until all brood attendants are assigned.

                            //set i to the number of Egg Movers + attendants
                            //set loopmax to the number of diggers + number of queen attendants + number of brood attendants
                            //loop until all egg movers are assigned.

                            //set i to the number of diggers + attendants + egg movers
                            //set loopmax to the number of diggers + number of queen attendants + number of brood attendants
                            //loop until all diggers are assigned.

                            //set i to the number of diggers + all attendants + egg movers
                            //set loopmax to the number of diggers + number of attendants + number of brood attendants + number of scouts 
                            //loop until scouts are assigned. 
                            #region Loop Logic
                            //the royal gaurd assignment is complete, assign the rest
                            if (i == loopMax)
                            {
                                //assign the last ant, reset the loopMax
                                if (passoverCount == 0)
                                {
                                    //set loop to assign diggers
                                    i = numberAntsTendingToQueen+1;
                                    loopMax = (numberAntsTendingToQueen + numberAntsDigging + 2);
                                }
                                else if (passoverCount == 1)
                                {
                                    //set loop to assign offspring tenders 
                                    i = (numberAntsTendingToQueen + numberAntsDigging + 1);
                                    loopMax = (numberAntsTendingToQueen + numberAntsDigging + numberAntsTendingToBrood + 2);

                                }
                                else if (passoverCount == 2)
                                {
                                    //set loop to assign egg movers
                                    i = (numberAntsTendingToQueen + numberAntsDigging + numberAntsTendingToBrood + 1);
                                    loopMax = (numberAntsTendingToQueen + numberAntsDigging + numberAntsTendingToBrood + numberAntsMovingEggs + 2);
                                }
                                else if (passoverCount == 3)
                                {
                                    //set loop to assign scouts 
                                    i = (numberAntsTendingToQueen + numberAntsDigging + numberAntsTendingToBrood + numberAntsMovingEggs + 1);
                                    loopMax = (numberAntsTendingToQueen + numberAntsDigging + numberAntsTendingToBrood + numberAntsMovingEggs + numberAntsScouting + 2);
                                }
                                

                                passoverCount++;
                            }
                            else
                            {
                                if (ants[i].TypeProperty != AntType.Queen && ants[i].TypeProperty != AntType.Egg && ants[i].TypeProperty != AntType.Larvae && ants[i].TypeProperty != AntType.Pupae)
                                {
                                    if (passoverCount == 0)
                                    {
                                        if (  ants[i].StateProperty != State.TendingToQueen && antsTendingToQueen.Count < numberAntsTendingToQueen)
                                        {
                                            ants[i].StateProperty = State.TendingToQueen;
                                            if(!antsTendingToQueen.Contains(ants[i]))
                                                antsTendingToQueen.Add(ants[i]);
                                        }
                                    }
                                    else if (passoverCount == 1)
                                    {
                                        if ( ants[i].StateProperty != State.Digging && antsDigging.Count < numberAntsDigging && jobFlags.Count > 0)
                                        {
                                            ants[i].StateProperty = State.Digging;
                                            if(!antsDigging.Contains(ants[i]))
                                                antsDigging.Add(ants[i]);
                                        }
                                    }
                                    else if (passoverCount == 2)
                                    {
                                        if ( ants[i].StateProperty != State.TendingToEggs && antsTendingToBrood.Count < numberAntsTendingToBrood)
                                        {
                                            ants[i].StateProperty = State.TendingToEggs;
                                            if(!antsTendingToBrood.Contains(ants[i]))
                                                antsTendingToBrood.Add(ants[i]);
                                        }
                                    }
                                    else if (passoverCount == 3)
                                    {
                                        if (ants[i].StateProperty != State.MovingEggs && antsMovingEggs.Count < numberAntsMovingEggs)
                                        {
                                            ants[i].StateProperty = State.MovingEggs;
                                            if (!antsMovingEggs.Contains(ants[i]))
                                                antsMovingEggs.Add(ants[i]);
                                        }
                                    }
                                    else if (passoverCount == 4)
                                    {
                                        if (ants[i].StateProperty != State.Scouting && ants[i].StateProperty != State.Gathering && antsScouting.Count < numberAntsScouting)
                                        {
                                            ants[i].StateProperty = State.Scouting;
                                            if(!antsScouting.Contains(ants[i]))
                                                antsScouting.Add(ants[i]);
                                        }
                                    }
                                }

                            }
                            #endregion

                        }



                    }
                }



            }
            catch (NullReferenceException ex)
            {

            }
           

        }
        #endregion

        #region Accessor Methods
        public int IDProperty
        {
            get
            {
                return ID;
            }
            set
            {
                this.ID = value;
            }
        }

        public string LocationProperty
        {
            get
            {
                return location; 
            }
            set
            {
                this.location = value;
            }
        }

        public int AntsDiggingProperty
        {
            get
            {
                return numberAntsDigging;
            }

            set
            {
                numberAntsDigging = value;
            }
        }

        public int AntsTendingToQueenProperty
        {
            get
            {
                return numberAntsTendingToQueen;
            }

            set
            {
                numberAntsTendingToQueen = value;
            }
        }

        public int AntsTendingToBroodProperty
        {
            get
            {
                return numberAntsTendingToBrood;
            }

            set
            {
                numberAntsTendingToBrood = value;
            }
        }
        public int AntsMoveingEggsProperty
        {
            get
            {
                return numberAntsMovingEggs;
            }

            set
            {
                numberAntsMovingEggs = value;
            }
        }
        public int AntsScoutingProperty
        {
            get
            {
                return numberAntsScouting;
            }

            set
            {
                numberAntsScouting = value;
            }
        }
        public int AntsTotalProperty
        {
            get
            {
                return antsTotal;
            }

            set
            {
                antsTotal = value;
                stepLimit = (int)(AntsTotalProperty * 0.3);
            }
        }
        #endregion 
    }
}
