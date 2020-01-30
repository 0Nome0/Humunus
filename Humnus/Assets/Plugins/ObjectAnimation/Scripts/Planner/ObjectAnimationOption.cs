using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace NerScript.Anime
{
    public enum AnimOption
    {
        RejectOthers, //再生している間、他のアニメを受け付けない
        RejectOthersForever, //再生してから永遠に他のアニメを受け付けない
        RemoveOthers, //再生する際に再生中のアニメを強制終了
        RunThisOnly, //このアニメのみ再生させる
    }
    [Serializable]
    internal class ObjectAnimationOption
    {

        [SerializeField] internal bool rejectOthers = false;
        [SerializeField] internal bool rejectOthersForever = false;
        [SerializeField] internal bool removeOthers = false;
        [SerializeField] internal bool runThisOnly = false;


        internal ObjectAnimationOption() { }

        public override string ToString()
        {
            StringBuilder str = new StringBuilder();

            str.Append("RejectOthers").Append(" : ").AppendLine(rejectOthers.ToString());
            str.Append("RejectOthersForever").Append(" : ").AppendLine(rejectOthersForever.ToString());
            str.Append("RemoveOthers").Append(" : ").AppendLine(removeOthers.ToString());
            str.Append("RunThisOnly").Append(" : ").AppendLine(runThisOnly.ToString());



            return str.ToString();
        }
    }

    public static partial class ObjectAnim
    {
        public static AnimationPlanner RejectOthers(this AnimationPlanner planner)
        {
            planner.option.rejectOthers = true;
            return planner;
        }
        public static AnimationPlanner RejectOthersForever(this AnimationPlanner planner)
        {
            planner.RejectOthers();
            planner.option.rejectOthersForever = true;
            return planner;
        }
        public static AnimationPlanner RemoveOthers(this AnimationPlanner planner)
        {
            planner.option.removeOthers = true;
            return planner;
        }
        public static AnimationPlanner RunThisOnly(this AnimationPlanner planner)
        {
            planner.RemoveOthers().RejectOthers();
            planner.option.runThisOnly = true;
            return planner;
        }
    }

}
