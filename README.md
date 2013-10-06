RTSControls
===========
Author: *Zachary Hoefler*

This unity project implements basic RTS-like controls for units. Units use pathfinding to avoid obstacles on the way to their destination, and are able to be put into formations.

NOTE: The current implementation is *super* slow to calculate pathfinding. The code was written for clarity as an example/starting point rather than for performance.

Input
-----
* **Drag the left mouse** to select units
* **Right Click** or **Spacebar**: Move units to mouse position
* **Number 1**: Arrange units in line formation
* **Number 2**: Arrange units in staggered formation
* **Arrow Keys**: Pan the camera

Implementation
--------------
* Pathfinding is done using the [A\* search algorithm](http://en.wikipedia.org/wiki/A*_search_algorithm).
* The collision grid used for pathfinding is created on startup via the `CollisionGridGenerator` class. It essentially does a bunch of raycasts on the Obstacle layer, and if it hits something, that space is considered blocked. If nothing is hit, the space is assumed to be open for movement.
* If a unit's destination is invalid, the generated "path" is just their start position to prevent the unit from attempting to search the entire map for a non-existent path before finally realizing there is none.

Future Goals (To-do list)
-------------------------
* Implement [jump point search](http://grastien.net/ban/articles/hg-aaai11.pdf‎) for pathfinding. Significantly speeds up A* pathfinding!
* Make formations more robust. Currently, they'll only work on units not in motion to a destination as setting the formation conflicts with their pathfinding.
* Better artwork than cubes :)
* Networking! First step would be making the simulation entirely deterministic. Second is setting up networking by synchronizing input. (Most RTS games implement networking this way)
