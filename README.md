# Enemy_Design_Code
This code utilizes a Markov Chain and a State-Machine to create a patrolling NPC
The code will randomly choose a state to start in one of 3 states
 * Patrolling area 1
 * Patrolling area 2
 * Idling in a location
Between these stats the NPC will choose randomly from the stats and do actions based on the state it is in

These are the probabilites of the next state it will choose after it has patrolled an area

if it is currently patrolling area 1
 * 40% chance continue
 * 30% chance patrol area 2
 * 30% chance idle

if it is currently patrolling area 2
 * 40% chance continue
 * 50% chance patrol area 1
 * 10% chance idle

if it is currently idle
 * 60% chance patrol area 1
 * 40% chance patrol area 2

It will continue to bounce between states
This repo is for other people to take this an add more states and more actions to be utilized in a game.
