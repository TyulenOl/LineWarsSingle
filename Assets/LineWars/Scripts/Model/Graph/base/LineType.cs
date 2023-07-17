namespace LineWars.Model
{
    // Линии по которым можно хотить всегда больше нуля.
    public enum LineType
    {
        Visibility = -2,
        Firing = -1,
        ScoutRoad = 0,
        InfantryRoad = 1,
        CountryRoad = 2,
        Highway = 3
    }

    public static class LineTypeHelper
    {
        public static LineType Up(LineType lineType)
        {
            if (lineType < LineType.InfantryRoad || lineType == LineType.Highway)
                return lineType;
            return ++lineType;
        }
        
        public static LineType Down(LineType lineType)
        {
            if (lineType <= LineType.InfantryRoad)
                return lineType;
            return --lineType;
        }
    }
}