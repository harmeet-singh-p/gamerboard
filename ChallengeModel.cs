using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameProj
{
    public class ChallengeModel
    {
        public string GameCode { get; set; }

        public string GameName { get; set; }

        public string ImageFile { get; set; }

        public string ChallengeCode { get; set; }

        public string ChallengeName { get; set; }

        public string ChallengeType { get; set; }

        public string ChallengeDetail { get; set; }

        public string ChallengeRem { get; set; }

        public int MyProperty { get; set; }

        public string ChallengeStartDate { get; set; }

        public string ChallengeEndDate { get; set; }

        public string ChallengeStartTime { get; set; }

        public string ChallengeEndTime { get; set; }

        public int FeePts { get; set; }

        public decimal FeeAmt { get; set; }

        public int RewardPts { get; set; }

        public decimal RewardAmt { get; set; }

        public string CreatedBy { get; set; }

        public string ChallengeFee 
        { 
            get 
            {
                string resultString = string.Empty;
                if(FeePts > 0)
                {
                    resultString = FeePts.ToString();
                }
                if(FeeAmt > 0 && !string.IsNullOrEmpty(resultString))
                {
                    resultString += " - ";
                }
                if (FeeAmt > 0)
                {
                    resultString += FeeAmt.ToString() + "LP";
                }
                return resultString;
            }
        }

        public string ChallengeReward
        {
            get
            {
                string resultString = string.Empty;
                if (RewardPts > 0)
                {
                    resultString = RewardPts.ToString();
                }
                if (RewardAmt > 0 && !string.IsNullOrEmpty(resultString))
                {
                    resultString += " - ";
                }
                if (RewardAmt > 0)
                {
                    resultString += RewardAmt.ToString() + "LP";
                }
                return resultString;
            }
        }
    }
}
