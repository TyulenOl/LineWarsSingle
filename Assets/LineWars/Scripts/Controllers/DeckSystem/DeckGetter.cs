using LineWars.Model;
using UnityEngine;

namespace LineWars.Controllers
{
    public abstract class DeckGetter: MonoBehaviour, IGetter<Deck>
    {
        public abstract bool CanGet();
        public abstract Deck Get();
    }
}