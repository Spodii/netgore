namespace DemoGame.Server
{
    public class NPCInventory : CharacterInventory
    {
// ReSharper disable SuggestBaseTypeForParameter
        public NPCInventory(NPC npc) : base(npc) // ReSharper restore SuggestBaseTypeForParameter
        {
        }
    }
}