namespace Nintendo.Blitz
{
    public enum VersusRule : int
    {
        None,
        TurfWar,
        SplatZones,
        TowerControl,
        Rainmaker,
        ClamBlitz,

        // Blitz internal names
        cPnt = TurfWar,
        cVar = SplatZones,
        cVlf = TowerControl,
        cVgl = Rainmaker,
        cVcl = ClamBlitz
    }
}