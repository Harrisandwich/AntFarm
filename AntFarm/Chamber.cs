using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntFarm
{
    public struct Chamber
    {
        #region Fields
        private ChamberType type;

        private const int MAX_OFFSPRING = 100;
        private const int MAX_FOOD = 5000;

        private int amountOfFood;
        private int amountOfOffspring;

        private int id;

        private bool capacityReached;

        private string location;
        #endregion


        #region Accessor Methods

        public int IDProperty
        {
            get
            {
                return id;
            }
            set
            {
                id = value;
                
            }

        }

        public int AmountOfFoodProperty
        {
            get
            {
                return amountOfFood;
            }
            set
            {
                amountOfFood = value;
                if (amountOfFood >= MAX_FOOD && type == ChamberType.FoodStash)
                {
                    capacityReached = true;
                }
                else
                {
                    capacityReached = false;
                }
            }
            
        }

        public int AmountOfOffspringProperty
        {
            get
            {
                return amountOfOffspring;
            }
            set
            {
                amountOfOffspring = value;
                if (amountOfOffspring >= MAX_OFFSPRING && type == ChamberType.Nursery)
                {
                    capacityReached = true;
                }
                else
                {
                    capacityReached = false;
                }
            }

        }

        public bool CapacityReachedProperty
        {
            get
            {
                return capacityReached;
            }
            set
            {
                capacityReached = value;
            }

        }

        public ChamberType TypeProperty
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

        public string LocationProperty
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

       
        #endregion


    }
}
