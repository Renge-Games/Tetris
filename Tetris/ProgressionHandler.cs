using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tetris
{
    public class ProgressionHandler
    {
        int delay;
        int counter;
        int level;
        int score;
        int spl;
        int spu;
        int splvl;

        public ProgressionHandler(int delay, int scorePerLine, int scorePerUpdate, int scorePerLevel)
        {
            this.delay = delay;
            counter = 0;
            level = 1;
            score = 0;
            spl = scorePerLine;
            spu = scorePerUpdate;
            splvl = scorePerLevel;
        }

        public bool UpdateReady
        {
            get
            {
                if(counter >= delay)
                {
                    return true;
                }
                return false;
            }
        }

        public int Score
        {
            get { return score; }
        }

        public int MaxScoreForLevel
        {
            get { return level * level * splvl; }
        }

        public int Level
        {
            get { return level; }
        }

        public void ResetTimerIfReady()
        {
            if (counter >= delay)
            {
                counter = 0;
            }
        }

        private void LevelUp()
        {
            level++;
            delay -= level;
            if (delay <= 10)
                delay = 10;
            if (level == 20)
                delay = 5;
        }

        public void LineCleared()
        {
            score += level * spl;
        }

        public void Update()
        {
            counter++;
            if (UpdateReady)
            {
                score += level * spu;
            }
            if (score >= MaxScoreForLevel)
                LevelUp();
        }

        public void Draw(SpriteBatch sb, Color color)
        {

        }
    }
}