using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
namespace AntFarm
{
    public class Pheromone
    {
        #region Fields
        private int strength;
        private PheromoneEnum type;
        private int colonyID;
        private String location;
        private String fullPath;

        private const int DECAY_RATE = 1;
        public  const int MAX = 100;
        #endregion

        #region Constructor
        public Pheromone(int colonyID, String location, PheromoneEnum type, bool loaded = false)
        {
            StrengthProperty = MAX;
            LocationProperty = location;
            TypeProperty = type;
            ColonyIDProperty = colonyID;
            FullPathProperty = LocationProperty + @"\" + TypeProperty + "_" + ColonyIDProperty.ToString("D2") + ".phe";
            if (!File.Exists(FullPathProperty) && loaded == false)
            {
                // Create a file to write to. 
                using (StreamWriter sw = File.CreateText(FullPathProperty))
                {
                    sw.WriteLine(this.StrengthProperty);
                    sw.Close();
                }
            }
        }
        #endregion

        #region Accessor Methods
        public int StrengthProperty
        {
            get
            {
                return strength;
            }
            set
            {
                strength = value;
            }
        }

        public PheromoneEnum TypeProperty
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
        #endregion

        #region Methods

        public void Update()
        {
            if (strength > 0)
            {
                strength -= DECAY_RATE;
                File.WriteAllText(FullPathProperty, String.Empty);
                if (File.Exists(FullPathProperty))
                {
                    // Create a file to write to. 
                    using (StreamWriter sw = File.CreateText(FullPathProperty))
                    {
                        sw.WriteLine(this.StrengthProperty);
                        sw.Close();
                    }
                }
            }
            else if (strength <= 0)
            {
                File.Delete(FullPathProperty);
            }
            
        }
        public void Add(int amount)
        {
            if (strength < MAX)
            {
                strength += amount;
                if (strength > MAX)
                {
                    strength = MAX;
                }
            }
        }
        #endregion
    }
}
