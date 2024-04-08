using ChoreDistributor.Business;
using ChoreDistributor.Models;

namespace ChoreDistributor.UnitTest
{
    public class LinearDistributionTests
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
                Assert.That(actual.First().Person.Name, Is.EqualTo(expectedPerson));
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
                    Assert.True(actual.Any(dc => dc.Person.Name == person.Name));
                }
            });
        }

        [Test]
        public void Distribute_ChoresToPeople_LinearlyEqual()
        {
            var people = new List<Person> { new("Foo"), new("Bar") };
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
                var actualFooCount = 0;
                var actualBarCount = 0;

                foreach (var distributedChore in actual)
                {
                    if (distributedChore.Person.Name == "Foo")
                    {
                        actualFooCount++;
                        Assert.IsTrue(expectedFooChores.Contains(distributedChore.Chore.Name));
                    }
                    else
                    {
                        actualBarCount++;
                        Assert.IsTrue(expectedBarChores.Contains(distributedChore.Chore.Name));
                    }
                }
                Assert.That(expectedFooChores.Count, Is.EqualTo(actualFooCount));
                Assert.That(expectedBarChores.Count, Is.EqualTo(actualBarCount));
            });
        }
    }
}