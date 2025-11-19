Taken from these videos:
https://www.youtube.com/watch?v=iXNwWpG7EhM
https://www.youtube.com/watch?v=P-U7GPXMtLY

TODO: make more typesafe coded listeners - they currently use Base which is not typesafe

To use an existing type <T> of event/listener...
1. create the event asset in Core/Game Events/
2. the triggering script must have a reference to the event asset & call .Raise()
3. any listeners must have the relevant "Unity [type] Listener" script attached
4. The listener script must point to a function that takes in the [type]

The VoidType type can be used where no parameters are passed in

To use CodedGameEventListener use the following example. This is simpler to maintain than the UnityEvent shite
	private CodedGameEventListener<T> _listener = new CodedGameEventListener<T>();
	[SerializeField] private [T]GameEvent _event; (e.g. IntGameEvent)
	Reg/Unreg with [_listener].Register(m_event, [method]) and [_listener].Unregister()

To create a new type <T> of listener e.g. AudioCue...
1. Create the 3 scripts...
	[type]GameEvent
	Unity[type]Event
	Unity[type]Listener
2. proceed as above


EXAMPLE OF VOID TYPE...
private CodedGameEventListener<VoidType> _btnlistener = new CodedGameEventListener<VoidType>();
[SerializeField] private VoidGameEvent _event;

private void RegisterListeners()
{
    _btnlistener.Register(_event, MyOnClickMethod);
}

private void MyOnClickMethod(VoidType v)
{
    //do something
}

private void UnregisterListeners()
{
    _btnlistener.Unregister();
}