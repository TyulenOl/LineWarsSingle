using System.Linq;
using DataStructures;
using NUnit.Framework;

namespace Tests
{
    public class TestRegeneratingSequence
    {
        [Test]
        public void Test1()
        {
            var sequence = new ExclusionarySequence(0, new[] {1, 2, 4, 10});
            CollectionAssert.AreEqual(new[] {0, 3, 5, 6}, sequence.Take(4));
        }

        [Test]
        public void Test2()
        {
            var sequence = new ExclusionarySequence(0, new[] {1, 2, 4, 10});
            Assert.AreEqual(0, sequence.Peek());
            Assert.AreEqual(0, sequence.Peek());
            Assert.AreEqual(0, sequence.Pop());
            Assert.AreEqual(3, sequence.Peek());
            Assert.AreEqual(3, sequence.Pop());
            Assert.AreEqual(5, sequence.Peek());
        }

        [Test]
        public void Test3()
        {
            var sequence = new ExclusionarySequence(0, new[] {1, 2, 2, 2, 2, 2, 4, 10});
            CollectionAssert.AreEqual(new[] {0, 3, 5, 6}, sequence.Take(4));
        }
    }
}