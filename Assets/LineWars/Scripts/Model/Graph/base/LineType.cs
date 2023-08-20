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
        public static bool CanUp(LineType lineType)
        {
            return lineType is LineType.InfantryRoad or LineType.CountryRoad;
        }

        public static bool CanDown(LineType lineType)
        {
            return lineType is LineType.Highway or LineType.CountryRoad;
        }
        
        public static LineType Up(LineType lineType)
        {
            if (CanUp(lineType))
                return ++lineType;
            return lineType;
        }
        
        public static LineType Down(LineType lineType)
        {
            if (CanDown(lineType))
                return --lineType;
            return lineType;
        }
    }
}