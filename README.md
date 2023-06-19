# Unity - Create With Code: Personal-Project

Game can be played here: https://play.unity.com/mg/other/bridge-to-valhalla

With this project, I was able to practice everything I learnt about Unity and C# throughout the Create with Code course.

## Mechanics

### Controls
You can control a 'Player' GameObject using WASD, attack enemies infront of you with SPACEBAR and block enemy attacks with LEFTCTRL.

### Menus
The game starts with a title screen, and a 'Start' button. Upon clicking start, a horn sound is played and the game begins. This will set the isGameActive bool to 'true'. It will also set the castle and player health values. The 'isGameActive' bool is used in other scripts so they know whether or not to run, for example player movement in the PlayerController script.
Upon losing, a Game Over screen is displayed, depending on how the player loses. You can then click 'Restart' to reload the game scene.

### Player
During the Update() method, the PlayerController script listens for the WASD keys for player movement and rotation, SPACEBAR for the enemy to attack, and LEFTCTRL for the player to block. Using an invisible entity infront of the player, attacking will check if there is an enemy collision with that invisible entity. If there is, the player will deal damage.
The player cannot move outside of some bounds I have set. This stops them running off the bridge, running into the castle and getting too close to the enemy spawn position.

### Prefab spawning
Basic enemies, hunters and armour/repair all have some randomness to their spawn position. Enemies all spawn on the same position on the X and Y axis, however their Z axis is randomised so they don't all run down the same area of the bridge. The armour/repair item can spawn anywhere the player is able to move.

### Basic Enemies
There is an array of 'basicEnemy' prefabs, containing the soldier and the archer. A random one of the two, using Random.Range(), will spawn on the left side of the screen every 3 seconds and move towards the castle.

- Soldier: The soldier will continue to run until they reach the castle. Upon arriving within 'attackDistance', they will stop running and begin attacking the castle. The time of their attack is recorded each time, and they cannot attack again until enough time has elapsed since.
- Archer: The archer will continue to run until they arre within 'attackDistance'. At this point, they will stop running and begin firing arrows at the castle. This is done by spawning an Arrow prefab with an Impulse force to the arrow upon spawning. If this collides with the castle, it will do damage. If it collides with anything else, it will be destroyed.

### Hunter Enemy
There is a 'Hunter' enemy, who will hunt the player down. One spawns every 15 seconds. They will repeatedly check the player position and compare it to their own to face and move towards the player. Upon arriving at the player, they will stop running and attack. When attacking, an 'isAttacking' variable is set to true, which stops them from continuing to move or rotate towards the player, allowing the player to dodge. The player can also block enemy hits, negating any damage taken.

### Enemy GameOver behaviour
If the player dies, the enemies will continue to attack their targets. However if the castle falls (even if this is after the player dies), all enemies will run into the castle doors. This is done with a gameStatus variable being set to "Castle Destroyed" when the castle health reaches 0.

### Armour/Repair
There is a crate that will spawn every 20 seconds which when the player enters it's trigger area, will be destroyed. When this happens, the castle is repaired for 5 damage (to a max of 100 health), and the player is gains 1 HP (no limit on how much HP the player can have).

### Audio
I have used various pieces of audio throughout the game. There are several ambient sounds running in the background to improve the atmosphere of the game. There are 3 death sounds for the player and enemies, which one will be randomly played each time there is a death. There are impact sounds for attacks, as well as bow strings for when the archer attacks.
The most fun to implement was the footsteps sound. I have set 4 'left foot' and 4 'right foot' sounds, which a random one is picked and played every ~0.3 seconds while a unit is walking. I track the time the last footstep was played and compare it to a 'stepGap' float variable so a footsteps aren't played hundreds times second, as well as alternating between left and right foot sounds by flipping a 'isRightFoot' bool back and forth.


## Improvements
There are many improvements which could be made to the game, however I need to draw the line somewhere! I've achieved what I set out to complete, as well as a bit more. However, if I were to make improvements, here's a few things I could do.

- Better UI. The current castle/player health is not intuitive and can be difficult to see depending on your screen size.
- Sprint button. It would be good if the player could sprint, perhaps even have a stamina bar. I could check whether LEFTSHIFT is being held and increase the player 'speed' variable to make them move faster. A stamina bar could be made using a 'slider' UI item and depleting an X amount from 100 from a 'stamina' variable every second. This could replenish over time when the player is not sprinting, or I could add more items to pick up to replenish it.
- Player/Castle damage text. When the player hits or is it, or when the castle is hit, the some text could appear above them to make this clearer.
- Weapons. None of the characters actually carry weapons, not even the archers. My aim was to practice my coding in C#, and I didn't want to spend too much time on something that I didn't think would help me with that.
- Enemy Waves. I could have a set number of enemies that can spawn before a new wave phase, which could have enemies that spawn quicker, do more damage or have more HP. I could even have the player get money when enemies are killed, which could allow them to upgrade their character. More damage, more health, faster or more stamina.
