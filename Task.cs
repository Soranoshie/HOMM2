namespace Inheritance.MapObjects
{
    public interface IMapObjectVisitor
    {
        void Visit(Dwelling dwelling);
        void Visit(Mine mine);
        void Visit(Creeps creeps);
        void Visit(Wolves wolves);
        void Visit(ResourcePile resourcePile);
    }
    
    public interface IMapObject
    {
        void Accept(IMapObjectVisitor visitor);
    }
    
    public interface IOwner
    {
        int Owner { set; }
    }

    public interface IArmy
    {
        Army Army { get; }
    }

    public interface ITreasure
    {
        Treasure Treasure { get; }
    }

    public class Dwelling : IOwner, IMapObject
    {
        public int Owner { get; set; }
        
        public void Accept(IMapObjectVisitor visitor)
        {
            visitor.Visit(this);
        }
    }

    public class Mine : IOwner, IArmy, ITreasure, IMapObject
    {
        public int Owner { get; set; }
        public Army Army { get; set; }
        public Treasure Treasure { get; set; }
        
        public void Accept(IMapObjectVisitor visitor)
        {
            visitor.Visit(this);
        }
    }

    public class Creeps : IArmy, ITreasure, IMapObject
    {
        public Army Army { get; set; }
        public Treasure Treasure { get; set; }
        
        public void Accept(IMapObjectVisitor visitor)
        {
            visitor.Visit(this);
        }
    }

    public class Wolves : IArmy, IMapObject
    {
        public Army Army { get; set; }
        
        public void Accept(IMapObjectVisitor visitor)
        {
            visitor.Visit(this);
        }
    }

    public class ResourcePile : ITreasure, IMapObject
    {
        public Treasure Treasure { get; set; }
        
        public void Accept(IMapObjectVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
    
    public class InteractionVisitor : IMapObjectVisitor
    {
        private Player player;

        public InteractionVisitor(Player player)
        {
            this.player = player;
        }

        public void Visit(Dwelling dwelling)
        {
            dwelling.Owner = player.Id;
        }

        public void Visit(Mine mine)
        {
            if (!player.CanBeat(mine.Army))
            {
                player.Die();
            }
            else
            {
                mine.Owner = player.Id;
                player.Consume(mine.Treasure);
            }
        }

        public void Visit(Creeps creeps)
        {
            if (player.CanBeat(creeps.Army))
                player.Consume(creeps.Treasure);
            else
                player.Die();
        }

        public void Visit(Wolves wolves)
        {
            // логика волка
        }

        public void Visit(ResourcePile resourcePile)
        {
            player.Consume(resourcePile.Treasure);
        }
    }

    public static class Interaction
    {
        public static void Make(Player player, IMapObject mapObject)
        {
            var visitor = new InteractionVisitor(player);
            mapObject.Accept(visitor);
        }
    }
}