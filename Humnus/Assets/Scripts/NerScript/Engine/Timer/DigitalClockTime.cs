using UnityEngine;
using System.Collections;


namespace NerScript
{
    public struct DigitalClockTime
    {
        //public bool negative;
        public int day;
        public int hour;
        public int minute;
        public int second;

        public string DayString(string fix = "0") => day <= 9 ? fix + day : "" + day;
        public string HourString(string fix = "0") => hour <= 9 ? fix + hour : "" + hour;
        public string MinuteString(string fix = "0") => minute <= 9 ? fix + minute : "" + minute;
        public string SecondString(string fix = "0") => second <= 9 ? fix + second : "" + second;

        public DigitalClockTime(int _second = 0, int _minute = 0, int _hour = 0, int _day = 0)
        {
            //negative = false;
            second = _second;
            minute = _minute;
            hour = _hour;
            day = _day;
        }

        public void Reset()
        {
            day = hour = minute = second = 0;
        }

        public static DigitalClockTime operator +(DigitalClockTime time1, DigitalClockTime time2)
        {
            time1.DigitCheck();
            time2.DigitCheck();
            time1.day += time2.day;
            time1.hour += time2.hour;
            time1.minute += time2.minute;
            time1.second += time2.second;
            time1.DigitCheck();
            return time1;
        }
        public static DigitalClockTime operator -(DigitalClockTime time1, DigitalClockTime time2)
        {
            time1.DigitCheck();
            time2.DigitCheck();
            time1.day -= time2.day;
            time1.hour -= time2.hour;
            time1.minute -= time2.minute;
            time1.second -= time2.second;
            time1.DigitCheck();
            return time1;
        }

        public static bool operator <(DigitalClockTime clock1, DigitalClockTime clock2)
        {
            clock1.DigitCheck();
            clock2.DigitCheck();

            if (clock1.day < clock2.day) return true;
            else if (clock1.day > clock2.day) return false;
            if (clock1.hour < clock2.hour) return true;
            else if (clock1.hour > clock2.hour) return false;
            if (clock1.minute < clock2.minute) return true;
            else if (clock1.minute > clock2.minute) return false;
            if (clock1.second < clock2.second) return true;
            else if (clock1.second > clock2.second) return false;
            return false;
        }

        public static bool operator >(DigitalClockTime clock1, DigitalClockTime clock2)
        {
            clock1.DigitCheck();
            clock2.DigitCheck();

            if (clock1.day < clock2.day) return false;
            else if (clock1.day > clock2.day) return true;
            if (clock1.hour < clock2.hour) return false;
            else if (clock1.hour > clock2.hour) return true;
            if (clock1.minute < clock2.minute) return false;
            else if (clock1.minute > clock2.minute) return true;
            if (clock1.second < clock2.second) return false;
            else if (clock1.second > clock2.second) return true;
            return false;
        }

        public static bool operator ==(DigitalClockTime clock1, DigitalClockTime clock2)
        {
            clock1.DigitCheck();
            clock2.DigitCheck();

            if (clock1.day == clock2.day &&
                clock1.hour == clock2.hour &&
                clock1.minute == clock2.minute &&
                clock1.second == clock2.second) return true;
            return false;
        }

        public static bool operator !=(DigitalClockTime clock1, DigitalClockTime clock2)
        {
            clock1.DigitCheck();
            clock2.DigitCheck();

            if (clock1.day == clock2.day &&
                clock1.hour == clock2.hour &&
                clock1.minute == clock2.minute &&
                clock1.second == clock2.second) return false;
            return true;
        }

        public static bool operator <=(DigitalClockTime clock1, DigitalClockTime clock2)
        {
            if (clock1 < clock2 || clock1 == clock2) return true;
            return false;
        }

        public static bool operator >=(DigitalClockTime clock1, DigitalClockTime clock2)
        {
            if (clock1 > clock2 || clock1 == clock2) return true;
            return false;
        }


        public void DigitCheck()
        {
            //negative = false;
            MoveUpCheck();
            //MoveDownCheck();
        }

        private void MoveUpCheck()
        {
            while (60 <= second)
            {
                second -= 60;
                minute++;
            }
            while (60 <= minute)
            {
                minute -= 60;
                hour++;
            }
            while (24 <= hour)
            {
                hour -= 24;
                day++;
            }
        }

        private void MoveDownCheck()
        {
            while (second < 0)
            {
                second += 60;
                minute--;
            }
            while (minute < 0)
            {
                minute += 60;
                hour--;
            }
            while (hour < 0)
            {
                hour += 60;
                day--;
            }

            while (day < 0)
            {
                day++;
                hour -= 24;
            }


        }
    }
}
