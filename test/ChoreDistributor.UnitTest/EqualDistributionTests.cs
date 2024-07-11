using ChoreDistributor.Business;
using ChoreDistributor.Business.Factories;
using ChoreDistributor.Models;
using NSubstitute;

namespace ChoreDistributor.UnitTest
{
    internal sealed class EqualDistributionTests
    {
        private EqualDistribution _choreDistribtion;
        private IRandomFactory _randomFactory;

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            _randomFactory = Substitute.For<IRandomFactory>();
            _randomFactory.Create().Returns(new Random());
            _choreDistribtion = new EqualDistribution(_randomFactory);
        }

        [Test]
        public void Distribute_MultiplePeopleSingleChore_RandomlyAssigned()
        {
            var foo = new Person("Foo");
            var bar = new Person("Bar");
            var people = new List<Person> { foo, bar };
            var chores = new List<Chore> { new("Chore1", 1) };

            var actual = _choreDistribtion.Distribute(people, chores);

            var fooWeights = actual[foo].Sum(c => c.Weighting);
            var barWeights = actual[bar].Sum(c => c.Weighting);
            var distributedWeights = new float[2] { fooWeights, barWeights };
            Assert.Multiple(() =>
            {
                CollectionAssert.Contains(distributedWeights, 1);
                CollectionAssert.Contains(distributedWeights, 0);
            });
        }

        [Test]
        public void Distribute_MultiplePeopleSingleChore_RandomlyAssignedToFoo()
        {
            _randomFactory.Create().Returns(new Random(1));
            var foo = new Person("Foo");
            var bar = new Person("Bar");
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
        public void Distribute_MultiplePeopleSingleChore_RandomlyAssignedToBar()
        {
            _randomFactory.Create().Returns(new Random(2));
            var foo = new Person("Foo");
            var bar = new Person("Bar");
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
        public void Distribute_SameNumberOfChoresAsPeople_OneChoreEach()
        {
            var people = new List<Person> { new("Name1"), new("Name2"), new("Name3") };
            var chores = new List<Chore> { new("Chore1", 1), new("Chore2", 1), new("Chore3", 1 ) };

            var actual = _choreDistribtion.Distribute(people, chores);

            Assert.Multiple(() =>
            {
                Assert.That(actual, Has.Count.EqualTo(3));
                foreach (var person in people)
                {
                    Assert.That(actual.Any(dc => dc.Key.Name == person.Name), Is.True);
                }
            });
        }

        [Test]
        public void Distribute_ChoresToPeople_ExactlyEqual([ValueSource(nameof(GetTestCases))] ExactlyEqualTestCase testCase)
        {
            var actual = _choreDistribtion.Distribute(testCase.People, testCase.Chores);

            Assert.Multiple(() =>
            {
                foreach (var person in testCase.People)
                {
                    Assert.That(actual[person].Sum(c => c.Weighting), Is.EqualTo(testCase.ExpectedWeight));
                }
            });
        }

        private static IEnumerable<ExactlyEqualTestCase> GetTestCases()
        {
            var foo = new Person("Foo");
            var bar = new Person("Bar");
            var twoPeople = new List<Person> { foo, bar };
            yield return new ExactlyEqualTestCase
            {
                People = twoPeople,
                Chores =
                [
                     new("Hoover", 9),
                     new("Wash pots", 8),
                     new("Empty bins", 11),
                     new("Mop floors", 4),
                     new("Polish surfices", 6)
                ],
                ExpectedWeight = 19
            };
            yield return new ExactlyEqualTestCase
            {
                People = twoPeople,
                Chores =
                [
                    new("Hoover", 2),
                    new("Wash pots", 1),
                    new("Empty bins", 8),
                    new("Mop floors", 1),
                    new("Polish surfices", 4)
                ],
                ExpectedWeight = 8
            };
            var threePeople = new List<Person>(twoPeople) { new("John") };
            yield return new ExactlyEqualTestCase
            {
                People = threePeople,
                Chores =
                [
                    new("Hoover", 2),
                    new("Wash pots", 1),
                    new("Empty bins", 8),
                    new("Mop floors", 1),
                    new("Polish surfices", 3),
                    new("Clean Cat Tray", 2),
                    new("Clean Oven", 10),
                    new("Chore", 1),
                    new("AnotherChore", 2)
                ],
                ExpectedWeight = 10
            };
        }

        public class ExactlyEqualTestCase
        {
            public IList<Person> People { get; set; } = [];
            public IList<Chore> Chores { get; set; } = [];
            public float ExpectedWeight { get; set; }
        }
    }
}