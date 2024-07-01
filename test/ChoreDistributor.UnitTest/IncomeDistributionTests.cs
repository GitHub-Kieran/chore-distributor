using ChoreDistributor.Business;
using ChoreDistributor.Models;

namespace ChoreDistributor.UnitTest
{
    internal sealed class IncomeDistributionTests
    {
        // TODO: Create SeededRandom with IRandom interface
        private IncomeDistribution _choreDistribtion;

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            _choreDistribtion = new IncomeDistribution();
        }

        [Test]
        public void Distribute_TwoPeopleSingleChore_LowestIncomeAssigned()
        {
            var foo = new Person("Foo") { Income = 30000 };
            var bar = new Person("Bar") { Income = 20000 };
            var people = new List<Person> { foo, bar };
            var chores = new List<Chore> { new("Chore1", 1) };

            var actual = _choreDistribtion.Distribute(people, chores);

            var fooWeights = actual[foo].Sum(c => c.Weighting);
            var barWeights = actual[bar].Sum(c => c.Weighting);
            Assert.Multiple(() =>
            {
                Assert.That(fooWeights, Is.EqualTo(0));
                Assert.That(barWeights, Is.EqualTo(1));
            });
        }

        [Test]
        public void Distribute_TwoPeopleSameIncomeSingleChore_RandomlyAssigned()
        {
            var foo = new Person("Foo") { Income = 30000 };
            var bar = new Person("Bar") { Income = 20000 };
            var people = new List<Person> { foo, bar };
            var chores = new List<Chore> { new("Chore1", 1) };

            var actual = _choreDistribtion.Distribute(people, chores);

            var fooWeights = actual[foo].Sum(c => c.Weighting);
            var barWeights = actual[bar].Sum(c => c.Weighting);
            Assert.Multiple(() =>
            {
                Assert.That(fooWeights, Is.EqualTo(0));
                Assert.That(barWeights, Is.EqualTo(1));
            });
        }

        [Test]
        public void Distribute_TwoPeopleFiveEqualChores_LowestIncomeAssignedMore()
        {
            var foo = new Person("Foo") { Income = 30000 };
            var bar = new Person("Bar") { Income = 20000 };
            var people = new List<Person> { foo, bar };
            var chores = new List<Chore> { new("Chore1", 1), new("Chore2", 1), new("Chore3", 1), new("Chore4", 1), new("Chore5", 1) };

            var actual = _choreDistribtion.Distribute(people, chores);

            var fooWeights = actual[foo].Sum(c => c.Weighting);
            var barWeights = actual[bar].Sum(c => c.Weighting);
            Assert.Multiple(() =>
            {
                Assert.That(fooWeights, Is.EqualTo(2));
                Assert.That(barWeights, Is.EqualTo(3));
            });
        }

        [Test]
        public void Distribute_TwoPeopleSameIncomeFiveEqualChores_RandomlyAssignedMore()
        {
            var foo = new Person("Foo") { Income = 30000 };
            var bar = new Person("Bar") { Income = 30000 };
            var people = new List<Person> { foo, bar };
            var chores = new List<Chore> { new("Chore1", 1), new("Chore2", 1), new("Chore3", 1), new("Chore4", 1), new("Chore5", 1) };

            var actual = _choreDistribtion.Distribute(people, chores);

            var fooWeights = actual[foo].Sum(c => c.Weighting);
            var barWeights = actual[bar].Sum(c => c.Weighting);
            var distributedWeights = new float[2] { fooWeights, barWeights };
            Assert.Multiple(() =>
            {
                CollectionAssert.Contains(distributedWeights, 3);
                CollectionAssert.Contains(distributedWeights, 2);
            });
        }
    }
}
