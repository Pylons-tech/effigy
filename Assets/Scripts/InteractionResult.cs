public readonly struct InteractionResult
{
    public readonly InteractionState State;

    public InteractionResult(InteractionState state)
    {
        State = state;
    }
}