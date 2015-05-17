using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.IO;

namespace AntFarm
{
    class Program
    {
        //This program used to be a console program. 
        static AntFarmForm mainForm = new AntFarmForm();
        [STAThreadAttribute]
        static void Main(string[] args)
        {
         
            
            mainForm.ShowDialog();
            mainForm.Focus();
              
        }

        
    }
}
