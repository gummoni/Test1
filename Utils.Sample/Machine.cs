using Processors;

public class Machine : ConnectableBase
{
    public Pipet Pipet1 { get; set; } = new Pipet() { Name = "P1" };
    public Pipet Pipet2 { get; set; } = new Pipet() { Name = "P2" };
    public XYZW XYZW { get; set; } = new XYZW();
}
