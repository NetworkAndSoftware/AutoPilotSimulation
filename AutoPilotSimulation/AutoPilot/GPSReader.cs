using System;
using System.Collections.Generic;
using System.Linq;
using Geometry;

namespace AutoPilotSimulation.AutoPilot
{
  internal class GPSReader
  {
    private readonly IGPS _gps;
    private readonly Queue<GPSReading> _gpsReadings = new Queue<GPSReading>(1000);

    public GPSReader(IGPS gps)
    {
      _gps = gps;
    }

    public void UpdateGPSReadings()
    {
      if (_gpsReadings.Count != 0 && _gps.LastReading == _gpsReadings.Last())
        return;

      if (_gpsReadings.Count >= 1000)
        _gpsReadings.Dequeue();

      _gpsReadings.Enqueue(_gps.LastReading);
    }

    public GPSReading MostRecentGPSReading()
    {
      return _gpsReadings.Last();
    }

    public Bearing GetCurrectCourseEstimate()
    {
      IOrderedEnumerable<GPSReading> lastsecond =
        _gpsReadings.Where(reading => DateTime.Now - reading.Time < new TimeSpan(0, 0, 10))
          .OrderByDescending(reading => reading.Time);
      if (lastsecond.Count() > 1)
      {
        GPSReading[] array = lastsecond.ToArray();
        return array[1].Coordinate.InitialCourse(array[0].Coordinate);
      }
      return null;
    }
  }
}