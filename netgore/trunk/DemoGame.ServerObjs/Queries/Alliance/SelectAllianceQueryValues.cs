namespace DemoGame.Server.Queries
{
    public struct SelectAllianceQueryValues
    {
        public readonly AllianceID ID;
        public readonly string Name;

        public SelectAllianceQueryValues(AllianceID id, string name)
        {
            ID = id;
            Name = name;
        }
    }
}