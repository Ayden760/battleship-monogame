using System.Collections.Generic;

public class ShipBase
{

    public int Length { get; set; }
    public int Hits { get; set; }
    public bool IsPlaced { get; set; } = false;
    public bool Destroyed => Hits >= Length;
    public List<Cell> Location { get; set; }
    public ShipBase(int length)
    {
        Length = length;
        Location = new List<Cell>();
    }
    public ShipBase(ShipBase other)
    {
        Length = other.Length;
        IsPlaced = other.IsPlaced;

        Location = new List<Cell>();

        foreach (var cell in other.Location)
        {
            Location.Add(new Cell
            {
                X = cell.X,
                Y = cell.Y
            });
        }
    }
}