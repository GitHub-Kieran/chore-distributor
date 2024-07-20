using ChoreDistributor.Business;
using ChoreDistributor.Models;

namespace ChoreDistributor.UnitTest
{
    internal sealed class LinearDistributionTests
    {
        private IChoreDistribution _choreDistribtion;

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            _choreDistribtion = new LinearDistribution();
        }

        [Test]
        public void Distribute_MultiplePeopleSingleChore_FirstPersonAssigned()
        {
            var expectedPerson = "Name1";
            var people = new List<Person> { new(expectedPerson), new("Name2") };
            var chores = new List<Chore> { new("Chore1", 1) };

            var actual = _choreDistribtion.Distribute(people, chores);

            Assert.Multiple(() =>
            {
                Assert.That(actual.Count, Is.EqualTo(1));
                Assert.That(actual.First().Key.Name, Is.EqualTo(expectedPerson));
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
                Assert.That(actual.Count, Is.EqualTo(3));
                foreach (var person in people)
                {
                    Assert.True(actual.Any(dc => dc.Key.Name == person.Name));
                }
            });
        }

        [Test]
        public void Distribute_ChoresToPeople_LinearlyEqual()
        {
            var foo = new Person("Foo");
            var bar = new Person("Bar");
            var people = new List<Person> { foo, bar };
            var chores = new List<Chore>
            { 
                new("Hoover", 6),
                new("Wash pots", 6), 
                new("Empty bins", 6),
                new("Mop floors", 5),
                new("Polish surfices", 5)
            };
            var expectedFooChores = new List<string> { "Hoover", "Wash pots", "Empty bins" };
            var expectedBarChores = new List<string> { "Mop floors", "Polish surfices" };

            var actual = _choreDistribtion.Distribute(people, chores);

            Assert.Multiple(() =>
            {
                var actualFooChores = actual[foo];
                foreach (var fooChore in expectedFooChores)
                {
                    Assert.IsTrue(actualFooChores.Any(c => c.Name == fooChore));
                }

                var actualBarChores = actual[bar];
                foreach (var barChore in expectedBarChores)
                {
                    Assert.IsTrue(actualBarChores.Any(c => c.Name == barChore));
                }

                Assert.That(expectedFooChores.Count, Is.EqualTo(actualFooChores.Count));
                Assert.That(expectedBarChores.Count, Is.EqualTo(actualBarChores.Count));
            });
        }
    }
}