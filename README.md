# Wake Up! -- Return-0 Interactive
A COMP3770 Game Development Project from University of Windsor

## Game Design Document & SharePoint
(site link here)

## Vision
Wake Up! is a 3D psychological horror game about completing a morning routine while identifying anomalies (strange occurrences) to determine whether the player is actually awake or still dreaming.
The player must observe their surroundings, complete their morning tasks, and decide whether to continue their routine and leave for work or go back to bed. Making the wrong decision causes the player to lose and reset the work week, forcing them to try again.

## Parameters
We picked five parameters that shape how Wake Up! feels to play, and each one has a target we can check in playtesting.

Realism: The house is semi-realistic, dimly lit, and built at real-world scale. The player walks at about 2.5 m/s with no sprint and a 70–80° field of view. Keeping “normal” believable is what makes an anomaly stand out.
Difficulty: Each morning has either no anomaly or exactly one, and the chance rises from about 50% on day 1 to 80% on day 5. Going back to bed repeats the day, but only twice, and a new player should need around 2–4 attempts to win.
Tension: There are no enemies or jump scares. The unease comes from not knowing whether something is wrong, and we’re aiming for an average survey rating of about 3.5/5.
Time Pressure: A wall clock is the only timer, with no countdown on screen. Each morning lasts about 4 minutes, which is enough to look around but not enough to relax.
Accessibility: A first-time player should be able to finish the tutorial day without any text instructions, using only W/A/S/D, the mouse, and E.

## Project Scope
The scope of Wake Up! will have to be limited, due to constraints of time and limited team members. The game will have about 5 morning tasks, 10-15 anomalies in a single map, and will include simple object interactions. It will be released for Windows PC as a single-player experience, and will not be ported to console platforms. Game assets will be created in-house mainly, but audio and some textures will be acquired from 3rd-party sources.

## In-and-Out Formalization

### Inputs
The game inputs were formalized as:
`Inputs = (P, S, G)`
- **P** – Player action
- **S** – Current game state
- **G** – Game parameters

### Game State
The game state was defined as:
`S = (D, M, K, T, A)`
Where:
- **D** – Current day
- **M** – Current map state
- **K** – Tasks for the current day
- **T** – Current timer value
- **A** – Selected anomaly

### Game Parameters
The game parameters were defined as:
`G = (D_max, PA, T_max, K_r)`
These control the maximum number of days, anomaly probability, timer limit, and required number of tasks.

### Outputs
The outputs were formalized as:
`Outputs = (S + 1, F, W, L)`
- **S + 1** – Updated game state
- **F** – Player feedback, such as visuals, sounds, task updates, and anomaly effects
- **W** – Win condition
- **L** – Lose condition
