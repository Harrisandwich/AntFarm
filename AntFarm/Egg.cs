using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace AntFarm
{
    public class Egg : Ant
    {
        #region Constructor
        public Egg(int id, int colonyID, String location, bool loaded = false)
            : base(1000, 0, id, colonyID, 0, location, AntType.Egg)
        {
            this.TypeProperty = AntType.Egg;
            this.StateProperty = State.Standby;
            //create file 
            this.LocationProperty = location;
            this.FullPathProperty = LocationProperty + @"\" + TypeProperty + "_" + ColonyIDProperty.ToString("D2") + "_" + IDProperty.ToString("D4") + "_" + StateProperty + ".ant";
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

        }
        #endregion

        #region Methods
        public override void Act()
        {
            Update();

            if (AgeProperty == MATURE_AGE)
            {
                //instantiat new larvae and delete egg. 
                if (BeingCarriedProperty == false)
                {
                    Hatch();
                }
            }
        }

        private void Hatch()
        {
            
            File.Delete(this.FullPathProperty);
            Larvae newForm = new Larvae(this.IDProperty, this.ColonyIDProperty,this.LocationProperty);
        }
        override public void Move()// The ant finds a suitable folder to enter, sets it to 'destination' then moves itself there. 
        {
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
        #endregion

        #region Unused
       
        override protected void Gather()//food gathering takes up a turn. if the ant has moved 
        {

        }
        override protected void Eat()//eating takes up a turn. It replenishes hunger to full
        {

        }
        override protected void Feed(Ant ant, int amount)//eating takes up a turn. It replenishes hunger to full
        {

        }
        override protected void PickUp(Ant ant)//the ant picks up and object. Picking up an object means that the object moves when this ant does
        {

        }
        override protected void PutDown() // the link between this ant and the object it was carrying is severed
        {

        }
        override protected void Chirp() //the ant emits a chirp, alerting closeby ants to its location
        {

        }
        override protected void LayPheromone(String pheromoneType, String directoryName, Pheromone placedPheromone)// the ant strengthens a pheremone or, if its a queen, lays a pheremone to direct other ants. it finds a pheremone file and sends the pheremone type to the file.
        {

        }
        #endregion
    }
}
