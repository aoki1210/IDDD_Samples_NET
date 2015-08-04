using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SaaSOvation.Common.Domain.Model;

namespace SaaSOvation.AgilePM.Domain.Model.Products.BacklogItems
{
    public class �r�W�l�X�D��x : ValueObject
    {
        public �r�W�l�X�D��x(�r�W�l�X�D��x���[�e�B���O ratings)
        {
            AssertionConcern.AssertArgumentNotNull(ratings, "The ratings must be provided.");
            this.Ratings = ratings;
        }

        public �r�W�l�X�D��x���[�e�B���O Ratings { get; private set; }

        public float CostPercentage(�r�W�l�X�D��x���v totals)
        {
            return (float)100 * this.Ratings.Cost / totals.TotalCost;
        }

        public float Priority(�r�W�l�X�D��x���v totals)
        {
            var costAndRisk = CostPercentage(totals) + RiskPercentage(totals);
            return ValuePercentage(totals) / costAndRisk;
        }

        public float RiskPercentage(�r�W�l�X�D��x���v totals)
        {
            return (float)100 * this.Ratings.Risk / totals.TotalRisk;
        }

        public float ValuePercentage(�r�W�l�X�D��x���v totals)
        {
            return (float)100 * this.TotalValue / totals.TotalValue;
        }

        public float TotalValue
        {
            get
            {
                return this.Ratings.Benefit + this.Ratings.Penalty;
            }
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return this.Ratings;
        }
    }
}
