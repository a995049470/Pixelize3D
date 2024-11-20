using Lost.Runtime.Footstone.Core;

namespace Lost.Runtime.Footstone.Game
{
    

    public class GameData<AComponent>
    {
        public AComponent Component1;
    }


    public class SimpleGameEntityProcessor<AComponent> : GameEntityProcessor<AComponent, GameData<AComponent>> where AComponent : EntityComponent
    {
        
        protected override GameData<AComponent> GenerateComponentData(Entity entity, AComponent component)
        {
            return new()
            {
                Component1 = component
            };
        }

        protected override bool IsAssociatedDataValid(Entity entity, AComponent component, GameData<AComponent> associatedData)
        {
            return associatedData.Component1 == component;
        }
    }

    public class GameData<AComponent, BComponent>
    {
        public AComponent Component1;
        public BComponent Component2;
    }

    public class SimpleGameEntityProcessor<AComponent, BComponent> : GameEntityProcessor<AComponent, GameData<AComponent, BComponent>> where AComponent : EntityComponent where BComponent : EntityComponent
    {
        public SimpleGameEntityProcessor() : base(typeof(BComponent))
        {

        }

        protected override GameData<AComponent, BComponent> GenerateComponentData(Entity entity, AComponent component)
        {
            return new()
            {
                Component1 = component,
                Component2 = entity.Get<BComponent>()
            };
        }

        protected override bool IsAssociatedDataValid(Entity entity, AComponent component, GameData<AComponent, BComponent> associatedData)
        {
            return associatedData.Component1 == component &&
            associatedData.Component2 == entity.Get<BComponent>();
        }
    }

    public class GameData<AComponent, BComponent, CComponent>
    {
        public AComponent Component1;
        public BComponent Component2;
        public CComponent Component3;
    }

    public class SimpleGameEntityProcessor<AComponent, BComponent, CComponent> : GameEntityProcessor<AComponent, GameData<AComponent, BComponent, CComponent>> where AComponent : EntityComponent where BComponent : EntityComponent where CComponent :EntityComponent
    {
        public SimpleGameEntityProcessor() : base(typeof(BComponent), typeof(CComponent))
        {

        }
        protected override GameData<AComponent, BComponent, CComponent> GenerateComponentData(Entity entity, AComponent component)
        {
            return new()
            {
                Component1 = component,
                Component2 = entity.Get<BComponent>(),
                Component3 = entity.Get<CComponent>()
            };
        }

        protected override bool IsAssociatedDataValid(Entity entity, AComponent component, GameData<AComponent, BComponent, CComponent> associatedData)
        {
            return associatedData.Component1 == component &&
            associatedData.Component2 == entity.Get<BComponent>() && 
            associatedData.Component3 == entity.Get<CComponent>();
        }
    }


    public class GameData<AComponent, BComponent, CComponent, DComponent>
    {
        public AComponent Component1;
        public BComponent Component2;
        public CComponent Component3;
        public DComponent Component4;
    }

    public class SimpleGameEntityProcessor<AComponent, BComponent, CComponent, DComponent> : GameEntityProcessor<AComponent, GameData<AComponent, BComponent, CComponent, DComponent>> where AComponent : EntityComponent where BComponent : EntityComponent where CComponent :EntityComponent where DComponent : EntityComponent
    {
        public SimpleGameEntityProcessor() : base(typeof(BComponent), typeof(CComponent), typeof(DComponent))
        {

        }
        protected override GameData<AComponent, BComponent, CComponent, DComponent> GenerateComponentData(Entity entity, AComponent component)
        {
            return new()
            {
                Component1 = component,
                Component2 = entity.Get<BComponent>(),
                Component3 = entity.Get<CComponent>(),
                Component4 = entity.Get<DComponent>()
            };
        }

