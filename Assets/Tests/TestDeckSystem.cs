using System.IO;
using LineWars.Controllers;
using LineWars.Model;
using NUnit.Framework;
using UnityEngine;

namespace Tests
{
    public class TestDeckSystem
    {
        private readonly SimpleFilePathGenerator<DeckInfo> filePathGenerator = new("json");

        private string[] files;
        
        private static ScriptableDeckCardsStorage CardsDatabase => GameRoot.Instance.CardsDatabase;
        private static DecksController DecksController => GameRoot.Instance.DecksController;

        [SetUp]
        public void SetUp()
        {
            files = new[]
            {
                filePathGenerator.GeneratePath(0),
                filePathGenerator.GeneratePath(1),
                filePathGenerator.GeneratePath(2),
            };

            foreach (var file in files)
            {
                if (File.Exists(file))
                    File.Delete(file);
            }
            
            var gameRootPrefab = Resources.Load<GameRoot>("GameRoot");
            Object.Instantiate(gameRootPrefab);
        }

        [Test]
        public void SimpleTest()
        {
            var builder = DecksController.StartBuildNewDeck();
            Assert.AreEqual(builder.DeckId, 0);
            
            builder.SetName("Test");
            Assert.AreEqual(builder.DeckName, "Test");
     

            builder.AddCard(CardsDatabase.IdToCard[0]);
            CollectionAssert.AreEqual(builder.CurrentCards, new[] {CardsDatabase.IdToCard[0]});
            
            var deck = DecksController.FinishBuildDeck(builder);
            Assert.IsTrue(File.Exists(files[0]));
        }
    }
}