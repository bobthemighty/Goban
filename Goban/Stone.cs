namespace Goban
{
    internal class Stone
    {
        public Stone(State state, int moveNumber)
        {
            State = state;
            MoveNumber = moveNumber;
        }

        public State State { get; private set; }
        public int MoveNumber { get; private set; }
    }
}