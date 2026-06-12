using System.Collections.Generic;
namespace BattleShip.Functions;

public static class ShipMover
{
    public static List<Cell> MoveUp(List<Cell> cells)
    {
        List<Cell> new_cells = new List<Cell>();
        foreach (Cell cell in cells)
        {
            new_cells.Add(new Cell(cell.X, cell.Y - 1));

        }
        return new_cells;

    }
    public static List<Cell> MoveDown(List<Cell> cells)
    {
        List<Cell> new_cells = new List<Cell>();
        foreach (Cell cell in cells)
        {
            new_cells.Add(new Cell(cell.X, cell.Y + 1));
        }
        return new_cells;
    }
    public static List<Cell> MoveLeft(List<Cell> cells)
    {
        List<Cell> new_cells = new List<Cell>();
        foreach (Cell cell in cells)
        {
            new_cells.Add(new Cell(cell.X - 1, cell.Y));
        }
        return new_cells;
    }
    public static List<Cell> MoveRight(List<Cell> cells)
    {
        List<Cell> new_cells = new List<Cell>();
        foreach (Cell cell in cells)
        {
            new_cells.Add(new Cell(cell.X + 1, cell.Y));
        }
        return new_cells;
    }
    public static List<Cell> Rotate(List<Cell> cells)
    {
        // Pivot (Drehpunkt)
        Cell pivot = cells[0];

        List<Cell> rotated = new List<Cell>();

        foreach (var c in cells)
        {
            // relativ zum Pivot
            int relX = c.X - pivot.X;
            int relY = c.Y - pivot.Y;

            // 90° Uhrzeigersinn: (x, y) -> (y, -x)
            int newX = pivot.X - relY;
            int newY = pivot.Y + relX;

            rotated.Add(new Cell(newX, newY));
        }

        return rotated;
    }
}