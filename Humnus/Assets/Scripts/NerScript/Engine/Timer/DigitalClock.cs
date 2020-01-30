using UnityEngine;
using System.Collections;

namespace NerScript
{
    public class DigitalClock
    {
        private DigitalClockTime time;
        public DigitalClockTime Time { get { return time; } }


        private int Day { get { return time.day; } set { time.day = value; time.DigitCheck(); } }
        private int Hour { get { return time.hour; } set { time.hour = value; time.DigitCheck(); } }
        private int Minute { get { return time.minute; } set { time.minute = value; time.DigitCheck(); } }
        private int Second { get { return time.second; } set { time.second = value; time.DigitCheck(); } }

        public DigitalClock(byte _second = 0, byte _minute = 0, byte _hour = 0, byte _day = 0)
        {
            time = new DigitalClockTime(_second, _minute, _hour, _day);
        }

        public void SetDay(int _day) => Day = _day;
        public void SetHour(int _hour) => Hour = _hour;
        public void SetMinute(int _minute) => Minute = _minute;
        public void SetSecond(int _second) => Second = _second;
        public void Reset() => time.Reset();

        public void AddDay(int add = 0) { Hour += add; }
        public void AddHour(int add = 0) { Hour += add; }
        public void AddMinute(int add = 0) { Minute += add; }
        public void AddSecond(int add = 0) { Second += add; }


        public static DigitalClock operator +(DigitalClock clock1, DigitalClock clock2)
        {
            return clock1 + clock2;
        }
        public static DigitalClock operator -(DigitalClock clock1, DigitalClock clock2)
        {
            return clock1 - clock2;
        }


        /// <summary>
        /// dd,hh,mm,ss,
        /// </summary>
        /// <param name="opt"></param>
        /// <returns></returns>
        public string GetString(string opt)
        {
            opt = opt.Replace("/dd/", time.DayString());
            opt = opt.Replace("/hh/", time.HourString(" "));
            opt = opt.Replace("/mm/", time.MinuteString());
            opt = opt.Replace("/ss/", time.SecondString());
            return opt;
        }
    }
}
