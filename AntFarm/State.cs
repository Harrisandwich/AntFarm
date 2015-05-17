using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntFarm
{
    public enum State
    {
        Scouting,  //searching randomly for food to gather or a trail to follow
        Gathering, //has found food and is following pheromones to and from the nest
        Carrying,  //seperate from gathering, this is for if the ant is moving the queen or offspring
        Attacking, //The ant is confronting a threat to the nest
        Defending, //the ant is preventing a threat from entering the nest
        Digging,   //the ant is creating tunnels and/or chambers for the nest 
        Investigating, //the ant has discovered a trail, but is not sure were it leads
        ReturningToNest, //the ant is returning to the nest for whatever reason other then gathering
        Eating, //the ant is feeding itself
        Feeding,//the ant is feeding another ant
        Standby,//the ant is not moving and is unsure what its purpose is
        LayingEggs,//the queen is laying eggs 
        TendingToEggs, //an ant is watching the eggs, and feeding the larvae
        TendingToQueen, //an ant is watching the queen and tending to her needs
        MovingEggs
    }
}
