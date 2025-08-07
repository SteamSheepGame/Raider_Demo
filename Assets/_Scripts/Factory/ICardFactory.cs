using UnityEngine;

namespace Demo.Core
{
    public interface ICardFactory
    {
        public ICardView CreateCard(IEntity entity);
    }
}