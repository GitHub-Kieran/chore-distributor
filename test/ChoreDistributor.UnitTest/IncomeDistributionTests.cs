using ChoreDistributor.Business;
using ChoreDistributor.Business.Factories;
using ChoreDistributor.Models;
using NSubstitute;

namespace ChoreDistributor.UnitTest
{
    internal sealed class IncomeDistributionTests
    {
        private IncomeDistribution _choreDistribtion;
        private IRandomFactory _randomFactory;

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            _randomFactory = Substitute.For<IRandomFactory>();
            _randomFactory.Create().Returns(new Random());
            _choreDistribtion = new IncomeDistribution(_randomFactory);
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
        public void Distribute_TwoPeopleSameIncomeSingleChore_RandomlyAssignedToFoo()
        {
            _randomFactory.Create().Returns(new Random(1));
            var foo = new Person("Foo") { Income = 30000 };
            var bar = new Person("Bar") { Income = 30000 };
            var people = new List<Person> { foo, bar };
            var chores = new List<Chore> { new("Chore1", 1) };

            var actual = _choreDistribtion.Distribute(people, chores);

            var fooWeights = actual[foo].Sum(c => c.Weighting);
            var barWeights = actual[bar].Sum(c => c.Weighting);
            Assert.Multiple(() =>
            {
                Assert.That(fooWeights, Is.EqualTo(1));
                Assert.That(barWeights, Is.EqualTo(0));
            });
        }

        [Test]
        public void Distribute_TwoPeopleSameIncomeSingleChore_RandomlyAssignedToBar()
        {
            _randomFactory.Create().Returns(new Random(2));
            var foo = new Person("Foo") { Income = 30000 };
            var bar = new Person("Bar") { Income = 30000 };
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

        [Test]
        public void Distribute_TwoPeopleSameIncomeFiveEqualChores_RandomlyAssignedMoreToFoo()
        {
            _randomFactory.Create().Returns(new Random(1));
            var foo = new Person("Foo") { Income = 30000 };
            var bar = new Person("Bar") { Income = 30000 };
            var people = new List<Person> { foo, bar };
            var chores = new List<Chore> { new("Chore1", 1), new("Chore2", 1), new("Chore3", 1), new("Chore4", 1), new("Chore5", 1) };

            var actual = _choreDistribtion.Distribute(people, chores);

            var fooWeights = actual[foo].Sum(c => c.Weighting);
            var barWeights = actual[bar].Sum(c => c.Weighting);
            Assert.Multiple(() =>
            {
                Assert.That(fooWeights, Is.EqualTo(3));
                Assert.That(barWeights, Is.EqualTo(2));
            });
        }

        [Test]
        public void Distribute_TwoPeopleSameIncomeFiveEqualChores_RandomlyAssignedMoreToBar()
        {
            _randomFactory.Create().Returns(new Random(2));
            var foo = new Person("Foo") { Income = 30000 };
            var bar = new Person("Bar") { Income = 30000 };
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
    }
}
