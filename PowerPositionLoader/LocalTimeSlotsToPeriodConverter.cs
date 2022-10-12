namespace PowerPositionLoader
{
    public class LocalTimeSlotsToPeriodConverter
    {
        public readonly Dictionary<int, string> PeriodToLocalTimeDictionary = new();

        public void DefinePeriodMappingToTimeSlots(DateTime cobDate)
        {
            var startTime = new DateTime(cobDate.Year, cobDate.Month, cobDate.Day, 0, 0, 0, DateTimeKind.Unspecified).Date.AddHours(-1);
            var endTime = startTime.AddHours(24);
            //not sure why it was mentioned to use Local london time,
            //if we need to convert we can get timezone (GMT Standard) and convert it here if needed.
            int period = 1;
            for(var dt = startTime; dt <= endTime; dt = dt.AddHours(1))
            {
                PeriodToLocalTimeDictionary.Add(period++, dt.ToString("HH:00"));
            }
        }
    }
}
