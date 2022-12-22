
public enum ResourcesType
{
    MetalScrap,
    InfluencePoints,
    ExplorationPoints,
    PowerSupplyTotal,
    PowerSupplyUsed,
    Unhexium,
}


public class TestCodeStuff
{
    bool _isAlive;
    bool _isImmune;

    public bool IsValidTarget()
        => _isAlive && !_isImmune;

    public void ToggleAlive()
    {
        _isAlive = !_isAlive;
    }


}

