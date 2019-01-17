namespace DPA_Musicsheets.ChainOfResponsibility
{
    public abstract class Command<T>
    {
        protected T _receiver;

        public Command(T receiver)
        {
            this._receiver = receiver;
        }

        public abstract void Execute();
    }
}