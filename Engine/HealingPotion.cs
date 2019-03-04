namespace Engine
{
    public class HealingPotion : Item
    {
        public int AmountToHeal { get; set; }

        public HealingPotion(int iD, string name, string namePlural,int amountToHeal) :base(iD, name, namePlural)
        {
            AmountToHeal = amountToHeal;
        }
    }
}