        protected override bool IsAssociatedDataValid(Entity entity, AComponent component, GameData<AComponent, BComponent, CComponent, DComponent> associatedData)
        {
            return associatedData.Component1 == component &&
            associatedData.Component2 == entity.Get<BComponent>() && 
            associatedData.Component3 == entity.Get<CComponent>() && 
            associatedData.Component4 == entity.Get<DComponent>();
        }
    }

    public class GameData<AComponent, BComponent, CComponent, DComponent, EComponent>
    {
        public AComponent Component1;
        public BComponent Component2;
        public CComponent Component3;
        public DComponent Component4;
        public EComponent Component5;
    }

    public class SimpleGameEntityProcessor<AComponent, BComponent, CComponent, DComponent, EComponent> : GameEntityProcessor<AComponent, GameData<AComponent, BComponent, CComponent, DComponent, EComponent>> where AComponent : EntityComponent where BComponent : EntityComponent where CComponent :EntityComponent where DComponent : EntityComponent where EComponent: EntityComponent
    {
        public SimpleGameEntityProcessor() : base(typeof(BComponent), typeof(CComponent), typeof(DComponent), typeof(EComponent))
        {

        }
        protected override GameData<AComponent, BComponent, CComponent, DComponent, EComponent> GenerateComponentData(Entity entity, AComponent component)
        {
            return new()
            {
                Component1 = component,
                Component2 = entity.Get<BComponent>(),
                Component3 = entity.Get<CComponent>(),
                Component4 = entity.Get<DComponent>(),
                Component5 = entity.Get<EComponent>()
            };
        }

        protected override bool IsAssociatedDataValid(Entity entity, AComponent component, GameData<AComponent, BComponent, CComponent, DComponent, EComponent> associatedData)
        {
            return associatedData.Component1 == component &&
            associatedData.Component2 == entity.Get<BComponent>() && 
            associatedData.Component3 == entity.Get<CComponent>() && 
            associatedData.Component4 == entity.Get<DComponent>() && 
            associatedData.Component5 == entity.Get<EComponent>();
        }
    }

    public class GameData<AComponent, BComponent, CComponent, DComponent, EComponent, FComponent>
    {
        public AComponent Component1;
        public BComponent Component2;
        public CComponent Component3;
        public DComponent Component4;
        public EComponent Component5;
        public FComponent Component6;
    }

    public class SimpleGameEntityProcessor<AComponent, BComponent, CComponent, DComponent, EComponent, FComponent> : GameEntityProcessor<AComponent, GameData<AComponent, BComponent, CComponent, DComponent, EComponent, FComponent>> where AComponent : EntityComponent where BComponent : EntityComponent where CComponent :EntityComponent where DComponent : EntityComponent where EComponent: EntityComponent where FComponent : EntityComponent
    {
        public SimpleGameEntityProcessor() : base(typeof(BComponent), typeof(CComponent), typeof(DComponent), typeof(EComponent))
        {

        }
        protected override GameData<AComponent, BComponent, CComponent, DComponent, EComponent, FComponent> GenerateComponentData(Entity entity, AComponent component)
        {
            return new()
            {
                Component1 = component,
                Component2 = entity.Get<BComponent>(),
                Component3 = entity.Get<CComponent>(),
                Component4 = entity.Get<DComponent>(),
                Component5 = entity.Get<EComponent>(),
                Component6 = entity.Get<FComponent>()
            };
        }

        protected override bool IsAssociatedDataValid(Entity entity, AComponent component, GameData<AComponent, BComponent, CComponent, DComponent, EComponent, FComponent> associatedData)
        {
            return associatedData.Component1 == component &&
            associatedData.Component2 == entity.Get<BComponent>() && 
            associatedData.Component3 == entity.Get<CComponent>() && 
            associatedData.Component4 == entity.Get<DComponent>() && 
            associatedData.Component5 == entity.Get<EComponent>() && 
            associatedData.Component6 == entity.Get<FComponent>();
        }
    }

    

}
