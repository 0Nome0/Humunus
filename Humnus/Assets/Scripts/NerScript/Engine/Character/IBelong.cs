using System;
using System.Collections.Generic;
using System.Linq;


public enum BelongTeams
{
    Enemy = -1,
    None = 0,
    Player = 1,
    Item,
}


public interface IBelong
{
    BelongTeams BelongTeam { get; }
}
