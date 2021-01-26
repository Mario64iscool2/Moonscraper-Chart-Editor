// Copyright (c) 2016-2020 Alexander Ong
// See LICENSE in project root for license information.

namespace MoonscraperChartEditor.Song
{
    [System.Serializable]
    public class Handshape : ChartObject
    {
        private readonly ID _classID = ID.Handshape;

        public override int classID { get { return (int)_classID; } }

        public uint length;

        public Handshape(uint _position, uint _length) : base(_position)
        {
            length = _length;
        }

        public Handshape(Handshape _handShape) : base(_handShape.tick)
        {
            length = _handShape.length;
        }

        public override SongObject Clone()
        {
            return new Handshape(this);
        }

        public override bool AllValuesCompare<T>(T songObject)
        {
            if (this == songObject && (songObject as Handshape).length == length)
                return true;
            else
                return false;
        }

        public uint GetCappedLengthForPos(uint pos)
        {
            uint newLength = length;
            if (pos > tick)
                newLength = pos - tick;
            else
                newLength = 0;

            Handshape nextHandshape = null;
            if (song != null && chart != null)
            {
                int arrayPos = SongObjectHelper.FindClosestPosition(this, chart.handShape);
                if (arrayPos == SongObjectHelper.NOTFOUND)
                    return newLength;

                while (arrayPos < chart.handShape.Count - 1 && chart.handShape[arrayPos].tick <= tick)
                {
                    ++arrayPos;
                }

                if (chart.handShape[arrayPos].tick > tick)
                    nextHandshape = chart.handShape[arrayPos];

                if (nextHandshape != null)
                {
                    // Cap sustain length
                    if (nextHandshape.tick < tick)
                        newLength = 0;
                    else if (pos > nextHandshape.tick)
                        // Cap sustain
                        newLength = nextHandshape.tick - tick;
                }
                // else it's the only starpower or it's the last starpower 
            }

            return newLength;
        }

        public void SetLengthByPos(uint pos)
        {
            length = GetCappedLengthForPos(pos);
        }

        public void CopyFrom(Handshape hs)
        {
            tick = hs.tick;
            length = hs.length;
        }
    }
}
