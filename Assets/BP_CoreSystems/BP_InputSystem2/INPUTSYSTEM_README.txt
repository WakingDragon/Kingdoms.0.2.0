INPUT SYSTEM

DEPENDENT ON:
	using UnityEngine.InputSystem
	using BP.Core

RESOURCES
	Good background explainer: https://www.youtube.com/watch?v=m5WsmlEOFiA
	Architecture way to proceed: https://www.youtube.com/watch?v=ZHOWqF-b51k
	Makes reference to touch input: https://www.youtube.com/watch?v=4MOOitENQVg
	Touch/drag (excl UI): https://www.youtube.com/watch?v=HfqRKy5oFDQ
	UI drag: https://www.youtube.com/watch?v=XCoDKZqa-PE
	On screen btns/joystick (it turns screen input to controller input) https://www.youtube.com/watch?v=TBcfhJoCVQo&list=PLKUARkaoYQT2nKuWy0mKwYURe2roBGJdr&index=13
	How to use new input system with UI:
		Graphics raycasting: https://www.youtube.com/watch?v=7h1cnGggY2M (from 5:50)
		Using different event types e.g. onMouseEnter with unity events: https://www.youtube.com/watch?v=DXcUhk7w-Us
		Using OnPointerEvent data? https://www.youtube.com/watch?v=Y8A5z0FmmS8
		Avoiding the button assignment in inspector: https://www.youtube.com/watch?v=tFIFHSgYRxM (from 9m)

	All SamYam's group on InputSystem: https://www.youtube.com/playlist?list=PLKUARkaoYQT2nKuWy0mKwYURe2roBGJdr

CONCEPT
	Universal Input manager SO that fires off events to Interfaced classes.
	So any game will only implement a subset of the action maps.
	Maps such as... UI,CardGame,Person,Flight,etc.
	The SO will implement all the interfaces and I need to create SO events for each type of input, which can then be received by an IInputReceiver (or something)

GUIDES
	ADD/AMEND A NEW INPUT ACTION
		> add/amend the action in input action asset
		> regen scripts
		> create/update subscribers 
		> create/amend SO events

	ADD A NEW ACTION MAP e.g. Flying
		> create the new map in BPInputActions, regen scripts
		> add to ActionMapEnum - ? could this be done automatically with a class?
		> add new input actions (and events)

	HANDLING TYPES
		> any key: just a listener (no btn or anything) for an anykey event.
		> UI button: use btn
		> see the subject-based videos above

TASKS:
	> add controls and events to input actions as needed