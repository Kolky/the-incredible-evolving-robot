using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;

namespace Tier.Misc
{
  public static class HighScores
  {
    public struct HighScoreEntry
    {
      public String Player;
      public int Score;
      public int Level;
      public int Time;
    }

    private static LinkedList<HighScoreEntry> scores = new LinkedList<HighScoreEntry>();

    public static void AddEntry(String player, int score, int level, int time)
    {
      // Create New Entry
      HighScoreEntry newEntry = new HighScoreEntry();
      newEntry.Player = player;
      newEntry.Score = score;
      newEntry.Level = level;
      newEntry.Time = time;

      // Check if list is NOT-empty
      if (HighScores.scores.Count > 0)
      {
        // Amount of entry's allowed
        {
          Boolean added = false;

          LinkedList<HighScoreEntry>.Enumerator iter = HighScores.scores.GetEnumerator();
          while (iter.MoveNext())
          {
            if (iter.Current.Score < newEntry.Score)
            {
              HighScores.scores.AddBefore(HighScores.scores.Find(iter.Current), new LinkedListNode<HighScoreEntry>(newEntry));
              added = true;
              break;
            }
          }

          if (!added && HighScores.scores.Count < Options.HighScore.MaxEntries)
            HighScores.scores.AddLast(newEntry);
        }
      }
      else
        HighScores.scores.AddLast(newEntry);


      if (HighScores.scores.Count > Options.HighScore.MaxEntries)
      {
        for(int i = HighScores.scores.Count; i > Options.HighScore.MaxEntries; i--) 
          HighScores.scores.RemoveLast();
      }
    }

    public static LinkedList<HighScoreEntry> ListEntries()
    {
      return HighScores.scores;
    }
  }
}
