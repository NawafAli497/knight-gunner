# Knight Gunner

![Unity](https://img.shields.io/badge/Unity-6000.0.62f1-black?logo=unity)
![C#](https://img.shields.io/badge/C%23-.NET-512BD4?logo=csharp)
![Platform](https://img.shields.io/badge/Platform-PC-blue)
![Render Pipeline](https://img.shields.io/badge/URP-2D%20Renderer-orange)

**Knight Gunner** is a 2D vertical platformer built in Unity, where a knight climbs through four seasonal levels. It combines two mechanics:

- **Charge jumping:** hold to build power, release to launch.
- **Mouse-aimed shooting:** bullets ricochet off walls and trigger switches that open the path forward.

Each season adds a new challenge: slippery ice, a race against the clock, moving platforms and falling hazards.

This was a team project built for a game development course at King Saud University.

<!-- Add a gameplay GIF or screenshots here, e.g. ![Gameplay](docs/gameplay.gif) -->

---

## Gameplay

| Level | What's new |
|-------|------------|
| **Spring** | Learn the basics: charge-jump between ledges and shoot switches to open blocked paths. |
| **Summer** | A **30-second timer** is ticking. Collect water bottles for **+10 seconds** and ride moving platforms. If time runs out, the game freezes and offers a restart. |
| **Fall** | Tree branches fall from above while you climb across platforms that move along waypoints. |
| **Winter** | **Ice physics:** the knight keeps sliding and must brake by steering against the slide. Ricochet bullets off reflective walls to hit switches that are out of direct line of sight. |

Each level ends at a portal that loads the next season. Clearing Winter takes you to the victory screen.

### Controls

| Action | Input |
|--------|-------|
| Move | `A` / `D` or `←` / `→` |
| Charge jump | Hold `Space` (the charge bar fills up) |
| Jump | Release `Space`. The direction you're holding sets the jump arc |
| Aim | Mouse |
| Shoot | Left click (only while standing on the ground) |

---

## Technical highlights

- **Charge-jump controller** ([ChargeJump.cs](Assets/Scripts/Player/ChargeJump.cs))
  - Jump force builds over time between a min and a max value, and a UI slider shows it.
  - On release, horizontal and vertical force are applied together as a single impulse.
  - Ground detection uses `Physics2D.OverlapCircle`.
  - Wall detection uses `OverlapBox`, so the run animation doesn't play while the knight pushes against a wall.

- **Custom ice physics**
  - Standing on an `Ice`-tagged surface switches to a momentum model in `FixedUpdate`: input accelerates the knight, friction decays the velocity, and steering against the slide brakes.
  - Momentum carries through charged jumps.

- **Clamped mouse aiming** ([Shooting.cs](Assets/Scripts/Player/Shooting.cs))
  - The gun follows the cursor within a ±45° cone.
  - Facing direction has a priority order: charge input first, then airborne velocity, then cursor position.
  - The gun is hidden while charging or in the air.
  - Each bullet spawns at the barrel tip and ignores the player's own collider.

- **Ricochet bullets** ([Bullet.cs](Assets/Scripts/Player/Bullet.cs))
  - On hitting a `ReflectWall`, the bullet reflects its velocity off the contact normal with `Vector2.Reflect`.
  - It reflects the velocity cached from the previous frame, because the physics engine has already changed the velocity by the time the collision callback runs.

- **Shootable switches** ([ShootableSwitch.cs](Assets/Scripts/Level/ShootableSwitch.cs)): trigger-based switches that open a wall when a bullet hits them. Combined with ricochets, they make simple puzzles.

- **Waypoint platforms** ([PathMover.cs](Assets/Scripts/Level/PathMover.cs), [MovingGround.cs](Assets/Scripts/Level/MovingGround.cs))
  - Platforms loop through a list of waypoints, with an optional pause at each one.
  - The player is parented to a platform on contact, so it rides along naturally.

- **Hazard spawner** ([BranchSpawner.cs](Assets/Scripts/Level/BranchSpawner.cs)): spawns falling branches on a timer at random positions inside an area.

- **Timer and game state** ([TimeLeftScript.cs](Assets/Scripts/UI/TimeLeftScript.cs), [GameControlScriptSummer.cs](Assets/Scripts/GameFlow/GameControlScriptSummer.cs))
  - A countdown that pickups can extend.
  - When it hits zero, the game pauses with `Time.timeScale` and shows the restart UI.

- **Smooth vertical camera** ([CamFollowPlayer.cs](Assets/Scripts/Camera/CamFollowPlayer.cs)): follows the climb with a smoothed lerp and a minimum height.

- **Animation state machine**: the Animator controller handles Idle, Run, Charge, Jump and Fall, driven by the player's grounded, running and charging state.

## Built with

- **Unity 6** (6000.0.62f1) and **C#**
- **Universal Render Pipeline** with the 2D Renderer
- **Tilemaps** for level design and **Physics 2D** for movement and collisions
- **TextMesh Pro** and **uGUI** for menus, the timer and the charge bar
- **Particle systems** for the snow and falling-leaf effects

---

## Project structure

```
Assets/
├── Animation/          Player Animator controller and clips
├── Art/
│   ├── Characters/     Knight sprite sheets and arm
│   ├── Materials/      Particle materials (snow, leaves)
│   ├── Sprites/        Bullet, branch, bottle and other sprites
│   └── Tilesets/       One folder per season (tileset and tile assets)
├── Physics/            2D physics materials
├── Prefabs/
│   ├── Player/         Knight, arm pivot, fire point, bullet
│   ├── Level/          Portal, switch, water bottle, tree branch
│   └── UI/             Timer, charge bar, restart UI, game control
├── Scenes/             MainMenu → Spring → Summer → Fall → Winter → Victory
├── Scripts/
│   ├── Player/         Movement, charge jump, aiming, shooting, bullets
│   ├── Level/          Platforms, switches, hazards
│   ├── Camera/         Camera follow
│   ├── UI/             Menus and timer display
│   └── GameFlow/       Restart and game-over logic
└── Settings/           URP and renderer settings
```

## Getting started

1. Install **Unity 6000.0.62f1** with [Unity Hub](https://unity.com/download). Any Unity 6 version should work.
2. Clone the repository:
   ```bash
   git clone https://github.com/NawafAli497/knight-gunner.git
   ```
3. In Unity Hub, click **Add → Add project from disk** and select the cloned folder.
4. Open `Assets/Scenes/MainMenu.unity` and press **Play**.

To make a standalone build, use **File → Build Profiles**. The scene order is already set up.

---

## Possible improvements

- Move from the legacy `Input` class to the new **Input System** (the package is already installed).
- Combine the four per-season imports of the tileset into one shared tile palette.
- Replace the per-level game controllers with a single game manager that handles timers, death and restart for every level.
- Add sound effects, music and a pause menu.

## Team

Built by **Nawaf Ali** and **Abdulmajeed** as a course project.

## Credits

The knight character sprites, the four-seasons tileset and the water bottle sprite are third-party assets, used here for educational purposes. All rights belong to their original creators.
