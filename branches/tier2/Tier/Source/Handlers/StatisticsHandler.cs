using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace Tier.Source.Handlers
{
  public enum StatisticType
  {
    ST_POWERUPCOUNT
  };

  public class StatisticsHandler
  {
    private TierGame game;
    private Hashtable statistics;

    public StatisticsHandler(TierGame game)
    {
      this.game = game;
      this.statistics = new Hashtable();
    }

    public void AddStatistic(StatisticType type)
    {
      long value = 0;

      if (statistics[type] != null)
      {
        value = (long)statistics[type];
      }

      statistics[type] = value + 1;
    }

    public void Reset()
    {
      statistics[StatisticType.ST_POWERUPCOUNT] = (long)0;
    }

    public long GetStatistic(StatisticType type)
    {
      if (statistics[type] == null)
        return 0;

      return (long)statistics[type];
    }
  }
}
