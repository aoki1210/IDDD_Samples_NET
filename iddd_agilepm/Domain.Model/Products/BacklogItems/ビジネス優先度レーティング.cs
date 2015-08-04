using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SaaSOvation.Common.Domain.Model;

namespace SaaSOvation.AgilePM.Domain.Model.Products.BacklogItems
{
    public class ビジネス優先度レーティング : ValueObject
    {
        public ビジネス優先度レーティング(int benefit, int penalty, int cost, int risk)
        {
            AssertionConcern.AssertArgumentRange(benefit, 1, 9, "Relative benefit must be between 1 and 9.");
            AssertionConcern.AssertArgumentRange(penalty, 1, 9, "Relative penalty must be between 1 and 9.");
            AssertionConcern.AssertArgumentRange(cost, 1, 9, "Relative cost must be between 1 and 9.");
            AssertionConcern.AssertArgumentRange(risk, 1, 9, "Relative risk must be between 1 and 9.");

            this.Benefit = benefit;
            this.Penalty = penalty;
            this.Cost = cost;
            this.Risk = risk;
        }

        public int Benefit { get; private set; }
        public int Penalty { get; private set; }
        public int Cost { get; private set; }
        public int Risk { get; private set; }

        public ビジネス優先度レーティング WithAdjustedBenefit(int benefit)
        {
            return new ビジネス優先度レーティング(benefit, this.Penalty, this.Cost, this.Risk);
        }

        public ビジネス優先度レーティング WithAdjustedCost(int cost)
        {
            return new ビジネス優先度レーティング(this.Benefit, this.Penalty, cost, this.Risk);
        }

        public ビジネス優先度レーティング WithAdjustedPentality(int penalty)
        {
            return new ビジネス優先度レーティング(this.Benefit, penalty, this.Cost, this.Risk);
        }

        public ビジネス優先度レーティング WithAdjustedRisk(int risk)
        {
            return new ビジネス優先度レーティング(this.Benefit, this.Penalty, this.Cost, risk);
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return this.Benefit;
            yield return this.Penalty;
            yield return this.Cost;
            yield return this.Risk;
        }
    }
}
