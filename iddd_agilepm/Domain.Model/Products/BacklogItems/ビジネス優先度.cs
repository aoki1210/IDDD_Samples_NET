using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SaaSOvation.Common.Domain.Model;

namespace SaaSOvation.AgilePM.Domain.Model.Products.BacklogItems
{
    public class ビジネス優先度 : ValueObject
    {
        public ビジネス優先度(ビジネス優先度レーティング ratings)
        {
            AssertionConcern.AssertArgumentNotNull(ratings, "The ratings must be provided.");
            this.Ratings = ratings;
        }

        public ビジネス優先度レーティング Ratings { get; private set; }

        public float CostPercentage(ビジネス優先度合計 totals)
        {
            return (float)100 * this.Ratings.Cost / totals.TotalCost;
        }

        public float Priority(ビジネス優先度合計 totals)
        {
            var costAndRisk = CostPercentage(totals) + RiskPercentage(totals);
            return ValuePercentage(totals) / costAndRisk;
        }

        public float RiskPercentage(ビジネス優先度合計 totals)
        {
            return (float)100 * this.Ratings.Risk / totals.TotalRisk;
        }

        public float ValuePercentage(ビジネス優先度合計 totals)
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
