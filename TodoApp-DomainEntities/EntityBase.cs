namespace TodoApp_DomainEntities
{
    public abstract class EntityBase : EntityBase<long>
    {
        public sealed override bool IsNew()
        {
            return this.Id < 0;
        }
    }

    public abstract class EntityBase<TKey> : IEntity<TKey>
    where TKey : struct
    {
        protected EntityBase()
        {
        }

        public TKey Id { get; set; }

        public virtual bool IsNew()
        {
            return Equals(Id, default(TKey));
        }
    }
}
