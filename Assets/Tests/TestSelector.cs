using System.Collections.Generic;
using System.Linq;
using LineWars;
using NUnit.Framework;
using UnityEngine;

namespace Tests
{
    public class TestSelector
    {
        private GameObject gm1;
        private GameObject gm2;

        [SetUp]
        public void Setup()
        {
            gm1 = new GameObject();
            gm2 = new GameObject();
        }

        [TearDown]
        public void TearDown()
        {
            Object.Destroy(gm1);
            Object.Destroy(gm2);
        }

        [Test]
        public void Test1()
        {
            Selector.SelectedObject = gm1;
            Assert.AreEqual(gm1, Selector.SelectedObject);
            CollectionAssert.AreEqual(new[] {gm1}, Selector.SelectedObjects);
        }

        [Test]
        public void Test2()
        {
            Selector.SelectedObject = null;
            Assert.AreEqual(null, Selector.SelectedObject);
            CollectionAssert.AreEqual(Enumerable.Empty<GameObject>(), Selector.SelectedObjects);
        }

        [Test]
        public void Test3()
        {
            Selector.SelectedObjects = new[] {gm1, gm2};
            Assert.AreEqual(gm1, Selector.SelectedObject);
            CollectionAssert.AreEqual(new[] {gm1, gm2}, Selector.SelectedObjects);
        }

        [Test]
        public void Test4()
        {
            Selector.SelectedObjects = null;
            Assert.AreEqual(null, Selector.SelectedObject);
            CollectionAssert.AreEqual(Enumerable.Empty<GameObject>(), Selector.SelectedObjects);
        }
    }
}