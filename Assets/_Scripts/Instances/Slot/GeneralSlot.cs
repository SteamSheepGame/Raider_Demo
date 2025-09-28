namespace Demo.Core
{
    public class GeneralSlot: BaseSlot
    {
        public override bool TryAccept(ICard card)
        {
            if (IsFilled) return false;
            return true;
        }
    }
}